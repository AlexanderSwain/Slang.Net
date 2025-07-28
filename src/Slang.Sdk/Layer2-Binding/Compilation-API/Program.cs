using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;
internal sealed unsafe class Program : CompilationBinding
{
    internal Module Parent { get; }
    internal override ProgramHandle Handle { get; }

    internal Program(Module parent)
    {
        Parent = parent;
        // Using the strongly-typed interop that returns ProgramHandle directly
        Handle = Program_Create(Parent.Handle, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang program: {error ?? "<No error was returned from Slang>"}");
    }

    ~Program()
    {
        Handle?.Dispose();
    }

    internal CompilationResult Compile(uint entryPointIndex, uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        Target target = Parent.Parent.Targets.ElementAt((int)targetIndex);
        EntryPoint entryPoint = Parent.GetEntryPointByIndex(entryPointIndex);

        SlangResult compileResult = SlangNativeInterop.Program_CompileProgram(Handle, entryPointIndex, targetIndex, out string compiledSource, out string error);

        return new CompilationResult(compiledSource ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), target, entryPoint, compileResult, error);
    }

    internal ShaderReflection GetReflection(uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        // Using the strongly-typed interop that returns ShaderReflectionHandle directly
        ShaderReflectionHandle resultHandle = Program_GetProgramReflection(Handle, targetIndex, out var error);

        if (resultHandle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to get shader reflection for target index {targetIndex}: {error ?? "<No error was returned from Slang>"}");

        return new ShaderReflection(this, resultHandle);
    }
}
