using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class ModifierReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override ModifierReflectionHandle Handle { get; }

        internal ModifierReflection(Reflection parent, ModifierReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal int GetID()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.Modifier_GetID(Handle, out error), () => error);
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.Modifier_GetName(Handle, out error), () => error);

        }
    }
}
