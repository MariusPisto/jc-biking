using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class DirectionBody
    {
        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}
