namespace GenerateMessages
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;

    public class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var init = new BusTopic("ctorder", connection);
            init.CreateIfNotExists().Wait();

            var sender = new BusTopicSender("ctorder", connection);

            var tasks = new List<Task>();

            for (var i = 0; i < 100; i++)
            {
                var s = new Sample()
                {
                    DeviceId = Guid.NewGuid(),
                    OccurredOn = DateTime.UtcNow,
                };

                tasks.Add(sender.Send(s));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}