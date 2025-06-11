public unsafe class VariableLayoutReflection : Slang.VariableLayoutReflection
{
    public VariableLayoutReflection(Slang.VariableLayoutReflection comObj) : base(comObj.getNative())
    {
    }
}