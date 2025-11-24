using Newtonsoft.Json;
using Server.Entities;
using Server.Entities.ORS;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            APIResponse response = await client.CallPostAsync($"{_openRouteServiceBaseUrl}/{profile}/geojson?api_key={_openRouteServiceApiKey}", JsonConvert.SerializeObject(body));
            if (response.Status != 200)
            {
                return null;
            }

            FeatureCollection route = JsonConvert.DeserializeObject<FeatureCollection>(response.Response);

            return route.Features[0];
        }
    }
}
