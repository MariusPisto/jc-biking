using System.Runtime.Serialization;

namespace Server.Entities.Response
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public double Lon { get; set; }
    }
}
