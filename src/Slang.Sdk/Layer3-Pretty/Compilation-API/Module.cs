using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Module
    {
        #region Definition
        public Session Parent { get; }
        internal Binding.Module Binding { get; }

        internal Module(Session parent, string moduleName)
        {
            Parent = parent;
            Binding = new Binding.Module(Parent.Binding, moduleName, null, null);
        }
        #endregion

        #region Pretty
        Program? _Program;
        public Program Program => _Program ??= new Program(this);
        #endregion
    }
}
