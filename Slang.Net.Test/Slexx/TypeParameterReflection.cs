public unsafe class TypeParameterReflection : Slang.TypeParameterReflection
{
    public TypeParameterReflection(Slang.TypeParameterReflection comObj) : base(comObj.getNative())
    {
    }
}