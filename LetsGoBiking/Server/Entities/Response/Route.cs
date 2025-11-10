using Server.Entities.ORS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Response
{
    public class Route
    {
        public virtual string type { get; } = "simple";
        public int position { get; set; }
        public Location start { get; set; }
        public Location end { get; set; }
        public RouteFeature feature { get; set; }
    }
}
