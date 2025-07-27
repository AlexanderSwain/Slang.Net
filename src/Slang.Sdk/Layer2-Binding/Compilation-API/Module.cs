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
        
        // Convert managed strings to UTF-8 before passing to native API
        byte* pName = ToUtf8(moduleName);
        byte* pPath = ToUtf8(modulePath);
        byte* pSource = ToUtf8(shaderSource);
        
        try
        {
            // Using the strongly-typed interop that returns ModuleHandle directly
            Handle = Module_Create(Parent.Handle, (char*)pName, (char*)pPath, (char*)pSource);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
        finally
        {
            // Clean up allocated UTF-8 strings
            FreeUtf8(pName);
            FreeUtf8(pPath);
            FreeUtf8(pSource);
        }
    }

    public Module(Session parent, string moduleName)
    {
        Parent = parent;

        // Convert managed strings to UTF-8 before passing to native API
        byte* pName = ToUtf8(moduleName);

        try
        {
            // Using the strongly-typed interop that returns ModuleHandle directly
            Handle = Module_Import(Parent.Handle, (char*)pName);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
        finally
        {
            // Clean up allocated UTF-8 strings
            FreeUtf8(pName);
        }
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
            char* pName = SlangNativeInterop.Module_GetName(Handle);
            if (pName == null)
                throw new SlangException(SlangResult.Fail, "Failed to retrieve module name: <No name was returned from Slang>");

            return FromUtf8((byte*)pName) ?? throw new SlangException(SlangResult.Fail, "Failed to convert module name from UTF-8");
        }
    }

    internal uint GetEntryPointCount()
    {
        return Call(() => SlangNativeInterop.Module_GetEntryPointCount(Handle));
    }

    internal EntryPoint GetEntryPointByIndex(uint index)
    {
        return new EntryPoint(this, Call(() => StrongTypeInterop.Module_GetEntryPointByIndex(Handle, index)));
    }

    internal EntryPoint GetEntryPointByName(string name)
    {
        // Convert managed strings to UTF-8 before passing to native API
        byte* pName = ToUtf8(name);
        try
        {
            return new EntryPoint(this, Call(() => StrongTypeInterop.Module_FindEntryPointByName(Handle, (char*)pName)));
        }
        finally
        {
            // Clean up allocated UTF-8 strings
            FreeUtf8(pName);
        }
    }

    ~Module()
    {
        Handle?.Dispose();
    }
}