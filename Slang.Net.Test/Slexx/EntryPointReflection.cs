public unsafe class EntryPointReflection : Slang.EntryPointReflection
{
    public EntryPointReflection(Slang.EntryPointReflection comObj) : base(comObj.getNative())
    {
    }
}