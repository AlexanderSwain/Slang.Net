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
    internal unsafe class VariableReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override VariableReflectionHandle Handle { get; }

        internal VariableReflection(Reflection parent, VariableReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string? GetName()
        {
            return Call(() => FromUtf8(SlangNativeInterop.VariableReflection_GetName(Handle)));
        }

        internal new TypeReflection GetType()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetType(Handle)));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            return new ModifierReflection(this, Call(() => StrongTypeInterop.VariableReflection_FindModifier(Handle, modifierId)));
        }

        internal uint GetUserAttributeCount()
        {
            return Call(() => SlangNativeInterop.VariableReflection_GetUserAttributeCount(Handle));
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {
            return new AttributeReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetUserAttributeByIndex(Handle, index)));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {
            var namePtr = ToUtf8(name);
            try
            {
                return new AttributeReflection(this, Call(() => StrongTypeInterop.VariableReflection_FindAttributeByName(Handle, namePtr)));
            }
            finally
            {
                FreeUtf8(namePtr);
            }
        }

        internal AttributeReflection FindUserAttributeByName(string name)
        {
            var namePtr = ToUtf8(name);
            try
            {
                return new AttributeReflection(this, Call(() => StrongTypeInterop.VariableReflection_FindUserAttributeByName(Handle, namePtr)));
            }
            finally
            {
                FreeUtf8(namePtr);
            }
        }

        internal bool HasDefaultValue()
        {
            return Call(() => SlangNativeInterop.VariableReflection_HasDefaultValue(Handle));
        }

        internal long GetDefaultValueInt()
        {
            return Call(() =>
            {
                long value;
                SlangNativeInterop.VariableReflection_GetDefaultValueInt(Handle, &value);
                return value;
            });
        }

        internal GenericReflection GetGenericContainer()
        {
            return new GenericReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetGenericContainer(Handle)));
        }

        internal VariableReflection ApplySpecializations(void** specializations, int count)
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.VariableReflection_ApplySpecializations(Handle, specializations, count)));
        }
    }
}
