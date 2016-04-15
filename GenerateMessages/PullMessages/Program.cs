namespace PullMessages
{
    using System.Configuration;
    using King.Service.ServiceBus;

    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var init = new BusTopic("ctorder", connection);
            init.CreateIfNotExists().Wait();

            var subscriber = new BusTopicSubscriber("ctorder", connection, "bydevice", "*");

        }
    }
}