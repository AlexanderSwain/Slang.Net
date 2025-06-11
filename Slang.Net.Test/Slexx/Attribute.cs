public unsafe class Attribute : Slang.Attribute
{
    public Attribute(Slang.Attribute comObj) : base(comObj.getNative()) 
    {
    }
}