using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class RouteFeature
    {
        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }
}
