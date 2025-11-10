using Server.Entities.ORS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.API
{
    internal class OpenRouteServiceAPI : BaseAPI
    {
        private readonly string _openRouteServiceApiKey;
        private readonly string _openRouteServiceBaseUrl = "https://api.openrouteservice.org/v2/directions/";

        public OpenRouteServiceAPI() {
            _openRouteServiceApiKey = LoadApiKey("ORS_API_KEY");
        }
    }
}
