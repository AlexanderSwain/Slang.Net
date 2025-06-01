using System;
using Slang;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("sessioning...");
        Session session = new Session(
            [
                new CompilerOption(Slang.CompilerOptionName.Obfuscate, new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null))
            ],
            [
                new PreprocessorMacroDesc("LIGHTING_SCALER", "12")
            ],
            [
                new ShaderModel(Slang.CompileTarget.SLANG_HLSL, "cs_5_0"),
            ],
            [
                @"C:\Users\lexxa\Code\Playground\Slang.Net\Slang.Net.Test\Shaders\"
            ]);
        Module module = new Module(session, "ParameterInfo", "ParameterInfo.slang", File.ReadAllText("Shaders/ParameterInfo.slang"));

        EntryPoint entryPoint = new EntryPoint(module, "CS");
        var parameters = entryPoint.Parameters;

        Slang.Program program = new Slang.Program(entryPoint);

        var source = program.Compile();
    }
}