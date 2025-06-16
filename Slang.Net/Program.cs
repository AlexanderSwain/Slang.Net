using Slang;
using System;
// Using global using directive
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        SessionBuilder all_hlsl_targets = new SessionBuilder()
            .AddCompilerOption(CompilerOptionName.WarningsAsErrors, new CompilerOptionValue(CompilerOptionValueKind.Int, 0, 0, "all", null))
            .AddCompilerOption(CompilerOptionName.Obfuscate, new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null))
            .AddPreprocessorMacro("LIGHTING_SCALER", "12")
            //.AddShaderModel(CompileTarget.SLANG_HLSL, "vs_5_0")
            //.AddShaderModel(CompileTarget.SLANG_HLSL, "gs_5_0")
            //.AddShaderModel(CompileTarget.SLANG_HLSL, "hs_5_0")
            //.AddShaderModel(CompileTarget.SLANG_HLSL, "ds_5_0")
            //.AddShaderModel(CompileTarget.SLANG_HLSL, "ps_5_0")
            .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
            .AddSearchPath(@"C:\Users\lexxa\Code\Playground\Slang.Net\Slang.Net\Shaders\");
        
        Session session = all_hlsl_targets.Create();
        Module module = session.LoadModule("ParameterInfo.slang");
        ShaderProgram program = module.Program;
        var entryPoint = program.EntryPoints.Where(x => x.Name == "CS").First();

        // This works
        var source = program.CppObj.Compile(0, 0);

        // This doesn't work
        source = entryPoint.Compile(program.CppObj);
    }
}