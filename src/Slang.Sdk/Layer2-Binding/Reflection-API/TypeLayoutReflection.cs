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
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetType(Handle)));
        }

        internal TypeKind GetKind()
        {
            return (TypeKind)Call(() => SlangNativeInterop.TypeLayoutReflection_GetKind(Handle));
        }

        internal nuint GetSize(ParameterCategory category)
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetSize(Handle, (int)category));
        }

        internal nuint GetStride(ParameterCategory category)
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetStride(Handle, (int)category));
        }

        internal int GetAlignment(ParameterCategory category)
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetAlignment(Handle, (int)category));
        }

        internal uint GetFieldCount()
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetFieldCount(Handle));
        }

        internal VariableLayoutReflection GetFieldByIndex(uint index)
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetFieldByIndex(Handle, index)));
        }

        internal int FindFieldIndexByName(string name)
        {
            var namePtr = ToUtf8(name);
            try
            {
                return Call(() => SlangNativeInterop.TypeLayoutReflection_FindFieldIndexByName(Handle, namePtr));
            }
            finally
            {
                FreeUtf8(namePtr);
            }
        }

        internal VariableLayoutReflection GetExplicitCounter()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetExplicitCounter(Handle)));
        }

        internal bool IsArray()
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_IsArray(Handle));
        }

        internal TypeLayoutReflection UnwrapArray()
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_UnwrapArray(Handle)));
        }

        internal nuint GetElementCount()
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetElementCount(Handle));
        }

        internal nuint GetTotalArrayElementCount()
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetTotalArrayElementCount(Handle));
        }

        internal nuint GetElementStride(ParameterCategory category)
        {
            return Call(() => SlangNativeInterop.TypeLayoutReflection_GetElementStride(Handle, (int)category));
        }

        internal TypeLayoutReflection GetElementTypeLayout()
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetElementTypeLayout(Handle)));
        }

        internal VariableLayoutReflection GetElementVarLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetElementVarLayout(Handle)));
        }

        internal VariableLayoutReflection GetContainerVarLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.TypeLayoutReflection_GetContainerVarLayout(Handle)));
        }
    }
}
