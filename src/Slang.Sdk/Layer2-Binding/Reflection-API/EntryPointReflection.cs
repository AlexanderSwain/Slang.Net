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
            string? error = null;
            var parentHandle = Call(() => StrongTypeInterop.EntryPointReflection_GetParent(Handle, out error), () => error);
            return new ShaderReflection(null!, parentHandle); // Parent will be set by ShaderReflection
        }

        internal FunctionReflection AsFunction()
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_AsFunction(Handle, out error), () => error));
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.EntryPointReflection_GetName(Handle, out error), () => error);
        }

        internal string? GetNameOverride()
        {
            string? error = null;
            // Test string?
            return Call(() => SlangNativeInterop.EntryPointReflection_GetNameOverride(Handle, out error), () => error);
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.EntryPointReflection_GetParameterCount(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal FunctionReflection GetFunction()
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetFunction(Handle, out error), () => error));
        }

        internal ShaderStage GetStage()
        {
            string? error = null;
            return (ShaderStage)Call(() => SlangNativeInterop.EntryPointReflection_GetStage(Handle, out error), () => error);
        }

        internal void GetComputeThreadGroupSize(uint axisCount, ulong* outSizeAlongAxis)
        {
            string? error = null;
            Call(() =>
            {
                SlangNativeInterop.EntryPointReflection_GetComputeThreadGroupSize(Handle, axisCount, outSizeAlongAxis, out error);
                return 0;
            }, () => error);
        }

        internal ulong GetComputeWaveSize()
        {
            string? error = null;
            return Call(() =>
            {
                ulong waveSize;
                SlangNativeInterop.EntryPointReflection_GetComputeWaveSize(Handle, &waveSize, out error);
                return waveSize;
            }, () => error);
        }

        internal bool UsesAnySampleRateInput()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.EntryPointReflection_UsesAnySampleRateInput(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetVarLayout(Handle, out error), () => error));
        }

        internal TypeLayoutReflection GetTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetResultVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongTypeInterop.EntryPointReflection_GetResultVarLayout(Handle, out error), () => error));
        }

        internal bool HasDefaultConstantBuffer()
        {
            string? error = null;
            return Call(() => SlangNativeInterop.EntryPointReflection_HasDefaultConstantBuffer(Handle, out error), () => error);
        }
    }
}
