public unsafe class GenericReflection : Slang.Cpp.GenericReflection
{
    public GenericReflection(Slang.Cpp.GenericReflection comObj) : base(comObj.getNative())
    {
    }
}