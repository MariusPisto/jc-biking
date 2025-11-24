using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.ORS
{
    [DataContract]
    public class Metadata
    {
        [DataMember(Name = "attribution")]
        public string Attribution { get; set; }

        [DataMember(Name = "service")]
        public string Service { get; set; }

        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DataMember(Name = "query")]
        public Query Query { get; set; }

        [DataMember(Name = "engine")]
        public Engine Engine { get; set; }
    }
}
