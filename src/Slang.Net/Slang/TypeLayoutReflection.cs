public unsafe class TypeLayoutReflection
{
    internal Slang.Cpp.TypeLayoutReflection cppObj { get; }
    
    public TypeLayoutReflection(Slang.Cpp.TypeLayoutReflection comObj)
    {
        cppObj = comObj;
    }
    
    public TypeReflection Type => new TypeReflection(cppObj.Type);
    public TypeKind Kind => (TypeKind)cppObj.Kind;
    
    // Size and alignment
    public UIntPtr GetSize(ParameterCategory category)
    {
        return cppObj.GetSize((Slang.Cpp.ParameterCategory)category);
    }
    
    public UIntPtr GetStride(ParameterCategory category)
    {
        return cppObj.GetStride((Slang.Cpp.ParameterCategory)category);
    }
    
    public int GetAlignment(ParameterCategory category)
    {
        return cppObj.GetAlignment((Slang.Cpp.ParameterCategory)category);
    }
    
    // Field access
    public uint FieldCount => cppObj.FieldCount;
    
    public VariableLayoutReflection GetFieldByIndex(uint index)
    {
        return new VariableLayoutReflection(cppObj.GetFieldByIndex(index));
    }
    
    public int FindFieldIndexByName(string name)
    {
        return cppObj.FindFieldIndexByName(name);
    }
    
    public VariableLayoutReflection? ExplicitCounter
    {
        get
        {
            var counter = cppObj.ExplicitCounter;
            return counter != null ? new VariableLayoutReflection(counter) : null;
        }
    }
    
    // Array properties
    public bool IsArray => cppObj.IsArray;
    
    public TypeLayoutReflection? UnwrapArray()
    {
        var layout = cppObj.UnwrapArray();
        return layout != null ? new TypeLayoutReflection(layout) : null;
    }
    
    public UIntPtr ElementCount => cppObj.ElementCount;
    public UIntPtr TotalArrayElementCount => cppObj.TotalArrayElementCount;
    
    public UIntPtr GetElementStride(ParameterCategory category)
    {
        return cppObj.GetElementStride((Slang.Cpp.ParameterCategory)category);
    }
    
    public TypeLayoutReflection? ElementTypeLayout
    {
        get
        {
            var layout = cppObj.ElementTypeLayout;
            return layout != null ? new TypeLayoutReflection(layout) : null;
        }
    }
    
    public VariableLayoutReflection? ElementVarLayout
    {
        get
        {
            var layout = cppObj.ElementVarLayout;
            return layout != null ? new VariableLayoutReflection(layout) : null;
        }
    }
    
    public VariableLayoutReflection? ContainerVarLayout
    {
        get
        {
            var layout = cppObj.ContainerVarLayout;
            return layout != null ? new VariableLayoutReflection(layout) : null;
        }
    }
}