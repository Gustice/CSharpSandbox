using System;
using System.Threading;
using LegacyNotStateOfTheArt.Common;

namespace LegacyNotStateOfTheArt.Elements
{
    static class ForkingJointing
    {
        public static void RunAll()
        {
            PrintHelper.PrintCaption("Forking and Joining");
            PrintHelper.PrintCaseStep("Naive joining");
            ForkThenJoin();

            PrintHelper.PrintCaseStep("Joining with timeout - successful");
            ForkThenJoinWithTimeOut(200, 400);
            PrintHelper.PrintCaseStep("Joining with timeout - aborted");
            ForkThenJoinWithTimeOut(600, 400);
        }

        static void ForkThenJoin()
        {
            var thread = new Thread(() => SimulateWork(400));
            PrintHelper.PrintCurrentSecAndMilli("");
            thread.Start();
            // Note: This essentially blocks the thread until forked thread is finished
            thread.Join();
            PrintHelper.PrintCurrentSecAndMilli("");
        }

        static void ForkThenJoinWithTimeOut(int worktime, int timeout)
        {
            var thread = new Thread(() => SimulateWork(worktime));
            PrintHelper.PrintCurrentSecAndMilli("");
            thread.Start();

            if (thread.Join(timeout))
                PrintHelper.PrintCaseSubStep("Operation completed");
            else
                PrintHelper.PrintCaseSubStep("Operation aborted");

            PrintHelper.PrintCurrentSecAndMilli("");
        }


        private static void SimulateWork(int delay)
        {
            PrintHelper.PrintCaseSubStep("Starting Thread");
            PrintHelper.PrintCaseSubStep($"  simulating work for {delay} ms");
            Thread.Sleep(delay);
            PrintHelper.PrintCaseSubStep("Ending Thread");
        }
    }
}
