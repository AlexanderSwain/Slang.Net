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
        Handle = Program_Create(Parent.Handle);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang program: {GetLastError() ?? "<No error was returned from Slang>"}");
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

        char* compiledSource = null;
        SlangResult compileResult = SlangNativeInterop.Program_CompileProgram(Handle, entryPointIndex, targetIndex, &compiledSource);
        string? diagnostics = GetLastError();

        return new CompilationResult(StringMarshaling.FromUtf8((byte*)compiledSource) ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), target, entryPoint, compileResult, diagnostics);
    }

    internal ShaderReflection GetReflection(uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        // Using the strongly-typed interop that returns ShaderReflectionHandle directly
        ShaderReflectionHandle resultHandle = Program_GetProgramReflection(Handle, targetIndex);

        if (resultHandle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to get shader reflection for target index {targetIndex}: {GetLastError() ?? "<No error was returned from Slang>"}");

        return new ShaderReflection(this, resultHandle);
    }
}
