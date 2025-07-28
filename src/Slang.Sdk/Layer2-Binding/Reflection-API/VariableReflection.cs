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

        internal string GetName()
        {string? error = null;
            return Call(() => SlangNativeInterop.VariableReflection_GetName(Handle, out error), () => error);
        }

        internal new TypeReflection GetType()
        {string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetType(Handle, out error), () => error));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {string? error = null;
            return new ModifierReflection(this, Call(() => StrongTypeInterop.VariableReflection_FindModifier(Handle, modifierId, out error), () => error));
        }

        internal uint GetUserAttributeCount()
        {string? error = null;
            return Call(() => SlangNativeInterop.VariableReflection_GetUserAttributeCount(Handle, out error), () => error);
        }

        internal AttributeReflection GetUserAttributeByIndex(uint index)
        {string? error = null;
            return new AttributeReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetUserAttributeByIndex(Handle, index, out error), () => error));
        }

        internal AttributeReflection FindAttributeByName(string name)
        {string? error = null;
            return new AttributeReflection(
                this, 
                Call(() => StrongTypeInterop.VariableReflection_FindAttributeByName(Handle, name, out error), () => error));
        }

        internal AttributeReflection FindUserAttributeByName(string name)
        {string? error = null;
            return new AttributeReflection(
                this, 
                Call(() => StrongTypeInterop.VariableReflection_FindUserAttributeByName(Handle, name, out error), () => error));
        }

        internal bool HasDefaultValue()
        {string? error = null;
            return Call(() => SlangNativeInterop.VariableReflection_HasDefaultValue(Handle, out error), () => error);
        }

        internal long GetDefaultValueInt()
        {string? error = null;
            return Call(() =>
            {
                long value;
                SlangNativeInterop.VariableReflection_GetDefaultValueInt(Handle, &value, out error);
                return value;
            }, () => error);
        }

        internal GenericReflection GetGenericContainer()
        {string? error = null;
            return new GenericReflection(this, Call(() => StrongTypeInterop.VariableReflection_GetGenericContainer(Handle, out error), () => error));
        }

        internal VariableReflection ApplySpecializations(void** specializations, int count)
        {string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.VariableReflection_ApplySpecializations(Handle, specializations, count, out error), () => error));
        }
    }
}
