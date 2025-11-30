using Newtonsoft.Json;
using Server.Entities;
using Server.Entities.JCDecaux;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
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
            try
            {
                Console.WriteLine("[JCDecauxAPI] Getting contracts...");
                APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/contracts?apiKey={_jcdecauxApiKey}").ConfigureAwait(false);
                
                if (response == null || response.Status != 200)
                {
                    Console.WriteLine($"[JCDecauxAPI] Failed to get contracts. Status: {response?.Status ?? -1}");
                    return null;
                }
                
                var contracts = JsonConvert
                    .DeserializeObject<List<Contract>>(response.Response)
                    ?.Where(c => !string.Equals(c.name, "jcdecauxbike", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Console.WriteLine($"[JCDecauxAPI] Retrieved {contracts?.Count ?? 0} contracts");
                return contracts;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Timeout getting contracts: {ex.Message}");
                return null;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Communication error getting contracts: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Error getting contracts: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }

        public async Task<List<Station>> GetStations(string contract)
        {
            try
            {
                Console.WriteLine($"[JCDecauxAPI] Getting stations for contract: {contract}");
                APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/stations?contract={contract}&apiKey={_jcdecauxApiKey}").ConfigureAwait(false);
                
                if (response == null || response.Status != 200)
                {
                    Console.WriteLine($"[JCDecauxAPI] Failed to get stations for {contract}. Status: {response?.Status ?? -1}");
                    return null;
                }
                
                var stations = JsonConvert
                    .DeserializeObject<List<Station>>(response.Response)
                    ?.Where(s =>
                        !string.Equals(s.contractName, "jcdecauxbike", StringComparison.OrdinalIgnoreCase) &&
                        !ContainsTest(s.contractName) &&
                        !ContainsTest(s.name))
                    .ToList();
                Console.WriteLine($"[JCDecauxAPI] Retrieved {stations?.Count ?? 0} stations for {contract}");
                return stations;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Timeout getting stations for {contract}: {ex.Message}");
                return null;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Communication error getting stations for {contract}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Error getting stations for {contract}: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }

        public async Task<List<Station>> GetAllStations()
        {
            try
            {
                Console.WriteLine("[JCDecauxAPI] Getting all stations...");
                APIResponse response = await client.CallGetAsync($"{_jcdecauxBaseUrl}/stations?apiKey={_jcdecauxApiKey}").ConfigureAwait(false);

                if (response == null || response.Status != 200)
                {
                    Console.WriteLine($"[JCDecauxAPI] Failed to get all stations. Status: {response?.Status ?? -1}");
                    return null;
                }

                var stations = JsonConvert
                    .DeserializeObject<List<Station>>(response.Response)
                    ?.Where(s =>
                        !string.Equals(s.contractName, "jcdecauxbike", StringComparison.OrdinalIgnoreCase) &&
                        !ContainsTest(s.contractName) &&
                        !ContainsTest(s.name))
                    .ToList();
                Console.WriteLine($"[JCDecauxAPI] Retrieved {stations?.Count ?? 0} total stations");
                return stations;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Timeout getting all stations: {ex.Message}");
                return null;
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Communication error getting all stations: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[JCDecauxAPI] Error getting all stations: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }

        private bool ContainsTest(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   value.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
