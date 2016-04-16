namespace PullMessages
{
    using System.Configuration;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;

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
        }
    }
}