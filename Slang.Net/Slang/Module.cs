// Using global using directive

public unsafe class Module : Slang.Cpp.Module
{
    public ShaderProgram Program => field ??= new ShaderProgram(this);

    internal Module(Slang.Cpp.Session comObj, string moduleName) : base(comObj, moduleName, null, null)
    {
    }
}