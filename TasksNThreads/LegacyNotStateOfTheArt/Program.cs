using System;
using System.Threading;
using LegacyNotStateOfTheArt.Elements;

namespace LegacyNotStateOfTheArt
{
    class Program
    {
        static void Main(string[] args)
        {
            StartMethods.RunAll();
            StopAThread.RunTest();
            CommonVariables.RunTest();
            ForkingJointing.RunAll();
            NoticingExceptions.RunAll();
            UsingThradPool.RunTest();



            Console.ReadLine();
        }
    }
}
