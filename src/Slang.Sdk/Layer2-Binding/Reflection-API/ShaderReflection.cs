using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding
{
    internal unsafe class ShaderReflection : Reflection, IEquatable<ShaderReflection>
    {
        internal Program Program { get; }

        internal override Reflection? Parent => null;
        internal override ShaderReflectionHandle Handle { get; }
        internal override ShaderReflectionHandle NativeHandle => new(StrongInterop.ShaderReflection.GetNative(Handle, out var _));


        internal ShaderReflection(Program program, ShaderReflectionHandle handle)
        {
            Program = program;
            Handle = handle;
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetParameterCount(Handle, out error), () => error);
        }

        internal uint GetTypeParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetTypeParameterCount(Handle, out error), () => error);
        }

        internal TypeParameterReflection GetTypeParameterByIndex(uint index)
        {
            string? error = null;
            return new TypeParameterReflection(this, Call(() => StrongInterop.ShaderReflection.GetTypeParameterByIndex(Handle, index, out error), () => error));
        }

        internal TypeParameterReflection FindTypeParameter(string name)
        {
            string? error = null;
            return new TypeParameterReflection(
                this,
                Call(() => StrongInterop.ShaderReflection.FindTypeParameter(Handle, name, out error), () => error));
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.ShaderReflection.GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal uint GetEntryPointCount()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetEntryPointCount(Handle, out error), () => error);
        }

        internal EntryPointReflection GetEntryPointByIndex(uint index)
        {
            string? error = null;
            return new EntryPointReflection(this, Call(() => StrongInterop.ShaderReflection.GetEntryPointByIndex(Handle, index, out error), () => error));
        }

        internal EntryPointReflection FindEntryPointByName(string name)
        {
            string? error = null;
            return new EntryPointReflection(
                this,
                Call(() => StrongInterop.ShaderReflection.FindEntryPointByName(Handle, name, out error), () => error));
        }

        internal uint GetGlobalConstantBufferBinding()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetGlobalConstantBufferBinding(Handle, out error), () => error);
        }

        internal nuint GetGlobalConstantBufferSize()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetGlobalConstantBufferSize(Handle, out error), () => error);
        }

        internal TypeReflection FindTypeByName(string name)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.ShaderReflection.FindTypeByName(Handle, name, out error), () => error));
        }

        internal FunctionReflection FindFunctionByName(string name)
        {
            string? error = null;
            return new FunctionReflection(
                this,
                Call(() => StrongInterop.ShaderReflection.FindFunctionByName(Handle, name, out error), () => error));
        }

        internal FunctionReflection FindFunctionByNameInType(TypeReflection type, string name)
        {
            string? error = null;
            return new FunctionReflection(
                this,
                Call(() => StrongInterop.ShaderReflection.FindFunctionByNameInType(Handle, type.Handle, name, out error), () => error));
        }

        internal VariableReflection FindVarByNameInType(TypeReflection type, string name)
        {
            string? error = null;
            return new VariableReflection(
                this,
                Call(() => StrongInterop.ShaderReflection.FindVarByNameInType(Handle, type.Handle, name, out error), () => error));
        }

        internal TypeLayoutReflection GetTypeLayout(TypeReflection type, int layoutRules)
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongInterop.ShaderReflection.GetTypeLayout(Handle, type.Handle, layoutRules, out error), () => error));
        }

        internal TypeReflection SpecializeType(TypeReflection type, int argCount, void** args)
        {
            string? error = null;
            return new TypeReflection(this, Call(() => StrongInterop.ShaderReflection.SpecializeType(Handle, type.Handle, argCount, args, out error), () => error));
        }

        internal bool IsSubType(TypeReflection subType, TypeReflection superType)
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.IsSubType(Handle, subType.Handle, superType.Handle, out error), () => error);
        }

        internal uint GetHashedStringCount()
        {
            string? error = null;
            return Call(() => StrongInterop.ShaderReflection.GetHashedStringCount(Handle, out error), () => error);
        }

        internal string? GetHashedString(uint index)
        {
            string? error = null;
            // Test string?
            return Call(() => StrongInterop.ShaderReflection.GetHashedString(Handle, index, out error), () => error);
        }

        internal TypeLayoutReflection GetGlobalParamsTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongInterop.ShaderReflection.GetGlobalParamsTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetGlobalParamsVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.ShaderReflection.GetGlobalParamsVarLayout(Handle, out error), () => error));
        }

        internal string? ToJson()
        {
            string? error = null;
            return Call(() =>
            {
                StrongInterop.ShaderReflection.ToJson(Handle, out string output, out error);
                return output;
            }, () => error);
        }

        public bool Equals(ShaderReflection? other)
        {
            return this == other;
        }
    }
}
