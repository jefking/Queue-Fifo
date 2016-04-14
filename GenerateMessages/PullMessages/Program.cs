namespace PullMessages
{
    using King.Service.ServiceBus;
    using System.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var subscriber = new TopicSubscriber("ctorder", connection, "bydevice", "*");

        }
    }
}