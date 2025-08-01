using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal unsafe class EntryPointReflection : Reflection, IEquatable<EntryPointReflection>
    {
        internal override Reflection? Parent { get; }
        internal override EntryPointReflectionHandle Handle { get; }
        internal override EntryPointReflectionHandle NativeHandle => new(StrongInterop.EntryPointReflection.GetNative(Handle, out var _));


        internal EntryPointReflection(Reflection parent, EntryPointReflectionHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        internal ShaderReflection GetParent()
        {
            string? error = null;
            var parentHandle = Call(() => StrongInterop.EntryPointReflection.GetParent(Handle, out error), () => error);
            return new ShaderReflection(null!, parentHandle); // Parent will be set by ShaderReflection
        }

        internal FunctionReflection AsFunction()
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongInterop.EntryPointReflection.AsFunction(Handle, out error), () => error));
        }

        internal string GetName()
        {
            string? error = null;
            return Call(() => StrongInterop.EntryPointReflection.GetName(Handle, out error), () => error);
        }

        internal string? GetNameOverride()
        {
            string? error = null;
            // Test string?
            return Call(() => StrongInterop.EntryPointReflection.GetNameOverride(Handle, out error), () => error);
        }

        internal uint GetParameterCount()
        {
            string? error = null;
            return Call(() => StrongInterop.EntryPointReflection.GetParameterCount(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetParameterByIndex(uint index)
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.EntryPointReflection.GetParameterByIndex(Handle, index, out error), () => error));
        }

        internal FunctionReflection GetFunction()
        {
            string? error = null;
            return new FunctionReflection(this, Call(() => StrongInterop.EntryPointReflection.GetFunction(Handle, out error), () => error));
        }

        internal Stage GetStage()
        {
            string? error = null;
            return (Stage)Call(() => StrongInterop.EntryPointReflection.GetStage(Handle, out error), () => error);
        }

        internal void GetComputeThreadGroupSize(uint axisCount, ulong* outSizeAlongAxis)
        {
            string? error = null;
            Call(() =>
            {
                StrongInterop.EntryPointReflection.GetComputeThreadGroupSize(Handle, axisCount, outSizeAlongAxis, out error);
                return 0;
            }, () => error);
        }

        internal ulong GetComputeWaveSize()
        {
            string? error = null;
            return Call(() =>
            {
                ulong waveSize;
                StrongInterop.EntryPointReflection.GetComputeWaveSize(Handle, out waveSize, out error);
                return waveSize;
            }, () => error);
        }

        internal bool UsesAnySampleRateInput()
        {
            string? error = null;
            return Call(() => StrongInterop.EntryPointReflection.UsesAnySampleRateInput(Handle, out error), () => error);
        }

        internal VariableLayoutReflection GetVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.EntryPointReflection.GetVarLayout(Handle, out error), () => error));
        }

        internal TypeLayoutReflection GetTypeLayout()
        {
            string? error = null;
            return new TypeLayoutReflection(this, Call(() => StrongInterop.EntryPointReflection.GetTypeLayout(Handle, out error), () => error));
        }

        internal VariableLayoutReflection GetResultVarLayout()
        {
            string? error = null;
            return new VariableLayoutReflection(this, Call(() => StrongInterop.EntryPointReflection.GetResultVarLayout(Handle, out error), () => error));
        }

        internal bool HasDefaultConstantBuffer()
        {
            string? error = null;
            return Call(() => StrongInterop.EntryPointReflection.HasDefaultConstantBuffer(Handle, out error), () => error);
        }

        public bool Equals(EntryPointReflection? other)
        {
            return this == other;
        }
    }
}
