using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests the Builder pattern for complex compilation options
/// </summary>
public class BuilderPatternTest : ISlangTest
{
    public string TestName => "Builder Pattern";
    public string Description => "Demonstrates using the Builder pattern for complex compilation configurations";

    public async Task<TestResult> RunAsync()
    {
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

        var result = TestHelpers.CheckCompilation("Builder pattern compilation", builderResult);
        
        if (result.Success)
        {
            return new TestResult(true, result.Message, 
                "Builder pattern allows for fluent, readable configuration of complex compilation options");
        }
        
        return result;
    }
}
