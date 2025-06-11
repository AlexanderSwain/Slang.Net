public unsafe class GenericReflection : Slang.GenericReflection
{
    public GenericReflection(Slang.GenericReflection comObj) : base(comObj.getNative())
    {
    }
}