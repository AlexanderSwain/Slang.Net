using System;
using Slang;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("sessioning...");
        Session session = new Session(
            [
                new CompilerOption(Slang.CompilerOptionName.WarningsAsErrors, new CompilerOptionValue(CompilerOptionValueKind.String, 0, 0, "all", null))
            ],
            [
                new PreprocessorMacroDesc("LIGHTING_SCALER", "12")
            ],
            [
                new ShaderModel(Slang.CompileTarget.SLANG_HLSL, "VS_5_0"),
            ],
            [
                "C:\\Users\\lexxa\\Code\\Playground\\Slang.Net.Test\\Shaders\\"
            ]);

        Module module = new Module(session, "test", "test.slang", File.ReadAllText("Shaders/test.slang"));
    }
}