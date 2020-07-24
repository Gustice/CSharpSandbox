using NUnit.Framework;
using JsonEval.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JsonTests
{
    public class SystemTextJsonTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConvertSameModelBothWays_Simple()
        {
            var obj = new ModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
            };

            var ser = JsonSerializer.Serialize(obj);
            Assert.AreEqual("{\"Type\":0,\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"}", ser);

            var bObj = JsonSerializer.Deserialize<ModelOne>(ser);

            Assert.IsTrue(obj == bObj);
        }

        [Test]
        public void ConvertSameModelBothWays_Complex()
        {
            var obj = new ModelTwo()
            {
                Type = ModelType.Complex,
                Model = new ModelOne() {
                    Type = ModelType.Complex,
                    Key = 1,
                    Label = "Label 1",
                    Value = "TestValue",
                },
                Values = new List<string>() { "Value1", "Value2", "Value3", },
            };

            var ser = JsonSerializer.Serialize(obj);
            Assert.AreEqual("{\"Type\":1,\"Model\":{\"Type\":1,\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"},\"Values\":[\"Value1\",\"Value2\",\"Value3\"]}", ser);
            
            var bObj = JsonSerializer.Deserialize<ModelTwo>(ser);

            Assert.IsTrue(obj == bObj);
        }

        [Test]
        public void ConvertSameModelBothWays_Derived()
        {
            var obj = new DerivedModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
                Reference = 2,
                Values = new List<string>() { "Value1", "Value2", "Value3", },
            };

            var ser = JsonSerializer.Serialize(obj);
            Assert.AreEqual("{\"Values\":[\"Value1\",\"Value2\",\"Value3\"],\"Reference\":2,\"Type\":0,\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"}", ser);
            
            var bObj = JsonSerializer.Deserialize<DerivedModelOne>(ser);

            Assert.IsTrue(obj == bObj);
        }

        [Test]
        public void ConvertSameModelBothWays_DerivedToBase()
        {
            var obj = new DerivedModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
                Reference = 2,
                Values = new List<string>() { "Value1", "Value2", "Value3", },
            }; https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to

            var ser = JsonSerializer.Serialize<ModelOne>(obj);
            Assert.AreEqual("{\"Type\":0,\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"}", ser);

            var bObj = JsonSerializer.Deserialize<DerivedModelOne>(ser);

            var cObj = new DerivedModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
                Reference = null,
                Values = null,
            };

            Assert.IsTrue(cObj == bObj);
        }

        [Test]
        public void ConvertModelWithPlainEnum()
        {
            var obj = new ModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
            };

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(new JsonStringEnumConverter());
            //options.WriteIndented = true;
            var ser = JsonSerializer.Serialize(obj, options);

            
            Assert.AreEqual("{\"Type\":\"simple\",\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"}", ser);
        }


        [Test]
        public void ConvertDifferentModelBothWays()
        {
            var obj = new ModelOne()
            {
                Type = ModelType.Simple,
                Key = 1,
                Label = "Label 1",
                Value = "TestValue",
            };

            var ser = JsonSerializer.Serialize(obj);
            Assert.AreEqual("{\"Type\":0,\"Key\":1,\"Value\":\"TestValue\",\"Label\":\"Label 1\"}", ser);

            var bObj = JsonSerializer.Deserialize<ModelOne>(ser);

            Assert.IsTrue(obj == bObj);
        }


    }
}