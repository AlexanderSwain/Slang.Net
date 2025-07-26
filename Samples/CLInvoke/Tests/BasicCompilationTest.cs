using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests basic Slang compilation to different target formats
/// </summary>
public class BasicCompilationTest : ISlangTest
{
    public string TestName => "Basic Compilation";
    public string Description => "Demonstrates basic compilation to HLSL, GLSL, and SPIR-V formats";

    public async Task<TestResult> RunAsync()
    {
        var results = new List<TestResult>();

        // Test HLSL compilation
        var hlslResult = CLI.slangc(
            target: "hlsl",
            profile: "cs_5_0",
            entry: "CS",
            inputFiles: ["AverageColor.slang"]);
        results.Add(TestHelpers.CheckCompilation("HLSL compilation", hlslResult));

        // Test GLSL compilation
        var glslResult = CLI.slangc(
            target: "glsl",
            profile: "glsl_460",
            entry: "CS",
            stage: "compute",
            inputFiles: ["AverageColor.slang"]);
        results.Add(TestHelpers.CheckCompilation("GLSL compilation", glslResult));

        // Test SPIR-V compilation
        var spirvResult = CLI.slangc(
            target: "spirv",
            profile: "sm_6_0",
            entry: "main",
            stage: "compute",
            inputFiles: ["ComponentAddition.slang"]);
        results.Add(TestHelpers.CheckCompilation("SPIR-V compilation", spirvResult));

        // Check if all sub-tests passed
        var allPassed = results.All(r => r.Success);
        var passedCount = results.Count(r => r.Success);
        
        var message = $"{passedCount}/{results.Count} target formats compiled successfully";
        var details = string.Join("\n   ", results.Select(r => 
            $"{(r.Success ? "✅" : "❌")} {r.Message}"));

        return new TestResult(allPassed, message, details);
    }
}
