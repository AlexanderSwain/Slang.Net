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

        internal string? GetName()
        {
            return Call(() => new string(SlangNativeInterop.FunctionReflection_GetName(Handle)));
        }

        internal TypeReflection GetReturnType()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetReturnType(Handle)));
        }

        internal uint GetParameterCount()
        {
            return Call(() => SlangNativeInterop.FunctionReflection_GetParameterCount(Handle));
        }

        internal VariableReflection GetParameterByIndex(uint index)
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetParameterByIndex(Handle, index)));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            return new ModifierReflection(this, Call(() => StrongTypeInterop.FunctionReflection_FindModifier(Handle, modifierId)));
        }

        internal uint GetUserAttributeCount()
        {
            return Call(() => SlangNativeInterop.FunctionReflection_GetUserAttributeCount(Handle));
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            return new AttributeReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetUserAttributeByIndex(Handle, index)));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
                return new AttributeReflection(
                    this, 
                    Call(() => 
                    {
                        fixed (char* namePtr = name)
                        {
                            return StrongTypeInterop.FunctionReflection_FindAttributeByName(Handle, namePtr);
                        }
                    }
                ));
        }

        internal GenericReflection GetGenericContainer()
        {
            return new GenericReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetGenericContainer(Handle)));
        }

        internal FunctionReflection ApplySpecializations(GenericReflection genRef)
        {
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_ApplySpecializations(Handle, genRef.Handle)));
        }

        internal FunctionReflection SpecializeWithArgTypes(uint typeCount, void** types)
        {
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_SpecializeWithArgTypes(Handle, typeCount, types)));
        }

        internal bool IsOverloaded()
        {
            return Call(() => SlangNativeInterop.FunctionReflection_IsOverloaded(Handle));
        }

        internal uint GetOverloadCount()
        {
            return Call(() => SlangNativeInterop.FunctionReflection_GetOverloadCount(Handle));
        }

        internal FunctionReflection GetOverload(uint index)
        {
            return new FunctionReflection(this, Call(() => StrongTypeInterop.FunctionReflection_GetOverload(Handle, index)));
        }
    }
}
