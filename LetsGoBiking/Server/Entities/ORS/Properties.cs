using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class Properties
    {
        [JsonPropertyName("segments")]
        public List<Segment> Segments { get; set; }

        [JsonPropertyName("way_points")]
        public List<int> WayPoints { get; set; }

        [JsonPropertyName("summary")]
        public Summary Summary { get; set; }
    }
}
