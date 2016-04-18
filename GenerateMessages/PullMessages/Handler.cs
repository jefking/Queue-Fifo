namespace PullMessages
{
    using King.Service.ServiceBus;
    using Models;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class Handler : IBusEventHandler<Sample>
    {
        int i = 0;
        Guid deviceId;
        public Handler(Guid deviceId)
        {
            this.deviceId = deviceId;
        }
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError("Error '{0}' caused: {1}", action, ex);
        }

        public Task<bool> Process(Sample data)
        {
            Trace.TraceInformation("Watching for Device: '{0}' #{1}. Received: {0} - {1}", this.deviceId, ++i, data.DeviceId, data.OccurredOn);

            return Task.FromResult<bool>(true);
        }
    }
}