using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeLayoutReflection : Reflection, IEquatable<TypeLayoutReflection>
    {
        internal override Reflection? Parent { get; }
        internal override TypeLayoutReflectionHandle Handle { get; }
        internal override TypeLayoutReflectionHandle NativeHandle => new(StrongInterop.TypeLayoutReflection.GetNative(Handle, out var _));


        internal TypeLayoutReflection(Reflection parent, TypeLayoutReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal new TypeReflection GetType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetType(Handle, out error), () => error));
        }

        internal TypeKind GetKind()
        {
            string? error = null;
            return (TypeKind)Call(() => StrongInterop.TypeLayoutReflection.GetKind(Handle, out error), () => error);
        }

        internal nuint GetSize(ParameterCategory category)
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetSize(Handle, (int)category, out error), () => error);
        }

        internal nuint GetStride(ParameterCategory category)
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetStride(Handle, (int)category, out error), () => error);
        }

        internal int GetAlignment(ParameterCategory category)
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetAlignment(Handle, (int)category, out error), () => error);
        }

        internal uint GetFieldCount()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetFieldCount(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetFieldByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetFieldByIndex(Handle, index, out error), () => error));
        }

        internal int FindFieldIndexByName(string name)
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.FindFieldIndexByName(Handle, name, out error), () => error);
        }

        internal VariableLayoutReflection GetExplicitCounter()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetExplicitCounter(Handle, out error), () => error));
        }

        internal bool IsArray()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.IsArray(Handle, out error), () => error);
        }

        internal TypeLayoutReflection UnwrapArray()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.UnwrapArray(Handle, out error), () => error));
        }

        internal nuint GetElementCount()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetElementCount(Handle, out error), () => error);
        }

        internal nuint GetTotalArrayElementCount()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetTotalArrayElementCount(Handle, out error), () => error);
        }

        internal nuint GetElementStride(ParameterCategory category)
        {
            string? error = null;
            return Call(() => StrongInterop.TypeLayoutReflection.GetElementStride(Handle, (int)category, out error), () => error);
        }

        internal TypeLayoutReflection GetElementTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetElementTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetElementVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetElementVarLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetContainerVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.TypeLayoutReflection.GetContainerVarLayout(Handle, out error), () => error));
        }

        public bool Equals(TypeLayoutReflection? other)
        {
            return this == other;
        }
    }
}
