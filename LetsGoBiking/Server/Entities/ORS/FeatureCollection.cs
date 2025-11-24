using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class FeatureCollection
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "bbox")]
        public List<double> Bbox { get; set; }

        [DataMember(Name = "features")]
        public List<RouteFeature> Features { get; set; }

        [DataMember(Name = "metadata")]
        public Metadata Metadata { get; set; }
    }
}
