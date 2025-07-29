using Slang.Sdk.Interop;
using Slang.Sdk.Tests;

namespace Slang.Sdk.Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Slang.Sdk Configuration Test ===\n");

            CLI_Tests.RunAllTests();
            InteropTest.RunExamples();
            BindingTests.RunAllTests();
            PrettyTests.RunAllTests();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}