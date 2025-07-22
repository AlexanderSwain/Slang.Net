using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class VariableLayout : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.VariableLayoutReflection Binding { get; }

        internal VariableLayout(Reflection parent, Binding.VariableLayoutReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
