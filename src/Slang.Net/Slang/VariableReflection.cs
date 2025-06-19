
public unsafe class VariableReflection : IComposedOf<Attribute>
{
    internal Slang.Cpp.VariableReflection cppObj { get; }

    #region Composite
    uint IComposedOf<Attribute>.Count => cppObj.UserAttributeCount;
    Attribute IComposedOf<Attribute>.GetByIndex(uint index) => new Attribute(cppObj.GetUserAttributeByIndex(index));
    #endregion

    public SlangCollection<Attribute> UserAttributes { get; }

    public VariableReflection(Slang.Cpp.VariableReflection comObj)
    {
        cppObj = comObj;
        UserAttributes = new SlangCollection<Attribute>(this);
    }
    
    public string Name => cppObj.Name;
    
    public TypeReflection Type => new TypeReflection(cppObj.Type);
    
    public Modifier? FindModifier(Modifier.ID id)
    {
        var cppModifier = cppObj.FindModifier((int)id);
        return cppModifier != null ? new Modifier(cppModifier) : null;
    }
    
    public uint GetUserAttributeCount()
    {
        return cppObj.UserAttributeCount;
    }
    
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
    
    public bool HasDefaultValue => cppObj.HasDefaultValue;
    
    public long? GetDefaultValueInt()
    {
        return cppObj.GetDefaultValueInt();
    }
    
    public GenericReflection? GenericContainer
    {
        get
        {
            var cppGeneric = cppObj.GenericContainer;
            return cppGeneric != null ? new GenericReflection(cppGeneric) : null;
        }
    }
    
    public VariableReflection? ApplySpecializations(GenericReflection? genRef)
    {
        var cppVar = cppObj.ApplySpecializations(genRef?.cppObj);
        return cppVar != null ? new VariableReflection(cppVar) : null;
    }
}