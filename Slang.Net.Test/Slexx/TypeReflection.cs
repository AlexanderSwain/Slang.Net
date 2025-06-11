public unsafe class TypeReflection : Slang.TypeReflection
{
    public TypeReflection(Slang.TypeReflection comObj) : base(comObj.getNative())
    {
    }
}