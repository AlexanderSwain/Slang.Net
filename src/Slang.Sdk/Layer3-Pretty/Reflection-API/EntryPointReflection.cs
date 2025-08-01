using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public unsafe class EntryPointReflection : Reflection, IEquatable<EntryPointReflection>,
        IComposition<VariableLayout>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.EntryPointReflection Binding { get; }

        internal EntryPointReflection(Reflection parent, Binding.EntryPointReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<VariableLayout>

        uint IComposition<VariableLayout>.Count => Binding.GetParameterCount();
        VariableLayout IComposition<VariableLayout>.GetByIndex(uint index) =>
            new VariableLayout(this, Binding.GetParameterByIndex(index));

        #endregion

        #region Collections

        SlangCollection<VariableLayout>? _Parameters;
        public SlangCollection<VariableLayout> Parameters => 
            _Parameters ??= new SlangCollection<VariableLayout>(this);

        #endregion

        #region Pretty
        public string Name => Binding.GetName();
        public string? NameOverride => Binding.GetNameOverride();
        public Stage Stage => Binding.GetStage();
        public bool UsesAnySampleRateInput => Binding.UsesAnySampleRateInput();
        public bool HasDefaultConstantBuffer => Binding.HasDefaultConstantBuffer();

        public Function Function => new Function(this, Binding.GetFunction());
        public VariableLayout VarLayout => new VariableLayout(this, Binding.GetVarLayout());
        public TypeLayout TypeLayout => new TypeLayout(this, Binding.GetTypeLayout());
        public VariableLayout? ResultVarLayout => 
            Binding.GetResultVarLayout() is { } result ? new VariableLayout(this, result) : null;

        public ulong[] GetComputeThreadGroupSize(uint axisCount)
        {
            var sizes = new ulong[axisCount];
            fixed (ulong* sizesPtr = sizes)
            {
                Binding.GetComputeThreadGroupSize(axisCount, sizesPtr);
            }
            return sizes;
        }

        public ulong GetComputeWaveSize()
        {
            return Binding.GetComputeWaveSize();
        }

        #endregion

        #region Equality
        public bool Equals(EntryPointReflection? other)
        {
            return this == other;
        }
        #endregion
    }
}
