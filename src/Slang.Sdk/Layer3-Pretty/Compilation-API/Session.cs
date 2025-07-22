using Slang.Sdk.Collections;
using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public partial class Session
        : IComposition<Target>
    {
        #region Definition
        internal Binding.Session Binding { get; }

        internal Session(CompilerOption[] options, PreprocessorMacro[] macros, Target[] models, string[] searchPaths)
        {
            Binding = new Binding.Session(options, macros, models, searchPaths);
        }
        #endregion

        #region Pretty

        public Module LoadModule(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            // Create a new module and load it from the specified path
            return new Module(this, path);
        }

        SlangCollection<Target>? _Targets;
        public SlangCollection<Target> Targets => _Targets ??= new SlangCollection<Target>(this);
        #endregion

        #region Composition

        // Targets
        Target IComposition<Target>.GetByIndex(uint index)
        {
            return Binding.Targets.ElementAt((int)index);
        }
        uint IComposition<Target>.Count => (uint)Binding.Targets.Count;

        #endregion
    }
}
