using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonTests.SystemTextJson
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
            public int? Id { get; set; }
            public ModelType Type { get; set; }
            public int Number { get; set; }
            public string Label { get; set; }
            public string Description { get; set; }
        }

        [Test]
        public void WithMissingItems()
        {
            {
                var obj = JsonSerializer.Deserialize<ParseModel>("{}");
                Assert.Multiple(() => {
                    Assert.AreEqual(null, obj.Id);
                    Assert.AreEqual(ModelType.Simple, obj.Type);
                    Assert.AreEqual(0, obj.Number);
                    Assert.AreEqual(null, obj.Label);
                    Assert.AreEqual(null, obj.Description); 
                });
            }

            {
                var obj = JsonSerializer.Deserialize<ParseModel>("{\"Id\": 0, \"Label\": \"\"}");
                Assert.Multiple(() => {
                    Assert.AreEqual(0, obj.Id);
                    Assert.AreEqual(ModelType.Simple, obj.Type);
                    Assert.AreEqual(0, obj.Number);
                    Assert.AreEqual("", obj.Label);
                    Assert.AreEqual(null, obj.Description);
                });
            }

            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var obj = JsonSerializer.Deserialize<ParseModel>("{\"id\": 0, \"LABEL\": \"\"}", options);
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
