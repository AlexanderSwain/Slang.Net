using Slang.Sdk.Interop;

namespace Slang.Sdk.Tests
{
    internal class PrettyTests
    {
        public static void RunAllTests()
        {
            SimpleCompileTest();
        }

        public static void SimpleCompileTest()
        {
            // Create a session with compiler options and search paths
            Session.Builder builder = new Session.Builder()
                .AddCompilerOption(CompilerOption.Name.WarningsAsErrors, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 0, 0, "all", null))
                .AddCompilerOption(CompilerOption.Name.Obfuscate, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null))
                .AddPreprocessorMacro("LIGHTING_SCALER", "12")
                .AddTarget(Targets.Hlsl.cs_5_0)
                .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\"); // Fixed: AverageColor.slang is copied to output directory root

            // Create the session
            Session session = builder.Create();

            // Load the module from the specified file
            Module module = session.LoadModule("AverageColor", $@"{ AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");

            // Get the shader program from the module
            Program program = module.Program;

            // Access the shader program from the module
            ShaderReflection reflection = module.Program.GetReflection(Targets.Hlsl.cs_5_0);

            //// Print the number of entry points in the shader program
            //var entryPoint = program.EntryPoints.Where(x => x.Name == "CS").First();

            //// Compile the shader program using the entry point
            //var source = entryPoint.Compile();

            var modules = session.Modules;

            var source = program.Compile(module.EntryPoints["CS"], Targets.Hlsl.cs_5_0);

            // Print the generated source code length
            Console.WriteLine(source.Source);
        }
    }
}
