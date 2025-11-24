using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class DirectionBody
    {
        [DataMember(Name = "coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }
    }
}
