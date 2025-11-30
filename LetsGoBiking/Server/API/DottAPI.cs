using Newtonsoft.Json;
using Server.Entities;
using Server.Entities.Dott;
using Server.Entities.JCDecaux;
using Server.ProxyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.API
{
    internal class DottAPI : BaseAPI
    {
        private readonly string _dottGbfsUrl = "https://gbfs.api.ridedott.com/public/v2/countries/fr/gbfs.json";
        private readonly string _dottGbfsUrlDe = "https://gbfs.api.ridedott.com/public/v2/countries/de/gbfs.json";

        public async Task<List<Station>> GetAllStations()
        {
            var allStations = new List<Station>();
            
            await FetchStationsForUrl(_dottGbfsUrl, allStations);
            await FetchStationsForUrl(_dottGbfsUrlDe, allStations);

            return allStations;
        }

        private async Task FetchStationsForUrl(string feedUrl, List<Station> allStations)
        {
            APIResponse feedResponse = await client.CallGetAsync(feedUrl);
            if (feedResponse.Status != 200) return;

            var feedData = JsonConvert.DeserializeObject<DottFeedResponse>(feedResponse.Response);
            if (feedData?.data?.en?.feeds == null) return;

            var stationInfoUrls = feedData.data.en.feeds
                .Where(f => f.name == "station_information")
                .Select(f => f.url)
                .ToList();

            foreach (var url in stationInfoUrls)
            {
                try 
                {
                    var parts = url.Split('/');
                    var contractName = parts.Length > 2 ? parts[parts.Length - 2] + "_dott" : "Dott";

                    APIResponse stationResponse = await client.CallGetAsync(url);
                    if (stationResponse.Status != 200) continue;

                    var stationData = JsonConvert.DeserializeObject<DottStationResponse>(stationResponse.Response);
                    if (stationData?.data?.stations == null) continue;

                    var stations = stationData.data.stations.Select(ds => new Station
                    {
                        number = 0,
                        contractName = contractName,
                        name = ds.name,
                        address = ds.name,
                        position = new Position { latitude = ds.lat, longitude = ds.lon },
                        banking = false,
                        bonus = false,
                        status = "OPEN",
                        connected = true,
                        totalStands = new Stand
                        {
                            capacity = 10,
                            availabilities = new Availabilities
                            {
                                bikes = (ds.vehicle_capacity?.dott_bicycle ?? 0) + (ds.vehicle_capacity?.dott_scooter ?? 0),
                                stands = 10
                            }
                        }
                    }).ToList();

                    allStations.AddRange(stations);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching Dott stations from {url}: {ex.Message}");
                }
            }
        }
    }
}
