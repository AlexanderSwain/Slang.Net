using System;
using Slang;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("sessioning...");
        Session session = new Session(
            [
                new CompilerOption(Slang.CompilerOptionName.Obfuscate, new CompilerOptionValue(CompilerOptionValueKind.Int, 0, 0, null, null))
            ],
            [
                new PreprocessorMacroDesc("LIGHTING_SCALER", "12")
            ],
            [
                new ShaderModel(Slang.CompileTarget.SLANG_HLSL, "CS_5_0"),
            ],
            [
                "C:\\Users\\lexxa\\Code\\Playground\\Slang.Net.Test\\Shaders\\"
            ]);

        Module module = new Module(session, "test", "test.slang", File.ReadAllText("Shaders/test.slang"));

        EntryPoint entryPoint = new EntryPoint(module, "computeMain");

        Slang.Program program = new Slang.Program(entryPoint);

        var source = program.Compile();
    }
}