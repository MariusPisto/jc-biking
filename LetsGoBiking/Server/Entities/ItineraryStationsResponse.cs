using Server.Entities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class ItineraryStationsResponse
    {
        public List<Route> walkRoutes { get; set; }
        public List<BikeRoute> bikeRoutes { get; set; }
    }
}
