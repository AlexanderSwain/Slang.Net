using Slang.Net.Test.Slexx;

public unsafe class ShaderReflection : Slang.ShaderReflection, IComposedOf<EntryPointReflection>
{
    #region Composed Of
    public uint Count => EntryPointCount;
    EntryPointReflection IComposedOf<EntryPointReflection>.GetByIndex(uint index) => new(GetEntryPointByIndex(index));
    #endregion

    public ShaderReflection(Slang.ShaderReflection comObj) : base(comObj.getNative())
    {
    }

    public SlangCollection<EntryPointReflection> EntryPoints => field ??= new(this);
}