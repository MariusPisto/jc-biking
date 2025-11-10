using System;
using System.Collections.Generic;
using System.Threading;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json; 

namespace FakeNotificationService
{
    public class Notification
    {
        public string Type { get; set; } 
        public string Level { get; set; } 
        public string Message { get; set; }
    }

    internal class Program
    {
        // Listes de messages prédéfinis
        private static readonly List<string> WeatherMessages = new List<string>
        {
            "Averse soudaine attendue dans 10 min.",
            "Vents forts (60km/h) détectés secteur Ouest.",
            "Alerte canicule : Température ressentie 35°C."
        };

        private static readonly List<string> PollutionMessages = new List<string>
        {
            "Pic de pollution - Qualité de l'air : Mauvaise.",
            "Qualité de l'air : Modérée. Activité sportive déconseillée.",
            "Alerte particules fines niveau 3."
        };

        static void Main(string[] args)
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();
            ISession session = connection.CreateSession();

            IDestination destination = session.GetTopic("project-notifications");

            IMessageProducer producer = session.CreateProducer(destination);
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            var random = new Random();
            Console.WriteLine("--- Service de notification démarré ---");
            Console.WriteLine("Envoi de notifications sur le Topic 'project-notifications'.");
            Console.WriteLine("Appuyez sur une touche pour arrêter...");

            while (!Console.KeyAvailable)
            {
                Notification notif;
                string notifType;

                if (random.Next(2) == 0)
                {
                    notifType = "weather";
                    notif = new Notification
                    {
                        Type = "meteo",
                        Level = "warning",
                        Message = WeatherMessages[random.Next(WeatherMessages.Count)]
                    };
                }
                else
                {
                    notifType = "pollution";
                    notif = new Notification
                    {
                        Type = "pollution",
                        Level = "high", 
                        Message = PollutionMessages[random.Next(PollutionMessages.Count)]
                    };
                }

                string jsonMessage = JsonConvert.SerializeObject(notif);
                ITextMessage message = session.CreateTextMessage(jsonMessage);

                message.Properties["notificationType"] = notifType;

                producer.Send(message);
                Console.WriteLine($"[ENVOYÉ] {jsonMessage}");

                Thread.Sleep(random.Next(5000, 10000));
            }

            Console.WriteLine("Arrêt du service...");
            session.Close();
            connection.Close();
        }
    }
}