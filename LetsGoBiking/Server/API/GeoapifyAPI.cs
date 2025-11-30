using Newtonsoft.Json;
using Server.Entities.Geoapify;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.API
{
    public class GeoapifyAPI : BaseAPI
    {
        private readonly string _apiKey;

        public GeoapifyAPI()
        {
            _apiKey = LoadApiKey("GEOAPIFY_API_KEY");
        }

        public async Task<List<Feature>> GetFeatures(string text)
        {
            try
            {
                string url = $"https://api.geoapify.com/v1/geocode/autocomplete?text={System.Uri.EscapeDataString(text)}&apiKey={_apiKey}";
                Console.WriteLine($"[GeoapifyAPI] Calling proxy for: {text}");
                
                APIResponse response = await client.CallGetAsync(url).ConfigureAwait(false);

                if (response == null)
                {
                    Console.WriteLine($"[GeoapifyAPI] Proxy returned null response for: {text}");
                    return new List<Feature>();
                }

                if (response.Status != 200)
                {
                    Console.WriteLine($"[GeoapifyAPI] Proxy returned status {response.Status} for: {text}");
                    return new List<Feature>();
                }

                var collection = JsonConvert.DeserializeObject<FeatureCollection>(response.Response);
                Console.WriteLine($"[GeoapifyAPI] Successfully retrieved {collection?.Features?.Count ?? 0} features for: {text}");
                return collection?.Features ?? new List<Feature>();
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[GeoapifyAPI] Timeout exception for: {text}");
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Feature>();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"[GeoapifyAPI] Communication exception for: {text}");
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Feature>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GeoapifyAPI] Unexpected error for: {text}");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return new List<Feature>();
            }
        }
    }
}
