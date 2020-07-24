namespace JsonEval.Models
{
    public class ModelOne
    {
        public ModelType Type { get; set; } = ModelType.Simple;
        public int Key { get; set; }
        public string Value { get; set; }
        public string Label { get; set; }


        public static bool operator == (ModelOne lhs, ModelOne rhs)
        {
            if (lhs.Type != rhs.Type) return false;
            if (lhs.Key != rhs.Key) return false;
            if (lhs.Value != rhs.Value) return false;
            if (lhs.Label != rhs.Label) return false;

            return true;
        }

        public static bool operator != (ModelOne lhs, ModelOne rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return (this == obj);
        }
    }
}
