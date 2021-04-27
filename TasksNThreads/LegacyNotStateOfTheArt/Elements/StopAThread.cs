using LegacyNotStateOfTheArt.Common;
using System.Threading;

namespace LegacyNotStateOfTheArt.Elements
{
    static class StopAThread
    {
        static bool _shouldStop = false;

        public static void RunTest()
        {
            PrintHelper.PrintCaption("Stop Threads by Request");

            var thread = new Thread(StopPerRequest);
            thread.Start();

            Thread.Sleep(700);
            _shouldStop = true;
            PrintHelper.PrintCaseSubStep("finished business thread");

        }

        private static void StopPerRequest()
        {
            int i = 0;
            if (_shouldStop)
                return;

            BusinessLogic(++i);

            if (_shouldStop)
                return;

            PrintHelper.PrintCaseSubStep("Other businesslogic");

            while (!_shouldStop)
                BusinessLogic(++i);
        }

        private static void BusinessLogic(int i)
        {
            Thread.Sleep(200);
            PrintHelper.PrintCaseSubStep("some business operation - number: " + i);
        }
    }
}
