using Slang.Net.Extensions;

namespace Slang
{
    public unsafe class EntryPointReflection
    {
        internal Slang.Cpp.EntryPointReflection cppObj { get; }
        private ShaderReflection? _parent;

        public EntryPointReflection(Slang.Cpp.EntryPointReflection comObj)
        {
            cppObj = comObj;
        }

        public ShaderReflection Parent
        {
            get => _parent!;
            internal set => _parent = value;
        }

        public string Name => cppObj.Name;
        public string NameOverride => cppObj.NameOverride;

        // Parameters
        public uint ParameterCount => cppObj.ParameterCount;
        public VariableLayoutReflection GetParameterByIndex(uint index) =>
            new VariableLayoutReflection(cppObj.GetParameterByIndex(index));

        // Function information
        public FunctionReflection Function => new FunctionReflection(cppObj.Function);
        public SlangStage Stage => (SlangStage)cppObj.Stage;

        // Compute-specific properties
        public uint[] GetComputeThreadGroupSize()
        {
            return cppObj.GetComputeThreadGroupSize();
        }
        public uint? ComputeWaveSize => cppObj.ComputeWaveSize;
        public bool UsesAnySampleRateInput => cppObj.UsesAnySampleRateInput;

        // Layout information
        public VariableLayoutReflection VarLayout => new VariableLayoutReflection(cppObj.VarLayout);
        public TypeLayoutReflection TypeLayout => new TypeLayoutReflection(cppObj.TypeLayout);
        public VariableLayoutReflection ResultVarLayout => new VariableLayoutReflection(cppObj.ResultVarLayout);
        public bool HasDefaultConstantBuffer => cppObj.HasDefaultConstantBuffer;

        public string Compile()
        {
            // [Fix] This is a bit hacky, but it works for now.
            ShaderReflection parent = Parent;
            var entryPointIndex = parent.EntryPoints.IndexOf(this);

            var source = parent.Parent.CppObj.Compile((uint)entryPointIndex, 0);
            return source;
        }

        public override bool Equals(object? obj)
        {
            if (obj is EntryPointReflection other)
                return cppObj.Equals(other.cppObj);
            return false;
        }

        public bool Equals(EntryPointReflection? other)
        {
            if (other is null) return false;
            return cppObj.Equals(other.cppObj);
        }

        public static bool operator ==(EntryPointReflection? left, EntryPointReflection? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(EntryPointReflection? left, EntryPointReflection? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return cppObj.GetHashCode();
        }
    }
}