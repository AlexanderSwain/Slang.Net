using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class ShaderReflection : Reflection
    {
        internal Program Program { get; }

        internal override Reflection? Parent => null;
        internal override ShaderReflectionHandle Handle { get; }

        internal ShaderReflection(Program program, ShaderReflectionHandle handle)
        {
            Program = program;
            Handle = handle;
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetParameterCount(Handle, out error), () => error);
        }

        internal uint GetTypeParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetTypeParameterCount(Handle, out error), () => error);
        }

        internal TypeParameterReflection GetTypeParameterByIndex(uint index)
        {
            string? error = null;
            return new TypeParameterReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetTypeParameterByIndex(Handle, index, out error), () => error));
        }

        internal TypeParameterReflection FindTypeParameter(string name)
        {
            string? error = null;
            return new TypeParameterReflection(
                this,
                Call(() => StrongTypeInterop.ShaderReflection_FindTypeParameter(Handle, name, out error), () => error));
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal uint GetEntryPointCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetEntryPointCount(Handle, out error), () => error);
        }

        internal EntryPointReflection GetEntryPointByIndex(uint index)
        {
            string? error = null;
            return new EntryPointReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetEntryPointByIndex(Handle, index, out error), () => error));
        }

        internal EntryPointReflection FindEntryPointByName(string name)
        {
            string? error = null;
            return new EntryPointReflection(
                this,
                Call(() => StrongTypeInterop.ShaderReflection_FindEntryPointByName(Handle, name, out error), () => error));
        }

        internal uint GetGlobalConstantBufferBinding()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferBinding(Handle, out error), () => error);
        }

        internal nuint GetGlobalConstantBufferSize()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferSize(Handle, out error), () => error);
        }

        internal TypeReflection FindTypeByName(string name)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.ShaderReflection_FindTypeByName(Handle, name, out error), () => error));
        }

        internal FunctionReflection FindFunctionByName(string name)
        {
            string? error = null;
            return new FunctionReflection(
                this,
                Call(() => StrongTypeInterop.ShaderReflection_FindFunctionByName(Handle, name, out error), () => error));
        }

        internal FunctionReflection FindFunctionByNameInType(TypeReflection type, string name)
        {
            string? error = null;
            return new FunctionReflection(
                this,
                Call(() => StrongTypeInterop.ShaderReflection_FindFunctionByNameInType(Handle, type.Handle, name, out error), () => error));
        }

        internal VariableReflection FindVarByNameInType(TypeReflection type, string name)
        {
            string? error = null;
            return new VariableReflection(
                this,
                Call(() => StrongTypeInterop.ShaderReflection_FindVarByNameInType(Handle, type.Handle, name, out error), () => error));
        }

        internal TypeLayoutReflection GetTypeLayout(TypeReflection type, int layoutRules)
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetTypeLayout(Handle, type.Handle, layoutRules, out error), () => error));
        }

        internal TypeReflection SpecializeType(TypeReflection type, int argCount, void** args)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongTypeInterop.ShaderReflection_SpecializeType(Handle, type.Handle, argCount, args, out error), () => error));
        }

        internal bool IsSubType(TypeReflection subType, TypeReflection superType)
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_IsSubType(Handle, subType.Handle, superType.Handle, out error), () => error);
        }

        internal uint GetHashedStringCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.ShaderReflection_GetHashedStringCount(Handle, out error), () => error);
        }

        internal string? GetHashedString(uint index)
        {
            string? error = null;
            // Test string?
            return Call(() => SlangNativeInterop.ShaderReflection_GetHashedString(Handle, index, out error), () => error);
        }

        internal TypeLayoutReflection GetGlobalParamsTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetGlobalParamsTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetGlobalParamsVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetGlobalParamsVarLayout(Handle, out error), () => error));
        }

        internal string? ToJson()
        {
            string? error = null;
            return Call(() =>
            {
                SlangNativeInterop.ShaderReflection_ToJson(Handle, out string output, out error);
                return output;
            }, () => error);
        }
    }
}
