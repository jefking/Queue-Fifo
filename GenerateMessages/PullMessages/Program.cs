namespace PullMessages
{
    using System.Collections.Generic;
    using System.Threading;
    using King.Service.ServiceBus;
    using Models;

    public class Program
    {
        static void Main(string[] args)
        {
            var connection = Config.Connection;
            var topicName = "ctorder";
            var subscriptionName = "bydevice{0}";
            var filter = "DeviceId = '{0}'";

            var init = new BusTopic(topicName, connection);
            init.CreateIfNotExists().Wait();

            foreach (var device in Config.Devices)
            {
                var sname = string.Format(subscriptionName, device.ToString().Split('-')[0]);
                var dfilter = string.Format(filter, device);
                var subscriber = new BusTopicSubscription(topicName, connection, sname, dfilter);
                subscriber.CreateIfNotExists().Wait();
            }

            var events = new List<BusEvents<Sample>>();

            foreach (var device in Config.Devices)
            {
                var sname = string.Format(subscriptionName, device.ToString().Split('-')[0]);
                var evt = new BusEvents<Sample>(new BusSubscriptionReciever(topicName, connection, sname), new Handler());
                events.Add(evt);
                evt.Run();
            }

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}