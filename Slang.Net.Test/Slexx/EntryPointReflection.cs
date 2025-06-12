using Slang.Net.Test.Extensions;

public unsafe class EntryPointReflection : Slang.EntryPointReflection
{
    public EntryPointReflection(Slang.EntryPointReflection comObj) : base(comObj.getNative())
    {
    }

    public string Compile()
    {
        // [Fix] This is a bit hacky, but it works for now.
        var entryPointIndex = new ShaderReflection(Parent).EntryPoints.IndexOf(this);
        var source = Parent.Parent.Compile((uint)entryPointIndex, 0);
        return source;
    }
}