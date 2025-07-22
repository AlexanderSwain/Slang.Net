using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class TypeLayout : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.TypeLayoutReflection Binding { get; }

        internal TypeLayout(Reflection parent, Binding.TypeLayoutReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
