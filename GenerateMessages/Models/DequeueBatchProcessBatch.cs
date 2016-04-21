namespace PullMessages
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using King.Service.Timing;
    using Models;
    using System;
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
        public DequeueBatchProcessBatch(IPoller<Sample> poller, IProcessor<Sample> processor, byte batchCount = 5, int minimumPeriodInSeconds = BaseTimes.MinimumStorageTiming, int maximumPeriodInSeconds = BaseTimes.MaximumStorageTiming)
            : base(poller, processor, batchCount, minimumPeriodInSeconds, maximumPeriodInSeconds)
        {
        }

        public DequeueBatchProcessBatch(IProcessor<Sample> processor)
            : base(new TopicPoller<Sample>(), processor)
        {
        }
        #endregion

        #region Methods
        protected override async Task Process(IEnumerable<IQueued<Sample>> msgs)
        {
            //SAMPLE IS TOO SMALL, unable to process...? Issues when a single message is in queue, and the device goes offline.
            if (10 < msgs.Count())
            {
                foreach (var msg in msgs)
                {
                    await msg.Abandon();
                }
            }
            //NOT SURE IF THIS IS SPECIFIC TO HOW IT IS THEN PROCESSED

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

            var data = datas.OrderBy(m => m.Data.OccurredOn); // Order By (Fifo)

        }

        public async Task FifoPercentage(IOrderedEnumerable<Helper> data)
        {
            var i = 0;
            foreach (var msg in data.Select(d => d.Message)) // Nice thing is that Data is not needed to interact with list
            {
                if (data.Count() * .8 >= i)
                {
                    await base.Process(msg);
                }
                else
                {
                    await msg.Abandon();
                }

                i++;
            }
        }

        public async Task MessageRelativeTiming(IOrderedEnumerable<Helper> data)
        {
            var relative = data.First().Data.OccurredOn.AddSeconds(5);// Messages must be > 5s newer than oldest message

            foreach (var msg in data.Where(d => relative > d.Data.OccurredOn).Select(d => d.Message))
            {
                await msg.Abandon();
            }

            foreach (var m in data.Where(d => relative <= d.Data.OccurredOn).Select(d => d.Message))
            {
                await base.Process(m);
            }
        }

        public async Task SeverRelativeTiming(IOrderedEnumerable<Helper> data)
        {
            var relative = DateTime.UtcNow.AddSeconds(-5);// Messages must be > 5s old

            foreach (var msg in data.Where(d => relative > d.Data.OccurredOn).Select(d => d.Message))
            {
                await msg.Abandon();
            }

            foreach (var msg in data.Where(d => relative <= d.Data.OccurredOn).Select(d => d.Message))
            {
                await base.Process(msg);
            }
        }

        //WHAT IF IT IS A COMBINATION??

        public async Task Voltron(IOrderedEnumerable<Helper> data)
        {
            var now = DateTime.UtcNow;
            var first = data.First().Data;
            var relative = first.OccurredOn;

            var top = (int)(data.Count() * .8);

            var abandon = (from h in data
                           where h.Data.OccurredOn < relative.AddSeconds(5)
                             && h.Data.OccurredOn < now.AddSeconds(-5)
                           select h).Take(top);

            foreach (var d in data)
            {
                if (abandon.ToList().IndexOf(d) == -1)
                {
                    await base.Process(d.Message);
                }
                else
                {
                    await d.Message.Abandon();
                }
            }
        }
        #endregion
    }
}