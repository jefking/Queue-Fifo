namespace PullMessages
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Models;

    public class Handler : IBusEventHandler<Sample>
    {
        int i = 0;
        public void OnError(string action, Exception ex)
        {
            Trace.TraceError("Error '{0}' caused: {1}", action, ex);
        }

        public Task<bool> Process(Sample data)
        {
            Trace.TraceInformation("{2}: Recieving: {0} - {1}", data.DeviceId, data.OccurredOn, ++i);

            return Task.FromResult<bool>(true);
        }
    }
}