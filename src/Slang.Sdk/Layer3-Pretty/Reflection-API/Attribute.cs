using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Attribute : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.AttributeReflection Binding { get; }

        internal Attribute(Reflection parent, Binding.AttributeReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
