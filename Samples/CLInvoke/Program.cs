using Slang.Sdk;
using System.Text;

// Fix Unicode character rendering by setting console encoding to UTF-8
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

Console.WriteLine("Slang CLI Invocation Sample");
Console.WriteLine("================================\n");

// Set working directory to the sample folder for easier file access
CLI.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

try
{
    // Test 1: Basic CLI Compilation Features
    Console.WriteLine("🔧 Test 1: Basic CLI Compilation");
    Console.WriteLine("----------------------------------");

    // Simple HLSL compilation
    var hlslResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"{(hlslResult.ExitCode == 0 ? "✅" : "❌")} HLSL Compilation - Exit Code: {hlslResult.ExitCode}");
    if (hlslResult.ExitCode == 0)
    {
        Console.WriteLine("📄 Generated HLSL (first 200 chars):");
        Console.WriteLine(hlslResult.StdOut.Length > 200 ?
            hlslResult.StdOut.Substring(0, 200) + "..." : hlslResult.StdOut);
    }
    else if (!string.IsNullOrEmpty(hlslResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {hlslResult.StdErr}");
    }
    Console.WriteLine();

    // GLSL compilation
    var glslResult = CLI.slangc(
        target: "glsl",
        profile: "glsl_460",
        entry: "CS",
        stage: "compute",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"{(glslResult.ExitCode == 0 ? "✅" : "❌")} GLSL Compilation - Exit Code: {glslResult.ExitCode}");
    if (glslResult.ExitCode == 0)
    {
        Console.WriteLine("📄 Generated GLSL (first 200 chars):");
        Console.WriteLine(glslResult.StdOut.Length > 200 ?
            glslResult.StdOut.Substring(0, 200) + "..." : glslResult.StdOut);
    }
    else if (!string.IsNullOrEmpty(glslResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {glslResult.StdErr}");
    }
    Console.WriteLine();

    // SPIR-V compilation
    var spirvResult = CLI.slangc(
        target: "spirv",
        profile: "sm_6_0",
        entry: "main",
        stage: "compute",
        inputFiles: ["ComponentAddition.slang"]);

    Console.WriteLine($"{(spirvResult.ExitCode == 0 ? "✅" : "❌")} SPIR-V Compilation - Exit Code: {spirvResult.ExitCode}");
    if (spirvResult.ExitCode != 0 && !string.IsNullOrEmpty(spirvResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {spirvResult.StdErr}");
    }
    Console.WriteLine();

    // Test 2: Using the Builder Pattern
    Console.WriteLine("🔧 Test 2: Builder Pattern & Advanced Options");
    Console.WriteLine("----------------------------------------------");

    var builderResult = CLI.slangc(
        new SlangC_Options.Builder()
            .SetTarget("hlsl")
            .SetProfile("cs_5_0")
            .SetEntry("CS")
            .SetOptimizationLevel("3")
            .SetDebugInfo(true)
            .Define("SAMPLE_COUNT", "4")
            .Define("USE_OPTIMIZATION", "1")
            .AddInputFile("AverageColor.slang")
            .Build());

    Console.WriteLine($"{(builderResult.ExitCode == 0 ? "✅" : "❌")} Builder Pattern Compilation - Exit Code: {builderResult.ExitCode}");
    if (builderResult.ExitCode != 0 && !string.IsNullOrEmpty(builderResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {builderResult.StdErr}");
    }
    Console.WriteLine();

    // Test 3: File Output
    Console.WriteLine("🔧 Test 3: File Output & Multiple Targets");
    Console.WriteLine("------------------------------------------");

    // Compile to output file
    var fileOutputResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        outputPath: "AverageColor_output.hlsl",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"{(fileOutputResult.ExitCode == 0 ? "✅" : "❌")} File Output Compilation - Exit Code: {fileOutputResult.ExitCode}");
    if (fileOutputResult.ExitCode != 0 && !string.IsNullOrEmpty(fileOutputResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {fileOutputResult.StdErr}");
    }

    // Check if file was created
    if (File.Exists(Path.Combine(CLI.WorkingDirectory, "AverageColor_output.hlsl")))
    {
        var fileContent = File.ReadAllText(Path.Combine(CLI.WorkingDirectory, "AverageColor_output.hlsl"));
        Console.WriteLine($"📁 Output file created successfully ({fileContent.Length} characters)");
    }
    else if (fileOutputResult.ExitCode == 0)
    {
        Console.WriteLine($"⚠️ Warning: Compilation succeeded but output file was not found");
    }
    Console.WriteLine();

    // Test 4: Reflection JSON Output
    Console.WriteLine("🔧 Test 4: Reflection JSON Output");
    Console.WriteLine("----------------------------------");

    var reflectionResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        reflectionJsonPath: "AverageColor_reflection.json",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"{(reflectionResult.ExitCode == 0 ? "✅" : "❌")} Reflection JSON Generation - Exit Code: {reflectionResult.ExitCode}");
    if (reflectionResult.ExitCode != 0 && !string.IsNullOrEmpty(reflectionResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {reflectionResult.StdErr}");
    }

    // Check if reflection file was created
    if (File.Exists(Path.Combine(CLI.WorkingDirectory, "AverageColor_reflection.json")))
    {
        var reflectionContent = File.ReadAllText(Path.Combine(CLI.WorkingDirectory, "AverageColor_reflection.json"));
        Console.WriteLine($"📁 Reflection JSON created successfully ({reflectionContent.Length} characters)");
        Console.WriteLine("🔍 First 300 chars of reflection data:");
        Console.WriteLine(reflectionContent.Length > 300 ?
            reflectionContent.Substring(0, 300) + "..." : reflectionContent);
    }
    else if (reflectionResult.ExitCode == 0)
    {
        Console.WriteLine($"⚠️ Warning: Compilation succeeded but reflection JSON file was not found");
    }
    Console.WriteLine();

    // Test 5: Advanced Compilation Options
    Console.WriteLine("🔧 Test 5: Advanced Compilation Options");
    Console.WriteLine("----------------------------------------");

    var advancedResult = CLI.slangc(
        new SlangC_Options.Builder()
            .SetTarget("hlsl")
            .SetProfile("cs_6_0")
            .SetEntry("CS")
            .SetOptimizationLevel("3")
            .SetDebugInfo(true)
            .SetWarningsAsErrors(false)
            .DisableWarning("unused-parameter")
            .Define("DEBUG_MODE", "1")
            .Define("MAX_THREADS", "1024")
            .SetModuleName("AverageColorModule")
            .AddInputFile("AverageColor.slang")
            .Build());

    Console.WriteLine($"{(advancedResult.ExitCode == 0 ? "✅" : "❌")} Advanced Options Compilation - Exit Code: {advancedResult.ExitCode}");
    if (advancedResult.ExitCode != 0 && !string.IsNullOrEmpty(advancedResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {advancedResult.StdErr}");
    }
    Console.WriteLine();

    // Test 6: Raw Command String (for experimental features)
    Console.WriteLine("🔧 Test 6: Raw Command String & Experimental Features");
    Console.WriteLine("------------------------------------------------------");

    var rawCommandResult = CLI.slangc(
        "-target hlsl -profile cs_5_0 -entry CS -O3 -g " +
        "-D EXPERIMENTAL=1 -D VERSION=100 " +
        "-- AverageColor.slang");

    Console.WriteLine($"{(rawCommandResult.ExitCode == 0 ? "✅" : "❌")} Raw Command String - Exit Code: {rawCommandResult.ExitCode}");
    if (rawCommandResult.ExitCode != 0 && !string.IsNullOrEmpty(rawCommandResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {rawCommandResult.StdErr}");
    }
    Console.WriteLine();

    // Test 7: Error Handling & Validation
    Console.WriteLine("🔧 Test 7: Error Handling & Validation");
    Console.WriteLine("---------------------------------------");

    // Test with non-existent file
    var errorResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        inputFiles: ["NonExistentShader.slang"]);

    Console.WriteLine($"{(errorResult.ExitCode != 0 ? "✅" : "❌")} Non-existent file test - Exit Code: {errorResult.ExitCode} (Expected: Non-zero)");
    if (errorResult.ExitCode != 0)
    {
        Console.WriteLine($"📝 Expected error message: {errorResult.StdErr.Split('\n')[0]}");
    }
    else
    {
        Console.WriteLine($"❌ Unexpected: Should have failed with non-existent file");
    }
    Console.WriteLine();

    // Test with invalid entry point
    var invalidEntryResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "InvalidEntryPoint",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"{(invalidEntryResult.ExitCode != 0 ? "✅" : "❌")} Invalid entry point test - Exit Code: {invalidEntryResult.ExitCode} (Expected: Non-zero)");
    if (invalidEntryResult.ExitCode != 0)
    {
        Console.WriteLine($"📝 Expected error message: {invalidEntryResult.StdErr.Split('\n')[0]}");
    }
    else
    {
        Console.WriteLine($"❌ Unexpected: Should have failed with invalid entry point");
    }
    Console.WriteLine();

    // Test 8: Multiple Input Files & Include Paths
    Console.WriteLine("🔧 Test 8: Multiple Files & Include Paths");
    Console.WriteLine("------------------------------------------");

    var multiFileResult = CLI.slangc(
        new SlangC_Options.Builder()
            .SetTarget("hlsl")
            .SetProfile("cs_5_0")
            .SetEntry("main")
            .AddIncludePaths(CLI.WorkingDirectory)
            .AddInputFile("ComponentAddition.slang")
            .Build());

    Console.WriteLine($"{(multiFileResult.ExitCode == 0 ? "✅" : "❌")} Multiple Files with Includes - Exit Code: {multiFileResult.ExitCode}");
    if (multiFileResult.ExitCode != 0 && !string.IsNullOrEmpty(multiFileResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {multiFileResult.StdErr}");
    }
    Console.WriteLine();

    // Test 9: Different Shader Stages
    Console.WriteLine("🔧 Test 9: Different Shader Stages");
    Console.WriteLine("-----------------------------------");

    var vertexResult = CLI.slangc(
        target: "hlsl",
        profile: "vs_5_0",
        entry: "main",
        stage: "vertex",
        inputFiles: ["ComponentAddition.slang"]);
    Console.WriteLine($"{(vertexResult.ExitCode == 0 ? "✅" : "❌")} Vertex Shader Stage - Exit Code: {vertexResult.ExitCode}");
    if (vertexResult.ExitCode != 0 && !string.IsNullOrEmpty(vertexResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {vertexResult.StdErr}");
    }

    var fragmentResult = CLI.slangc(
        target: "hlsl",
        profile: "ps_5_0",
        entry: "main",
        stage: "fragment",
        inputFiles: ["ComponentAddition.slang"]);
    Console.WriteLine($"{(fragmentResult.ExitCode == 0 ? "✅" : "❌")} Fragment Shader Stage - Exit Code: {fragmentResult.ExitCode}");
    if (fragmentResult.ExitCode != 0 && !string.IsNullOrEmpty(fragmentResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {fragmentResult.StdErr}");
    }
    Console.WriteLine();

    // Test 10: Working Directory Demonstration
    Console.WriteLine("🔧 Test 10: Working Directory Control");
    Console.WriteLine("-------------------------------------");

    Console.WriteLine($"📁 Current Working Directory: {CLI.WorkingDirectory}");

    // Change working directory temporarily
    var originalWD = CLI.WorkingDirectory;
    CLI.WorkingDirectory = Path.Combine(originalWD, @"Shaders\");
    Console.WriteLine($"📁 Changed Working Directory: {CLI.WorkingDirectory}");

    var wdResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        inputFiles: [Path.Combine("CLInvoke", "AverageColor.slang")]);

    Console.WriteLine($"{(wdResult.ExitCode == 0 ? "✅" : "❌")} Different Working Directory - Exit Code: {wdResult.ExitCode}");
    if (wdResult.ExitCode != 0 && !string.IsNullOrEmpty(wdResult.StdErr))
    {
        Console.WriteLine($"❌ Error: {wdResult.StdErr}");
    }

    // Restore working directory
    CLI.WorkingDirectory = originalWD;
    Console.WriteLine($"📁 Restored Working Directory: {CLI.WorkingDirectory}");
    Console.WriteLine();

    // Summary - Calculate actual results
    int totalTests = 0;
    int passedTests = 0;
    
    // Count successful tests (exit code 0 for positive tests, non-zero for negative tests)
    var testResults = new[]
    {
        ("HLSL Compilation", hlslResult.ExitCode == 0),
        ("GLSL Compilation", glslResult.ExitCode == 0),
        ("SPIR-V Compilation", spirvResult.ExitCode == 0),
        ("Builder Pattern", builderResult.ExitCode == 0),
        ("File Output", fileOutputResult.ExitCode == 0),
        ("Reflection JSON", reflectionResult.ExitCode == 0),
        ("Advanced Options", advancedResult.ExitCode == 0),
        ("Raw Command String", rawCommandResult.ExitCode == 0),
        ("Non-existent file (should fail)", errorResult.ExitCode != 0),
        ("Invalid entry point (should fail)", invalidEntryResult.ExitCode != 0),
        ("Multiple Files with Includes", multiFileResult.ExitCode == 0),
        ("Vertex Shader Stage", vertexResult.ExitCode == 0),
        ("Fragment Shader Stage", fragmentResult.ExitCode == 0),
        ("Working Directory Control", wdResult.ExitCode == 0)
    };

    totalTests = testResults.Length;
    passedTests = testResults.Count(t => t.Item2);

    Console.WriteLine("🎉 CLI Invocation Feature Summary");
    Console.WriteLine("==================================");
    Console.WriteLine($"📊 Test Results: {passedTests}/{totalTests} tests passed ({(passedTests * 100.0 / totalTests):F1}%)");
    Console.WriteLine();
    
    // Show detailed feature status
    foreach (var (testName, passed) in testResults)
    {
        Console.WriteLine($"{(passed ? "✅" : "❌")} {testName}");
    }
    
    Console.WriteLine();
    Console.WriteLine("📋 Features Demonstrated:");
    Console.WriteLine("• Basic compilation with target/profile/entry parameters");
    Console.WriteLine("• Builder pattern for complex option configuration");
    Console.WriteLine("• File output generation");
    Console.WriteLine("• Reflection JSON metadata generation");
    Console.WriteLine("• Optimization levels and debug information");
    Console.WriteLine("• Preprocessor definitions and macros");
    Console.WriteLine("• Warning control and error handling");
    Console.WriteLine("• Raw command string for experimental features");
    Console.WriteLine("• Multiple input files and include paths");
    Console.WriteLine("• Different shader stages (compute, vertex, fragment)");
    Console.WriteLine("• Working directory control");
    Console.WriteLine("• Multiple target formats (HLSL, GLSL, SPIR-V)");
    Console.WriteLine("• Comprehensive error handling and validation");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Unexpected error: {ex.Message}");
    Console.WriteLine($"📍 Stack trace: {ex.StackTrace}");
}

Console.WriteLine("\n🏁 Sample completed! Press any key to exit...");
Console.ReadKey();
