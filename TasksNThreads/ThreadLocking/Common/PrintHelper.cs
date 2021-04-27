using System;
using System.Runtime.CompilerServices;

namespace ThreadLocking.Common
{
    static class PrintHelper
    {

        public static void PrintCaption(string caption, [CallerMemberName] string className = "")
        {
            var restore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{caption} {className}");
            Console.ForegroundColor = restore;
        }

        public static void PrintCaseStep(string output, [CallerMemberName] string member = "")
        {
            Console.WriteLine($"  {member}: {output}");
        }
        public static void PrintCaseSubStep(string output)
        {
            Console.WriteLine($"    {output}");
        }


        public static void PrintCurrentSecAndMilli(string text, int indent = 4)
        {
            var space = "".PadRight(indent, ' ');
            Console.WriteLine($"{space}{text}{DateTime.Now.ToString("ss : fff")} (s : ms)");
        }

        internal static void PrintError(string output)
        {
            var restore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"    {output}");
            Console.ForegroundColor = restore;
        }
    }
}
