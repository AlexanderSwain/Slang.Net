using Slang;

public unsafe class Session : Slang.Session
{
    internal Session(Slang.CompilerOption[] compilerOptions, Slang.PreprocessorMacroDesc[] macros, Slang.ShaderModel[] models, string[] searchPaths) : base(compilerOptions, macros, models, searchPaths)
    {
    }

    public Module LoadModule(string moduleName)
    {
        return new Module(this, moduleName, null, null);
    }
}

public class SessionBuilder
{
    public List<Slang.CompilerOption> CompilerOptions { get; } = new();
    public List<Slang.PreprocessorMacroDesc> PreprocessorMacroDesc { get; } = new();
    public List<Slang.ShaderModel> ShaderModel { get; } = new();
    public List<string> SearchPaths { get; } = new ();

    public SessionBuilder AddCompilerOption(Slang.CompilerOptionName name, Slang.CompilerOptionValue value)
    {
        CompilerOptions.Add(new Slang.CompilerOption(name, value));
        return this;
    }

    public SessionBuilder AddPreprocessorMacro(string name, string value)
    {
        PreprocessorMacroDesc.Add(new Slang.PreprocessorMacroDesc(name, value));
        return this;
    }

    public SessionBuilder AddShaderModel(CompileTarget target, string profile)
    {
        ShaderModel.Add(new Slang.ShaderModel(target, profile));
        return this;
    }

    public SessionBuilder AddSearchPath(string absolutePath)
    {
        SearchPaths.Add(absolutePath);
        return this;
    }

    public Session Create()
    {
        return new Session(CompilerOptions.ToArray(), PreprocessorMacroDesc.ToArray(), ShaderModel.ToArray(), SearchPaths.ToArray());
    }
}