using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Generic : Reflection
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.GenericReflection Binding { get; }

        internal Generic(Reflection parent, Binding.GenericReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion
    }
}
