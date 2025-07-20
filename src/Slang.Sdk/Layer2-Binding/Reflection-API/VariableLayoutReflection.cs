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
    internal unsafe class VariableLayoutReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override VariableLayoutReflectionHandle Handle { get; }

        internal VariableLayoutReflection(Reflection parent, VariableLayoutReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal VariableReflection GetVariable()
        {
            return new VariableReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetVariable(Handle)));
        }

        internal string? GetName()
        {
            return Call(() => FromUtf8(SlangNativeInterop.VariableLayoutReflection_GetName(Handle)));
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            return new ModifierReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_FindModifier(Handle, modifierId)));
        }

        internal TypeLayoutReflection GetTypeLayout()
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetTypeLayout(Handle)));
        }

        internal ParameterCategory GetCategory()
        {
            return (ParameterCategory)Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategory(Handle));
        }

        internal uint GetCategoryCount()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategoryCount(Handle));
        }

        internal ParameterCategory GetCategoryByIndex(uint index)
        {
            return (ParameterCategory)Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategoryByIndex(Handle, index));
        }

        internal nuint GetOffset(ParameterCategory category)
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetOffset(Handle, (int)category));
        }

        internal new TypeReflection GetType()
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetType(Handle)));
        }

        internal uint GetBindingIndex()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetBindingIndex(Handle));
        }

        internal uint GetBindingSpace()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetBindingSpace(Handle));
        }

        internal VariableLayoutReflection GetSpace(ParameterCategory category)
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetSpace(Handle, (int)category)));
        }

        internal int GetImageFormat()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetImageFormat(Handle));
        }

        internal string? GetSemanticName()
        {
            return Call(() => FromUtf8(SlangNativeInterop.VariableLayoutReflection_GetSemanticName(Handle)));
        }

        internal nuint GetSemanticIndex()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetSemanticIndex(Handle));
        }

        internal uint GetStage()
        {
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetStage(Handle));
        }

        internal VariableLayoutReflection GetPendingDataLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetPendingDataLayout(Handle)));
        }
    }
}
