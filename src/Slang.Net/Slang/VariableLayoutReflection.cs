public unsafe class VariableLayoutReflection
{
    internal Slang.Cpp.VariableLayoutReflection cppObj { get; }
    
    public VariableLayoutReflection(Slang.Cpp.VariableLayoutReflection comObj)
    {
        cppObj = comObj;
    }
    
    public VariableReflection Variable => new VariableReflection(cppObj.Variable);
    public string Name => cppObj.Name;
    
    public Modifier? FindModifier(Modifier.ID id)
    {
        var cppModifier = cppObj.FindModifier((int)id);
        return cppModifier != null ? new Modifier(cppModifier) : null;
    }
    
    public TypeLayoutReflection TypeLayout => new TypeLayoutReflection(cppObj.TypeLayout);
    
    // Categories
    public ParameterCategory Category => (ParameterCategory)cppObj.Category;
    public uint CategoryCount => cppObj.CategoryCount;
    
    public ParameterCategory GetCategoryByIndex(uint index)
    {
        return (ParameterCategory)cppObj.GetCategoryByIndex(index);
    }
    
    // Offsets and binding
    public UIntPtr GetOffset(ParameterCategory category)
    {
        return cppObj.GetOffset((Slang.Cpp.ParameterCategory)category);
    }
    
    public TypeReflection Type => new TypeReflection(cppObj.Type);
    public uint BindingIndex => cppObj.BindingIndex;
    public uint BindingSpace => cppObj.BindingSpace;
    
    public UIntPtr GetBindingSpace(ParameterCategory category)
    {
        return cppObj.GetBindingSpace((Slang.Cpp.ParameterCategory)category);
    }
    
    // Image format
    public ImageFormat ImageFormat => (ImageFormat)cppObj.ImageFormat;
    
    // Semantics
    public string SemanticName => cppObj.SemanticName;
    public UIntPtr SemanticIndex => cppObj.SemanticIndex;
    
    // Stage and pending data
    public uint Stage => cppObj.Stage;
    
    public VariableLayoutReflection? PendingDataLayout
    {
        get
        {
            var layout = cppObj.PendingDataLayout;
            return layout != null ? new VariableLayoutReflection(layout) : null;
        }
    }
}