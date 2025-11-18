using Server.Entities.ORS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Response
{
    [DataContract]
    public class Route
    {
        [DataMember(Name = "type")]
        public virtual string Type { get; set; } = "simple";
        
        [DataMember(Name = "start")]
        public Location Start { get; set; }
        
        [DataMember(Name = "end")]
        public Location End { get; set; }
        
        [DataMember(Name = "feature")]
        public RouteFeature Feature { get; set; }
    }
}
