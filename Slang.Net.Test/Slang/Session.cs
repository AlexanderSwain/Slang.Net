// Using global using directive

public unsafe class Session : Slang.Cpp.Session
{
    internal Session(Slang.CompilerOption[] compilerOptions, Slang.Cpp.PreprocessorMacroDesc[] macros, Slang.Cpp.ShaderModel[] models, string[] searchPaths) : base(compilerOptions, macros, models, searchPaths)
    {
    }

    public Module LoadModule(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        if (extension != ".slang")
            throw new ArgumentException($"{(string.IsNullOrEmpty(extension) ? "<no extension>" : extension)} files are not supported. Only .slang files are supported.", nameof(fileName));

        return new Module(this, fileName);
    }
}

public class SessionBuilder
{
    public List<Slang.CompilerOption> CompilerOptions { get; } = new();
    public List<Slang.Cpp.PreprocessorMacroDesc> PreprocessorMacroDesc { get; } = new();
    public List<Slang.Cpp.ShaderModel> ShaderModel { get; } = new();
    public List<string> SearchPaths { get; } = new ();

    public SessionBuilder AddCompilerOption(Slang.CompilerOptionName name, Slang.CompilerOptionValue value)
    {
        CompilerOptions.Add(new Slang.CompilerOption(name, value));
        return this;
    }

    public SessionBuilder AddPreprocessorMacro(string name, string value)
    {
        PreprocessorMacroDesc.Add(new Slang.Cpp.PreprocessorMacroDesc(name, value));
        return this;
    }

    public SessionBuilder AddShaderModel(Slang.CompileTarget target, string profile)
    {
        ShaderModel.Add(new Slang.Cpp.ShaderModel(target, profile));
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