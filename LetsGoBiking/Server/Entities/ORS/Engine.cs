using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class Engine
    {
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "build_date")]
        public string BuildDate { get; set; }

        [DataMember(Name = "graph_date")]
        public string GraphDate { get; set; }

        [DataMember(Name = "osm_date")]
        public string OsmDate { get; set; }
    }
}
