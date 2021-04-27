using LegacyNotStateOfTheArt.Common;
using System.Threading;

namespace LegacyNotStateOfTheArt.Elements
{
    static class UsingThradPool
    {
        public static void RunTest()
        {
            PrintHelper.PrintCaption("Using Thread Pool");
            var thread = new Thread(() => RunOnThreadpool("Normal-Thread"));
            thread.Start();

            ThreadPool.QueueUserWorkItem(param => RunOnThreadpool("Queued Item"));

            // Note: Tasks are run on Threadpool by default
            System.Threading.Tasks.Task.Run(() => RunOnThreadpool("From the Task"));

        }

        private static void RunOnThreadpool(string threadName)
        {
            if (Thread.CurrentThread.IsBackground)
                PrintHelper.PrintCaseSubStep($"Started background thread '{threadName}'");
            else
                PrintHelper.PrintCaseSubStep($"Started forground thread '{threadName}'");
        }

    }
}
