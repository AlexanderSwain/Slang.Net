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
        public string? SourceCode { get; }

        /// <summary>
        /// The compiled byte code.
        /// </summary>
        public byte[]? ByteCode { get; }

        /// <summary>
        /// The target compilation format.
        /// </summary>
        public ProgramTarget Target { get; }

        /// <summary>
        /// The entry point that was compiled.
        /// </summary>
        public ProgramEntryPoint? EntryPoint { get; }

        /// <summary>
        /// Whether the compilation was successful.
        /// </summary>
        public SlangResult Result { get; }

        public Target.CompileOutputType CompileOutputType => Target.Value.CompileOutputType();

        /// <summary>
        /// Any diagnostics or error messages from compilation.
        /// </summary>
        internal string? Diagnostics { get; }

        public CompilationResult(byte[] compiled, ProgramTarget target, ProgramEntryPoint? entryPoint, SlangResult result, string? diagnostics)
        {
            Target = target;

            // Set the output based on the compile output type
            if (CompileOutputType == Interop.Target.CompileOutputType.ByteCode)
            {
                ByteCode = compiled;
                SourceCode = null;
            }
            else
            {
                SourceCode = Encoding.UTF8.GetString(compiled);
                ByteCode = null;
            }

            EntryPoint = entryPoint;
            Result = result;
            Diagnostics = diagnostics;
        }
    }
}
