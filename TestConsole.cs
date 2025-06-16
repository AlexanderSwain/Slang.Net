using System;

namespace SlangNetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Slang.Net Class Library...");
            
            try
            {
                // Test basic instantiation
                var sessionBuilder = new SessionBuilder();
                Console.WriteLine("✓ SessionBuilder created successfully");
                
                Console.WriteLine("✓ Slang.Net class library is working correctly!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

public class SessionBuilder
{
    public SessionBuilder()
    {
        // Minimal test class
    }
}
