using CLInvoke.Tests;
using System.Reflection;

namespace CLInvoke;

/// <summary>
/// Test runner that discovers and executes all Slang CLI tests
/// </summary>
public class TestRunner
{
    private readonly List<ISlangTest> _tests;

    public TestRunner()
    {
        _tests = DiscoverTests();
    }

    /// <summary>
    /// Discover all test classes that implement ISlangTest
    /// </summary>
    private static List<ISlangTest> DiscoverTests()
    {
        var tests = new List<ISlangTest>();
        var assembly = Assembly.GetExecutingAssembly();
        
        var testTypes = assembly.GetTypes()
            .Where(t => typeof(ISlangTest).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var testType in testTypes)
        {
            try
            {
                if (Activator.CreateInstance(testType) is ISlangTest test)
                {
                    tests.Add(test);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Warning: Could not create instance of test {testType.Name}: {ex.Message}");
            }
        }

        return tests;
    }

    /// <summary>
    /// Run all discovered tests
    /// </summary>
    public async Task<TestSummary> RunAllTestsAsync()
    {
        Console.WriteLine("Slang CLI Invocation Sample");
        Console.WriteLine("================================\n");
        
        var results = new List<(ISlangTest Test, TestResult Result)>();
        
        for (int i = 0; i < _tests.Count; i++)
        {
            var test = _tests[i];
            
            TestHelpers.PrintTestHeader(i + 1, test.TestName, test.Description);
            
            try
            {
                var result = await test.RunAsync();
                results.Add((test, result));
                
                TestHelpers.PrintTestResult(test.TestName, result);
            }
            catch (Exception ex)
            {
                var errorResult = new TestResult(false, $"Test threw exception: {ex.Message}");
                results.Add((test, errorResult));
                TestHelpers.PrintTestResult(test.TestName, errorResult);
            }
            
            Console.WriteLine();
        }

        return GenerateSummary(results);
    }

    /// <summary>
    /// Generate a summary of all test results
    /// </summary>
    private static TestSummary GenerateSummary(List<(ISlangTest Test, TestResult Result)> results)
    {
        var totalTests = results.Count;
        var passedTests = results.Count(r => r.Result.Success);
        var failedTests = totalTests - passedTests;
        
        Console.WriteLine("🎉 CLI Invocation Test Summary");
        Console.WriteLine("===============================");
        Console.WriteLine($"📊 Results: {passedTests}/{totalTests} tests passed ({(passedTests * 100.0 / totalTests):F1}%)");
        
        if (failedTests > 0)
        {
            Console.WriteLine($"❌ Failed: {failedTests} tests");
        }
        
        Console.WriteLine();
        
        // Show detailed results
        foreach (var (test, result) in results)
        {
            Console.WriteLine($"{(result.Success ? "✅" : "❌")} {test.TestName}");
        }
        
        Console.WriteLine();
        Console.WriteLine("📋 Features Demonstrated:");
        Console.WriteLine("• Basic compilation with target/profile/entry parameters");
        Console.WriteLine("• Builder pattern for complex option configuration");
        Console.WriteLine("• File output generation");
        Console.WriteLine("• Reflection JSON metadata generation");
        Console.WriteLine("• Advanced compilation options (optimization, warnings, defines)");
        Console.WriteLine("• Working directory control for cross-project compilation");
        Console.WriteLine("• Comprehensive error handling and validation");
        
        return new TestSummary(totalTests, passedTests, failedTests, results);
    }
}

/// <summary>
/// Summary of test execution results
/// </summary>
public record TestSummary(
    int TotalTests, 
    int PassedTests, 
    int FailedTests, 
    List<(ISlangTest Test, TestResult Result)> Results);
