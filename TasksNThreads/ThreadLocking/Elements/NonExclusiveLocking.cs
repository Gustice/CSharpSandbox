using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreadLocking.Common;

namespace ThreadLocking.Elements
{
    public static class NonExclusiveLocking
    {
        static List<ValuableRessource> _ressources = new List<ValuableRessource>() {
            new ValuableRessource() { Key = 1, Value = "123"},
            new ValuableRessource() { Key = 2, Value = "456"},
            new ValuableRessource() { Key = 3, Value = "789"},
        };

        public static void RunTest()
        {
            PrintHelper.PrintCaption("NonExclusiveLocking");

            var fetcher1 = new Thread(() => FetchRessource(1, 1, distance: 50));
            var fetcher2 = new Thread(() => FetchRessource(2, 1, distance: 25));
            var fetcher3 = new Thread(() => FetchRessource(3, 1, distance: 10));

            fetcher1.Start();
            fetcher2.Start();
            Task.Run(() => Task.Delay(200)).GetAwaiter().GetResult();
            fetcher3.Start();
            
            Task.Run(() => Task.Delay(500)).GetAwaiter().GetResult();
        }


        private static ReaderWriterLockSlim _token = new ReaderWriterLockSlim();
        static void FetchRessource(int fetcher, int target, int distance)
        {

            bool abortFetch = false;
            try
            {
                Console.WriteLine($"Fetcher-{fetcher} waiting for ReadLock");
                _token.EnterReadLock();
                Console.WriteLine($"Fetcher-{fetcher} >>> EnterReadLock");
                Console.WriteLine($"Fetcher-{fetcher} Checking Ressource");
                Thread.Sleep(50);
                if (_ressources.Any(x => x.Key == target))
                {
                    Console.WriteLine($"Fetcher-{fetcher} - Ressource found -> will fetch");
                }
                else
                {
                    Console.WriteLine($"Fetcher-{fetcher} - Ressource not found -> Abort");
                    abortFetch = true;
                }
            }
            finally
            {
                _token.ExitReadLock();
                Console.WriteLine($"Fetcher-{fetcher} <<< ExitReadLock");

            }
            if (abortFetch)
                return;

            Console.WriteLine($"Fetcher-{fetcher} Ressource found ... takes {distance} to reach");
            Thread.Sleep(distance);

            try
            {
                Console.WriteLine($"Fetcher-{fetcher} waiting for WriteLock");
                _token.EnterWriteLock();
                Console.WriteLine($"Fetcher-{fetcher} >>> EnterWriteLock");

                if (_ressources.Any(x => x.Key == target))
                {
                    Console.WriteLine($"Fetcher-{fetcher} - Ressource still present -> taking");
                    Thread.Sleep(100);

                    ValuableRessource taken = _ressources.FirstOrDefault(x => x.Key == target);
                    _ressources.Remove(taken);

                    Console.WriteLine($"Fetcher-{fetcher} - Ressource taken");
                }
                else
                {
                    Console.WriteLine($"Fetcher-{fetcher} - Ressource already teaken -> abort");
                }
            }
            finally
            {
                _token.ExitWriteLock();
                Console.WriteLine($"Fetcher-{fetcher} <<< ExitWriteLock");
            }
        }
    }
}
