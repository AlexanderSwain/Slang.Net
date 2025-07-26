using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Utility class for common test operations and output formatting
/// </summary>
public static class TestHelpers
{
    /// <summary>
    /// Format a test result for console output
    /// </summary>
    public static void PrintTestResult(string testName, TestResult result)
    {
        var icon = result.Success ? "✅" : "❌";
        Console.WriteLine($"{icon} {testName} - {result.Message}");
        
        if (!string.IsNullOrEmpty(result.Details))
        {
            Console.WriteLine($"   {result.Details}");
        }
    }

    /// <summary>
    /// Print a test header with formatting
    /// </summary>
    public static void PrintTestHeader(int testNumber, string testName, string description)
    {
        Console.WriteLine($"🔧 Test {testNumber}: {testName}");
        Console.WriteLine(new string('-', Math.Max(20, testName.Length + 12)));
        if (!string.IsNullOrEmpty(description))
        {
            Console.WriteLine($"💡 {description}");
        }
    }

    /// <summary>
    /// Truncate output for display
    /// </summary>
    public static string TruncateOutput(string output, int maxLength = 200)
    {
        if (string.IsNullOrEmpty(output))
            return "";
            
        return output.Length > maxLength 
            ? output.Substring(0, maxLength) + "..." 
            : output;
    }

    /// <summary>
    /// Check if a compilation result is successful and format the message
    /// </summary>
    public static TestResult CheckCompilation(string operation, object result)
    {
        // Use reflection to get ExitCode and StdErr properties
        var exitCodeProp = result.GetType().GetProperty("ExitCode");
        var stdErrProp = result.GetType().GetProperty("StdErr");
        var stdOutProp = result.GetType().GetProperty("StdOut");
        
        if (exitCodeProp == null)
            return new TestResult(false, "Unable to check result");
            
        var exitCode = (int)exitCodeProp.GetValue(result)!;
        var stdErr = stdErrProp?.GetValue(result)?.ToString() ?? "";
        var stdOut = stdOutProp?.GetValue(result)?.ToString() ?? "";
        
        if (exitCode == 0)
        {
            var details = !string.IsNullOrEmpty(stdOut) 
                ? $"📄 Output: {TruncateOutput(stdOut, 150)}"
                : "";
            return new TestResult(true, $"{operation} succeeded (Exit Code: {exitCode})", details);
        }
        else
        {
            var errorMsg = !string.IsNullOrEmpty(stdErr) 
                ? TruncateOutput(stdErr.Split('\n')[0])
                : "Unknown error";
            return new TestResult(false, $"{operation} failed (Exit Code: {exitCode})", $"❌ Error: {errorMsg}");
        }
    }

    /// <summary>
    /// Check if a compilation result failed as expected (for negative tests)
    /// </summary>
    public static TestResult CheckExpectedFailure(string operation, object result, string reason)
    {
        var exitCodeProp = result.GetType().GetProperty("ExitCode");
        var stdErrProp = result.GetType().GetProperty("StdErr");
        
        if (exitCodeProp == null)
            return new TestResult(false, "Unable to check result");
            
        var exitCode = (int)exitCodeProp.GetValue(result)!;
        var stdErr = stdErrProp?.GetValue(result)?.ToString() ?? "";
        
        if (exitCode != 0)
        {
            var errorMsg = !string.IsNullOrEmpty(stdErr) 
                ? TruncateOutput(stdErr.Split('\n')[0])
                : "Unknown error";
            return new TestResult(true, $"{operation} failed as expected (Exit Code: {exitCode})", $"📝 {reason}: {errorMsg}");
        }
        else
        {
            return new TestResult(false, $"{operation} should have failed but succeeded", $"❌ Expected failure: {reason}");
        }
    }
}
