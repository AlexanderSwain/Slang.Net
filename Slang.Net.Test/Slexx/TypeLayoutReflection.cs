public unsafe class TypeLayoutReflection : Slang.TypeLayoutReflection
{
    public TypeLayoutReflection(Slang.TypeLayoutReflection comObj) : base(comObj.getNative())
    {
    }
}