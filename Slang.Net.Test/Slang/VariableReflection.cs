using Slang.Net.Test.Slexx;

public unsafe class VariableReflection : Slang.Cpp.VariableReflection, IComposedOf<Attribute>
{
    #region Composite
    uint IComposedOf<Attribute>.Count => base.UserAttributeCount;
    Attribute IComposedOf<Attribute>.GetByIndex(uint index) => new Attribute(base.GetUserAttributeByIndex(index));
    #endregion

    public SlangCollection<Attribute> UserAttributes { get; }

    public VariableReflection(Slang.Cpp.VariableReflection comObj) : base(comObj.getNative())
    {
        UserAttributes = new SlangCollection<Attribute>(this);
    }
}