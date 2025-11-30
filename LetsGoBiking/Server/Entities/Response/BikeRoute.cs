using Server.Entities.ORS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Response
{
    [DataContract]
    public class BikeRoute : Route
    {
        [DataMember(Name = "type")]
        public override string Type { get; set; } = "bike";
        
        [DataMember(Name = "addressStart")]
        public string AddressStart { get; set; }
        
        [DataMember(Name = "availableBikes")]
        public int AvailableBikes { get; set; }
        
        [DataMember(Name = "addressEnd")]
        public string AddressEnd { get; set; }
        
        [DataMember(Name = "availableDropPlace")]
        public int AvailableDropPlace { get; set; }

        [DataMember(Name = "vehicleType")]
        public string VehicleType { get; set; }
    }
}
