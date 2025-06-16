using Slang;

public unsafe class Module : Slang.Module
{
    public ShaderProgram Program => field ??= new ShaderProgram(this);

    internal Module(Slang.Session comObj, string moduleName) : base(comObj, moduleName, null, null)
    {
    }
}