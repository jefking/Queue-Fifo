namespace PullMessages
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            Initialize(connection, "ctorder", "bydevice", "").Wait();
        }

        static async Task Initialize(string connection, string topicName, string subscriptionName, string filter)
        {
            var init = new BusTopic(topicName, connection);
            await init.CreateIfNotExists();

            var subscriber = new BusTopicSubscriber(topicName, connection, subscriptionName, filter);
            await subscriber.CreateIfNotExists();

            var tc = TopicClient.CreateFromConnectionString(connection, topicName);
            var s = new BusEvents<Sample>(new BusTopicClient(tc), new Handler());
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

                return true;
            }
        }
    }
}