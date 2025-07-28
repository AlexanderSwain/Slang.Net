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
            string? error = null;
            return Call(() => SlangNativeInterop.Attribute_GetName(Handle, out error), () => error);
        }

        internal uint GetArgumentCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.Attribute_GetArgumentCount(Handle, out error), () => error);
        }

        internal TypeReflection GetArgumentType(uint index)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.Attribute_GetArgumentType(Handle, index, out error), () => error));
        }

        internal int GetArgumentValueInt(uint index)
        {
            string? error = null;
            return Call(() =>
            {
                int value;
                SlangNativeInterop.Attribute_GetArgumentValueInt(Handle, index, &value, out error);
                return value;
            }, () => error);
        }

        internal float GetArgumentValueFloat(uint index)
        {
            string? error = null;
            return Call(() =>
            {
                float value;
                SlangNativeInterop.Attribute_GetArgumentValueFloat(Handle, index, &value, out error);
                return value;
            }, () => error);
        }

        internal string? GetArgumentValueString(uint index)
        {
            string? error = null;
            // Test string?
            return Call(() => SlangNativeInterop.Attribute_GetArgumentValueString(Handle, index, out error), () => error);
        }
    }
}
