#r "nuget: Newtonsoft.Json, 12.0.1"
using Newtonsoft.Json;

var someObj = new { type = "SomeType", value = 123};

Console.WriteLine(JsonConvert.SerializeObject(someObj));
