using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class TypeParameter : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.TypeParameterReflection Binding { get; }

        internal TypeParameter(Reflection parent, Binding.TypeParameterReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
