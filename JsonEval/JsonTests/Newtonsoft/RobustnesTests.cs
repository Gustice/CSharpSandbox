using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonTests.Newtonsoft
{
    [TestFixture]
    class RobustnesTests
    {
        enum ModelType
        {
            Simple,
            Complex
        }

        class ParseModel
        {
            public int ? Id { get; set; }
            public ModelType Type { get; set; }
            public int Number { get; set; }
            public string Label { get; set; }
            public string Description { get; set; }
        }

        [Test]
        public void WithMissingItems()
        {
            {
                var obj = JsonConvert.DeserializeObject<ParseModel>("{}");
                Assert.Multiple(() => {
                    Assert.AreEqual(null, obj.Id);
                    Assert.AreEqual(ModelType.Simple, obj.Type);
                    Assert.AreEqual(0, obj.Number);
                    Assert.AreEqual(null, obj.Label);
                    Assert.AreEqual(null, obj.Description);
                });
            }

            {
                var obj = JsonConvert.DeserializeObject<ParseModel>("{\"id\": 0, \"label\": \"\"}");
                Assert.Multiple(() => {
                    Assert.AreEqual(0, obj.Id);
                    Assert.AreEqual(ModelType.Simple, obj.Type);
                    Assert.AreEqual(0, obj.Number);
                    Assert.AreEqual("", obj.Label);
                    Assert.AreEqual(null, obj.Description);
                });
            }

            {
                var obj = JsonConvert.DeserializeObject<ParseModel>("{\"ID\": 0, \"LABEL\": \"\"}");
                Assert.Multiple(() => {
                    Assert.AreEqual(0, obj.Id);
                    Assert.AreEqual(ModelType.Simple, obj.Type);
                    Assert.AreEqual(0, obj.Number);
                    Assert.AreEqual("", obj.Label);
                    Assert.AreEqual(null, obj.Description);
                });
            }
        }
    }
}
