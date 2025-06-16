public unsafe class Attribute : Slang.Cpp.Attribute
{
    public Attribute(Slang.Cpp.Attribute comObj) : base(comObj.getNative()) 
    {
    }
}