namespace Slang.Sdk.Tests
{
    internal class CLI_Tests
    {
        public static void RunAllTests()
        {
            Runtime_ArchitectureDetection();
            SlangCLI_CanFindSlangcExecutable();
            SlangCLI_ShowsHelpInformation();

            CLI.WorkingDirectory = Path.Combine(CLI.WorkingDirectory, "Tests\\Shaders\\");
            SlangCLI_TestCompileShader();
        }

        public static void Runtime_ArchitectureDetection()
        {
            // Test that Runtime.Directory detection handles architecture correctly
            try
            {
                string runtimeDirectory = Slang.Runtime.CLI_Directory;
                Console.WriteLine($"📁 Runtime Directory: {runtimeDirectory}");
                
                // Verify the directory exists
                if (!Directory.Exists(runtimeDirectory))
                {
                    throw new Exception($"Runtime directory does not exist: {runtimeDirectory}");
                }
                
                // Check if slangc.exe exists in the runtime directory
                string slangcPath = Path.Combine(runtimeDirectory, "slangc.exe");
                if (!File.Exists(slangcPath))
                {
                    throw new Exception($"slangc.exe not found in runtime directory: {slangcPath}");
                }
                                 
                Console.WriteLine($"🏗️ Detected Runtime Architecture: {Runtime.AsString()}");
                Console.WriteLine($"💻 Process Architecture: {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
                Console.WriteLine($"🖥️ OS Architecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}");
                
                Console.WriteLine("✅ Runtime_ArchitectureDetection: PASSED");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Runtime_ArchitectureDetection: FAILED - {ex.Message}");
                throw;
            }
        }

        public static void SlangCLI_CanFindSlangcExecutable()
        {
            // Test that SlangCLI can find slangc.exe and execute successfully
            var result = CLI.slangc();
            
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
            var result = CLI.slangc(inputFiles: new[] { "nonexistent-file.slang" });
            
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
            var cliResult = CLI.slangc(
                target: "hlsl",
                profile: "cs_5_0",
                entry: "CS",
                inputFiles: ["AverageColor.slang"]);

            //SlangC_Options.Builder paramsBuilder = new SlangC_Options.Builder()
            //    .SetTarget("hlsl")
            //    .SetProfile("cs_5_0")
            //    .SetEntry("CS")
            //    .AddIncludePaths(Path.Combine(CLI.WorkingDirectory, "AverageColor.slang"));
            //
            //CLI_Results cliResult = CLI.slangc(paramsBuilder.Build());
            //
            //string args = paramsBuilder.Build().ToString();
            //Console.WriteLine(args);
            //Console.WriteLine(cliResult.StdOut);

            //var args = "-target spirv -profile sm_6_6 -stage compute -entry main -O3 -g " +
            //           "-source-embed-style text -source-embed-name MyShader " +
            //           "-- ComponentAddition.slang";
            //
            //// -source-embed-style -source-embed-name flags are not currently suppored SlangC_Options, but can still be used as a raw string
            //CLI_Results result = CLI.slangc(args);

        }
    }
}
