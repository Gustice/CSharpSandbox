using JsonEval.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace JsonEval.PartsNewtonsoft
{
    class SerializeCycle
    {


        public static void StaticProcessingInRam()
        {
            var obj = new ModelOne()
            {
                Key = 1,
                Value = "MyValue",
                Label = "MyLabel",
            };

            string ser = JsonConvert.SerializeObject(obj);
            Console.WriteLine(ser);

            var dObj = JsonConvert.DeserializeObject<ModelOne>(ser);
            Console.WriteLine(dObj);
        }

        public static void StaticProcessingToFile()
        {
            var obj = new ModelOne()
            {
                Key = 2,
                Value = "MyValue",
                Label = "MyLabel",
            };

            using (var writer = new StreamWriter(@"StaticProcessingToFile.txt"))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
            // File.WriteAllText("StaticProcessingToFile.txt", JsonConvert.SerializeObject(obj));

            var dObj = new ModelOne();
            using (var reader = new StreamReader("StaticProcessingToFile.txt"))
            {
                string str = reader.ReadToEnd();
                dObj = JsonConvert.DeserializeObject<ModelOne>(str);
            }
            // var obj = JsonConvert.DeserializeObject(File.ReadAllText("StaticProcessingToFile.txt"));

            Console.WriteLine(dObj);
        }



        internal static void ProcessingInRamWithObject()
        {
            var obj = new ModelOne()
            {
                Key = 3,
                Value = "MyValue",
                Label = "MyLabel",
            };

            JsonSerializer serializer = new JsonSerializer();

            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = NullValueHandling.Ignore;

            //using (StreamWriter sw = new StreamWriter(@"c:\json.txt"))
            //using (JsonWriter writer = new JsonTextWriter(sw))
            //{
            //    serializer.Serialize(writer, product);
            //    // {"ExpiryDate":new Date(1230375600000),"Price":0}
            //}


            string ser = JsonConvert.SerializeObject(obj);
            Console.WriteLine(ser);

            var dObj = JsonConvert.DeserializeObject<ModelOne>(ser);



        }
    }
}
