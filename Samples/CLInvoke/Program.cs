using Slang.Sdk;
using System.Text;
using CLInvoke;

// Fix Unicode character rendering by setting console encoding to UTF-8
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Set working directory to the sample folder for easier file access
CLI.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

try
{
    // Create and run the test suite
    var testRunner = new TestRunner();
    var summary = await testRunner.RunAllTestsAsync();
    
    // Exit with appropriate code
    Environment.ExitCode = summary.FailedTests > 0 ? 1 : 0;
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Unexpected error running tests: {ex.Message}");
    Console.WriteLine($"� Stack trace: {ex.StackTrace}");
    Environment.ExitCode = 1;
}

Console.WriteLine("\n🏁 Sample completed! Press any key to exit...");
Console.ReadKey();
