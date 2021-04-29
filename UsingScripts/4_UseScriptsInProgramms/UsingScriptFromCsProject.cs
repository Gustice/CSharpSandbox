// This program can be run by "dotnet run" command in terminal

using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace UsingScripts
{
    class Program
    {
        static void Main(string[] args)
        {
            string scriptFile = File.ReadAllText("SimpleScript.csx"); 
            var options = ScriptOptions.Default.WithReferences(typeof(Program).Assembly).WithImports("System.Collections.Generic", "System.Linq");
            var script = CSharpScript.Create(scriptFile, options);
            script.Compile();
            var result = script.RunAsync().Result;
        }
    }
}
