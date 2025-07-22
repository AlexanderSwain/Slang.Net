using Slang.Sdk.Interop;
using Slang.Sdk.Tests;

namespace Slang.Sdk.Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Slang.Sdk Configuration Test ===\n");

            try
            {
                InteropTest.RunExamples();
                BindingTests.RunAllTests();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Configuration test failed: {ex.Message}");
                Console.WriteLine($"   Stack trace: {ex.StackTrace}");
                return;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}