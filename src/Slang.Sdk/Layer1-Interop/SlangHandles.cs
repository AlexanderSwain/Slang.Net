using static Slang.Sdk.Interop.SlangNativeInterop;
using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// Base class for safe handles to native Slang objects.
/// </summary>
public abstract class SlangHandle : SafeHandle, IEquatable<SlangHandle>
{
    /// <summary>
    /// Gets the native handle as an nint.
    /// </summary>
    public nint Handle => handle;
    public abstract nint NativeHandle { get; }

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

    #region Equalality
    public static bool operator ==(SlangHandle? left, SlangHandle? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.NativeHandle == right.NativeHandle;
    }

    public static bool operator !=(SlangHandle? left, SlangHandle? right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (left is null || right is null) return true;
        return left.NativeHandle != right.NativeHandle;
    }

    public override bool Equals(object? obj)
    {
        if (obj is SlangHandle handle) return Equals(handle); 
        return false;
    }

    public bool Equals(SlangHandle? other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return NativeHandle.GetHashCode();
    }
    #endregion

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
    public override nint NativeHandle => SlangNativeInterop.Session_GetNative(Handle, out var _);
    public SessionHandle() { }

    public SessionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Session_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang module objects.
/// </summary>
public sealed class ModuleHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.Module_GetNative(Handle, out var _);
    public ModuleHandle() { }

    public ModuleHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Module_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang entry point objects.
/// </summary>
public sealed class EntryPointHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.EntryPoint_GetNative(Handle, out var _);
    public EntryPointHandle() { }

    public EntryPointHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        EntryPoint_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang program objects.
/// </summary>
public sealed class ProgramHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.Program_GetNative(Handle, out var _);
    public ProgramHandle() { }

    public ProgramHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Program_Release(Handle, out var _);
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
    public override nint NativeHandle => SlangNativeInterop.Attribute_GetNative(Handle, out var _);
    public AttributeReflectionHandle() { }

    public AttributeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Attribute_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang EntryPoint objects.
/// </summary>
public sealed class EntryPointReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.EntryPointReflection_GetNative(Handle, out var _);
    public EntryPointReflectionHandle() { }

    public EntryPointReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        EntryPointReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Function objects.
/// </summary>
public sealed class FunctionReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.FunctionReflection_GetNative(Handle, out var _);
    public FunctionReflectionHandle() { }

    public FunctionReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        FunctionReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Generic objects.
/// </summary>
public sealed class GenericReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.GenericReflection_GetNative(Handle, out var _);
    public GenericReflectionHandle() { }

    public GenericReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        GenericReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Modifier objects.
/// </summary>
public sealed class ModifierReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.Modifier_GetNative(Handle, out var _);
    public ModifierReflectionHandle() { }

    public ModifierReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        Modifier_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang ShaderReflection objects.
/// </summary>
public sealed class ShaderReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.ShaderReflection_GetNative(Handle, out var _);
    public ShaderReflectionHandle() { }

    public ShaderReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        ShaderReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeLayout objects.
/// </summary>
public sealed class TypeLayoutReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.TypeLayoutReflection_GetNative(Handle, out var _);
    public TypeLayoutReflectionHandle() { }

    public TypeLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeLayoutReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeParameter objects.
/// </summary>
public sealed class TypeParameterReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.TypeParameterReflection_GetNative(Handle, out var _);
    public TypeParameterReflectionHandle() { }

    public TypeParameterReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeParameterReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeReflection objects.
/// </summary>
public sealed class TypeReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.TypeReflection_GetNative(Handle, out var _);
    public TypeReflectionHandle() { }

    public TypeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        TypeReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang VariableLayout objects.
/// </summary>
public sealed class VariableLayoutReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.VariableLayoutReflection_GetNative(Handle, out var _);
    public VariableLayoutReflectionHandle() { }

    public VariableLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        VariableLayoutReflection_Release(Handle, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Variable objects.
/// </summary>
public sealed class VariableReflectionHandle : SlangHandle
{
    public override nint NativeHandle => SlangNativeInterop.VariableReflection_GetNative(Handle, out var _);
    public VariableReflectionHandle() { }

    public VariableReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        VariableReflection_Release(Handle, out var _);
        return true;
    }
}

#endregion