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
            JCDecauxAPI api = new JCDecauxAPI();
            string contract = "lyon";
            List<Station> stations = await api.GetStations(contract);

            GeoCoordinate originCoord = new GeoCoordinate(req.originLat, req.originLng);
            Station closestOriginStation = stations
                .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.bikes > 0)
                .OrderBy(s => originCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                .FirstOrDefault();

            GeoCoordinate destCoord = new GeoCoordinate(req.destLat, req.destLng);
            Station closestDestStation = stations
                .Where(s => s.totalStands != null && s.totalStands.availabilities != null && s.totalStands.availabilities.stands > 0)
                .OrderBy(s => destCoord.GetDistanceTo(new GeoCoordinate(s.position.latitude, s.position.longitude)))
                .FirstOrDefault();

            if (closestOriginStation == null || closestDestStation == null)
            {
                throw new Exception("No available station found for origin or destination.");
            }

            OpenRouteServiceAPI ORSApi = new OpenRouteServiceAPI();

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
