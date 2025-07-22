using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Variable : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.VariableReflection Binding { get; }

        internal Variable(Reflection parent, Binding.VariableReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
