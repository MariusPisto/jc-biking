using Newtonsoft.Json;
using System.Collections.Generic;

namespace Server.Entities.Dott
{
    public class DottStationResponse
    {
        public DottStationData data { get; set; }
    }

    public class DottStationData
    {
        public List<DottStation> stations { get; set; }
    }

    public class DottStation
    {
        public string station_id { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public DottVehicleCapacity vehicle_capacity { get; set; }
    }

    public class DottVehicleCapacity
    {
        public int dott_bicycle { get; set; }
        public int dott_scooter { get; set; }
    }
}
