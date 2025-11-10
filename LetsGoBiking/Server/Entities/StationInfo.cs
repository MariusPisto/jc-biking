using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class StationInfo
    {
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int availableBikes { get; set; }
        public int availableDropPlace { get; set; }
    }
}
