using AsyncBasics.DynamicContext;
using AsyncBasics.StaticContext;
using System;

namespace AsyncBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            StatSimpleStart.RunTest();
            var ss = new DynSimpleStart();
            ss.RunTest();

            Console.ReadLine();
        }
    }
}
