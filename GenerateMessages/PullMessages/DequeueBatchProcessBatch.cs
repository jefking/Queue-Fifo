namespace PullMessages
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Timing;
    using Models;
    using System.Collections.Generic;
    using System.Diagnostics;
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
            var datas = new List<Helper>();
            foreach (var msg in msgs)
            {
                var d = new Helper
                {
                    Message = msg,
                    Data = await msg.Data()
                };

                datas.Add(d);
            }


            //Just Debugging
            foreach (var p in from d in datas
                              orderby d.Data.OccurredOn
                              select d.Data.OccurredOn)
            {
                Trace.TraceInformation("{0}", p);
            }
            //Just Debugging

            await base.Process(from d in datas
                               orderby d.Data.OccurredOn
                               select d.Message);
        }
        #endregion

        public class Helper
        {
            public IQueued<Sample> Message;
            public Sample Data;
        }
    }
}