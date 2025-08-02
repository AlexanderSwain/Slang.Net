using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding;
internal sealed unsafe class Program : CompilationBinding, IEquatable<Program>
{
    internal Module Parent { get; }
    internal override ProgramHandle Handle { get; }
    internal override ProgramHandle NativeHandle => new(StrongInterop.Program.GetNative(Handle, out var _));


    internal Program(Module parent)
    {
        Parent = parent;
        // Using the strongly-typed interop that returns ProgramHandle directly
        Handle = StrongInterop.Program.Create(Parent.Handle, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang program: {error ?? "<No error was returned from Slang>"}");
    }

    ~Program()
    {
        Handle?.Dispose();
    }

    internal CompilationResult CompileTarget(uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        Target target = Parent.Parent.Targets.ElementAt((int)targetIndex);

        SlangResult compileResult = StrongInterop.Program.CompileTarget(Handle, targetIndex, out byte[]? compiledSource, out string? error);

        return new CompilationResult(compiledSource ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), targetIndex, null, compileResult, error);
    }

    internal CompilationResult CompileEntryPoint(uint entryPointIndex, uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        SlangResult compileResult = StrongInterop.Program.CompileEntryPoint(Handle, entryPointIndex, targetIndex, out byte[]? compiledSource, out string? error);

        return new CompilationResult(compiledSource ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), targetIndex, entryPointIndex, compileResult, error);
    }

    internal ShaderReflection GetReflection(uint targetIndex)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

        // Using the strongly-typed interop that returns ShaderReflectionHandle directly
        ShaderReflectionHandle resultHandle = StrongInterop.Program.GetProgramReflection(Handle, targetIndex, out var error);

        if (resultHandle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to get shader reflection for target index {targetIndex}: {error ?? "<No error was returned from Slang>"}");

        return new ShaderReflection(this, resultHandle);
    }

    #region Equality
    public static bool operator ==(Program? left, Program? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        if (left.NativeHandle == right.NativeHandle) return true;
        return false;
    }

    public static bool operator !=(Program? left, Program? right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (left is null || right is null) return true;
        if (left.NativeHandle == right.NativeHandle) return false;
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Program entryPoint) return Equals(entryPoint);
        return false;
    }

    public bool Equals(Program? other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return NativeHandle.GetHashCode();
    }
    #endregion
}
