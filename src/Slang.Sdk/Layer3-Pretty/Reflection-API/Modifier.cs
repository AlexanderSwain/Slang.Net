using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Modifier : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.ModifierReflection Binding { get; }

        internal Modifier(Reflection parent, Binding.ModifierReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
