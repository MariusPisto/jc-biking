using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    public class Metadata
    {
        [JsonPropertyName("attribution")]
        public string Attribution { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("query")]
        public Query Query { get; set; }

        [JsonPropertyName("engine")]
        public Engine Engine { get; set; }
    }
}
