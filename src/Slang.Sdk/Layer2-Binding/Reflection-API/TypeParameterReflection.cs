using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class TypeParameterReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeParameterReflectionHandle Handle { get; }

        internal TypeParameterReflection(Reflection parent, TypeParameterReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string GetName()
        {string? error = null;
            return Call(() => SlangNativeInterop.TypeParameterReflection_GetName(Handle, out error), () => error);
        }

        internal uint GetIndex()
        {string? error = null;
            return Call(() => SlangNativeInterop.TypeParameterReflection_GetIndex(Handle, out error), () => error);
        }

        internal uint GetConstraintCount()
        {string? error = null;
            return Call(() => SlangNativeInterop.TypeParameterReflection_GetConstraintCount(Handle, out error), () => error);
        }

        internal TypeReflection GetConstraintByIndex(int index)
        {string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeParameterReflection_GetConstraintByIndex(Handle, index, out error), () => error));
        }
    }
}
