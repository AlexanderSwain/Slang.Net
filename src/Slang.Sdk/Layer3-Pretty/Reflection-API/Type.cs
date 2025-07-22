using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Type : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.TypeReflection Binding { get; }

        internal Type(Reflection parent, Binding.TypeReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
