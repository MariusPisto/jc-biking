using Newtonsoft.Json;
using Server.Entities.Geoapify;
using Server.ProxyService;
using System.Collections.Generic;
using System.Linq;
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
            string url = $"https://api.geoapify.com/v1/geocode/autocomplete?text={System.Uri.EscapeDataString(text)}&apiKey={_apiKey}";
            APIResponse response = await client.CallGetAsync(url);

            if (response.Status != 200)
            {
                return new List<Feature>();
            }

            var collection = JsonConvert.DeserializeObject<FeatureCollection>(response.Response);
            return collection?.Features ?? new List<Feature>();
        }
    }
}
