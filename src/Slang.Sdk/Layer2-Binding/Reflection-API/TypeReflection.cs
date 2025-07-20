using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;

namespace Slang.Sdk.Binding
{
    internal class TypeReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeReflectionHandle Handle { get; }

        internal TypeReflection(Reflection parent, TypeReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal TypeKind GetKind()
        {
            throw new NotImplementedException();
        }

        internal uint GetFieldCount()
        {
            throw new NotImplementedException();
        }

        internal VariableReflection GetFieldByIndex(uint index)
        {
            return new VariableReflection(this, Call(() => TypeReflection_GetFieldByIndex(Handle, index)));
        }

        internal bool IsArray()
        {
            throw new NotImplementedException();
        }

        internal TypeReflection UnwrapArray()
        {
            return new TypeReflection(this, Call(() => TypeReflection_UnwrapArray(Handle)));
        }
    }
}
