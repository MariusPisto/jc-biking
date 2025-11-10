using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class ItineraryRequest
    {
        public double originLat { get; set; }
        public double originLng { get; set; }
        public double destLat { get; set; }
        public double destLng { get; set; }
    }
}
