using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonTests.SystemTextJson
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
                var ser = JsonSerializer.Serialize(variable);
                Assert.AreEqual("\"Value\"", ser);
            }
            {
                int variable = 1234;
                var ser = JsonSerializer.Serialize(variable);
                Assert.AreEqual("1234", ser);
            }
            {
                double variable = 12.34;
                var ser = JsonSerializer.Serialize(variable);
                Assert.AreEqual("12.34", ser);
            }
            {
                EType variable = EType.Type1;
                var ser = JsonSerializer.Serialize(variable);
                Assert.AreEqual("0", ser);
            }
            {
                EType variable = EType.Type1;
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter());
                var ser = JsonSerializer.Serialize(variable, options);
                Assert.AreEqual("\"Type1\"", ser);
            }

            {
                int[] variable = new int[] { 1, 2, 3, 4 };
                var ser = JsonSerializer.Serialize(variable);
                Assert.AreEqual("[1,2,3,4]", ser);
            }

            {
                List<Element> variable = new List<Element>() {
                 new Element() { Number = 1, Label = "No 1", },
                 new Element() { Number = 2, Label = "No 2", },
                };
                var ser = JsonSerializer.Serialize(variable);
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

                /// Note: This doesn't work somehow
                //var ser = JsonSerializer.Serialize(variable);
                //Assert.AreEqual("{" +
                //    "\"1\":\"No 1\"," +
                //    "\"2\":\"No 2\"" +
                //    "}", ser);
            }
        }


        [Test]
        public void UsingOptions()
        {
            var element = new Element() { Number = 1, Label = "Label 1"};

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var jsonString = JsonSerializer.Serialize(element, options);
            Assert.IsTrue(jsonString.Contains(Environment.NewLine));
        }
    }
}
