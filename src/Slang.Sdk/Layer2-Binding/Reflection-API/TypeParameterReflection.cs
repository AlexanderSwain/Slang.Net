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
    internal unsafe class TypeParameterReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override TypeParameterReflectionHandle Handle { get; }

        internal TypeParameterReflection(Reflection parent, TypeParameterReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal string? GetName()
        {
            return Call(() => new string(SlangNativeInterop.TypeParameterReflection_GetName(Handle)));
        }

        internal uint GetIndex()
        {
            return Call(() => SlangNativeInterop.TypeParameterReflection_GetIndex(Handle));
        }

        internal uint GetConstraintCount()
        {
            return Call(() => SlangNativeInterop.TypeParameterReflection_GetConstraintCount(Handle));
        }

        internal TypeReflection GetConstraintByIndex(int index)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.TypeParameterReflection_GetConstraintByIndex(Handle, index)));
        }
    }
}
