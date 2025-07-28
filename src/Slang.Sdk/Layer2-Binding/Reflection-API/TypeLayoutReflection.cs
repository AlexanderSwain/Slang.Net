using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeLayoutReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeLayoutReflectionHandle Handle { get; }

        internal TypeLayoutReflection(Reflection parent, TypeLayoutReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal new TypeReflection GetType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetType(Handle, out error), () => error));
        }

        internal TypeKind GetKind()
        {
            string? error = null;
            return (TypeKind)Call(() => SlangNativeInterop.TypeLayoutReflection_GetKind(Handle, out error), () => error);
        }

        internal nuint GetSize(ParameterCategory category)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetSize(Handle, (int)category, out error), () => error);
        }

        internal nuint GetStride(ParameterCategory category)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetStride(Handle, (int)category, out error), () => error);
        }

        internal int GetAlignment(ParameterCategory category)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetAlignment(Handle, (int)category, out error), () => error);
        }

        internal uint GetFieldCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetFieldCount(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetFieldByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetFieldByIndex(Handle, index, out error), () => error));
        }

        internal int FindFieldIndexByName(string name)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_FindFieldIndexByName(Handle, name, out error), () => error);
        }

        internal VariableLayoutReflection GetExplicitCounter()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetExplicitCounter(Handle, out error), () => error));
        }

        internal bool IsArray()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_IsArray(Handle, out error), () => error);
        }

        internal TypeLayoutReflection UnwrapArray()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_UnwrapArray(Handle, out error), () => error));
        }

        internal nuint GetElementCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetElementCount(Handle, out error), () => error);
        }

        internal nuint GetTotalArrayElementCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetTotalArrayElementCount(Handle, out error), () => error);
        }

        internal nuint GetElementStride(ParameterCategory category)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetElementStride(Handle, (int)category, out error), () => error);
        }

        internal TypeLayoutReflection GetElementTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetElementTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetElementVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetElementVarLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetContainerVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetContainerVarLayout(Handle, out error), () => error));
        }
    }
}
