using Newtonsoft.Json;
using System.Collections.Generic;

namespace Server.Entities.Geoapify
{
    public class FeatureCollection
    {
        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }

    public class Feature
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class Properties
    {
        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }
    }
}
