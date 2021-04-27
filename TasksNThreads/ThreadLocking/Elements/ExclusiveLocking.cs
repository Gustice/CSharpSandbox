using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreadLocking.Common;

namespace ThreadLocking.Elements
{
    public static class ExclusiveLocking
    {
        static List<ValuableRessource> _ressources = new List<ValuableRessource>() {
            new ValuableRessource() { Key = 1, Value = "123"},
            new ValuableRessource() { Key = 2, Value = "456"},
            new ValuableRessource() { Key = 3, Value = "789"},
        };

        public async static void RunTest()
        {
            PrintHelper.PrintCaption("ExclusiveLocking");

            var fetcher1 = new Thread(() => FetchRessource(1, 1, distance: 50));
            var fetcher2 = new Thread(() => FetchRessource(2, 1, distance: 25));

            fetcher1.Start();
            fetcher2.Start();
            while (fetcher1.IsAlive || fetcher2.IsAlive)
            {
                Thread.Sleep(100);
            }
        }


        private static object _token = new object();
        static void FetchRessource(int fetcher, int target, int distance)
        {
            try
            {
                if (_ressources.Any(x => x.Key == target))
                {
                    Console.WriteLine($"Fetcher-{fetcher} Ressource found ... takes {distance} to reach");
                    Thread.Sleep(distance);

                    Console.WriteLine($"Fetcher-{fetcher} - Checking Lock");
                    lock (_token)
                    {
                        Console.WriteLine($"Fetcher-{fetcher} - Locked Ressource");
                        Thread.Sleep(100);

                        ValuableRessource taken = _ressources.FirstOrDefault(x => x.Key == target);
                        _ressources.Remove(taken);


                        string result = taken != null
                                    ? taken.Value + " is taken"
                                    : "Ressource was removed before";

                        Console.WriteLine($"Fetcher-{fetcher} - " + result);
                    }
                    Console.WriteLine($"Fetcher-{fetcher} - Release Lock");
                }
                else
                {
                    Console.WriteLine($"Fetcher-{fetcher} - Ressource not found");
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}
