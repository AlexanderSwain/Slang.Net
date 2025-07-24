using Slang.CLI;
using static Slang.CLI.SlangCLI;

namespace Slang.Sdk.Tests
{
    public class CLI_Tests
    {
        public static void RunAllTests()
        {
            // Set the working directory to the shaders directory
            WorkingDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\";

            // Test 1: Basic CLI compilation
            ResultsCLI cliResult = slangc(
                target: "hlsl", 
                profile: "cs_5_0", 
                entry: "CS", 
                outputPath: "output1.hlsl", 
                inputFiles: ["AverageColor.slang"]);
        }
    }
}
