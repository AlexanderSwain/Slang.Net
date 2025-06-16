public unsafe class TypeLayoutReflection : Slang.Cpp.TypeLayoutReflection
{
    public TypeLayoutReflection(Slang.Cpp.TypeLayoutReflection comObj) : base(comObj.getNative())
    {
    }
}