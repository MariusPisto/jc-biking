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

namespace Serveur
{
    public class Contact
    {
        public string name { get; set; }
        public string commercial_name { get; set; }
        public List<string> cities { get; set; }
        public string country_code { get; set; }
    }

    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }
    }

    public class Stands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class Station
    {
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public string lastUpdate { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public object shape { get; set; }
        public Stands totalStands { get; set; }
        public Stands mainStands { get; set; }
        public Stands overflowStands { get; set; }
    }

    public class ItineraryRequest
    {
        public double originLat { get; set; }
        public double originLng { get; set; }
        public double destLat { get; set; }
        public double destLng { get; set; }
    }

    public class ItineraryResponse
    {
        public string originContract { get; set; }
        public string destContract { get; set; }
        public Station originStation { get; set; }
        public Station destStation { get; set; }
        public double walkingDistance { get; set; }
        public double bikeDistance { get; set; }
        public List<string> instructions { get; set; }
    }

    public class GeoCoordinate
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double GetDistanceTo(GeoCoordinate other)
        {
            // Haversine formula
            double R = 6371000; // meters
            double lat1 = Latitude * Math.PI / 180;
            double lat2 = other.Latitude * Math.PI / 180;
            double dLat = (other.Latitude - Latitude) * Math.PI / 180;
            double dLon = (other.Longitude - Longitude) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }

    public class ItineraryStationsResponse
    {
        public LocationInfo start { get; set; }
        public StationInfo pickup { get; set; }
        public StationInfo dropoff { get; set; }
        public LocationInfo end { get; set; }
    }

    public class LocationInfo
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class StationInfo
    {
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int availableBikes { get; set; }
        public int availableDropPlace { get; set; }
    }

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

        private async Task HandleRequests()
        {
            while (true)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                Console.WriteLine($"Received {context.Request.HttpMethod} {context.Request.Url.AbsolutePath}");
                try
                {
                    if (context.Request.HttpMethod == "GET" && context.Request.Url.AbsolutePath.TrimEnd('/') == "/itinerary")
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
                                // contractName retiré
                            };
                            Console.WriteLine($"Request params: origin=({req.originLat},{req.originLng}), dest=({req.destLat},{req.destLng})");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Parameter parsing error: {ex.Message}");
                            context.Response.StatusCode = 400; // Bad Request
                            byte[] error = Encoding.UTF8.GetBytes($"Invalid or missing query parameters: {ex.Message}");
                            context.Response.OutputStream.Write(error, 0, error.Length);
                            context.Response.OutputStream.Close();
                            continue;
                        }

                        try
                        {
                            ItineraryStationsResponse resp = await ComputeItinerary(req);
                            string respJson = JsonConvert.SerializeObject(resp);
                            context.Response.ContentType = "application/json";
                            byte[] buffer = Encoding.UTF8.GetBytes(respJson);
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            context.Response.OutputStream.Close();
                            Console.WriteLine("Response sent.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in ComputeItinerary: {ex.Message}");
                            context.Response.StatusCode = 500;
                            byte[] error = Encoding.UTF8.GetBytes("Internal server error");
                            context.Response.OutputStream.Write(error, 0, error.Length);
                            context.Response.OutputStream.Close();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message}");
                    try
                    {
                        context.Response.StatusCode = 500;
                        byte[] error = Encoding.UTF8.GetBytes("Internal server error");
                        context.Response.OutputStream.Write(error, 0, error.Length);
                        context.Response.OutputStream.Close();
                    }
                    catch { }
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

            return new ItineraryStationsResponse
            {
                start = new LocationInfo
                {
                    latitude = req.originLat,
                    longitude = req.originLng
                },
                pickup = new StationInfo
                {
                    address = closestOriginStation.address,
                    latitude = closestOriginStation.position.latitude,
                    longitude = closestOriginStation.position.longitude,
                    availableBikes = closestOriginStation.totalStands.availabilities.bikes,
                    availableDropPlace = closestOriginStation.totalStands.availabilities.stands
                },
                dropoff = new StationInfo
                {
                    address = closestDestStation.address,
                    latitude = closestDestStation.position.latitude,
                    longitude = closestDestStation.position.longitude,
                    availableBikes = closestDestStation.totalStands.availabilities.bikes,
                    availableDropPlace = closestDestStation.totalStands.availabilities.stands
                },
                end = new LocationInfo
                {
                    latitude = req.destLat,
                    longitude = req.destLng
                }
            };
        }

        private bool IsNearCity(double lat, double lng, string city)
        {
            // TODO: Implement city geocoding or use a static map
            // For demo, always return true
            return true;
        }
    }
}
