using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeParameterReflection : Reflection, IEquatable<TypeParameterReflection>
    {
        internal override Reflection? Parent { get; }
        internal override TypeParameterReflectionHandle Handle { get; }
        internal override TypeParameterReflectionHandle NativeHandle => new(StrongInterop.TypeParameterReflection.GetNative(Handle, out var _));


        internal TypeParameterReflection(Reflection parent, TypeParameterReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeParameterReflection.GetName(Handle, out error), () => error);
        }

        internal uint GetIndex()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeParameterReflection.GetIndex(Handle, out error), () => error);
        }

        internal uint GetConstraintCount()
        {
            string? error = null;
            return Call(() => StrongInterop.TypeParameterReflection.GetConstraintCount(Handle, out error), () => error);
        }

        internal TypeReflection GetConstraintByIndex(int index)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.TypeParameterReflection.GetConstraintByIndex(Handle, index, out error), () => error));
        }

        public bool Equals(TypeParameterReflection? other)
        {
            return this == other;
        }
    }
}
