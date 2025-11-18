using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class Properties
    {
        [DataMember(Name = "segments")]
        public List<Segment> Segments { get; set; }

        [DataMember(Name = "way_points")]
        public List<int> WayPoints { get; set; }

        [DataMember(Name = "summary")]
        public Summary Summary { get; set; }
    }
}
