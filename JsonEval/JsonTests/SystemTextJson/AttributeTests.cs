using NUnit.Framework;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonTests.SystemTextJson
{
    [TestFixture]
    class AttributeTests
    {
        class Element
        {
            [JsonPropertyName("ser_number")]
            public int Number { get; set; }
            [JsonPropertyName("ser_label")]
            public string Label { get; set; }
        }

        enum EType
        {
            Type1,
            Type2,
            Type3
        }

        [Test]
        public void CustomeNames()
        {
            var obj = new Element() { Number = 12, Label = "LabelNo12" };

            var output = JsonSerializer.Serialize(obj);
            Assert.AreEqual("{\"ser_number\":12,\"ser_label\":\"LabelNo12\"}", output);

            var dObj = JsonSerializer.Deserialize<Element>(output);
            Assert.AreEqual(obj.Number, dObj.Number);
            Assert.AreEqual(obj.Label, dObj.Label);
        }
    }
}
