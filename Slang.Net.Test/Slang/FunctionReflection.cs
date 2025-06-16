public unsafe class FunctionReflection : Slang.FunctionReflection
{
    public FunctionReflection(Slang.FunctionReflection comObj) : base(comObj.getNative())
    {
    }
}