using System.Data;

namespace Slang
{
    public unsafe class ShaderReflection :
        IComposedOf<TypeParameterReflection>,
        IComposedOf<VariableLayoutReflection>,
        IComposedOf<EntryPointReflection>
    {
        public ShaderProgram Parent { get; }
        internal Slang.Cpp.ShaderReflection cppObj { get; }

        #region Composed Of
        uint IComposedOf<TypeParameterReflection>.Count => TypeParameterCount;
        TypeParameterReflection IComposedOf<TypeParameterReflection>.GetByIndex(uint index) => GetTypeParameterByIndex(index);
        uint IComposedOf<VariableLayoutReflection>.Count => ParameterCount;
        VariableLayoutReflection IComposedOf<VariableLayoutReflection>.GetByIndex(uint index) => GetParameterByIndex(index);
        uint IComposedOf<EntryPointReflection>.Count => EntryPointCount;
        EntryPointReflection IComposedOf<EntryPointReflection>.GetByIndex(uint index) => GetEntryPointByIndex(index);
        #endregion

        public ShaderReflection(ShaderProgram shaderProgram, uint targetIndex)
        {
            Parent = shaderProgram;
            cppObj = new Slang.Cpp.ShaderReflection(shaderProgram.CppObj.GetReflection(targetIndex).getNative());
        }

        internal ShaderReflection(ShaderProgram parent, Slang.Cpp.ShaderReflection cppObj)
        {
            Parent = parent;
            this.cppObj = cppObj;
        }

        #region .Net Style Collection Abstractions
        private SlangCollection<TypeParameterReflection>? _TypeParameters;
        public SlangCollection<TypeParameterReflection> TypeParameters => _TypeParameters ??= new(this);

        private SlangCollection<EntryPointReflection>? _Parameters;
        public SlangCollection<EntryPointReflection> Parameters => _Parameters ??= new(this);

        private SlangCollection<EntryPointReflection>? _EntryPoints;
        public SlangCollection<EntryPointReflection> EntryPoints => _EntryPoints ??= new(this);
        #endregion

        #region Reflection API
        public uint ParameterCount => cppObj.ParameterCount;
        public uint TypeParameterCount => cppObj.TypeParameterCount;

        // Next version will fix all the memory leaks
        public TypeParameterReflection GetTypeParameterByIndex(uint index) => new(cppObj.GetTypeParameterByIndex(index));
        public TypeParameterReflection FindTypeParameter(string name) => new(cppObj.FindTypeParameter(name));
        public VariableLayoutReflection GetParameterByIndex(uint index) => new(cppObj.GetParameterByIndex(index));

        public uint EntryPointCount => cppObj.EntryPointCount;
        public EntryPointReflection GetEntryPointByIndex(uint index) => new(cppObj.GetEntryPointByIndex(index));
        public EntryPointReflection FindEntryPointByName(string name) => new(cppObj.FindEntryPointByName(name));

        public uint GlobalConstantBufferBinding => cppObj.GlobalConstantBufferBinding;
        public ulong GlobalConstantBufferSize => cppObj.GlobalConstantBufferSize;

        public TypeReflection FindTypeByName(string name) => new(cppObj.FindTypeByName(name));
        public FunctionReflection FindFunctionByName(string name) => new(cppObj.FindFunctionByName(name));
        public FunctionReflection FindFunctionByNameInType(TypeReflection type, string name) => new(cppObj.FindFunctionByNameInType(type.cppObj, name));
        public VariableReflection FindVarByNameInType(TypeReflection type, string name) => new(cppObj.FindVarByNameInType(type.cppObj, name));

        public TypeLayoutReflection GetTypeLayout(TypeReflection type, int layoutRules) => new(cppObj.GetTypeLayout(type.cppObj, layoutRules));

        public TypeReflection SpecializeType(TypeReflection type, TypeReflection[] specializationArgs) => new(cppObj.SpecializeType(type.cppObj, specializationArgs.Select(x => x.cppObj).ToArray()));

        public bool IsSubType(TypeReflection subType, TypeReflection superType) => cppObj.IsSubType(subType.cppObj, superType.cppObj);

        public uint HashedStringCount => cppObj.HashedStringCount;
        public string GetHashedString(uint index) => cppObj.GetHashedString(index);

        public TypeLayoutReflection GlobalParamsTypeLayout => new(cppObj.GlobalParamsTypeLayout);
        public VariableLayoutReflection GlobalParamsVarLayout => new(cppObj.GlobalParamsVarLayout);

        public System.String ToJson() => cppObj.ToJson();
        #endregion
    }
}