using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.SlangNativeInterop;

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

        internal TypeKind Kind
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal uint FieldCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal VariableReflection GetFieldByIndex(uint index)
        {
            throw new NotImplementedException();
        }

        internal bool IsArray
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal TypeReflection UnwrapArray()
        {
            return new TypeReflection(
                this,
                Call(() => new TypeReflectionHandle(TypeReflection_UnwrapArray(Handle)))
            );
        }
    }
}
