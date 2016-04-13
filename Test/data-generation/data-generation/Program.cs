using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generation
{
    class Program
    {
        static void Main(string[] args)
        {
            var datas = new List<Data>();
            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                var v = random.Next(0, 10);

                var d = new Data
                {
                    OccurredOn = DateTime.Now.AddSeconds(i),
                    EnqueuedAt = DateTime.Now.AddMinutes(1).AddSeconds(v),
                    Value = (short)i,
                };

                //Console.WriteLine("{0} | {1}", d.OccurredOn, d.EnqueuedAt);

                datas.Add(d);
            }
            
            Console.Write(Newtonsoft.Json.JsonConvert.SerializeObject(datas));

            Console.Read();
        }
    }

    public class Data
    {
        public DateTime OccurredOn;
        public DateTime EnqueuedAt;
        public short Value;
    }
}