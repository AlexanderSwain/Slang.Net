using Slang.Net.Slexx;
using System;
using System.Xml.Linq;

public unsafe class ShaderProgram : 
    IComposedOf<TypeParameterReflection>, 
    IComposedOf<VariableLayoutReflection>,
    IComposedOf<EntryPointReflection>
{
    internal Slang.Cpp.Program CppObj { get; }

    internal ShaderProgram(Module module)
    {
        CppObj = new Slang.Cpp.Program(module);
    }

    #region Composed Of
    uint IComposedOf<TypeParameterReflection>.Count => Reflection.TypeParameterCount;
    TypeParameterReflection IComposedOf<TypeParameterReflection>.GetByIndex(uint index)
    {
        return new(Reflection.GetTypeParameterByIndex(index));
    }

    uint IComposedOf<VariableLayoutReflection>.Count => Reflection.ParameterCount;
    VariableLayoutReflection IComposedOf<VariableLayoutReflection>.GetByIndex(uint index)
    {
        return new(Reflection.GetParameterByIndex(index));
    }

    uint IComposedOf<EntryPointReflection>.Count => Reflection.EntryPointCount;
    EntryPointReflection IComposedOf<EntryPointReflection>.GetByIndex(uint index)
    {
        return new(Reflection.GetEntryPointByIndex(index));
    }
    #endregion

    #region Reflection API
    ShaderReflection Reflection => field ??= new(CppObj.GetReflection());

    public SlangCollection<TypeParameterReflection> TypeParameters => field ??= new(this);
    public SlangCollection<VariableLayoutReflection> Parameters => field ??= new(this);
    public SlangCollection<EntryPointReflection> EntryPoints => field ??= new(this);
    public uint GlobalConstantBufferBinding => Reflection.GlobalConstantBufferBinding;
    public ulong GlobalConstantBufferSize => Reflection.GlobalConstantBufferSize;
    public TypeReflection FindTypeByName(string name) => new(Reflection.FindTypeByName(name));
    public FunctionReflection FindFunctionByName(String name) => new(Reflection.FindFunctionByName(name));
    public FunctionReflection FindFunctionByNameInType(TypeReflection type, String name) => new(Reflection.FindFunctionByNameInType(type, name));
    public VariableReflection FindVarByNameInType(TypeReflection type, String name) => new(Reflection.FindVarByNameInType(type, name));
    public TypeLayoutReflection GetTypeLayout(TypeReflection type, LayoutRules layoutRules) => new(Reflection.GetTypeLayout(type, (int)layoutRules));
    public TypeReflection SpecializeType(TypeReflection type, TypeReflection[] specializationArgs) => new(Reflection.SpecializeType(type, specializationArgs));
    public bool IsSubType(TypeReflection subType, TypeReflection superType) => Reflection.IsSubType(subType, superType);
    public uint HashedStringCount => Reflection.HashedStringCount;
    string GetHashedString(uint index) => Reflection.GetHashedString(index);
    public TypeLayoutReflection GlobalParamsTypeLayout => new(Reflection.GlobalParamsTypeLayout);
    public VariableLayoutReflection GlobalParamsVarLayout => new(Reflection.GlobalParamsVarLayout);
    public string ToJson() => Reflection.ToJson();
    #endregion
}