public unsafe class TypeReflection : Slang.Cpp.TypeReflection
{
    public TypeReflection(Slang.Cpp.TypeReflection comObj) : base(comObj.getNative())
    {
    }
}