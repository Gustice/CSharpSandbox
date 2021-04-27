using LegacyNotStateOfTheArt.Common;
using System;
using System.Threading;

namespace LegacyNotStateOfTheArt.Elements
{
    static class StartMethods
    {
        public class Closure
        {
            private readonly Action _other;
            private readonly int output;
            public Closure(int threeHundred, Action other)
            {
                _other = other;
                output = threeHundred;
            }

            public void Run()
            {
                _other();
                PrintHelper.PrintCaseSubStep(output.ToString());
            }
        }

        public static void RunAll()
        {
            PrintHelper.PrintCaption("Different Approaches to start");

            PrintHelper.PrintCaseStep("By Delegate");
            ByDelegate();

            PrintHelper.PrintCaseStep("By parametrized Delegate");
            ByParametrizedDelegate(100);

            PrintHelper.PrintCaseStep("By closure (most coherend to lambdas)");
            ByClosure(100);

            PrintHelper.PrintCaseStep("By lambdas which is BEST");
            var thread = new Thread ( () => { PrintHelper.PrintCaseSubStep("Ouput by Lamba");} );
            thread.Start();
            thread.Join();
        }

        private static void ByClosure(int v)
        {
            var closure = new Closure(100, () => PrintHelper.PrintCaseSubStep("Closure Ouptut"));
            var thread = new Thread(closure.Run);
            thread.Start();
            thread.Join();
        }

        private static void ByParametrizedDelegate(int param)
        {
            ParameterizedThreadStart myDelegate = ThreadActionWithParameter;
            var thread = new Thread(myDelegate);
            thread.Start(param);
            thread.Join();
        }

        private static void ByDelegate()
        {
            ThreadStart @start = ThreadAction;
            var thread = new Thread(@start);
            thread.Start();
            thread.Join();
        }

        static void ThreadAction()
        {
            PrintHelper.PrintCaseStep("Starting by Delegate");
        }

        static void ThreadActionWithParameter(object obj)
        {
            PrintHelper.PrintCaseStep($"Starting by parametrized Delegate: {obj}");
        }
    }
}
