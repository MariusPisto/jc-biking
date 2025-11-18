using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri httpUrl = new Uri("http://localhost:8733");
            ServiceHost host = new ServiceHost(typeof(ServerService), httpUrl);

            // Soap endpoint
            host.AddServiceEndpoint(typeof(IServerService), new BasicHttpBinding(), "soap");

            // Rest endpoint
            ServiceEndpoint RestEndpoint = host.AddServiceEndpoint(typeof(IServerService), new WebHttpBinding(), "api");
            RestEndpoint.Behaviors.Add(new WebHttpBehavior());

            // Enable metadata exchange (for the SOAP endpoint)
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            try
            {
                host.Open();
                Console.WriteLine("Service is hosted at " + httpUrl);
                Console.WriteLine("SOAP endpoint: " + httpUrl + "soap");
                Console.WriteLine("REST endpoint: " + httpUrl + "api");
                Console.WriteLine("Host is running... Press <Enter> key to stop");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting service: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    Console.WriteLine("Inner stack trace: " + ex.InnerException.StackTrace);
                }
                Console.ReadLine();
            }
            finally
            {
                if (host.State == CommunicationState.Opened)
                {
                    host.Close();
                }
            }
        }
    }
}
