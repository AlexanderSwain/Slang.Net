using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

            var text = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
            // Load the module from the specified file
            Module module = session.LoadModule("AverageColor.slang", null, null);

            // Get the shader program from the module
            Slang.Sdk.Program program = module.Program;

            // Access the shader program from the module
            ShaderReflection reflection = module.Program.GetReflection(Targets.Hlsl.cs_5_0);

            //// Print the number of entry points in the shader program
            //var entryPoint = program.EntryPoints.Where(x => x.Name == "CS").First();
            //
            //// Compile the shader program using the entry point
            //var source = entryPoint.Compile();

            var source = program.Compile(null, Targets.Hlsl.cs_5_0);

            // Print the generated source code length
            Console.WriteLine(source.Source);
        }
    }
}
