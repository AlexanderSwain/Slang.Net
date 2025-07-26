using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests error handling with invalid inputs
/// </summary>
public class ErrorHandlingTest : ISlangTest
{
    public string TestName => "Error Handling";
    public string Description => "Demonstrates proper error handling for invalid inputs and configurations";

    public async Task<TestResult> RunAsync()
    {
        var results = new List<TestResult>();

        // Test with non-existent file
        var errorResult = CLI.slangc(
            target: "hlsl",
            profile: "cs_5_0",
            entry: "CS",
            inputFiles: ["NonExistentShader.slang"]);
        
        results.Add(TestHelpers.CheckExpectedFailure("Non-existent file test", errorResult, 
            "Should fail when input file doesn't exist"));

        // Test with invalid entry point
        var invalidEntryResult = CLI.slangc(
            target: "hlsl",
            profile: "cs_5_0",
            entry: "InvalidEntryPoint",
            inputFiles: ["AverageColor.slang"]);
        
        results.Add(TestHelpers.CheckExpectedFailure("Invalid entry point test", invalidEntryResult, 
            "Should fail when entry point doesn't exist"));

        // Check if all sub-tests passed
        var allPassed = results.All(r => r.Success);
        var passedCount = results.Count(r => r.Success);
        
        var message = $"{passedCount}/{results.Count} error conditions handled correctly";
        var details = string.Join("\n   ", results.Select(r => 
            $"{(r.Success ? "✅" : "❌")} {r.Message}"));

        return new TestResult(allPassed, message, details);
    }
}
