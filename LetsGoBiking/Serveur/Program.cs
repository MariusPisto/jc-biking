using System;

namespace Serveur
{
    class Program
    {
        static void Main(string[] args)
        {
            RoutingService routingService = new RoutingService("http://localhost:8733/");
            routingService.Start();
            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("REST RoutingService running at http://localhost:8733/");
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
        }
    }
}
