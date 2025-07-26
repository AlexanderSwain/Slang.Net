using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class CompilationResult
    {
        /// <summary>
        /// The compiled source code.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// The target compilation format.
        /// </summary>
        public Target Target { get; }

        /// <summary>
        /// The entry point that was compiled.
        /// </summary>
        public EntryPoint EntryPoint { get; }

        /// <summary>
        /// Whether the compilation was successful.
        /// </summary>
        public SlangResult Result { get; }

        /// <summary>
        /// Any diagnostics or error messages from compilation.
        /// </summary>
        internal string? Diagnostics { get; }

        public CompilationResult(string source, Target target, EntryPoint entryPoint, SlangResult result, string? diagnostics)
        {
            Source = source;
            Target = target;
            EntryPoint = entryPoint;
            Result = result;
            Diagnostics = diagnostics;
        }
    }
}
