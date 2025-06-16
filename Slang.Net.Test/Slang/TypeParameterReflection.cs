public unsafe class TypeParameterReflection : Slang.Cpp.TypeParameterReflection
{
    public TypeParameterReflection(Slang.Cpp.TypeParameterReflection comObj) : base(comObj.getNative())
    {
    }
}