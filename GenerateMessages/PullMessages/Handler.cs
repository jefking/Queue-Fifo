namespace PullMessages
{
    using King.Service.ServiceBus;
    using Models;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class Handler : IBusEventHandler<Sample>
    {
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError("Error '{0}' caused: {1}", action, ex);
        }

        public Task<bool> Process(Sample data)
        {
            Trace.TraceInformation("Received: {0}-{1}", data.DeviceId, data.OccurredOn);

            return Task.FromResult<bool>(true);
        }
    }
}