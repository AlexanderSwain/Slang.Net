
public unsafe class ShaderProgram : 
    IComposedOf<TypeParameterReflection>, 
    IComposedOf<VariableLayoutReflection>,
    IComposedOf<EntryPointReflection>
{
    internal Slang.Cpp.Program CppObj { get; }

    internal ShaderProgram(Module module)
    {
        CppObj = new Slang.Cpp.Program(module.cppObj);
    }

    #region Composed Of
    uint IComposedOf<TypeParameterReflection>.Count => Reflection.TypeParameterCount;
    TypeParameterReflection IComposedOf<TypeParameterReflection>.GetByIndex(uint index)
    {
        return Reflection.GetTypeParameterByIndex(index);
    }

    uint IComposedOf<VariableLayoutReflection>.Count => Reflection.ParameterCount;
    VariableLayoutReflection IComposedOf<VariableLayoutReflection>.GetByIndex(uint index)
    {
        return Reflection.GetParameterByIndex(index);
    }

    uint IComposedOf<EntryPointReflection>.Count => Reflection.EntryPointCount;
    EntryPointReflection IComposedOf<EntryPointReflection>.GetByIndex(uint index)
    {
        return Reflection.GetEntryPointByIndex(index);
    }
    #endregion

    #region Reflection API
    ShaderReflection Reflection => field ??= new(this);

    public SlangCollection<TypeParameterReflection> TypeParameters => field ??= new(this);
    public SlangCollection<VariableLayoutReflection> Parameters => field ??= new(this);
    public SlangCollection<EntryPointReflection> EntryPoints => field ??= new(this);
    public uint GlobalConstantBufferBinding => Reflection.GlobalConstantBufferBinding;
    public ulong GlobalConstantBufferSize => Reflection.GlobalConstantBufferSize;
    public TypeReflection FindTypeByName(string name) => Reflection.FindTypeByName(name);
    public FunctionReflection FindFunctionByName(String name) => Reflection.FindFunctionByName(name);
    public FunctionReflection FindFunctionByNameInType(TypeReflection type, String name) => Reflection.FindFunctionByNameInType(type, name);
    public VariableReflection FindVarByNameInType(TypeReflection type, String name) => Reflection.FindVarByNameInType(type, name);
    public TypeLayoutReflection GetTypeLayout(TypeReflection type, LayoutRules layoutRules) => Reflection.GetTypeLayout(type, (int)layoutRules);
    public TypeReflection SpecializeType(TypeReflection type, TypeReflection[] specializationArgs) => Reflection.SpecializeType(type, specializationArgs);
    public bool IsSubType(TypeReflection subType, TypeReflection superType) => Reflection.IsSubType(subType, superType);
    public uint HashedStringCount => Reflection.HashedStringCount;
    string GetHashedString(uint index) => Reflection.GetHashedString(index);
    public TypeLayoutReflection GlobalParamsTypeLayout => Reflection.GlobalParamsTypeLayout;
    public VariableLayoutReflection GlobalParamsVarLayout => Reflection.GlobalParamsVarLayout;
    public string ToJson() => Reflection.ToJson();
    #endregion
}