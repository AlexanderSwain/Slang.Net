using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.Utilities;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding;

internal unsafe sealed class Module : CompilationBinding
{
    internal Session Parent { get; }
    internal override Interop.ModuleHandle Handle { get; }

    public Module(Session parent, string moduleName, string modulePath, string shaderSource)
    {
        Parent = parent;

        // Using the strongly-typed interop that returns ModuleHandle directly
        Handle = Module_Create(Parent.Handle, moduleName, modulePath, shaderSource, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {error ?? "<No error was returned from Slang>"}");
    }

    public Module(Session parent, string moduleName)
    {
        Parent = parent;

        // Convert managed strings to UTF-8 before passing to native API
        byte* pName = ToUtf8(moduleName);

        Handle = Module_Import(Parent.Handle, moduleName, out var error);

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
            return SlangNativeInterop.Module_GetName(Handle, out var error) ?? throw new SlangException(SlangResult.Fail, error ?? "Failed to retrieve module name: <No name was returned from Slang>");
        }
    }

    internal uint GetEntryPointCount()
    {
        string? error = null;
        return Call(() => SlangNativeInterop.Module_GetEntryPointCount(Handle, out error), () => error);
    }

    internal EntryPoint GetEntryPointByIndex(uint index)
    {
        string? error = null;
        return new EntryPoint(this, Call(() => StrongTypeInterop.Module_GetEntryPointByIndex(Handle, index, out error), () => error));
    }

    internal EntryPoint GetEntryPointByName(string name)
    {
        string? error = null;
        return new EntryPoint(this, Call(() => StrongTypeInterop.Module_FindEntryPointByName(Handle, name, out error), () => error));
    }

    ~Module()
    {
        Handle?.Dispose();
    }
}