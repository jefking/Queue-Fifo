﻿using System;
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

            for (var i = 0; i < 10; i++)
            {
                var d = new Data
                {
                    OccurredOn = DateTime.Now.AddMinutes(i),
                    EnqueuedAt = DateTime.Now.AddMinutes(i + 1),
                    Value = (short)i,
                };

                datas.Add(d);
            }

            Console.Write(datas.ToString());
        }
    }

    public class Data
    {
        public DateTime OccurredOn;
        public DateTime EnqueuedAt;
        public short Value;
    }
}