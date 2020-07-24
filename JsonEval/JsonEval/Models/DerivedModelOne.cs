using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonEval.Models
{
    public class DerivedModelOne : ModelOne
    {
        public List<string> Values { get; set; }
        public int? Reference { get; set; }


        public static bool operator == (DerivedModelOne lhs, DerivedModelOne rhs)
        {
            if ((ModelOne)lhs != (ModelOne)rhs) return false;
            if (lhs.Values != rhs.Values)
                if (!Enumerable.SequenceEqual(lhs.Values, rhs.Values)) return false;
            if (lhs.Reference != rhs.Reference) return false;

            return true;
        }

        public static bool operator != (DerivedModelOne lhs, DerivedModelOne rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return (this == obj);
        }
    }
}
