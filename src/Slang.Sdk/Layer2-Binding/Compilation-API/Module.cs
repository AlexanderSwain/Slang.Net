using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;

internal unsafe sealed class Module
{
    internal Session Parent { get; }
    internal Interop.ModuleHandle Handle { get; }

    public Module(Session parent, string moduleName, string modulePath, string shaderSource)
    {
        Parent = parent;
        fixed (char* pName = moduleName)
        fixed (char* pPath = modulePath)
        fixed (char* pSource = shaderSource)
        {
            // Using the strongly-typed interop that returns ModuleHandle directly
            Handle = CreateModule(Parent.Handle, pName, pPath, pSource);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
    }

    ~Module()
    {
        Handle?.Dispose();
    }
}