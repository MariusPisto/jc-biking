using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class Query
    {
        [DataMember(Name = "coordinates")]
        public List<List<double>> Coordinates { get; set; }

        [DataMember(Name = "profile")]
        public string Profile { get; set; }

        [DataMember(Name = "profileName")]
        public string ProfileName { get; set; }

        [DataMember(Name = "format")]
        public string Format { get; set; }

        [DataMember(Name = "language")]
        public string Language { get; set; }
    }
}
