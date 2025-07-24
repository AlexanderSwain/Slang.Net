using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeReflectionHandle Handle { get; }

        internal TypeReflection(Reflection parent, TypeReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal TypeKind GetKind()
        {
            return (TypeKind)Call(() => SlangNativeInterop.TypeReflection_GetKind(Handle));
        }

        internal uint GetFieldCount()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetFieldCount(Handle));
        }

        internal VariableReflection GetFieldByIndex(uint index)
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetFieldByIndex(Handle, index)));
        }

        internal bool IsArray()
        {
            return Call(() => SlangNativeInterop.TypeReflection_IsArray(Handle));
        }

        internal TypeReflection UnwrapArray()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_UnwrapArray(Handle)));
        }

        internal nuint GetElementCount()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetElementCount(Handle));
        }

        internal TypeReflection GetElementType()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetElementType(Handle)));
        }

        internal uint GetRowCount()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetRowCount(Handle));
        }

        internal uint GetColumnCount()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetColumnCount(Handle));
        }

        internal ScalarType GetScalarType()
        {
            return (ScalarType)Call(() => SlangNativeInterop.TypeReflection_GetScalarType(Handle));
        }

        internal TypeReflection GetResourceResultType()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetResourceResultType(Handle)));
        }

        internal int GetResourceShape()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetResourceShape(Handle));
        }

        internal int GetResourceAccess()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetResourceAccess(Handle));
        }

        internal string? GetName()
        {
            return Call(() => FromUtf8((byte*)SlangNativeInterop.TypeReflection_GetName(Handle)));
        }

        internal uint GetUserAttributeCount()
        {
            return Call(() => SlangNativeInterop.TypeReflection_GetUserAttributeCount(Handle));
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            return new AttributeReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetUserAttributeByIndex(Handle, index)));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            return new AttributeReflection(
                this, 
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.TypeReflection_FindAttributeByName(Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal TypeReflection ApplySpecializations(GenericReflection genRef)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeReflection_ApplySpecializations(Handle, genRef.Handle)));
        }

        internal GenericReflection GetGenericContainer()
        {
            return new GenericReflection(this, Call(() => StrongTypeInterop.TypeReflection_GetGenericContainer(Handle)));
        }
    }
}
