using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Specialized;
using System.ComponentModel;
using Server.API;
using Server.Entities;
using Server.Entities.ORS;
using Server.Entities.Response;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    public class RoutingService
    {
        private readonly HttpListener _listener;
        public RoutingService(string prefix)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine("RoutingService started...");
            Task.Run(() => HandleRequests());
        }

        private void AddCorsHeaders(HttpListenerResponse response)
        {
            // Allow all origins for now
            response.Headers.Set("Access-Control-Allow-Origin", "*");
            response.Headers.Set("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Set("Access-Control-Allow-Headers", "Content-Type, Accept");
        }

        private async Task HandleRequests()
        {
            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                Console.WriteLine($"Received {context.Request.HttpMethod} {context.Request.Url.AbsolutePath}");
                try
                {
                    // Handle CORS preflight
                    if (context.Request.HttpMethod == "OPTIONS")
                    {
                        AddCorsHeaders(context.Response);
                        context.Response.StatusCode = 200;
                        context.Response.OutputStream.Close();
                        continue;
                    }

                    if (context.Request.HttpMethod == "GET" && context.Request.Url.AbsolutePath == "/itinerary")
                    {
                        ItineraryRequest req = null;
                        try
                        {
                            NameValueCollection query = context.Request.QueryString;

                            if (string.IsNullOrEmpty(query["originLat"]) ||
                                string.IsNullOrEmpty(query["originLng"]) ||
                                string.IsNullOrEmpty(query["destLat"]) ||
                                string.IsNullOrEmpty(query["destLng"]))
                            {
                                throw new Exception("Missing required query parameters (originLat, originLng, destLat, destLng).");
                            }

                            req = new ItineraryRequest
                            {
                                originLat = double.Parse(query["originLat"], CultureInfo.InvariantCulture),
                                originLng = double.Parse(query["originLng"], CultureInfo.InvariantCulture),
                                destLat = double.Parse(query["destLat"], CultureInfo.InvariantCulture),
                                destLng = double.Parse(query["destLng"], CultureInfo.InvariantCulture)
                            };
                            Console.WriteLine($"Request params: origin=({req.originLat},{req.originLng}), dest=({req.destLat},{req.destLng})");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Parameter parsing error: {ex.Message}");
                            AddCorsHeaders(context.Response);
                            context.Response.StatusCode = 400; // Bad Request
                            byte[] error = Encoding.UTF8.GetBytes($"Invalid or missing query parameters: {ex.Message}");
                            context.Response.OutputStream.Write(error, 0, error.Length);
                            context.Response.OutputStream.Close();
                            continue;
                        }

                        try
                        {
                            ItineraryStationsResponse resp = await ComputeItinerary(req);
                            string respJson = System.Text.Json.JsonSerializer.Serialize(resp);
                            AddCorsHeaders(context.Response);
                            context.Response.ContentType = "application/json";
                            byte[] buffer = Encoding.UTF8.GetBytes(respJson);
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            context.Response.OutputStream.Close();
                            Console.WriteLine("Response sent.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in ComputeItinerary: {ex.Message}");
                            AddCorsHeaders(context.Response);
                            context.Response.StatusCode = 500;
                            byte[] error = Encoding.UTF8.GetBytes("Internal server error");
                            context.Response.OutputStream.Write(error, 0, error.Length);
                            context.Response.OutputStream.Close();
                        }
                    }
                    else
                    {
                        AddCorsHeaders(context.Response);
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message}");
                    try {
                        AddCorsHeaders(context.Response);
                        context.Response.StatusCode = 500;
                        byte[] error = Encoding.UTF8.GetBytes("Internal server error");
                        context.Response.OutputStream.Write(error, 0, error.Length);
                        context.Response.OutputStream.Close();
                    } catch {}
                }
            }
        }

        private async Task<ItineraryStationsResponse> ComputeItinerary(ItineraryRequest req)
        {
            Console.WriteLine($"Searching itinerary from ({req.originLat}, {req.originLng}) to ({req.destLat}, {req.destLng})");

            JCDecauxAPI Api = new JCDecauxAPI();
            OpenRouteServiceAPI ORSApi = new OpenRouteServiceAPI();

            List<Station> Stations = await Api.GetAllStations();

            GeoCoordinate OriginCoord = new GeoCoordinate(req.originLat, req.originLng);
            GeoCoordinate DestCoord = new GeoCoordinate(req.destLat, req.destLng);

            GeoCoordinate CurrentOriginCoord = OriginCoord;
            Station LastStation = null;

            List<Route> WalkRoutes = new List<Route>();
            List<BikeRoute> BikeRoutes = new List<BikeRoute>();

            Station ClosestLastDestination = Stations
                .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.stands > 0)
                .OrderBy(s => DestCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                .FirstOrDefault();

            HashSet<string> PassedContract = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Location DestinationLocation = new Location
            {
                latitude = DestCoord.Latitude,
                longitude = DestCoord.Longitude
            };
            bool destinationReachedByFoot = false;

            while (LastStation != ClosestLastDestination)
            {
                Console.WriteLine($"Loocking the closest station from ({CurrentOriginCoord.Latitude}, {CurrentOriginCoord.Longitude})");
                Station ClosestOriginStation = Stations
                    .Where(s =>
                        s.totalStands != null &&
                        s.totalStands.availabilities != null &&
                        s.totalStands.availabilities.bikes > 0 &&
                        (LastStation == null || LastStation.contractName != s.contractName) &&
                        !PassedContract.Contains(s.contractName))
                    .OrderBy(s => CurrentOriginCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                    .FirstOrDefault();

                if (ClosestOriginStation == null)
                {
                    Console.WriteLine("No more origin stations available. Breaking loop.");
                    break;
                }

                Console.WriteLine($"Origin station is at ({ClosestOriginStation.position.latitude}, {ClosestOriginStation.position.longitude}) in the contract {ClosestOriginStation.contractName}");

                Console.WriteLine($"Checking if quicker to go by foot to the next station without using the last bike route");

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
                    latitude = CurrentOriginCoord.Latitude,
                    longitude = CurrentOriginCoord.Longitude
                };
                Location PickupLocation = new Location
                {
                    latitude = ClosestOriginStation.position.latitude,
                    longitude = ClosestOriginStation.position.longitude
                };
                Location DropoffLocation = new Location
                {
                    latitude = ClosestDestinationStation.position.latitude,
                    longitude = ClosestDestinationStation.position.longitude
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
                        start = StartLocation,
                        end = DestinationLocation,
                        feature = WalkDirectRoute
                    });
                    destinationReachedByFoot = true;
                    CurrentOriginCoord = DestCoord;
                    LastStation = ClosestLastDestination;
                    break;
                }

                WalkRoutes.Add( 
                    new Route
                    {
                        start = StartLocation,
                        end = PickupLocation,
                        feature = ToPickupRoute
                    }
                );
                BikeRoutes.Add( 
                    new BikeRoute
                    {
                        start = PickupLocation,
                        end = DropoffLocation,
                        feature = BikeRoute,
                        addressStart = ClosestOriginStation.address,
                        availableBikes = ClosestOriginStation.totalStands.availabilities.bikes,
                        addressEnd = ClosestDestinationStation.address,
                        availableDropPlace = ClosestDestinationStation.totalStands.availabilities.stands
                    }
                );

                CurrentOriginCoord = new GeoCoordinate(DropoffLocation.latitude, DropoffLocation.longitude);
                LastStation = ClosestDestinationStation;
                PassedContract.Add(ClosestOriginStation.contractName);
            }

            if (!destinationReachedByFoot)
            {
                // Add the last walking part
                Location LastStartLocation = new Location
                {
                    latitude = CurrentOriginCoord.Latitude,
                    longitude = CurrentOriginCoord.Longitude
                };

                RouteFeature LastWalkRoute = await ORSApi.getRoute(
                    LastStartLocation,
                    DestinationLocation,
                    "foot-walking"
                );

                WalkRoutes.Add(
                    new Route
                    {
                        start = LastStartLocation,
                        end = DestinationLocation,
                        feature = LastWalkRoute
                    });
            }


            return new ItineraryStationsResponse
            {
                walkRoutes = WalkRoutes,
                bikeRoutes = BikeRoutes,
            };

            GeoCoordinate originCoord = new GeoCoordinate(req.originLat, req.originLng);
            Station closestOriginStation = Stations
                .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.bikes > 0)
                .OrderBy(s => originCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                .FirstOrDefault();

            GeoCoordinate destCoord = new GeoCoordinate(req.destLat, req.destLng);
            Station closestDestStation = Stations
                .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.stands > 0)
                .OrderBy(s => destCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                .FirstOrDefault();

            if (closestOriginStation == null || closestDestStation == null)
            {
                throw new Exception("No available station found for origin or destination.");
            }

            Location startLocation = new Location
            {
                latitude = req.originLat,
                longitude = req.originLng
            };

            Location pickupLocation = new Location
            {
                latitude = closestOriginStation.position.latitude,
                longitude = closestOriginStation.position.longitude
            };

            Location dropoffLocation = new Location
            {
                latitude = closestDestStation.position.latitude,
                longitude = closestDestStation.position.longitude
            };

            Location endLocation = new Location
            {
                latitude = req.destLat,
                longitude = req.destLng
            };

            RouteFeature toPickupRoute = await ORSApi.getRoute(
                startLocation,
                pickupLocation, 
                "foot-walking"
            );

            RouteFeature bikeRoute = await ORSApi.getRoute(
                pickupLocation,
                dropoffLocation,
                "cycling-regular"
            );

            RouteFeature toDestinationRoute = await ORSApi.getRoute(
                dropoffLocation,
                endLocation,
                "foot-walking"
            );

            // Check the time
            double fullTime = toPickupRoute.Properties.Summary.Duration + bikeRoute.Properties.Summary.Duration + toDestinationRoute.Properties.Summary.Duration;

            // Get only foot
            RouteFeature onlyFootRoute = await ORSApi.getRoute(
                startLocation,
                endLocation,
                "foot-walking"
            );

            if (fullTime <= onlyFootRoute.Properties.Summary.Duration)
            {
                return new ItineraryStationsResponse
                    {
                        walkRoutes = new List<Route>
                    {
                        new Route
                        {
                            start = startLocation,
                            end = pickupLocation,
                            feature = toPickupRoute
                        },
                        new Route
                        {
                            start = dropoffLocation,
                            end = endLocation,
                            feature = toDestinationRoute
                        }
                    },
                        bikeRoutes = new List<BikeRoute>
                    {
                        new BikeRoute
                        {
                            start = pickupLocation,
                            end = dropoffLocation,
                            feature = bikeRoute,
                            addressStart = closestOriginStation.address,
                            availableBikes = closestOriginStation.totalStands.availabilities.bikes,
                            addressEnd = closestDestStation.address,
                            availableDropPlace = closestDestStation.totalStands.availabilities.stands
                        },
                    }
                };
            } else
            {
                return new ItineraryStationsResponse
                {
                    walkRoutes = new List<Route>
                    {
                        new Route
                        {
                            start = startLocation,
                            end = endLocation,
                            feature = onlyFootRoute,
                        }
                    }
                };
            }

        }

        private bool IsNearCity(double lat, double lng, string city)
        {
            // TODO: Implement city geocoding or use a static map
            // For demo, always return true
            return true;
        }
    }
}
