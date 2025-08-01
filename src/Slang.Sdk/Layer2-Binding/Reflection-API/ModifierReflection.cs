using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class ModifierReflection : Reflection, IEquatable<ModifierReflection>
    {
        internal override Reflection? Parent { get; }
        internal override ModifierReflectionHandle Handle { get; }
        internal override ModifierReflectionHandle NativeHandle => new(StrongInterop.Modifier.GetNative(Handle, out var _));


        internal ModifierReflection(Reflection parent, ModifierReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal int GetID()
        {
            string? error = null;
            return Call(() => StrongInterop.Modifier.GetID(Handle, out error), () => error);
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => StrongInterop.Modifier.GetName(Handle, out error), () => error);

        }

        public bool Equals(ModifierReflection? other)
        {
            return this == other;
        }
    }
}
