using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding;

internal unsafe sealed class Module : CompilationBinding, IEquatable<Module>
{
    internal Session Parent { get; }
    internal override Interop.ModuleHandle Handle { get; }
    internal override Interop.ModuleHandle NativeHandle => new(StrongInterop.Module.GetNative(Handle, out var _));


    public Module(Session parent, string moduleName, string modulePath, string shaderSource)
    {
        Parent = parent;

        // Using the strongly-typed interop that returns ModuleHandle directly
        Handle = StrongInterop.Module.Create(Parent.Handle, moduleName, modulePath, shaderSource, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {error ?? "<No error was returned from Slang>"}");
    }

    public Module(Session parent, string moduleName)
    {
        Parent = parent;

        // Convert managed strings to UTF-8 before passing to native API
        byte* pName = ToUtf8(moduleName);

        Handle = StrongInterop.Module.Import(Parent.Handle, moduleName, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {error ?? "<No error was returned from Slang>"}");
    }

    public Module(Session parent, Interop.ModuleHandle handle)
    {
        Parent = parent;
        Handle = handle;
    }

    public string Name
    {
        get
        {
            // Use call, for consistency with other properties
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

            // Using the strongly-typed interop to get the name of the module
            return StrongInterop.Module.GetName(Handle, out var error) ?? throw new SlangException(SlangResult.Fail, error ?? "Failed to retrieve module name: <No name was returned from Slang>");
        }
    }

    ~Module()
    {
        Handle?.Dispose();
    }

    #region Equality
    public static bool operator ==(Module? left, Module? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        if (left.NativeHandle == right.NativeHandle) return true;
        return false;
    }

    public static bool operator !=(Module? left, Module? right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (left is null || right is null) return true;
        if (left.NativeHandle == right.NativeHandle) return false;
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Module entryPoint) return Equals(entryPoint);
        return false;
    }

    public bool Equals(Module? other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return NativeHandle.GetHashCode();
    }
    #endregion
}