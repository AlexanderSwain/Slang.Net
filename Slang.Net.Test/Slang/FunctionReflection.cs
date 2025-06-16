public unsafe class FunctionReflection : Slang.Cpp.FunctionReflection
{
    public FunctionReflection(Slang.Cpp.FunctionReflection comObj) : base(comObj.getNative())
    {
    }
}