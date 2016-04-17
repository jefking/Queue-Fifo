namespace GenerateMessages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
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

        public static async Task Send(string name, string connection, IEnumerable<BrokeredMessage> samples)
        {
            var sender = new BusTopicSender("ctorder", connection);

            foreach (var s in samples)
            {
                Trace.TraceInformation("Sending: {0}", s.Properties["DeviceId"]);

                await sender.Send(s);
            }
        }

        public static IEnumerable<BrokeredMessage> Create()
        {
            var random = new Random();
            var samples = new List<BrokeredMessage>();

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

                    var msg = new BrokeredMessage(s);

                    msg.Properties["DeviceId"] = device.ToString();

                    samples.Add(msg);
                }
            }

            return samples;
        }
    }
}