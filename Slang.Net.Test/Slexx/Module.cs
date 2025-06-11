using Slang;

public unsafe class Module : Slang.Module
{
    public ShaderProgram Program => field ??= new ShaderProgram(this);

    internal Module(Slang.Session comObj, string? moduleName, string? modulePath, string? shaderSource) : base(comObj, moduleName, modulePath, shaderSource)
    {
    }
}