using Server.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Response
{
    [DataContract]
    public class ItineraryResponse
    {
        [DataMember(Name = "walkRoutes")]
        public List<Route> WalkRoutes { get; set; }
        
        [DataMember(Name = "bikeRoutes")]
        public List<BikeRoute> BikeRoutes { get; set; }
    }
}
