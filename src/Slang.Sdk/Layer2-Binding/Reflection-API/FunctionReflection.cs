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
    internal unsafe class FunctionReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override FunctionReflectionHandle Handle { get; }

        internal FunctionReflection(Reflection parent, FunctionReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string GetName()
        {
            string? error = null;
            // Test string?
            return Call(() => SlangNativeInterop.FunctionReflection_GetName(Handle, out error), () => error);
        }

        internal TypeReflection GetReturnType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetReturnType(Handle, out error), () => error));
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.FunctionReflection_GetParameterCount(Handle, out error), () => error);
        }

        internal VariableReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            string? error = null;
            return new ModifierReflection(this, Call(() => StrongTypeInterop.FunctionReflection_FindModifier(Handle, modifierId, out error), () => error));
        }

        internal uint GetUserAttributeCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.FunctionReflection_GetUserAttributeCount(Handle, out error), () => error);
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            string? error = null;
            return new AttributeReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetUserAttributeByIndex(Handle, index, out error), () => error));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            string? error = null;
            return new AttributeReflection(
                this,
                Call(() => StrongTypeInterop.FunctionReflection_FindAttributeByName(Handle, name, out error), () => error));
        }

        internal GenericReflection GetGenericContainer()
        {
            string? error = null;
            return new GenericReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetGenericContainer(Handle, out error), () => error));
        }

        internal FunctionReflection ApplySpecializations(GenericReflection genRef)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_ApplySpecializations(Handle, genRef.Handle, out error), () => error));
        }

        internal FunctionReflection SpecializeWithArgTypes(uint typeCount, void** types)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_SpecializeWithArgTypes(Handle, typeCount, types, out error), () => error));
        }

        internal bool IsOverloaded()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.FunctionReflection_IsOverloaded(Handle, out error), () => error);
        }

        internal uint GetOverloadCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.FunctionReflection_GetOverloadCount(Handle, out error), () => error);
        }

        internal FunctionReflection GetOverload(uint index)
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetOverload(Handle, index, out error), () => error));
        }
    }
}
