using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Tests advanced compilation options like warnings, optimization, and defines
/// </summary>
public class AdvancedOptionsTest : ISlangTest
{
    public string TestName => "Advanced Options";
    public string Description => "Demonstrates advanced compilation options including warnings, optimization, and preprocessor defines";

    public async Task<TestResult> RunAsync()
    {
        var advancedResult = CLI.slangc(
            new SlangC_Options.Builder()
                .SetTarget("hlsl")
                .SetProfile("cs_6_0")
                .SetEntry("CS")
                .SetOptimizationLevel("3")
                .SetDebugInfo(true)
                .SetWarningsAsErrors(false)
                .DisableWarning("unreachable-code")
                .Define("DEBUG_MODE", "1")
                .Define("MAX_THREADS", "1024")
                .SetModuleName("AverageColorModule")
                .AddInputFile("AverageColor.slang")
                .Build());

        var result = TestHelpers.CheckCompilation("Advanced options compilation", advancedResult);
        
        if (result.Success)
        {
            return new TestResult(true, result.Message, 
                "Advanced options: O3 optimization, debug info, warning control, preprocessor defines, custom module name");
        }
        
        return result;
    }
}
