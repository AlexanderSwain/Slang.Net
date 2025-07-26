using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests working directory control for cross-project compilation
/// </summary>
public class WorkingDirectoryTest : ISlangTest
{
    public string TestName => "Working Directory Control";
    public string Description => "Demonstrates changing working directory to compile shaders from other projects";

    public async Task<TestResult> RunAsync()
    {
        // Save current working directory
        var originalWorkingDir = CLI.WorkingDirectory;
        
        try
        {
            // Change to DirectX sample directory to access its shader
            var directXSampleDir = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "..", "..", "..", "..", 
                "Slang.Net.Samples.DirectXSilkNet"));

            CLI.WorkingDirectory = directXSampleDir;

            // Compile a shader from the DirectX sample directory
            var workingDirResult = CLI.slangc(
                target: "hlsl",
                profile: "vs_5_0",
                entry: "VS",
                stage: "vertex",
                inputFiles: ["SimpleVertex.slang"]);

            var result = TestHelpers.CheckCompilation("Working directory compilation", workingDirResult);
            
            if (result.Success)
            {
                return new TestResult(true, result.Message, 
                    $"📁 Successfully compiled shader from: {Path.GetFileName(directXSampleDir)}");
            }
            
            return result;
        }
        finally
        {
            // Always restore original working directory
            CLI.WorkingDirectory = originalWorkingDir;
        }
    }
}
