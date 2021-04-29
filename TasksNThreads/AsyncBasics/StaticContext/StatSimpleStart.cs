using AsyncBasics.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBasics.StaticContext
{
    static class StatSimpleStart
    {
        public static void RunTest()
        {
            PrintHelper.PrintCaption("SimpleStart in Static context");
            var task1 = new Task(() =>
            {
                string threadType = Thread.CurrentThread.IsThreadPoolThread ? "ThreadPool" : "Custom";
                Thread.Sleep(400);
                Console.WriteLine(threadType);
            }, TaskCreationOptions.LongRunning);

            task1.Start();
            Console.WriteLine("wait for it...");
            task1.Wait();
            Console.WriteLine("done!");

            var task2 = new Task(() => Print("input"));
            task2.Start();
            var task3 = Task.Run(() => Print("input-run"));

            var task4 = Task.Factory.StartNew(() => Print("from factory"), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            task4.Wait();
            // Task cannot be started a second time task4.Start(); // .---> throws Exception
        }

        static void Print(string input)
        {
            System.Console.WriteLine("A task was started with - " + input);
        }
    }
}
