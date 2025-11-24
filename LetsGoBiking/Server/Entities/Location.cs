using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }
        
        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }
    }
}
