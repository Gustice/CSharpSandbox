using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonTests.Newtonsoft
{
    [TestFixture]
    class AttributeTests
    {
        class Element
        {
            [JsonProperty("ser_number")]
            public int Number { get; set; }
            [JsonProperty("ser_label")]
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
            var obj = new Element() { Number = 12, Label = "LabelNo12"};

            var output = JsonConvert.SerializeObject(obj);
            Assert.AreEqual("{\"ser_number\":12,\"ser_label\":\"LabelNo12\"}", output);

            var dObj = JsonConvert.DeserializeObject<Element>(output);
            Assert.AreEqual(obj.Number, dObj.Number);
            Assert.AreEqual(obj.Label, dObj.Label);

        }
    }
}
