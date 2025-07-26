using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests reflection JSON metadata generation
/// </summary>
public class ReflectionTest : ISlangTest
{
    public string TestName => "Reflection JSON";
    public string Description => "Demonstrates generating reflection metadata in JSON format";

    public async Task<TestResult> RunAsync()
    {
        var reflectionResult = CLI.slangc(
            target: "hlsl",
            profile: "cs_5_0",
            entry: "CS",
            reflectionJsonPath: "AverageColor_reflection.json",
            inputFiles: ["AverageColor.slang"]);

        var compilationResult = TestHelpers.CheckCompilation("Reflection JSON generation", reflectionResult);
        
        if (!compilationResult.Success)
            return compilationResult;

        // Check if reflection file was created
        var reflectionFile = Path.Combine(CLI.WorkingDirectory, "AverageColor_reflection.json");
        if (File.Exists(reflectionFile))
        {
            var reflectionContent = File.ReadAllText(reflectionFile);
            var preview = TestHelpers.TruncateOutput(reflectionContent, 300);
            
            return new TestResult(true, "Reflection JSON generation succeeded", 
                $"📁 Reflection JSON created ({reflectionContent.Length} chars)\n🔍 Preview: {preview}");
        }
        else
        {
            return new TestResult(false, "Reflection JSON generation succeeded but file was not created",
                "⚠️ Warning: Compilation succeeded but reflection JSON file was not found");
        }
    }
}
