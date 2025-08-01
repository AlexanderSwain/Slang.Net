using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongInterop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class FunctionReflection : Reflection, IEquatable<FunctionReflection>
    {
        internal override Reflection? Parent { get; }
        internal override FunctionReflectionHandle Handle { get; }
        internal override FunctionReflectionHandle NativeHandle => new(StrongInterop.FunctionReflection.GetNative(Handle, out var _));


        internal FunctionReflection(Reflection parent, FunctionReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string GetName()
        {
            string? error = null;
            // Test string?
            return Call(() => StrongInterop.FunctionReflection.GetName(Handle, out error), () => error);
        }

        internal TypeReflection GetReturnType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.FunctionReflection.GetReturnType(Handle, out error), () => error));
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.FunctionReflection.GetParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongInterop.FunctionReflection.GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            string? error = null;
            return new ModifierReflection(this, Call(() => StrongInterop.FunctionReflection.FindModifier(Handle, modifierId, out error), () => error));
        }

        internal uint GetUserAttributeCount()
        {
            string? error = null;
            return Call(() => StrongInterop.FunctionReflection.GetUserAttributeCount(Handle, out error), () => error);
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            string? error = null;
            return new AttributeReflection(this, Call(() => StrongInterop.FunctionReflection.GetUserAttributeByIndex(Handle, index, out error), () => error));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            string? error = null;
            return new AttributeReflection(
                this,
                Call(() => StrongInterop.FunctionReflection.FindAttributeByName(Handle, name, out error), () => error));
        }

        internal GenericReflection GetGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongInterop.FunctionReflection.GetGenericContainer(Handle, out error), () => error));
        }

        internal FunctionReflection ApplySpecializations(GenericReflection genRef)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongInterop.FunctionReflection.ApplySpecializations(Handle, genRef.Handle, out error), () => error));
        }

        internal FunctionReflection SpecializeWithArgTypes(uint typeCount, void** types)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongInterop.FunctionReflection.SpecializeWithArgTypes(Handle, typeCount, types, out error), () => error));
        }

        internal bool IsOverloaded()
        {
            string? error = null;
            return Call(() => StrongInterop.FunctionReflection.IsOverloaded(Handle, out error), () => error);
        }

        internal uint GetOverloadCount()
        {
            string? error = null;
            return Call(() => StrongInterop.FunctionReflection.GetOverloadCount(Handle, out error), () => error);
        }

        internal FunctionReflection GetOverload(uint index)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongInterop.FunctionReflection.GetOverload(Handle, index, out error), () => error));
        }

        public bool Equals(FunctionReflection? other)
        {
            return this == other;
        }
    }
}
