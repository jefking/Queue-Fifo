namespace PullMessages
{
    using King.Service;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using Models;
    using System.Collections.Generic;

    public class DeviceFactory : ITaskFactory<Config>
    {
        public IEnumerable<IRunnable> Tasks(Config passthrough)
        {
            var connection = passthrough.Connection;
            var topicName = "ctorder";
            var subscriptionName = "bydevice{0}";
            var filter = "DeviceId = '{0}'";

            var tasks = new List<IRunnable>();

            tasks.Add(new InitializeStorageTask(new BusTopic(topicName, connection)));

            foreach (var device in passthrough.Devices)
            {
                var sname = string.Format(subscriptionName, device.ToString().Split('-')[0]);
                var dfilter = string.Format(filter, device);

                tasks.Add(new InitializeStorageTask(new BusTopicSubscription(topicName, connection, sname, dfilter)));
                tasks.Add(new RecurringRunner(new DequeueBatchProcessBatch(new BusPoller<Sample>(new BusSubscriptionReciever(topicName, connection, sname)), new BatchProcessor(), 32)));

            }

            return tasks;
        }
    }
}