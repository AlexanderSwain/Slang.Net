using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class ShaderReflection : Reflection
    {
        #region Definition
        public Program Program { get; }
        public override Reflection? Parent => null;
        internal override Binding.ShaderReflection Binding { get; }


        internal ShaderReflection(Program program, Binding.ShaderReflection binding)
        {
            Program = program;
            Binding = binding;
        }
        #endregion


    }
}
