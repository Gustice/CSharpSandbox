using LegacyNotStateOfTheArt.Common;
using System;
using System.Threading;

namespace LegacyNotStateOfTheArt.Elements
{
    static class CommonVariables
    {
        [ThreadStatic]
        private static int _local = 42;
        // CAUTION: This variable is only initialized once for the first thread. 
        //			 because static variables allways are initialized once in the constructor

        private static int _global = 42; // this will be iterated with each thread

        private static ThreadLocal<int> _tLocal = new ThreadLocal<int>(() => 42);

        public static void RunTest()
        {
            PrintHelper.PrintCaption("Threadlocal variables");

            for (int i = 0; i < 5; i++)
            {
                var thread = new Thread(Increment);
                thread.Name = $"Thread-Nr [{i}]";   
                thread.Start();

                Thread.Sleep(50); // besserer Output
            }
        }

        private static void Increment()
        {
            string threadName = Thread.CurrentThread.Name;
            PrintHelper.PrintCaseSubStep($"{threadName} - static Local:  [{_local++}]");
            PrintHelper.PrintCaseSubStep($"{threadName} - thread Local:  [{(_tLocal.Value)++}]");
            PrintHelper.PrintCaseSubStep($"{threadName} - Global: [{_global++}]");
        }
    }
}
