using static Slang.Sdk.Interop.StrongInterop;
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
    public override nint NativeHandle => StrongInterop.Session.GetNative(this, out var _);
    public SessionHandle() { }

    public SessionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.Session.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang module objects.
/// </summary>
public sealed class ModuleHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.Module.GetNative(this, out var _);
    public ModuleHandle() { }

    public ModuleHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.Module.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang entry point objects.
/// </summary>
public sealed class EntryPointHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.EntryPoint.GetNative(this, out var _);
    public EntryPointHandle() { }

    public EntryPointHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.EntryPoint.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang program objects.
/// </summary>
public sealed class ProgramHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.Program.GetNative(this, out var _);
    public ProgramHandle() { }

    public ProgramHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.Program.Release(this, out var _);
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
    public override nint NativeHandle => StrongInterop.Attribute.GetNative(this, out var _);
    public AttributeReflectionHandle() { }

    public AttributeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.Attribute.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang EntryPoint objects.
/// </summary>
public sealed class EntryPointReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.EntryPointReflection.GetNative(this, out var _);
    public EntryPointReflectionHandle() { }

    public EntryPointReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.EntryPointReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Function objects.
/// </summary>
public sealed class FunctionReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.FunctionReflection.GetNative(this, out var _);
    public FunctionReflectionHandle() { }

    public FunctionReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.FunctionReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Generic objects.
/// </summary>
public sealed class GenericReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.GenericReflection.GetNative(this, out var _);
    public GenericReflectionHandle() { }

    public GenericReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.GenericReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Modifier objects.
/// </summary>
public sealed class ModifierReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.Modifier.GetNative(this, out var _);
    public ModifierReflectionHandle() { }

    public ModifierReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.Modifier.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang ShaderReflection objects.
/// </summary>
public sealed class ShaderReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.ShaderReflection.GetNative(this, out var _);
    public ShaderReflectionHandle() { }

    public ShaderReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.ShaderReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeLayout objects.
/// </summary>
public sealed class TypeLayoutReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.TypeLayoutReflection.GetNative(this, out var _);
    public TypeLayoutReflectionHandle() { }

    public TypeLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.TypeLayoutReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeParameter objects.
/// </summary>
public sealed class TypeParameterReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.TypeParameterReflection.GetNative(this, out var _);
    public TypeParameterReflectionHandle() { }

    public TypeParameterReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.TypeParameterReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang TypeReflection objects.
/// </summary>
public sealed class TypeReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.TypeReflection.GetNative(this, out var _);
    public TypeReflectionHandle() { }

    public TypeReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.TypeReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang VariableLayout objects.
/// </summary>
public sealed class VariableLayoutReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.VariableLayoutReflection.GetNative(this, out var _);
    public VariableLayoutReflectionHandle() { }

    public VariableLayoutReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.VariableLayoutReflection.Release(this, out var _);
        return true;
    }
}

/// <summary>
/// Safe handle for Slang Variable objects.
/// </summary>
public sealed class VariableReflectionHandle : SlangHandle
{
    public override nint NativeHandle => StrongInterop.VariableReflection.GetNative(this, out var _);
    public VariableReflectionHandle() { }

    public VariableReflectionHandle(nint handle) : base(handle) { }

    protected override bool ReleaseHandle()
    {
        // Currently does nothing since it only contain fields that are managed by Slang
        // In a future update, maybe cascade delete the children
        StrongInterop.VariableReflection.Release(this, out var _);
        return true;
    }
}

#endregion