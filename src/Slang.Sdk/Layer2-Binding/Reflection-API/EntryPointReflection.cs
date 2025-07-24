using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class EntryPointReflection : Reflection
    {
        internal override Reflection? Parent { get; }
        internal override EntryPointReflectionHandle Handle { get; }

        internal EntryPointReflection(Reflection parent, EntryPointReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal ShaderReflection GetParent()
        {
            var parentHandle = Call(() => StrongTypeInterop.EntryPointReflection_GetParent(Handle));
            return new ShaderReflection(null!, parentHandle); // Parent will be set by ShaderReflection
        }

        internal FunctionReflection AsFunction()
        {
            return new FunctionReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_AsFunction(Handle)));
        }

        internal string? GetName()
        {
            return Call(() => FromCharPtr(SlangNativeInterop.EntryPointReflection_GetName(Handle)));
        }

        internal string? GetNameOverride()
        {
            return Call(() => FromCharPtr(SlangNativeInterop.EntryPointReflection_GetNameOverride(Handle)));
        }

        internal uint GetParameterCount()
        {
            return Call(() => SlangNativeInterop.EntryPointReflection_GetParameterCount(Handle));
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetParameterByIndex(Handle, index)));
        }

        internal FunctionReflection GetFunction()
        {
            return new FunctionReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetFunction(Handle)));
        }

        internal ShaderStage GetStage()
        {
            return (ShaderStage)Call(() => SlangNativeInterop.EntryPointReflection_GetStage(Handle));
        }

        internal void GetComputeThreadGroupSize(uint axisCount, ulong* outSizeAlongAxis)
        {
            Call(() => 
            {
                SlangNativeInterop.EntryPointReflection_GetComputeThreadGroupSize(Handle, axisCount, outSizeAlongAxis);
                return 0;
            });
        }

        internal ulong GetComputeWaveSize()
        {
            return Call(() =>
            {
                ulong waveSize;
                SlangNativeInterop.EntryPointReflection_GetComputeWaveSize(Handle, &waveSize);
                return waveSize;
            });
        }

        internal bool UsesAnySampleRateInput()
        {
            return Call(() => SlangNativeInterop.EntryPointReflection_UsesAnySampleRateInput(Handle));
        }

        internal VariableLayoutReflection GetVarLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetVarLayout(Handle)));
        }

        internal TypeLayoutReflection GetTypeLayout()
        {
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetTypeLayout(Handle)));
        }

        internal VariableLayoutReflection GetResultVarLayout()
        {
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetResultVarLayout(Handle)));
        }

        internal bool HasDefaultConstantBuffer()
        {
            return Call(() => SlangNativeInterop.EntryPointReflection_HasDefaultConstantBuffer(Handle));
        }
    }
}
