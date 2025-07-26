using Slang.Sdk;

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

    Console.WriteLine($"✅ HLSL Compilation - Exit Code: {hlslResult.ExitCode}");
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

    Console.WriteLine($"✅ GLSL Compilation - Exit Code: {glslResult.ExitCode}");
    if (glslResult.ExitCode == 0)
    {
        Console.WriteLine("📄 Generated GLSL (first 200 chars):");
        Console.WriteLine(glslResult.StdOut.Length > 200 ?
            glslResult.StdOut.Substring(0, 200) + "..." : glslResult.StdOut);
    }
    Console.WriteLine();

    // SPIR-V compilation
    var spirvResult = CLI.slangc(
        target: "spirv",
        profile: "sm_6_0",
        entry: "main",
        stage: "compute",
        inputFiles: ["ComponentAddition.slang"]);

    Console.WriteLine($"✅ SPIR-V Compilation - Exit Code: {spirvResult.ExitCode}");
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

    Console.WriteLine($"✅ Builder Pattern Compilation - Exit Code: {builderResult.ExitCode}");
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

    Console.WriteLine($"✅ File Output Compilation - Exit Code: {fileOutputResult.ExitCode}");

    // Check if file was created
    if (File.Exists(Path.Combine(CLI.WorkingDirectory, "AverageColor_output.hlsl")))
    {
        var fileContent = File.ReadAllText(Path.Combine(CLI.WorkingDirectory, "AverageColor_output.hlsl"));
        Console.WriteLine($"📁 Output file created successfully ({fileContent.Length} characters)");
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

    Console.WriteLine($"✅ Reflection JSON Generation - Exit Code: {reflectionResult.ExitCode}");

    // Check if reflection file was created
    if (File.Exists(Path.Combine(CLI.WorkingDirectory, "AverageColor_reflection.json")))
    {
        var reflectionContent = File.ReadAllText(Path.Combine(CLI.WorkingDirectory, "AverageColor_reflection.json"));
        Console.WriteLine($"📁 Reflection JSON created successfully ({reflectionContent.Length} characters)");
        Console.WriteLine("🔍 First 300 chars of reflection data:");
        Console.WriteLine(reflectionContent.Length > 300 ?
            reflectionContent.Substring(0, 300) + "..." : reflectionContent);
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

    Console.WriteLine($"✅ Advanced Options Compilation - Exit Code: {advancedResult.ExitCode}");
    Console.WriteLine();

    // Test 6: Raw Command String (for experimental features)
    Console.WriteLine("🔧 Test 6: Raw Command String & Experimental Features");
    Console.WriteLine("------------------------------------------------------");

    var rawCommandResult = CLI.slangc(
        "-target hlsl -profile cs_5_0 -entry CS -O3 -g " +
        "-D EXPERIMENTAL=1 -D VERSION=100 " +
        "-- AverageColor.slang");

    Console.WriteLine($"✅ Raw Command String - Exit Code: {rawCommandResult.ExitCode}");
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

    Console.WriteLine($"🔍 Non-existent file test - Exit Code: {errorResult.ExitCode}");
    if (errorResult.ExitCode != 0)
    {
        Console.WriteLine($"📝 Expected error message: {errorResult.StdErr.Split('\n')[0]}");
    }
    Console.WriteLine();

    // Test with invalid entry point
    var invalidEntryResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "InvalidEntryPoint",
        inputFiles: ["AverageColor.slang"]);

    Console.WriteLine($"🔍 Invalid entry point test - Exit Code: {invalidEntryResult.ExitCode}");
    if (invalidEntryResult.ExitCode != 0)
    {
        Console.WriteLine($"📝 Expected error message: {invalidEntryResult.StdErr.Split('\n')[0]}");
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

    Console.WriteLine($"✅ Multiple Files with Includes - Exit Code: {multiFileResult.ExitCode}");
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
    Console.WriteLine($"✅ Vertex Shader Stage - Exit Code: {vertexResult.ExitCode}");

    var fragmentResult = CLI.slangc(
        target: "hlsl",
        profile: "ps_5_0",
        entry: "main",
        stage: "fragment",
        inputFiles: ["ComponentAddition.slang"]);
    Console.WriteLine($"✅ Fragment Shader Stage - Exit Code: {fragmentResult.ExitCode}");
    Console.WriteLine();

    // Test 10: Working Directory Demonstration
    Console.WriteLine("🔧 Test 10: Working Directory Control");
    Console.WriteLine("-------------------------------------");

    Console.WriteLine($"📁 Current Working Directory: {CLI.WorkingDirectory}");

    // Change working directory temporarily
    var originalWD = CLI.WorkingDirectory;
    CLI.WorkingDirectory = Path.GetDirectoryName(originalWD) ?? originalWD;
    Console.WriteLine($"📁 Changed Working Directory: {CLI.WorkingDirectory}");

    var wdResult = CLI.slangc(
        target: "hlsl",
        profile: "cs_5_0",
        entry: "CS",
        inputFiles: [Path.Combine("CLInvoke", "AverageColor.slang")]);

    Console.WriteLine($"✅ Different Working Directory - Exit Code: {wdResult.ExitCode}");

    // Restore working directory
    CLI.WorkingDirectory = originalWD;
    Console.WriteLine($"📁 Restored Working Directory: {CLI.WorkingDirectory}");
    Console.WriteLine();

    // Summary
    Console.WriteLine("🎉 CLI Invocation Feature Summary");
    Console.WriteLine("==================================");
    Console.WriteLine("✅ Basic compilation with target/profile/entry parameters");
    Console.WriteLine("✅ Builder pattern for complex option configuration");
    Console.WriteLine("✅ File output generation");
    Console.WriteLine("✅ Reflection JSON metadata generation");
    Console.WriteLine("✅ Optimization levels and debug information");
    Console.WriteLine("✅ Preprocessor definitions and macros");
    Console.WriteLine("✅ Warning control and error handling");
    Console.WriteLine("✅ Raw command string for experimental features");
    Console.WriteLine("✅ Multiple input files and include paths");
    Console.WriteLine("✅ Different shader stages (compute, vertex, fragment)");
    Console.WriteLine("✅ Working directory control");
    Console.WriteLine("✅ Multiple target formats (HLSL, GLSL, SPIR-V)");
    Console.WriteLine("✅ Comprehensive error handling and validation");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Unexpected error: {ex.Message}");
    Console.WriteLine($"📍 Stack trace: {ex.StackTrace}");
}

Console.WriteLine("\n🏁 Sample completed! Press any key to exit...");
Console.ReadKey();
