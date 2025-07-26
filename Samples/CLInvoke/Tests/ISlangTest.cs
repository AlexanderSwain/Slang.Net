using Slang.Sdk;

namespace CLInvoke.Tests;

/// <summary>
/// Interface for all Slang CLI tests
/// </summary>
public interface ISlangTest
{
    /// <summary>
    /// The display name for this test
    /// </summary>
    string TestName { get; }
    
    /// <summary>
    /// A brief description of what this test demonstrates
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Execute the test and return the result
    /// </summary>
    /// <returns>Test result with success status and optional message</returns>
    Task<TestResult> RunAsync();
}

/// <summary>
/// Result of a test execution
/// </summary>
public record TestResult(bool Success, string Message = "", string? Details = null);
