using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreadLocking.Common;

namespace ThreadLocking.Elements
{
    public static class SignalingWithReset
    {
        static Queue<ValuableRessource> _queueForGate = new Queue<ValuableRessource>();
        static Queue<ValuableRessource> _queueForTurnstile = new Queue<ValuableRessource>();
        private static readonly ManualResetEvent _gate = new ManualResetEvent(false);
        private static readonly AutoResetEvent _turnstile = new AutoResetEvent(false);

        readonly static object _token1 = new object();
        private static object _token2 = new object();

        public static void RunTest()
        {
            PrintHelper.PrintCaption("SignalingWithReset");

            // Gate lets multiple singals pass 
            var producer1 = new Thread(() => ProduceForGate());
            // Turnstyle lets only one signal pass even with multiple signals occured
            var producer2 = new Thread(() => ProduceForTurnstile());
            
            var consumer1 = new Thread(() => ConsumeAll());
            var consumer2 = new Thread(() => ConsumeTurnstile());

            producer1.Start();
            producer2.Start();
            consumer1.Start();
            consumer2.Start();

            while(producer1.IsAlive)
            {
                Thread.Sleep(100);
            }
        }


        static void ProduceForGate()
        {
            int count = 4;
            int i = 1;
            while (count-- > 0)
            {
                for (int j = 0; j < 3; j++)
                {
                    var rnd = new Random().Next(0, 42);
                    _queueForGate.Enqueue(new ValuableRessource() { Key = i, Value = rnd.ToString() });
                    Console.WriteLine("# enqueud was: " + rnd);
                    _gate.Set();
                }
                Thread.Sleep(100);
            }
        }
        static void ConsumeAll()
        {
            while (true)
            {
                _gate.WaitOne();
                if (_queueForGate.Any())
                {
                    ValuableRessource result = _queueForGate.Dequeue();
                    Console.WriteLine($"# Gate dequeued: {result.Key}:{result.Value}");
                    Thread.Sleep(10);
                }
            }
        }


        static void ProduceForTurnstile()
        {
            int count = 4;
            int i = 1;
            while (count-- > 0)
            {
                lock (_token2)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var rnd = new Random().Next(0, 42);
                        _queueForTurnstile.Enqueue(new ValuableRessource() { Key = i, Value = rnd.ToString() });
                        _turnstile.Set();
                        Console.WriteLine("+ enqueud was: " + rnd);
                        Monitor.Pulse(_token2);
                    }
                    Thread.Sleep(100);
                }
            }
        }
        static void ConsumeTurnstile()
        {
            while (true)
            {
                _turnstile.WaitOne();
                lock (_token2)
                {
                    if (_queueForTurnstile.Any())
                    {
                        ValuableRessource result = _queueForTurnstile.Dequeue();
                        Console.WriteLine($"+ Turn dequeued: {result.Key}:{result.Value}");
                        Thread.Sleep(10);
                    }
                }
            }
        }

    }
}
