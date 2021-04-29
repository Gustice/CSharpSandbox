// Note:
//  Use "dotnet restore" to install defined nuget packages
//  Run "dotnet script init" to create an omnisharp.json with entry "enableScriptNuGetReferences": true"
//  Run in VSCode "OmniSharp: Restart OmniSharp" command to restart intellisense

#r "nuget: Newtonsoft.Json, 12.0.1"
using Newtonsoft.Json;

var someObj = new { type = "SomeType", value = 123};

Console.WriteLine(JsonConvert.SerializeObject(someObj));
