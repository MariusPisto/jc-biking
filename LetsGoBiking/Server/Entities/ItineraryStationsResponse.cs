using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class ItineraryStationsResponse
    {
        public LocationInfo start { get; set; }
        public StationInfo pickup { get; set; }
        public StationInfo dropoff { get; set; }
        public LocationInfo end { get; set; }
    }
}
