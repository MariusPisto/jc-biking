using Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class ItineraryResponse
    {
        public string originContract { get; set; }
        public string destContract { get; set; }
        public Station originStation { get; set; }
        public Station destStation { get; set; }
        public double walkingDistance { get; set; }
        public double bikeDistance { get; set; }
        public List<string> instructions { get; set; }
    }
}
