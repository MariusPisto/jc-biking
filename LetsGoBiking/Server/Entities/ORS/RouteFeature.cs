using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class RouteFeature
    {
        [DataMember(Name = "bbox")]
        public List<double> Bbox { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "properties")]
        public Properties Properties { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }
    }
}
