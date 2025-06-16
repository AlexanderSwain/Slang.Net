using Slang.Net.Slexx;

public unsafe class ShaderReflection : Slang.Cpp.ShaderReflection, IComposedOf<EntryPointReflection>
{
    #region Composed Of
    public uint Count => EntryPointCount;
    EntryPointReflection IComposedOf<EntryPointReflection>.GetByIndex(uint index) => new(GetEntryPointByIndex(index));
    #endregion

    public ShaderReflection(Slang.Cpp.ShaderReflection comObj) : base(comObj.getNative())
    {
    }

    public SlangCollection<EntryPointReflection> EntryPoints => field ??= new(this);
}