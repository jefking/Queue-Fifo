namespace GenerateMessages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Models;

    public class Program
    {
        static void Main(string[] args)
        {
            var name = "ctorder";
            var connection = Config.Connection;
            var init = new BusTopic(name, connection);
            init.CreateIfNotExists().Wait();

            var samples = Create();

            Send(name, connection, samples).Wait();
        }

        public static async Task Send(string name, string connection, IEnumerable<Sample> samples)
        {
            var sender = new BusTopicSender("ctorder", connection);

            foreach (var s in samples)
            {
                Trace.TraceInformation("Sending: {0} - {1}", s.DeviceId, s.OccurredOn);

                await sender.Send(s);
            }
        }

        public static IEnumerable<Sample> Create()
        {
            var random = new Random();
            var samples = new List<Sample>();

            foreach (var device in Config.Devices)
            {
                for (var i = 0; i < 10; i++)
                {
                    var v = random.Next(0, 10);

                    var s = new Sample()
                    {
                        OccurredOn = DateTime.Now.AddSeconds(v),
                        DeviceId = device,
                    };

                    samples.Add(s);
                }
            }

            return samples;
        }
    }
}