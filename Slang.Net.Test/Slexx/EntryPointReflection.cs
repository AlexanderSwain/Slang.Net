using Slang.Net.Test.Extensions;
using System.Diagnostics;

public unsafe class EntryPointReflection : Slang.EntryPointReflection
{
    public EntryPointReflection(Slang.EntryPointReflection comObj) : base(comObj.getNative())
    {
    }

    public string Compile(Slang.Program cpp)
    {
        // [Fix] This is a bit hacky, but it works for now.
        ShaderReflection parent = new ShaderReflection(Parent);
        var entryPointIndex = parent.EntryPoints.IndexOf(this);

        var source = parent.Parent.Compile((uint)entryPointIndex, 0);
        return source;
    }
}