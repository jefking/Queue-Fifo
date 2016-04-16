namespace PullMessages
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;

    public class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            string topicName = "ctorder";
            string subscriptionName = "bydevice";
            string filter = "SELECT *";

            Initialize(connection, topicName, subscriptionName, filter).Wait();

            var events = new BusEvents<Sample>(new BusSubscriptionReciever(topicName, connection, filter), new Handler());
            events.Run();

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        static async Task Initialize(string connection, string topicName, string subscriptionName, string filter)
        {
            var init = new BusTopic(topicName, connection);
            await init.CreateIfNotExists();

            var subscriber = new BusTopicSubscription(topicName, connection, subscriptionName, filter);
            await subscriber.CreateIfNotExists();
        }

        public class Handler : IBusEventHandler<Sample>
        {
            public void OnError(string action, Exception ex)
            {
                Trace.TraceError("Action '{0}' caused: {1}", action, ex);
            }

            public Task<bool> Process(Sample data)
            {
                Trace.TraceInformation(data.ToString());

                return new Task<bool>(() => { return true; });
            }
        }
    }
}