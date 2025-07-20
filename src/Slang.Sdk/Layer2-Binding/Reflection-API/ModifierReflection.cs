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
            return Call(() => SlangNativeInterop.Modifier_GetID(Handle));
        }

        internal string? GetName()
        {
            return Call(() => FromUtf8(SlangNativeInterop.Modifier_GetName(Handle)));
        }
    }
}
