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
    internal unsafe class AttributeReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override AttributeReflectionHandle Handle { get; }

        internal AttributeReflection(Reflection parent, AttributeReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string? GetName()
        {
            return Call(() => FromUtf8(SlangNativeInterop.Attribute_GetName(Handle)));
        }

        internal uint GetArgumentCount()
        {
            return Call(() => SlangNativeInterop.Attribute_GetArgumentCount(Handle));
        }

        internal TypeReflection GetArgumentType(uint index)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.Attribute_GetArgumentType(Handle, index)));
        }

        internal int GetArgumentValueInt(uint index)
        {
            return Call(() =>
            {
                int value;
                SlangNativeInterop.Attribute_GetArgumentValueInt(Handle, index, &value);
                return value;
            });
        }

        internal float GetArgumentValueFloat(uint index)
        {
            return Call(() =>
            {
                float value;
                SlangNativeInterop.Attribute_GetArgumentValueFloat(Handle, index, &value);
                return value;
            });
        }

        internal string? GetArgumentValueString(uint index)
        {
            return Call(() => FromUtf8(SlangNativeInterop.Attribute_GetArgumentValueString(Handle, index)));
        }
    }
}
