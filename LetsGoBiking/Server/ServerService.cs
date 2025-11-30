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

        public async Task<ItineraryResponse> ItineraryAsync(double OriginLat, double OriginLng, double DestLat, double DestLng, bool useDott = false)
        {
            try
            {   
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                }

                Console.WriteLine($"Searching itinerary from ({OriginLat}, {OriginLng}) to ({DestLat}, {DestLng}) with Dott: {useDott}");

                JCDecauxAPI Api = new JCDecauxAPI();
                OpenRouteServiceAPI ORSApi = new OpenRouteServiceAPI();

                List<Station> Stations = await Api.GetAllStations();

                if (useDott)
                {
                    Console.WriteLine("Fetching Dott stations...");
                    DottAPI DottApi = new DottAPI();
                    List<Station> DottStations = await DottApi.GetAllStations();
                    Console.WriteLine($"Found {DottStations.Count} Dott stations.");
                    Stations.AddRange(DottStations);
                }

                GeoCoordinate OriginCoord = new GeoCoordinate(OriginLat, OriginLng);
                GeoCoordinate DestCoord = new GeoCoordinate(DestLat, DestLng);

                List<Route> WalkRoutes = new List<Route>();
                List<BikeRoute> BikeRoutes = new List<BikeRoute>();

                HashSet<string> UsedContract = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                
                // 1. Global Selection of Contracts
                var selectedSegments = new List<(string Contract, Station PickupStation, Station DropoffStation, RouteFeature BikeRoute, double DistanceFromStart)>();
                
                // Calculate direct walk time once for comparison
                Location GlobalStartLocation = new Location { Latitude = OriginCoord.Latitude, Longitude = OriginCoord.Longitude };
                Location GlobalEndLocation = new Location { Latitude = DestCoord.Latitude, Longitude = DestCoord.Longitude };
                
                RouteFeature GlobalWalkDirectRoute = await ORSApi.getRoute(GlobalStartLocation, GlobalEndLocation, "foot-walking");
                double globalWalkDirectDuration = GlobalWalkDirectRoute.Properties.Summary.Duration;
                Console.WriteLine($"\nGlobal Direct Walk Duration: {globalWalkDirectDuration}s\n");

                while (true)
                {
                    Console.WriteLine($"Looking for best contract close to the line from ({OriginCoord.Latitude}, {OriginCoord.Longitude}) to ({DestCoord.Latitude}, {DestCoord.Longitude})");

                    var candidateStation = Stations
                        .Where(s =>
                            s.totalStands != null &&
                            s.totalStands.availabilities != null &&
                            s.totalStands.availabilities.bikes > 0 &&
                            !UsedContract.Contains(s.contractName))
                        .Select(s => new
                        {
                            Station = s,
                            DistanceToLine = new GeoCoordinate(s.position.latitude, s.position.longitude)
                                .GetDistanceFromSegment(OriginCoord, DestCoord)
                        })
                        .OrderBy(x => x.DistanceToLine)
                        .FirstOrDefault();

                    if (candidateStation == null)
                    {
                        Console.WriteLine("No more candidate stations/contracts available.");
                        break;
                    }

                    string candidateContract = candidateStation.Station.contractName;
                    Console.WriteLine("\n\n================================");
                    Console.WriteLine($"Candidate contract found: {candidateContract} (Distance to line: {candidateStation.DistanceToLine:F2}m)");
                    // Find best pickup (closest to Global Start) and dropoff (closest to Global End) in this contract
                    Station BestPickupStation = Stations
                        .Where(s => s.contractName == candidateContract && s.totalStands.availabilities.bikes > 0)
                        .OrderBy(s => OriginCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                        .FirstOrDefault();

                    Station BestDropoffStation = Stations
                        .Where(s => s.contractName == candidateContract && s.totalStands.availabilities.stands > 0)
                        .OrderBy(s => DestCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                        .FirstOrDefault();

                    if (BestPickupStation == null || BestDropoffStation == null)
                    {
                        Console.WriteLine("Could not find valid pickup or dropoff in candidate contract. Skipping/Removing.");
                        UsedContract.Add(candidateContract);
                        continue;
                    }

                    Console.WriteLine($"  Best Pickup for {candidateContract}: {BestPickupStation.name} (Lat: {BestPickupStation.position.latitude}, Lng: {BestPickupStation.position.longitude})");
                    Console.WriteLine($"  Best Dropoff for {candidateContract}: {BestDropoffStation.name} (Lat: {BestDropoffStation.position.latitude}, Lng: {BestDropoffStation.position.longitude})");

                    // Evaluate if this contract is useful
                    Location PickupLoc = new Location { Latitude = BestPickupStation.position.latitude, Longitude = BestPickupStation.position.longitude };
                    Location DropoffLoc = new Location { Latitude = BestDropoffStation.position.latitude, Longitude = BestDropoffStation.position.longitude };

                    RouteFeature ToPickup = await ORSApi.getRoute(GlobalStartLocation, PickupLoc, "foot-walking");
                    RouteFeature Bike = await ORSApi.getRoute(PickupLoc, DropoffLoc, "cycling-regular");
                    RouteFeature DropoffToDest = await ORSApi.getRoute(DropoffLoc, GlobalEndLocation, "foot-walking");

                    double mixedDuration = 
                        ToPickup.Properties.Summary.Duration + 
                        Bike.Properties.Summary.Duration + 
                        DropoffToDest.Properties.Summary.Duration;

                    Console.WriteLine($"Evaluation for {candidateContract}: Mixed {mixedDuration}s vs Direct {globalWalkDirectDuration}s");

                    if (mixedDuration < globalWalkDirectDuration)
                    {
                        Console.WriteLine($"Contract {candidateContract} is useful. Adding to selection.");
                        selectedSegments.Add((
                            candidateContract,
                            BestPickupStation,
                            BestDropoffStation,
                            Bike,
                            OriginCoord.GetDistanceTo(new GeoCoordinate(BestPickupStation.position.latitude, BestPickupStation.position.longitude))
                        ));
                        UsedContract.Add(candidateContract);
                    }
                    else
                    {
                        Console.WriteLine($"Contract {candidateContract} is NOT useful (slower than walking). Stopping algorithm.");
                        break;
                    }
                }

                // 2. Chain the selected segments
                // Sort by distance from start to ensure correct order
                var sortedSegments = selectedSegments.OrderBy(s => s.DistanceFromStart).ToList();
                
                Location CurrentLocation = GlobalStartLocation;

                foreach (var segment in sortedSegments)
                {
                    Station pickup = segment.PickupStation;
                    Station dropoff = segment.DropoffStation;
                    Location PickupLoc = new Location { Latitude = pickup.position.latitude, Longitude = pickup.position.longitude };
                    Location DropoffLoc = new Location { Latitude = dropoff.position.latitude, Longitude = dropoff.position.longitude };

                    Console.WriteLine($"Building route segment: Walk to {pickup.name} -> Bike to {dropoff.name}");

                    // Walk from Current to Pickup
                    RouteFeature walkRoute = await ORSApi.getRoute(CurrentLocation, PickupLoc, "foot-walking");
                    WalkRoutes.Add(new Route
                    {
                        Start = CurrentLocation,
                        End = PickupLoc,
                        Feature = walkRoute
                    });

                    // Bike from Pickup to Dropoff
                    BikeRoutes.Add(new BikeRoute
                    {
                        Start = PickupLoc,
                        End = DropoffLoc,
                        Feature = segment.BikeRoute,
                        AddressStart = pickup.address,
                        AvailableBikes = pickup.totalStands.availabilities.bikes,
                        AddressEnd = dropoff.address,
                        AvailableDropPlace = dropoff.totalStands.availabilities.stands,
                        VehicleType = pickup.contractName.Contains("_dott") ? "scooter" : "bike"
                    });

                    CurrentLocation = DropoffLoc;
                }

                // 3. Final Walk
                Console.WriteLine("Building final walk segment.");
                RouteFeature finalWalk = await ORSApi.getRoute(CurrentLocation, GlobalEndLocation, "foot-walking");
                WalkRoutes.Add(new Route
                {
                    Start = CurrentLocation,
                    End = GlobalEndLocation,
                    Feature = finalWalk
                });

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
        public async Task<List<Address>> GetAddressesAsync(string text)
        {
            try
            {
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                }

                Console.WriteLine($"Searching addresses for: {text}");
                GeoapifyAPI api = new GeoapifyAPI();
                var features = await api.GetFeatures(text);

                return features.Select(f => new Address
                {
                    Label = f.Properties.Formatted,
                    Lat = f.Geometry.Coordinates[1],
                    Lon = f.Geometry.Coordinates[0]
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAddressesAsync: {ex.Message}");
                return new List<Address>();
            }
        }
    }
}
