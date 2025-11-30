using Newtonsoft.Json;
using Server.Entities;
using Server.Entities.ORS;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.API
{
    internal class OpenRouteServiceAPI : BaseAPI
    {
        private readonly string _openRouteServiceApiKey;
        private readonly string _openRouteServiceBaseUrl = "https://api.openrouteservice.org/v2/directions";

        public OpenRouteServiceAPI() {
            _openRouteServiceApiKey = LoadApiKey("ORS_API_KEY");
        }

        public async Task<RouteFeature> getRoute(Location start, Location end, string profile)
        {
            try
            {
                List<List<double>> coordinatesList = new List<List<double>>
                {
                    new List<double> { start.Longitude, start.Latitude },
                    new List<double> { end.Longitude, end.Latitude }
                };

                DirectionBody body = new DirectionBody
                {
                    Coordinates = coordinatesList,
                    Language = "fr"
                };

                Console.WriteLine($"[OpenRouteServiceAPI] Getting route ({profile}) from ({start.Latitude}, {start.Longitude}) to ({end.Latitude}, {end.Longitude})");
                APIResponse response = await client.CallPostAsync($"{_openRouteServiceBaseUrl}/{profile}/geojson?api_key={_openRouteServiceApiKey}", JsonConvert.SerializeObject(body)).ConfigureAwait(false);
                
                if (response == null || response.Status != 200)
                {
                    Console.WriteLine($"[OpenRouteServiceAPI] Failed to get route. Status: {response?.Status ?? -1}");
                    return null;
                }

                FeatureCollection route = JsonConvert.DeserializeObject<FeatureCollection>(response.Response);
                Console.WriteLine($"[OpenRouteServiceAPI] Successfully retrieved route");
                return route?.Features?[0];
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[OpenRouteServiceAPI] Timeout getting route: {ex.Message}");
                return null;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"[OpenRouteServiceAPI] Communication error getting route: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenRouteServiceAPI] Error getting route: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }
    }
}
