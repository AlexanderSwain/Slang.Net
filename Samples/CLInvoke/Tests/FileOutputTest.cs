using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests file output capabilities
/// </summary>
public class FileOutputTest : ISlangTest
{
    public string TestName => "File Output";
    public string Description => "Demonstrates compiling shaders to output files";

    public async Task<TestResult> RunAsync()
    {
        // Compile to output file
        var fileOutputResult = CLI.slangc(
            target: "hlsl",
            profile: "cs_5_0",
            entry: "CS",
            outputPath: "AverageColor_output.hlsl",
            inputFiles: ["AverageColor.slang"]);

        var compilationResult = TestHelpers.CheckCompilation("File output compilation", fileOutputResult);
        
        if (!compilationResult.Success)
            return compilationResult;

        // Check if file was created
        var outputFile = Path.Combine(CLI.WorkingDirectory, "AverageColor_output.hlsl");
        if (File.Exists(outputFile))
        {
            var fileContent = File.ReadAllText(outputFile);
            return new TestResult(true, "File output compilation succeeded", 
                $"📁 Output file created successfully ({fileContent.Length} characters)");
        }
        else
        {
            return new TestResult(false, "File output compilation succeeded but file was not created",
                "⚠️ Warning: Compilation succeeded but output file was not found");
        }
    }
}
