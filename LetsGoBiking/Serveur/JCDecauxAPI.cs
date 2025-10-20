using Newtonsoft.Json;
using Serveur.ProxyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    internal class JCDecauxAPI

    {
        private readonly string _jcdecauxApiKey;
        private readonly string _jcdecauxBaseUrl = "https://api.jcdecaux.com/vls/v3";

        public JCDecauxAPI()
        {
            _jcdecauxApiKey = LoadApiKey();
        }

        private string LoadApiKey()
        {
            var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            if (File.Exists(envPath))
            {
                var lines = File.ReadAllLines(envPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("JCDECAUX_API_KEY="))
                    {
                        return line.Substring("JCDECAUX_API_KEY=".Length).Trim();
                    }
                }
            }
            throw new Exception("JCDECAUX_API_KEY not found in .env file");
        }

        public async Task<List<Contact>> GetContracts()
        {
            using (ProxyServiceClient client = new ProxyServiceClient())
            {
                APIResponse response = await client.CallAsync($"{_jcdecauxBaseUrl}/contracts?apiKey={_jcdecauxApiKey}");
                if (response.Status != 200)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<List<Contact>>(response.Response);
            }
        }

        public async Task<List<Station>> GetStations(string contract)
        {
            using (ProxyServiceClient client = new ProxyServiceClient())
            {
                APIResponse response = await client.CallAsync($"{_jcdecauxBaseUrl}/stations?contract={contract}&apiKey={_jcdecauxApiKey}");
                if (response.Status != 200)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<List<Station>>(response.Response);
            }
        }
    }
}
