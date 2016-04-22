namespace Algo.Test
{
    using King.Azure.Data;
    using Models;
    using Newtonsoft.Json;
    using NSubstitute;
    using PullMessages;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class testing
    {
        [TestCase("in-order")]
        [TestCase("out-of-order")]
        public async Task FifoPercentage(string file)
        {
            var batch = LoadFile(file);
            var processor = Substitute.For<IProcessor<Sample>>();

            var i = 0;
            foreach (var b in batch)
            {
                b.Message = Substitute.For<IQueued<Sample>>();
                if (batch.Count() * .8 >= i)
                {
                    processor.Process(b.Data);
                }
                else
                {
                    b.Message.Abandon();
                }

                i++;
            }

            var ordered = batch.OrderBy(h => h.Data.OccurredOn);
            var dbpb = new DequeueBatchProcessBatch(processor);
            await dbpb.FifoPercentage(ordered);

            // Validate
            i = 0;
            foreach (var b in ordered)
            {
                if (batch.Count() * .8 >= i)
                {
                    processor.Received().Process(b.Data);
                }
                else
                {
                    b.Message.Received().Abandon();
                }

                i++;
            }
        }

        public IEnumerable<Helper> LoadFile(string file)
        {
            var filePath = string.Format(@"C:\Users\jefkin\Documents\GitHub\Queue-Fifo\DataSets\{0}.json", file);
            var data = File.ReadAllText(filePath);
            var samples = JsonConvert.DeserializeObject<List<Sample>>(data);

            var helpers = new List<Helper>();
            foreach (var d in samples)
            {
                var h = new Helper
                {
                    Data = d,

                };

                helpers.Add(h);
            }

            return helpers;
        }
    }
}