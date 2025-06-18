using Slang;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a session with compiler options and search paths
        SessionBuilder builder = new SessionBuilder()
            .AddCompilerOption(CompilerOptionName.WarningsAsErrors, new CompilerOptionValue(CompilerOptionValueKind.Int, 0, 0, "all", null))
            .AddCompilerOption(CompilerOptionName.Obfuscate, new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null))
            .AddPreprocessorMacro("LIGHTING_SCALER", "12")
            .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
            .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}");

        // Create the session
        Session session = builder.Create();

        // Load the module from the specified file
        Module module = session.LoadModule("AverageColor.slang");

        // Access the shader program from the module
        ShaderProgram program = module.Program;

        // Print the number of entry points in the shader program
        var entryPoint = program.EntryPoints.Where(x => x.Name == "CS").First();

        // Compile the shader program using the entry point
        var source = entryPoint.Compile();

        // Print the generated source code length
        Console.WriteLine(source);
    }
}