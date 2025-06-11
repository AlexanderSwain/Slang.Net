public unsafe class ShaderReflection : Slang.ShaderReflection
{
    public ShaderReflection(Slang.ShaderReflection comObj) : base(comObj.getNative())
    {
    }
}