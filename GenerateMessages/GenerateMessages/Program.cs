namespace GenerateMessages
{
    using King.Service.ServiceBus;
    using System.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var sender = new TopicSender("ctorder", connection);
        }
    }
}