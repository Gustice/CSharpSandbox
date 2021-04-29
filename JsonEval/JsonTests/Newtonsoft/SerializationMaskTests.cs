using Newtonsoft.Json;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace JsonTests.Newtonsoft
{
    [TestFixture]
    public class SerializationMaskTests
    {
        //[JsonObject(MemberSerialization.OptOut)] // <- Implicit OptOut
        class UntaggedModel
        {
            public int Prop { get; set; } = 1;
            public int PropGetter { get; } = 2;
            public int PublicField = 3;
            private int _privateField = 4;
        }
        [Test]
        public void SerializeDefault()
        {
            var obj = new UntaggedModel();

            var output = JsonConvert.SerializeObject(obj);
            TestHelper.AreEqualWithoutWhitespaces("{" +
                "\"PublicField\":3," +
                "\"Prop\":1," +
                "\"PropGetter\":2" +
                "}", output);
        }



        [JsonObject(MemberSerialization.OptIn)]
        class Full_TaggedModel
        {
            [JsonProperty]
            public int Prop { get; set; } = 1;
            [JsonProperty]
            public int PropGetter { get; } = 2;
            [JsonProperty]
            public int PublicField = 3;
            [JsonProperty]
            private int _privateField = 4;
        }
        [Test]
        public void SerializeDefault_FullTaggedModel1()
        {
            var obj = new Full_TaggedModel();

            var output = JsonConvert.SerializeObject(obj);
            TestHelper.AreEqualWithoutWhitespaces("{" +
                "\"PublicField\":3," +
                "\"_privateField\":4," +
                "\"Prop\":1," +
                "\"PropGetter\":2" +
                "}", output);
        }



        [JsonObject(MemberSerialization.OptIn)]
        class TaggedModel
        {
            [JsonProperty]
            public int Prop { get; set; } = 1;
            public int PropGetter { get; } = 2;
            public int PublicField = 3;
            private int _privateField = 4;
        }
        [Test]
        public void SerializeFieldsOnly()
        {
            var obj = new TaggedModel();
            var output = JsonConvert.SerializeObject(obj);
            TestHelper.AreEqualWithoutWhitespaces("{\"Prop\":1}", output);
        }



        class OneProp_TaggedModel
        {
            [JsonIgnore] // <- To be ignored
            public int Prop { get; set; } = 1;
            public int PropGetter { get; } = 2;
            public int PublicField = 3;
            private int _privateField = 4;
        }
        [Test]
        public void SerializeDefault_TaggedModel1()
        {
            var obj = new OneProp_TaggedModel();

            var output = JsonConvert.SerializeObject(obj);
            TestHelper.AreEqualWithoutWhitespaces("{" +
                "\"PublicField\":3," +
                //"\"Prop\":1," +
                "\"PropGetter\":2" +
                "}", output);
        }



        class OneFild_TaggedModel
        {
            public int Prop { get; set; } = 1;
            public int PropGetter { get; } = 2;
            public int PublicField = 3;
            [JsonProperty] // To be serialized
            private int _privateField = 4;
        }
        [Test]
        public void SerializeDefault_TaggedModel2()
        {
            var obj = new OneFild_TaggedModel();

            var output = JsonConvert.SerializeObject(obj);
            TestHelper.AreEqualWithoutWhitespaces("{" +
                "\"PublicField\":3," +
                "\"_privateField\":4," +
                "\"Prop\":1," +
                "\"PropGetter\":2" +
                "}", output);
        }
    }

    public static class TestHelper
    {
        static Regex _wsMatcher = new Regex(@"\s+");

        public static string RemoveWhiteSpaces(this string self)
        {
            return _wsMatcher.Replace(self, "");
        }

        public static void AreEqualWithoutWhitespaces(string expected, string actual)
        {
            Assert.AreEqual(expected.RemoveWhiteSpaces(), actual.RemoveWhiteSpaces());
        }
    }
}
