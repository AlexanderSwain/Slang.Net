using CompilationAPI;
using Slang.Sdk;
using System.Text;

// Set console encoding for better output formatting
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("🎯 Slang.Net Compilation API Showcase");
Console.WriteLine("=====================================\n");

try
{
    var showcase = new CompilationAPIShowcase();
    await showcase.RunAllDemosAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Fatal error: {ex.Message}");
    Console.WriteLine($"💥 Stack trace: {ex.StackTrace}");
    Environment.ExitCode = 1;
}

Console.WriteLine("\n🏁 Compilation API showcase completed! Press any key to exit...");
Console.ReadKey();
