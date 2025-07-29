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
            return Call(() => StrongInterop.Attribute.GetName(Handle, out error), () => error);
        }

        internal uint GetArgumentCount()
        {
            string? error = null;
            return Call(() => StrongInterop.Attribute.GetArgumentCount(Handle, out error), () => error);
        }

        internal TypeReflection GetArgumentType(uint index)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.Attribute.GetArgumentType(Handle, index, out error), () => error));
        }

        internal int GetArgumentValueInt(uint index)
        {
            string? error = null;
            return Call(() =>
            {
                int value;
                StrongInterop.Attribute.GetArgumentValueInt(Handle, index, out value, out error);
                return value;
            }, () => error);
        }

        internal float GetArgumentValueFloat(uint index)
        {
            string? error = null;
            return Call(() =>
            {
                float value;
                StrongInterop.Attribute.GetArgumentValueFloat(Handle, index, out value, out error);
                return value;
            }, () => error);
        }

        internal string? GetArgumentValueString(uint index)
        {
            string? error = null;
            // Test string?
            return Call(() => StrongInterop.Attribute.GetArgumentValueString(Handle, index, out error), () => error);
        }
    }
}
