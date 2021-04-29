using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonEval.Models
{
    public class ModelTwo
    {
        public ModelType Type { get; set; } = ModelType.Complex;
        public ModelOne Model { get; set; } = new ModelOne() { Type = ModelType.Complex };
        public List<string> Values { get; set; }

        public static bool operator ==(ModelTwo lhs, ModelTwo rhs)
        {
            if (lhs.Type != rhs.Type) return false;
            if (lhs.Model != rhs.Model) return false;

            if (!Enumerable.SequenceEqual(lhs.Values, rhs.Values)) return false;
            
            return true;
        }

        public static bool operator !=(ModelTwo lhs, ModelTwo rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return (this == obj);
        }
    }
}
