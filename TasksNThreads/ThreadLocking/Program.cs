using System;
using System.Threading.Tasks;
using ThreadLocking.Elements;

namespace ThreadLocking
{
    class Program
    {
        static void Main(string[] args)
        {
            ExclusiveLocking.RunTest();
            NonExclusiveLocking.RunTest();
            Signaling.RunTest();
            SignalingWithReset.RunTest();
            AbortCondition.RunTest();

            Console.ReadLine();
        }
    }
}
