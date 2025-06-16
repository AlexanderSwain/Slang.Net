//using System;
//using Slang;
//using System.IO;
//using System.Linq;

//public class Debug
//{
//    public static void Main(string[] args)
//    {
//        try
//        {
//            SessionBuilder all_hlsl_targets = new SessionBuilder()
//                .AddCompilerOption(CompilerOptionName.WarningsAsErrors, new CompilerOptionValue(CompilerOptionValueKind.Int, 0, 0, "all", null))
//                .AddCompilerOption(CompilerOptionName.Obfuscate, new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null))
//                .AddPreprocessorMacro("LIGHTING_SCALER", "12")
//                .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
//                .AddSearchPath(@"C:\Users\lexxa\Code\Playground\Slang.Net\Slang.Net.Test\Shaders\");
            
//            Session session = all_hlsl_targets.Create();
//            Module module = session.LoadModule("ParameterInfo");
//            ShaderProgram program = module.Program;
            
//            Console.WriteLine($"Program has {program.EntryPoints.Count()} entry points:");
            
//            int index = 0;
//            foreach (var ep in program.EntryPoints)
//            {
//                Console.WriteLine($"  [{index}] Name: '{ep.Name}', Stage: {ep.Stage}");
//                index++;
//            }
            
//            // Test the working compilation method first
//            Console.WriteLine("\nTesting program compilation (working method):");
//            var source1 = program.CppObj.Compile(0, 0);
//            Console.WriteLine($"Success! Generated {source1.Length} characters of source code.");
            
//            Console.WriteLine("\nProgram compilation completed successfully!");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception: {ex.GetType().Name}");
//            Console.WriteLine($"Message: {ex.Message}");
//            Console.WriteLine($"Stack trace: {ex.StackTrace}");
//        }
//    }
//}
