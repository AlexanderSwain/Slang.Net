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
            return Call(() => SlangNativeInterop.ShaderReflection_GetParameterCount(Handle));
        }

        internal uint GetTypeParameterCount()
        {
            return Call(() => SlangNativeInterop.ShaderReflection_GetTypeParameterCount(Handle));
        }

        internal TypeParameterReflection GetTypeParameterByIndex(uint index)
        {
            return new TypeParameterReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetTypeParameterByIndex(Handle, index)));
        }

        internal TypeParameterReflection FindTypeParameter(string name)
        {
            return new TypeParameterReflection(
                this,
                Call(() =>
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindTypeParameter(Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetParameterByIndex(Handle, index)));
        }

        internal uint GetEntryPointCount()
        {
            return Call(() => SlangNativeInterop.ShaderReflection_GetEntryPointCount(Handle));
        }

        internal EntryPointReflection GetEntryPointByIndex(uint index)
        {
            return new EntryPointReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetEntryPointByIndex(Handle, index)));
        }

        internal EntryPointReflection FindEntryPointByName(string name)
        {
            return new EntryPointReflection(
                this, 
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindEntryPointByName(Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal uint GetGlobalConstantBufferBinding()
        {
            return Call(() => SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferBinding(Handle));
        }

        internal nuint GetGlobalConstantBufferSize()
        {
            return Call(() => SlangNativeInterop.ShaderReflection_GetGlobalConstantBufferSize(Handle));
        }

        internal TypeReflection FindTypeByName(string name)
        {
            return new TypeReflection(
                this, 
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindTypeByName(Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal FunctionReflection FindFunctionByName(string name)
        {
            return new FunctionReflection(
                this,
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindFunctionByName(Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal FunctionReflection FindFunctionByNameInType(TypeReflection type, string name)
        {
            return new FunctionReflection(
                this, 
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindFunctionByNameInType(Handle, type.Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                })
            );
        }

        internal VariableReflection FindVarByNameInType(TypeReflection type, string name)
        {
            return new VariableReflection(
                this, 
                Call(() => 
                {
                    byte* pName = ToUtf8(name);
                    try
                    {
                        return StrongTypeInterop.ShaderReflection_FindVarByNameInType(Handle, type.Handle, (char*)pName);
                    }
                    finally
                    {
                        FreeUtf8(pName);
                    }
                }
            ));
        }

        internal TypeLayoutReflection GetTypeLayout(TypeReflection type, int layoutRules)
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetTypeLayout(Handle, type.Handle, layoutRules)));
        }

        internal TypeReflection SpecializeType(TypeReflection type, int argCount, void** args)
        {
            return new TypeReflection(this, Call(() => StrongTypeInterop.ShaderReflection_SpecializeType(Handle, type.Handle, argCount, args)));
        }

        internal bool IsSubType(TypeReflection subType, TypeReflection superType)
        {
            return Call(() => SlangNativeInterop.ShaderReflection_IsSubType(Handle, subType.Handle, superType.Handle));
        }

        internal uint GetHashedStringCount()
        {
            return Call(() => SlangNativeInterop.ShaderReflection_GetHashedStringCount(Handle));
        }

        internal string? GetHashedString(uint index)
        {
            return Call(() => FromUtf8((byte*)SlangNativeInterop.ShaderReflection_GetHashedString(Handle, index)));
        }

        internal TypeLayoutReflection GetGlobalParamsTypeLayout()
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetGlobalParamsTypeLayout(Handle)));
        }

        internal VariableLayoutReflection GetGlobalParamsVarLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.ShaderReflection_GetGlobalParamsVarLayout(Handle)));
        }

        internal string? ToJson()
        {
            return Call(() =>
            {
                char* output = null;
                SlangNativeInterop.ShaderReflection_ToJson(Handle, &output);
                return FromUtf8((byte*)output);
            });
        }
    }
}
