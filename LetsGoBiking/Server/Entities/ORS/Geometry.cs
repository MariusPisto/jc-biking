using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
