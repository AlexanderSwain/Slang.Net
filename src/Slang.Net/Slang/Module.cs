// Using global using directive

public unsafe class Module
{
    internal Slang.Cpp.Module cppObj { get; }
    public ShaderProgram Program => field ??= new ShaderProgram(this);

    internal Module(Session session, string moduleName)
    {
        cppObj = new Slang.Cpp.Module(session.cppObj, moduleName, null, null);
    }
}