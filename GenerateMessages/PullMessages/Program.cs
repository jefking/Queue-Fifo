namespace PullMessages
{
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Models;

    public class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var topicName = "ctorder";
            var subscriptionName = "bydevice";
            string filter = null; //"SELECT *";

            Initialize(connection, topicName, subscriptionName, filter).Wait();

            var events = new BusEvents<Sample>(new BusSubscriptionReciever(topicName, connection, subscriptionName), new Handler());
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
    }
}