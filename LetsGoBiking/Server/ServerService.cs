using Server.Entities.Response;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.API;
using Server.Entities.ORS;
using Server.Entities;
using System.ServiceModel.Web;
using System.Net;

namespace Server
{
    internal class ServerService : IServerService
    {
        public void GetOptions()
        {
            var response = WebOperationContext.Current.OutgoingResponse;

            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");

            response.StatusCode = HttpStatusCode.OK;
        }

        public async Task<ItineraryResponse> ItineraryAsync(double OriginLat, double OriginLng, double DestLat, double DestLng)
        {
            try
            {   
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                }

                Console.WriteLine($"Searching itinerary from ({OriginLat}, {OriginLng}) to ({DestLat}, {DestLng})");

                JCDecauxAPI Api = new JCDecauxAPI();
                OpenRouteServiceAPI ORSApi = new OpenRouteServiceAPI();

                List<Station> Stations = await Api.GetAllStations();

                GeoCoordinate OriginCoord = new GeoCoordinate(OriginLat, OriginLng);
                GeoCoordinate DestCoord = new GeoCoordinate(DestLat, DestLng);

                GeoCoordinate CurrentOriginCoord = OriginCoord;
                Station LastStation = null;

                List<Route> WalkRoutes = new List<Route>();
                List<BikeRoute> BikeRoutes = new List<BikeRoute>();

                Station ClosestLastDestination = Stations
                    .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.stands > 0)
                    .OrderBy(s => DestCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                    .FirstOrDefault();

                HashSet<string> UsedContract = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                Location DestinationLocation = new Location
                {
                    Latitude = DestCoord.Latitude,
                    Longitude = DestCoord.Longitude
                };
                bool destinationReachedByFoot = false;

                while (LastStation != ClosestLastDestination)
                {
                    Console.WriteLine($"Looking the closest station from ({CurrentOriginCoord.Latitude}, {CurrentOriginCoord.Longitude})");
                    Station ClosestOriginStation = Stations
                        .Where(s =>
                            s.totalStands != null &&
                            s.totalStands.availabilities != null &&
                            s.totalStands.availabilities.bikes > 0 &&
                            !UsedContract.Contains(s.contractName))
                        .OrderBy(s => CurrentOriginCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                        .FirstOrDefault();

                    if (ClosestOriginStation == null)
                    {
                        Console.WriteLine("No more origin stations available. Breaking loop.");
                        break;
                    }

                    Console.WriteLine($"Origin station is at ({ClosestOriginStation.position.latitude}, {ClosestOriginStation.position.longitude}) in the contract {ClosestOriginStation.contractName}");

                    Console.WriteLine($"Checking if quicklier to go by foot to the next station without using the last bike route");

                    Station ClosestDestinationStation = Stations
                        .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.stands > 0 && s.contractName == ClosestOriginStation.contractName)
                        .OrderBy(s => DestCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                        .FirstOrDefault();
                    if (ClosestDestinationStation == null)
                    {
                        Console.WriteLine("No destination station available for this contract. Breaking loop.");
                        break;
                    }
                    Console.WriteLine($"Destination station is at ({ClosestDestinationStation.position.latitude}, {ClosestDestinationStation.position.longitude}) in the contract {ClosestDestinationStation.contractName}");


                    Location StartLocation = new Location
                    {
                        Latitude = CurrentOriginCoord.Latitude,
                        Longitude = CurrentOriginCoord.Longitude
                    };
                    Location PickupLocation = new Location
                    {
                        Latitude = ClosestOriginStation.position.latitude,
                        Longitude = ClosestOriginStation.position.longitude
                    };
                    Location DropoffLocation = new Location
                    {
                        Latitude = ClosestDestinationStation.position.latitude,
                        Longitude = ClosestDestinationStation.position.longitude
                    };

                    RouteFeature WalkDirectRoute = await ORSApi.getRoute(
                        StartLocation,
                        DestinationLocation,
                        "foot-walking"
                    );

                    RouteFeature ToPickupRoute = await ORSApi.getRoute(
                        StartLocation,
                        PickupLocation,
                        "foot-walking"
                    );
                    RouteFeature BikeRoute = await ORSApi.getRoute(
                        PickupLocation,
                        DropoffLocation,
                        "cycling-regular"
                    );
                    RouteFeature DropoffToDestinationRoute = await ORSApi.getRoute(
                        DropoffLocation,
                        DestinationLocation,
                        "foot-walking"
                    );

                    double mixedDuration =
                        ToPickupRoute.Properties.Summary.Duration +
                        BikeRoute.Properties.Summary.Duration +
                        DropoffToDestinationRoute.Properties.Summary.Duration;
                    double walkDirectDuration = WalkDirectRoute.Properties.Summary.Duration;

                    if (walkDirectDuration <= mixedDuration)
                    {
                        Console.WriteLine("Walking directly to destination is faster. Removing remaining bike routes.");
                        WalkRoutes.Add(new Route
                        {
                            Start = StartLocation,
                            End = DestinationLocation,
                            Feature = WalkDirectRoute
                        });
                        destinationReachedByFoot = true;
                        CurrentOriginCoord = DestCoord;
                        LastStation = ClosestLastDestination;
                        break;
                    }

                    WalkRoutes.Add(
                        new Route
                        {
                            Start = StartLocation,
                            End = PickupLocation,
                            Feature = ToPickupRoute
                        }
                    );
                    BikeRoutes.Add(
                        new BikeRoute
                        {
                            Start = PickupLocation,
                            End = DropoffLocation,
                            Feature = BikeRoute,
                            AddressStart = ClosestOriginStation.address,
                            AvailableBikes = ClosestOriginStation.totalStands.availabilities.bikes,
                            AddressEnd = ClosestDestinationStation.address,
                            AvailableDropPlace = ClosestDestinationStation.totalStands.availabilities.stands
                        }
                    );

                    CurrentOriginCoord = new GeoCoordinate(DropoffLocation.Latitude, DropoffLocation.Longitude);
                    LastStation = ClosestDestinationStation;
                    UsedContract.Add(ClosestOriginStation.contractName);
                }

                if (!destinationReachedByFoot)
                {
                    // Add the last walking part
                    Location LastStartLocation = new Location
                    {
                        Latitude = CurrentOriginCoord.Latitude,
                        Longitude = CurrentOriginCoord.Longitude
                    };

                    RouteFeature LastWalkRoute = await ORSApi.getRoute(
                        LastStartLocation,
                        DestinationLocation,
                        "foot-walking"
                    );

                    WalkRoutes.Add(
                        new Route
                        {
                            Start = LastStartLocation,
                            End = DestinationLocation,
                            Feature = LastWalkRoute
                        });
                }

                return new ItineraryResponse
                {
                    WalkRoutes = WalkRoutes,
                    BikeRoutes = BikeRoutes,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ItineraryAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw to let WCF handle it
            }
        }
    }
}
