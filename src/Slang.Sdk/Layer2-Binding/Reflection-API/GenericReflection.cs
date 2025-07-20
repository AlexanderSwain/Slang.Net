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
    internal unsafe class GenericReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override GenericReflectionHandle Handle { get; }

        internal GenericReflection(Reflection parent, GenericReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string? GetName()
        {
            return Call(() => new string(SlangNativeInterop.GenericReflection_GetName(Handle)));
        }

        internal uint GetTypeParameterCount()
        {
            return Call(() => SlangNativeInterop.GenericReflection_GetTypeParameterCount(Handle));
        }

        internal VariableReflection GetTypeParameter(uint index)
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetTypeParameter(Handle, index)));
        }

        internal uint GetValueParameterCount()
        {
            return Call(() => SlangNativeInterop.GenericReflection_GetValueParameterCount(Handle));
        }

        internal VariableReflection GetValueParameter(uint index)
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetValueParameter(Handle, index)));
        }

        internal uint GetTypeParameterConstraintCount(TypeParameterReflection typeParam)
        {
            return Call(() => SlangNativeInterop.GenericReflection_GetTypeParameterConstraintCount(Handle, typeParam.Handle));
        }

        internal TypeReflection GetTypeParameterConstraintType(TypeParameterReflection typeParam, uint index)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetTypeParameterConstraintType(Handle, typeParam.Handle, index)));
        }

        internal int GetInnerKind()
        {
            return Call(() => SlangNativeInterop.GenericReflection_GetInnerKind(Handle));
        }

        internal GenericReflection GetOuterGenericContainer()
        {
            return new GenericReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetOuterGenericContainer(Handle)));
        }

        internal TypeReflection GetConcreteType(TypeParameterReflection typeParam)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetConcreteType(Handle, typeParam.Handle)));
        }

        internal long GetConcreteIntVal(VariableReflection valueParam)
        {
            return Call(() =>
            {
                long value;
                SlangNativeInterop.GenericReflection_GetConcreteIntVal(Handle, valueParam.Handle, &value);
                return value;
            });
        }

        internal GenericReflection ApplySpecializations(GenericReflection genRef)
        {
            return new GenericReflection(this, Call(() => StrongTypeInterop.GenericReflection_ApplySpecializations(Handle, genRef.Handle)));
        }
    }
}
