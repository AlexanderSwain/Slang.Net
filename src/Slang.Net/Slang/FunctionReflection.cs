public unsafe class FunctionReflection : IComposedOf<Attribute>
{
    internal Slang.Cpp.FunctionReflection cppObj { get; }

    #region Composite
    uint IComposedOf<Attribute>.Count => cppObj.UserAttributeCount;
    Attribute IComposedOf<Attribute>.GetByIndex(uint index) => new Attribute(cppObj.GetUserAttributeByIndex(index));
    #endregion

    public SlangCollection<Attribute> UserAttributes { get; }

    public FunctionReflection(Slang.Cpp.FunctionReflection comObj)
    {
        cppObj = comObj;
        UserAttributes = new SlangCollection<Attribute>(this);
    }

    public string Name => cppObj.Name;
    public TypeReflection ReturnType => new TypeReflection(cppObj.ReturnType);

    // Parameters
    public uint ParameterCount => cppObj.ParameterCount;
    public VariableReflection GetParameterByIndex(uint index) => 
        new VariableReflection(cppObj.GetParameterByIndex(index));

    // Attributes
    public uint GetUserAttributeCount() => cppObj.UserAttributeCount;
    public Attribute? GetUserAttributeByIndex(uint index)
    {
        var cppAttr = cppObj.GetUserAttributeByIndex(index);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }
    public Attribute? FindAttributeByName(string name)
    {
        var cppAttr = cppObj.FindAttributeByName(name);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }
    public Attribute? FindUserAttributeByName(string name)
    {
        var cppAttr = cppObj.FindUserAttributeByName(name);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }

    // Modifiers
    public Modifier? FindModifier(Modifier.ID id)
    {
        var cppModifier = cppObj.FindModifier((int)id);
        return cppModifier != null ? new Modifier(cppModifier) : null;
    }

    // Generic operations
    public GenericReflection? GenericContainer
    {
        get
        {
            var cppGeneric = cppObj.GenericContainer;
            return cppGeneric != null ? new GenericReflection(cppGeneric) : null;
        }
    }
    public FunctionReflection? ApplySpecializations(GenericReflection? genRef)
    {
        var cppFunc = cppObj.ApplySpecializations(genRef?.cppObj);
        return cppFunc != null ? new FunctionReflection(cppFunc) : null;
    }
    public FunctionReflection? SpecializeWithArgTypes(TypeReflection[] types)
    {
        var cppTypes = new Slang.Cpp.TypeReflection[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            cppTypes[i] = types[i].cppObj;
        }
        var cppFunc = cppObj.SpecializeWithArgTypes(cppTypes);
        return cppFunc != null ? new FunctionReflection(cppFunc) : null;
    }

    // Overloading
    public bool IsOverloaded => cppObj.IsOverloaded;
    public uint OverloadCount => cppObj.OverloadCount;
    public FunctionReflection? GetOverload(uint index)
    {
        var cppFunc = cppObj.GetOverload(index);
        return cppFunc != null ? new FunctionReflection(cppFunc) : null;
    }
}