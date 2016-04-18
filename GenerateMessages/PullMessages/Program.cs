namespace PullMessages
{
    using King.Service;
    using Models;
    using System.Threading;

    public class Program
    {
        static void Main(string[] args)
        {
            var manager = new RoleTaskManager<Config>(new DeviceFactory());

            if (manager.OnStart(new Config()))
            {
                manager.Run();

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}