using Server.Entities.ORS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Response
{
    public class BikeRoute : Route
    {
        public override string type { get; } = "bike";
        public string addressStart { get; set; }
        public int availableBikes { get; set; }
        public string addressEnd { get; set; }
        public int availableDropPlace { get; set; }
    }
}
