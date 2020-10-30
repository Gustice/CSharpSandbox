using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonEval.PartsNewtonsoft
{

    class ProcessingWithoutModel
    {


        public static void ReadKey(string input, string key)
        {
            var obj = JObject.Parse(input);
            var item = obj.GetValue(key);
            Console.WriteLine($"Key '{key}' = '{item}'\n    In input: {input}");
        }

        public static void ChangeValue(string input, string key, string newValue)
        {
            var obj = JObject.Parse(input);
            var item = obj.GetValue(key);

            obj.Remove(key);
            item = newValue;
            obj.Add(key, item);
            var newStream = obj.ToString(Newtonsoft.Json.Formatting.None, null);
            Console.WriteLine($"Input: {input} \nChanged to: {newStream}");
        }

    }
}
