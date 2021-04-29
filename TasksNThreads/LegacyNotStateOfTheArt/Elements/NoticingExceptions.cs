using LegacyNotStateOfTheArt.Common;
using System;
using System.Threading;

namespace LegacyNotStateOfTheArt.Elements
{
    static class NoticingExceptions
    {

        static public void RunAll()
        {
            PrintHelper.PrintCaption("Exception Management");

            PrintHelper.PrintCaseStep("Correct Exception Management");
            CorrectExceptionManagement();

            PrintHelper.PrintCaseStep("Wrong Exception Management -- not called here");
            // WrongExceptionManagement();
            // Note: When the foreground thread is finished before the background thread 
            //  throws the exception. The eception won't be noticed because the background-
            //  thread is closed automatically with the last foreground-thread
        }

        static void WrongExceptionManagement()
        {
            var threaed = new Thread(() =>
            {
                PrintHelper.PrintCaseSubStep("Starting Thread with Excepetion");
                Thread.Sleep(200);
                throw new InvalidOperationException("Ordinary Exception");
            })
            {
                IsBackground = true,
                // Todo: Somehow different to training: Check if behaviour is different in .Net Core
                Name = "Silently fails" // It seems this is not correct anymore
            };
            threaed.IsBackground = true;
            try
            {
                threaed.Start();
            }
            catch (Exception)
            {
                PrintHelper.PrintError("This should not appear");
            }
            // Note: Exception will not be noticed
        }

        static void CorrectExceptionManagement()
        {
            var threaed = new Thread(() =>
            {
                try
                {
                    PrintHelper.PrintCaseSubStep("Starting Thread with Excepetion");
                    Thread.Sleep(200);
                    throw new Exception("Ordinary Exception");
                }
                catch (Exception)
                {
                    PrintHelper.PrintCaseSubStep("Exception is handled inside of Thread");
                }
            });

            threaed.Start();
        }
    }
}
