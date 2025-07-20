using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class ShaderReflection : Reflection
    {
        internal Program Program { get; }

        internal override Reflection? Parent => null;
        internal override ShaderReflectionHandle Handle { get; }

        internal ShaderReflection(Program program, ShaderReflectionHandle handle)
        {
            Program = program;
            Handle = handle;
        }
    }
}
