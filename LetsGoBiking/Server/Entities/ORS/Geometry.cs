using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class Geometry
    {
        [DataMember(Name = "coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
