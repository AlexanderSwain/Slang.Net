using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

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
            return Call(() => FromCharPtr(SlangNativeInterop.Attribute_GetName(Handle)));
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
            return Call(() => FromCharPtr(SlangNativeInterop.Attribute_GetArgumentValueString(Handle, index)));
        }
    }
}
