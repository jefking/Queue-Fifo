namespace PullMessages
{
    using System.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        }
    }
}
