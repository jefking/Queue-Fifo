namespace PullMessages
{
    using King.Service.ServiceBus;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class BatchProcessor : IBusEventHandler<Sample>
    {
        public async Task<bool> Process(IEnumerable<Sample> datas)
        {
            var success = false;
            foreach (var d in datas)
            {
                success &= await this.Process(d);
            }

            return success;
        }

        public Task<bool> Process(Sample data)
        {
            Trace.TraceInformation("Occurred On: {0}", data.OccurredOn);

            return Task.FromResult<bool>(true);
        }

        public void OnError(string action, Exception ex)
        {
            Trace.TraceError("Error '{0}' caused: {1}", action, ex);
        }
    }
}