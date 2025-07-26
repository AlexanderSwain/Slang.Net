using Slang.Sdk.Binding;
using Slang.Sdk.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class EntryPoint
    {
        #region Definition
        public Module Parent { get; }
        internal Binding.EntryPoint Binding { get; }

        internal EntryPoint(Module parent, uint index)
        {
            Parent = parent;
            Binding = new Binding.EntryPoint(Parent.Binding, index);
        }

        internal EntryPoint(Module parent, string name)
        {
            Parent = parent;
            Binding = new Binding.EntryPoint(Parent.Binding, name);
        }

        internal EntryPoint(Module parent, Binding.EntryPoint binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region Pretty

        public int Index
        {
            get
            {
                return Binding.Index;
            }
        }

        public string Name
        { 
            get
            {
                return Binding.Name;
            } 
        }

        public CompilationResult Compile(Interop.Target target)
        {
            var targetIndex = Parent.Parent.Targets.IndexOf(target);

            if (targetIndex is null)
                throw new ArgumentException($"Target {target} was not included in parent session {Parent.Parent}. Use Session.Builder.AddTarget(target) to include it.", nameof(target));

            var result = Binding.Compile((int)targetIndex);
            return new CompilationResult(result.Source, target, this, result.Result, result.Diagnostics);
        }
        #endregion
    }
}
