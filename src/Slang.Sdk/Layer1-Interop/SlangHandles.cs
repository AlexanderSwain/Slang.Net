using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// Base class for safe handles to native Slang objects.
/// </summary>
public abstract class SlangHandle : SafeHandle
{
    protected SlangHandle() : base(IntPtr.Zero, true)
    {
    }

    protected SlangHandle(nint handle, bool ownsHandle) : base(handle, ownsHandle)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// Gets the native handle as an nint.
    /// </summary>
    public nint Handle => handle;

    /// <summary>
    /// Implicitly converts the safe handle to an nint.
    /// </summary>
    public static implicit operator nint(SlangHandle? slangHandle)
    {
        return slangHandle?.handle ?? IntPtr.Zero;
    }
}

/// <summary>
/// Safe handle for Slang session objects.
/// </summary>
public sealed class SlangSessionHandle : SlangHandle
{
    public SlangSessionHandle() { }

    public SlangSessionHandle(nint handle) : base(handle, true) { }

    protected override bool ReleaseHandle()
    {
        // Sessions are typically cleaned up automatically by Slang
        // But we could add explicit cleanup here if needed
        return true;
    }
}

/// <summary>
/// Safe handle for Slang module objects.
/// </summary>
public sealed class SlangModuleHandle : SlangHandle
{
    public SlangModuleHandle() { }

    public SlangModuleHandle(nint handle) : base(handle, true) { }

    protected override bool ReleaseHandle()
    {
        // Modules are typically cleaned up automatically by Slang
        return true;
    }
}

/// <summary>
/// Safe handle for Slang program objects.
/// </summary>
public sealed class SlangProgramHandle : SlangHandle
{
    public SlangProgramHandle() { }

    public SlangProgramHandle(nint handle) : base(handle, true) { }

    protected override bool ReleaseHandle()
    {
        // Programs are typically cleaned up automatically by Slang
        return true;
    }
}

/// <summary>
/// Safe handle for Slang reflection objects that need explicit release.
/// </summary>
public sealed class SlangReflectionHandle : SlangHandle
{
    private readonly Action<nint>? _releaseAction;

    public SlangReflectionHandle() { }

    public SlangReflectionHandle(nint handle, Action<nint>? releaseAction = null) : base(handle, true)
    {
        _releaseAction = releaseAction;
    }

    protected override bool ReleaseHandle()
    {
        if (_releaseAction != null && handle != IntPtr.Zero)
        {
            _releaseAction(handle);
        }
        return true;
    }

    /// <summary>
    /// Creates a safe handle for a shader reflection object.
    /// </summary>
    public static SlangReflectionHandle ForShaderReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.ShaderReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a type reflection object.
    /// </summary>
    public static SlangReflectionHandle ForTypeReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.TypeReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a type layout reflection object.
    /// </summary>
    public static SlangReflectionHandle ForTypeLayoutReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.TypeLayoutReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a variable reflection object.
    /// </summary>
    public static SlangReflectionHandle ForVariableReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.VariableReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a variable layout reflection object.
    /// </summary>
    public static SlangReflectionHandle ForVariableLayoutReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.VariableLayoutReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a function reflection object.
    /// </summary>
    public static SlangReflectionHandle ForFunctionReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.FunctionReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for an entry point reflection object.
    /// </summary>
    public static SlangReflectionHandle ForEntryPointReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.EntryPointReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a generic reflection object.
    /// </summary>
    public static SlangReflectionHandle ForGenericReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.GenericReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for a type parameter reflection object.
    /// </summary>
    public static SlangReflectionHandle ForTypeParameterReflection(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.TypeParameterReflection_Release);
    }

    /// <summary>
    /// Creates a safe handle for an attribute reflection object.
    /// </summary>
    public static SlangReflectionHandle ForAttribute(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.Attribute_Release);
    }

    /// <summary>
    /// Creates a safe handle for a modifier object.
    /// </summary>
    public static SlangReflectionHandle ForModifier(nint handle)
    {
        return new SlangReflectionHandle(handle, SlangNativeInterop.Modifier_Release);
    }

    /// <summary>
    /// Creates a handle without automatic release (for objects that don't need explicit cleanup).
    /// </summary>
    public static SlangReflectionHandle WithoutRelease(nint handle)
    {
        return new SlangReflectionHandle(handle, null);
    }
}