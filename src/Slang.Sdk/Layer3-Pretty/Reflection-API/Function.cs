using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Function : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.FunctionReflection Binding { get; }

        internal Function(Reflection parent, Binding.FunctionReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
