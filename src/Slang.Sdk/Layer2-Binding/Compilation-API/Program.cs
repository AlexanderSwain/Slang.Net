using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;
internal sealed unsafe class Program
{
    internal Module Parent { get; }
    internal ProgramHandle Handle { get; }

    internal Program(Module parent)
    {
        Parent = parent;
        Handle = new ProgramHandle(
            Program_Create(Parent.Handle)
        );

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
        SlangResult compileResult = CompileProgram(Handle, entryPointIndex, targetIndex, &compiledSource);
        string? diagnostics = GetLastError();

        return new CompilationResult(new string(compiledSource), target, null/*EntryPoint is not yet implemented*/, compileResult, diagnostics);
    }

    internal ShaderReflection GetReflection(uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        ShaderReflectionHandle resultHandle = new ShaderReflectionHandle(
            GetProgramReflection(Handle, targetIndex)
        );

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to get shader reflection for target index {targetIndex}: {GetLastError() ?? "<No error was returned from Slang>"}");

        return new ShaderReflection(this, resultHandle);
    }
}
