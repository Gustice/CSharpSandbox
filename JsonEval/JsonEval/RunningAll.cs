using JsonEval.PartsNewtonsoft;
using Newtonsoft.Json;
using System;

namespace JsonEval
{
    class RunningAll
    {
        static void Main(string[] args)
        {
                SerializeCycle.StaticProcessingInRam();

                SerializeCycle.StaticProcessingToFile();
                var obj = new Models.ModelOne() { Key=1234, Value="Value of Object", Label = "Label of Object", Type=Models.ModelType.Simple};
                var stream = JsonConvert.SerializeObject(obj);

                ProcessingWithoutModel.ReadKey(stream, "Value");
                ProcessingWithoutModel.ChangeValue(stream, "Value", "Totally new value in Object");

                Console.ReadLine();
        }
    }
}
