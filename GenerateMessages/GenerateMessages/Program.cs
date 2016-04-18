namespace GenerateMessages
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class Program
    {
        static void Main(string[] args)
        {
            var name = "ctorder";
            var config = new Config();
            var connection = config.Connection;
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
            var config = new Config();

            foreach (var device in config.Devices)
            {
                for (var i = 0; i < 10; i++)
                {
                    var v = random.Next(0, 60);

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