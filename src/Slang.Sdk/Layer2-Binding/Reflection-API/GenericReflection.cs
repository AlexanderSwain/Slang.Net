using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

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

        internal string GetName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.GenericReflection_GetName(Handle, out error), () => error);
        }

        internal uint GetTypeParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.GenericReflection_GetTypeParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetTypeParameter(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetTypeParameter(Handle, index, out error), () => error));
        }

        internal uint GetValueParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.GenericReflection_GetValueParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetValueParameter(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetValueParameter(Handle, index, out error), () => error));
        }

        internal uint GetTypeParameterConstraintCount(TypeParameterReflection typeParam)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.GenericReflection_GetTypeParameterConstraintCount(Handle, typeParam.Handle, out error), () => error);
        }

        internal TypeReflection GetTypeParameterConstraintType(TypeParameterReflection typeParam, uint index)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetTypeParameterConstraintType(Handle, typeParam.Handle, index, out error), () => error));
        }

        internal int GetInnerKind()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.GenericReflection_GetInnerKind(Handle, out error), () => error);
        }

        internal GenericReflection GetOuterGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetOuterGenericContainer(Handle, out error), () => error));
        }

        internal TypeReflection GetConcreteType(TypeParameterReflection typeParam)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.GenericReflection_GetConcreteType(Handle, typeParam.Handle, out error), () => error));
        }

        internal long GetConcreteIntVal(VariableReflection valueParam)
        {
            string? error = null;
            return Call(() =>
            {
                long value;
                SlangNativeInterop.GenericReflection_GetConcreteIntVal(Handle, valueParam.Handle, &value, out error);
                return value;
            }, () => error);
        }

        internal GenericReflection ApplySpecializations(GenericReflection genRef)
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongTypeInterop.GenericReflection_ApplySpecializations(Handle, genRef.Handle, out error), () => error));
        }
    }
}
