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
            return Call(() => StrongInterop.GenericReflection.GetName(Handle, out error), () => error);
        }

        internal uint GetTypeParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.GenericReflection.GetTypeParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetTypeParameter(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongInterop.GenericReflection.GetTypeParameter(Handle, index, out error), () => error));
        }

        internal uint GetValueParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.GenericReflection.GetValueParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetValueParameter(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongInterop.GenericReflection.GetValueParameter(Handle, index, out error), () => error));
        }

        internal uint GetTypeParameterConstraintCount(TypeParameterReflection typeParam)
        {
            string? error = null;
            return Call(() => StrongInterop.GenericReflection.GetTypeParameterConstraintCount(Handle, typeParam.Handle, out error), () => error);
        }

        internal TypeReflection GetTypeParameterConstraintType(TypeParameterReflection typeParam, uint index)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.GenericReflection.GetTypeParameterConstraintType(Handle, typeParam.Handle, index, out error), () => error));
        }

        internal int GetInnerKind()
        {
            string? error = null;
            return Call(() => StrongInterop.GenericReflection.GetInnerKind(Handle, out error), () => error);
        }

        internal GenericReflection GetOuterGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongInterop.GenericReflection.GetOuterGenericContainer(Handle, out error), () => error));
        }

        internal TypeReflection GetConcreteType(TypeParameterReflection typeParam)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.GenericReflection.GetConcreteType(Handle, typeParam.Handle, out error), () => error));
        }

        internal long GetConcreteIntVal(VariableReflection valueParam)
        {
            string? error = null;
            return Call(() =>
            {
                long value;
                StrongInterop.GenericReflection.GetConcreteIntVal(Handle, valueParam.Handle, out value, out error);
                return value;
            }, () => error);
        }

        internal GenericReflection ApplySpecializations(GenericReflection genRef)
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongInterop.GenericReflection.ApplySpecializations(Handle, genRef.Handle, out error), () => error));
        }
    }
}
