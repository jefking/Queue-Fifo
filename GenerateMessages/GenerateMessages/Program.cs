namespace GenerateMessages
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Models;

    public class Program
    {
        static void Main(string[] args)
        {
            var name = "ctorder";
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var init = new BusTopic(name, connection);
            init.CreateIfNotExists().Wait();

            var samples = new List<Sample>();

            for (var i = 0; i < 100; i++)
            {
                var s = new Sample()
                {
                    DeviceId = Guid.NewGuid(),
                    OccurredOn = DateTime.UtcNow,
                };

                samples.Add(s);
            }

            Send(name, connection, samples).Wait();
        }

        public static async Task Send(string name, string connection, IEnumerable<Sample> samples)
        {
            var sender = new BusTopicSender("ctorder", connection);

            foreach (var s in samples)
            {
                Trace.TraceInformation("Sending: {0}", s.ToString());

                await sender.Send(s);
            }
        }
    }
}