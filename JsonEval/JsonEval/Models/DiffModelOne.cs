using System;
using System.Collections.Generic;
using System.Text;

namespace JsonEval.Models
{
    public class DiffModelOne
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }


        public static bool operator == (DiffModelOne lhs, DiffModelOne rhs)
        {
            if (lhs.Key != rhs.Key) return false;
            if (lhs.Value != rhs.Value) return false;
            if (lhs.Description != rhs.Description) return false;

            return true;
        }

        public static bool operator != (DiffModelOne lhs, DiffModelOne rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return (this == obj);
        }
    }
}
