namespace PullMessages
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class DequeueBatchProcessBatch : DequeueBatch<Sample>
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="poller">Poller</param>
        /// <param name="batchCount">Number of items to dequeue at a time</param>
        /// <param name="minimumPeriodInSeconds">Minimum, time in seconds</param>
        /// <param name="maximumPeriodInSeconds">Maximum, time in seconds</param>
        public DequeueBatchProcessBatch(IPoller<Sample> poller, IBatchJob<Sample> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(poller, processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }
        #endregion

        #region Properties
        public IBatchJob<Sample> BatchJob
        {
            get
            {
                return (IBatchJob<Sample>)base.processor;
            }
        }
        #endregion

        #region Methods
        protected override async Task Process(IEnumerable<IQueued<Sample>> msgs)
        {
            var datas = new List<Sample>();
            foreach (var msg in msgs)
            {
                var d = await msg.Data();
                datas.Add(d);
            }

            var ordered = datas.OrderBy(d => d.OccurredOn);

            //Abandon messages not ready for processing
            //Work on messages that are ready for processing

            await this.BatchJob.Process(datas);
        }
        #endregion
    }
}