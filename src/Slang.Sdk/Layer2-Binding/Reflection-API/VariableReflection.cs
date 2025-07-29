using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongInterop;
using static Slang.Sdk.Interop.StrongInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class VariableReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override VariableReflectionHandle Handle { get; }

        internal VariableReflection(Reflection parent, VariableReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => StrongInterop.VariableReflection.GetName(Handle, out error), () => error);
        }

        internal new TypeReflection GetType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.VariableReflection.GetType(Handle, out error), () => error));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            string? error = null;
            return new ModifierReflection(this, Call(() => StrongInterop.VariableReflection.FindModifier(Handle, modifierId, out error), () => error));
        }

        internal uint GetUserAttributeCount()
        {
            string? error = null;
            return Call(() => StrongInterop.VariableReflection.GetUserAttributeCount(Handle, out error), () => error);
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            string? error = null;
            return new AttributeReflection(this, Call(() => StrongInterop.VariableReflection.GetUserAttributeByIndex(Handle, index, out error), () => error));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            string? error = null;
            return new AttributeReflection(
                this,
                Call(() => StrongInterop.VariableReflection.FindAttributeByName(Handle, name, out error), () => error));
        }

        internal AttributeReflection FindUserAttributeByName(string name)
        {
            string? error = null;
            return new AttributeReflection(
                this,
                Call(() => StrongInterop.VariableReflection.FindUserAttributeByName(Handle, name, out error), () => error));
        }

        internal bool HasDefaultValue()
        {
            string? error = null;
            return Call(() => StrongInterop.VariableReflection.HasDefaultValue(Handle, out error), () => error);
        }

        internal long GetDefaultValueInt()
        {
            string? error = null;
            return Call(() =>
            {
                long value;
                StrongInterop.VariableReflection.GetDefaultValueInt(Handle, out value, out error);
                return value;
            }, () => error);
        }

        internal GenericReflection GetGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongInterop.VariableReflection.GetGenericContainer(Handle, out error), () => error));
        }

        internal VariableReflection ApplySpecializations(void** specializations, int count)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongInterop.VariableReflection.ApplySpecializations(Handle, specializations, count, out error), () => error));
        }
    }
}
