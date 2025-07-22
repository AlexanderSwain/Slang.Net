using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class EntryPoint : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.EntryPointReflection Binding { get; }

        internal EntryPoint(Reflection parent, Binding.EntryPointReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
