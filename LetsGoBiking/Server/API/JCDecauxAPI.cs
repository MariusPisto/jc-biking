using Newtonsoft.Json;
using Server.Entities;
using Server.Entities.JCDecaux;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.API
{
    internal class JCDecauxAPI : BaseAPI
    {
        private readonly string _jcdecauxApiKey;
        private readonly string _jcdecauxBaseUrl = "https://api.jcdecaux.com/vls/v3";

        public JCDecauxAPI()
        {
            _jcdecauxApiKey = LoadApiKey("JCDECAUX_API_KEY");
        }

        public async Task<List<Contract>> GetContracts()
        {
            APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/contracts?apiKey={_jcdecauxApiKey}");
            if (response.Status != 200)
            {
                return null;
            }
            return JsonConvert
                .DeserializeObject<List<Contract>>(response.Response)
                ?.Where(c => !string.Equals(c.name, "jcdecauxbike", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Station>> GetStations(string contract)
        {
            APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/stations?contract={contract}&apiKey={_jcdecauxApiKey}");
            if (response.Status != 200)
            {
                return null;
            }
            return JsonConvert
                .DeserializeObject<List<Station>>(response.Response)
                ?.Where(s =>
                    !string.Equals(s.contractName, "jcdecauxbike", StringComparison.OrdinalIgnoreCase) &&
                    !ContainsTest(s.contractName) &&
                    !ContainsTest(s.name))
                .ToList();
        }

        public async Task<List<Station>> GetAllStations()
        {
            APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/stations?apiKey={_jcdecauxApiKey}");

            return JsonConvert
                .DeserializeObject<List<Station>>(response.Response)
                ?.Where(s =>
                    !string.Equals(s.contractName, "jcdecauxbike", StringComparison.OrdinalIgnoreCase) &&
                    !ContainsTest(s.contractName) &&
                    !ContainsTest(s.name))
                .ToList();
        }

        private bool ContainsTest(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
