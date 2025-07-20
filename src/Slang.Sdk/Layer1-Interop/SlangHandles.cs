using static Slang.Sdk.Interop.SlangNativeInterop;
using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// Base class for safe handles to native Slang objects.
/// </summary>
public abstract class SlangHandle : SafeHandle
{
    /// <summary>
    /// Gets the native handle as an nint.
    /// </summary>
    public nint Handle => handle;

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected SlangHandle() : base(IntPtr.Zero, true/*Always 1 SafeHandle per Slang object*/)
    {
    }

    protected SlangHandle(nint handle) : base(IntPtr.Zero, true/*Always 1 SafeHandle per Slang object*/)
    {
        base.SetHandle(handle);
    }

    // Expose the SetHandle method as internal.
    internal new void SetHandle(nint handle)
    {
        base.SetHandle(handle);
    }

    /// <summary>
    /// Implicitly converts the safe handle to an nint.
    /// </summary>
    public static implicit operator nint(SlangHandle? slangHandle)
    {
        return slangHandle?.handle ?? IntPtr.Zero;
    }

    public override string ToString()
    {
        return $"{GetType().Name}{{ Handle = 0x{Handle:X} }}";
    }
}

#region Compilation API
/// <summary>
/// Safe handle for Slang session objects.
/// </summary>
public sealed class SessionHandle : SlangHandle
{
    public SessionHandle() { }

    public SessionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Session_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang module objects.
/// </summary>
public sealed class ModuleHandle : SlangHandle
{
    public ModuleHandle() { }

    public ModuleHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Module_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang program objects.
/// </summary>
public sealed class ProgramHandle : SlangHandle
{
    public ProgramHandle() { }

    public ProgramHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Program_Release(Handle);
        return true;
    }
}

#endregion

#region Reflection API
/// <summary>
/// Safe handle for Slang Attribute objects.
/// </summary>
public sealed class AttributeReflectionHandle : SlangHandle
{
    public AttributeReflectionHandle() { }

    public AttributeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Attribute_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang EntryPoint objects.
/// </summary>
public sealed class EntryPointReflectionHandle : SlangHandle
{
    public EntryPointReflectionHandle() { }

    public EntryPointReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        EntryPointReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Function objects.
/// </summary>
public sealed class FunctionReflectionHandle : SlangHandle
{
    public FunctionReflectionHandle() { }

    public FunctionReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        FunctionReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Generic objects.
/// </summary>
public sealed class GenericReflectionHandle : SlangHandle
{
    public GenericReflectionHandle() { }

    public GenericReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        GenericReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Modifier objects.
/// </summary>
public sealed class ModifierReflectionHandle : SlangHandle
{
    public ModifierReflectionHandle() { }

    public ModifierReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Modifier_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang ShaderReflection objects.
/// </summary>
public sealed class ShaderReflectionHandle : SlangHandle
{
    public ShaderReflectionHandle() { }

    public ShaderReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        ShaderReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeLayout objects.
/// </summary>
public sealed class TypeLayoutReflectionHandle : SlangHandle
{
    public TypeLayoutReflectionHandle() { }

    public TypeLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeLayoutReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeParameter objects.
/// </summary>
public sealed class TypeParameterReflectionHandle : SlangHandle
{
    public TypeParameterReflectionHandle() { }

    public TypeParameterReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeParameterReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeReflection objects.
/// </summary>
public sealed class TypeReflectionHandle : SlangHandle
{
    public TypeReflectionHandle() { }

    public TypeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang VariableLayout objects.
/// </summary>
public sealed class VariableLayoutReflectionHandle : SlangHandle
{
    public VariableLayoutReflectionHandle() { }

    public VariableLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        VariableLayoutReflection_Release(Handle);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Variable objects.
/// </summary>
public sealed class VariableReflectionHandle : SlangHandle
{
    public VariableReflectionHandle() { }

    public VariableReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        VariableReflection_Release(Handle);
        return true;
    }
}

#endregion