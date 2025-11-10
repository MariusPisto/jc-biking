using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class Engine
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("build_date")]
        public string BuildDate { get; set; }

        [JsonPropertyName("graph_date")]
        public string GraphDate { get; set; }

        [JsonPropertyName("osm_date")]
        public string OsmDate { get; set; }
    }
}
