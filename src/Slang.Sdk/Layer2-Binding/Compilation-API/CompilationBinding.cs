using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Binding
{
    internal abstract class CompilationBinding
    {
        internal abstract SlangHandle Handle { get; }

        public override string ToString()
        {
            return $"{GetType().Name}: <Handle>{Handle}</Handle>";
        }
    }
}
