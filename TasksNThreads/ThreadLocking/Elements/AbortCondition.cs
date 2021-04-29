using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadLocking.Common;
using System.Threading;

namespace ThreadLocking.Elements
{
    static class AbortCondition
    {
        static object _token = new object();
        static int accountBalance = 100;

        public static void RunTest()
        {
            PrintHelper.PrintCaption("AbortCondition");

            var fetcher1 = new Thread(() => AccessWithTimeOut(90));
            var fetcher2 = new Thread(() => AccessWithTimeOut(80));

            fetcher1.Start();
            fetcher2.Start();
        }

        static void AccessWithTimeOut(int withdrawl)
        {
            bool wasAquired = false;
            try
            {
                Console.WriteLine($"Waiting to withdraw {withdrawl}");
                Monitor.TryEnter(_token, TimeSpan.FromMilliseconds(200), ref wasAquired);
                if (wasAquired)
                {
                    Console.WriteLine($"Working on withdraw {withdrawl} ...");
                    Thread.Sleep(300);
                    if (accountBalance >= withdrawl)
                    {
                        Console.WriteLine($"{withdrawl} widthdrawed");
                        accountBalance -= withdrawl;
                    }
                }
                else
                {
                    Console.WriteLine($"Timeout withdrawing {withdrawl}");
                    throw new TimeoutException("timed out!");
                }
            }
            catch (System.Exception)    
            {
                    
            }
            finally
            {
                if (wasAquired)
                {
                    Monitor.Exit(_token);
                }
            }
        }
    }
}
