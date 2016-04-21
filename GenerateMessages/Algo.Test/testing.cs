namespace Algo.Test
{
    using King.Azure.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Newtonsoft.Json;
    using NSubstitute;
    using PullMessages;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class testing
    {
        [TestMethod]
        public async Task Batches()
        {
            var processor = Substitute.For<IProcessor<Sample>>();
            processor.Process(Arg.Any<Sample>());

            var batch = LoadFile("batches");
            var dbpb = new DequeueBatchProcessBatch(processor);
            await dbpb.FifoPercentage(batch);

            // Validate
        }

        public IOrderedEnumerable<Helper> LoadFile(string file)
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
                    Message = Substitute.For<IQueued<Sample>>()
                };

                helpers.Add(h);
            }

            return helpers.OrderBy(h => h.Data.OccurredOn);
        }
    }
}