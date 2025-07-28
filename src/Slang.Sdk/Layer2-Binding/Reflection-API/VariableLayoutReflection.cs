using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

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
            string? error = null;
            return new VariableReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetVariable(Handle, out error), () => error));
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetName(Handle, out error), () => error);
        }

        internal ModifierReflection FindModifier(int modifierId)
        {
            string? error = null;
            return new ModifierReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_FindModifier(Handle, modifierId, out error), () => error));
        }

        internal TypeLayoutReflection GetTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetTypeLayout(Handle, out error), () => error));
        }

        internal ParameterCategory GetCategory()
        {
            string? error = null;
            return (ParameterCategory)Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategory(Handle, out error), () => error);
        }

        internal uint GetCategoryCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategoryCount(Handle, out error), () => error);
        }

        internal ParameterCategory GetCategoryByIndex(uint index)
        {
            string? error = null;
            return (ParameterCategory)Call(() => SlangNativeInterop.VariableLayoutReflection_GetCategoryByIndex(Handle, index, out error), () => error);
        }

        internal nuint GetOffset(ParameterCategory category)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetOffset(Handle, (int)category, out error), () => error);
        }

        internal new TypeReflection GetType()
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetType(Handle, out error), () => error));
        }

        internal uint GetBindingIndex()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetBindingIndex(Handle, out error), () => error);
        }

        internal uint GetBindingSpace()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetBindingSpace(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetSpace(ParameterCategory category)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetSpace(Handle, (int)category, out error), () => error));
        }

        internal int GetImageFormat()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetImageFormat(Handle, out error), () => error);
        }

        internal string GetSemanticName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetSemanticName(Handle, out error), () => error);
        }

        internal nuint GetSemanticIndex()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetSemanticIndex(Handle, out error), () => error);
        }

        internal uint GetStage()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.VariableLayoutReflection_GetStage(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetPendingDataLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.VariableLayoutReflection_GetPendingDataLayout(Handle, out error), () => error));
        }
    }
}
