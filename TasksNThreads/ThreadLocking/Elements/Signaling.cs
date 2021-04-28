using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreadLocking.Common;

namespace ThreadLocking.Elements
{
    public static class Signaling
    {
        static Queue<ValuableRessource> _queue = new Queue<ValuableRessource>();
        readonly static object _token = new object();
        static int count = 10;

        public static void RunTest()
        {
            PrintHelper.PrintCaption("Signaling");

            var producer = new Thread(() => Produce());
            string first = "first";
            string snd = "second";

            var consumer1 = new Thread(() => Consume(first));
            var consumer2 = new Thread(() => Consume(snd));

            producer.Start();
            consumer1.Start();
            consumer2.Start();

            while(producer.IsAlive)
            {
                Thread.Sleep(100);
            }
        }


        static void Produce()
        {
            int i = 1;
            while (count-- > 0)
            {
                lock (_token)
                {
                    var rnd = new Random().Next(0, 42);
                    _queue.Enqueue(new ValuableRessource() { Key = i, Value = rnd.ToString()});
                    Console.WriteLine("enqueud was: " + rnd);
                    Monitor.Pulse(_token);
                    Thread.Sleep(100);
                }
            }

        }

        static void Consume(string name)
        {
            lock (_token)
            {
                while (true)
                {
                    Monitor.Wait(_token);
                    if (_queue.Any())
                    {
                        ValuableRessource result = _queue.Dequeue();
                        Console.WriteLine($"{name} dequeued: {result.Key}:{result.Value}");
                    }
                }
            }
        }
    }
}
