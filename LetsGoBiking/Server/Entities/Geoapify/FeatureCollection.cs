using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Entities.Geoapify
{
    [DataContract]
    public class FeatureCollection
    {
        [DataMember]
        public List<Feature> Features { get; set; }
    }

    [DataContract]
    public class Feature
    {
        [DataMember]
        public Properties Properties { get; set; }

        [DataMember]
        public Geometry Geometry { get; set; }
    }

    [DataContract]
    public class Properties
    {
        [DataMember]
        public string Formatted { get; set; }
    }

    [DataContract]
    public class Geometry
    {
        [DataMember]
        public List<double> Coordinates { get; set; }
    }
}
