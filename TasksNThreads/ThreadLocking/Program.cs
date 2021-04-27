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

            Console.ReadLine();
        }
    }
}
