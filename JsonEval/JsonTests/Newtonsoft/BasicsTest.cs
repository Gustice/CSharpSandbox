using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonTests.Newtonsoft
{
    [TestFixture]
    class BasicsTest
    {
        class Element
        {
            public int Number { get; set; }
            public string Label { get; set; }
        }

        enum EType
        {
            Type1,
            Type2,
            Type3
        }

        [JsonConverter(typeof(StringEnumConverter))]
        enum EnType
        {
            Type1,
            Type2,
            Type3
        }

        [Test]
        public void SerializingJustElements()
        {
            {
                string variable = "Value";
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("\"Value\"", ser);
            }
            {
                int variable = 1234;
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("1234", ser);
            }
            {
                double variable = 12.34;
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("12.34", ser);
            }
            {
                EType variable = EType.Type1;
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("0", ser);
            }
            {
                EType variable = EType.Type1;
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter());
                var ser = JsonConvert.SerializeObject(variable, settings);
                Assert.AreEqual("\"Type1\"", ser);
            }

            {
                int[] variable = new int[] { 1, 2, 3, 4 };
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("[1,2,3,4]", ser);
            }

            {
                List<Element> variable = new List<Element>() {
                 new Element() { Number = 1, Label = "No 1", },
                 new Element() { Number = 2, Label = "No 2", },
                };
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("[" +
                    "{\"Number\":1,\"Label\":\"No 1\"}," +
                    "{\"Number\":2,\"Label\":\"No 2\"}" +
                    "]", ser);
            }
            {
                Dictionary<int, string> variable = new Dictionary<int, string>() {
                 { 1, "No 1" },
                 { 2, "No 2" },
                };
                var ser = JsonConvert.SerializeObject(variable);
                Assert.AreEqual("{" +
                    "\"1\":\"No 1\"," +
                    "\"2\":\"No 2\"" +
                    "}", ser);
            }
        }

        [Test]
        public void SerializingEnums()
        {
            {
                EType variable = EType.Type2;

                var set = new JsonSerializerSettings();
                set.Converters.Add(new StringEnumConverter());
                var ser = JsonConvert.SerializeObject(variable, set);
                Assert.AreEqual("\"Type2\"", ser);
            }

        }


        [Test]
        public void UsingBothMethods()
        {
            var element = new Element() { Number = 1, Label = "No 1" };

            var ser1 = JsonConvert.SerializeObject(element);
            Assert.AreEqual("{\"Number\":1,\"Label\":\"No 1\"}", ser1);

            var conv = new JsonSerializer();
            //conv.Formatting = Formatting.Indented;
            conv.Converters.Add(new JavaScriptDateTimeConverter());

            var buffer = new byte[128];
            using (MemoryStream mem = new MemoryStream(buffer))
            using (StreamWriter sw = new StreamWriter(mem))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                conv.Serialize(writer, element);
                sw.Flush();

                string ser2 = Encoding.UTF8.GetString(buffer,0, ser1.Length);
                Assert.AreEqual("{\"Number\":1,\"Label\":\"No 1\"}", ser2);
            }
        }

    }
}
