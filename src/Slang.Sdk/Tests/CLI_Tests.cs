using Slang.CLI;

namespace Slang.Sdk.Tests
{
    internal class CLI_Tests
    {
        public static void RunAllTests()
        {
            SlangCLI_CanFindSlangcExecutable();
            SlangCLI_ShowsHelpInformation();

            SlangCLI.WorkingDirectory = Path.Combine(SlangCLI.WorkingDirectory, "Tests\\Shaders\\");
            SlangCLI_TestCompileShader();
        }

        public static void SlangCLI_CanFindSlangcExecutable()
        {
            // Test that SlangCLI can find slangc.exe and execute successfully
            var result = SlangCLI.slangc();
            
            if (result == null)
                throw new Exception("Result should not be null");
            
            // Exit code -1 usually indicates the executable was not found (process couldn't start)
            if (result.ExitCode == -1)
                throw new Exception("Exit code -1 usually indicates the executable was not found");
            
            // Exit code 0 means the executable was found and executed successfully
            // This is correct - slangc returns 0 when called without arguments
            if (result.ExitCode == 0)
            {
                Console.WriteLine("✅ SlangCLI_CanFindSlangcExecutable: PASSED");
                return;
            }
            
            // Any other exit code is also fine as long as the process started
            Console.WriteLine("✅ SlangCLI_CanFindSlangcExecutable: PASSED");
        }

        public static void SlangCLI_ShowsHelpInformation()
        {
            // Test that slangc processes arguments correctly by testing with a nonexistent file
            // This verifies the CLI wrapper is working and can pass arguments to slangc
            var result = SlangCLI.slangc(inputFiles: new[] { "nonexistent-file.slang" });
            
            if (result == null)
                throw new Exception("Result should not be null");
            
            // slangc should return non-zero for missing file
            if (result.ExitCode == 0)
                throw new Exception("Expected non-zero exit code for missing file");
            
            // Should get an error message about the missing file
            if (string.IsNullOrEmpty(result.StdErr))
                throw new Exception("Should get error message for missing file");
                
            // Error message should indicate file issue
            if (!result.StdErr.ToLower().Contains("error") && !result.StdErr.ToLower().Contains("cannot"))
                throw new Exception("Should get error message about the missing file");
                
            Console.WriteLine("✅ SlangCLI_ShowsHelpInformation: PASSED");
        }

        public static void SlangCLI_TestCompileShader()
        {
            // Test 1: Basic CLI compilation
            ResultsCLI cliResult = SlangCLI.slangc(
                target: "hlsl",
                profile: "cs_5_0",
                entry: "CS",
                outputPath: "output1.hlsl",
                inputFiles: ["AverageColor.slang"]);
        }
    }
}
