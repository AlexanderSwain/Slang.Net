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
        //var entryPoint = Parent.Parent.EntryPoints.ElementAt(entryPointIndex);
        var target = Parent.Parent.Targets.ElementAt((int)targetIndex);
        char* compiledSource = null;
        SlangResult compileResult = SlangNativeInterop.Program_CompileProgram(Handle, entryPointIndex, targetIndex, &compiledSource);
        string? diagnostics = GetLastError();

        return new CompilationResult(StringMarshaling.FromUtf8((byte*)compiledSource), target, null/*EntryPoint is not yet implemented*/, compileResult, diagnostics);
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
