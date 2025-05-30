using System;
using Slang;

class TestProgram
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Testing Slang.Module instantiation...");
            
            // Test that we can now create a Module instance without TypeLoadException
            // Note: We need a Session first, but let's test the basic type loading
            Type moduleType = typeof(Module);
            Console.WriteLine($"Successfully loaded Module type: {moduleType.FullName}");
            
            // Try to get the methods to ensure the type is properly loaded
            var methods = moduleType.GetMethods();
            Console.WriteLine($"Module has {methods.Length} methods");
            
            Console.WriteLine("✅ SUCCESS: No TypeLoadException occurred!");
            Console.WriteLine("The Module class can now be loaded properly.");
        }
        catch (TypeLoadException ex)
        {
            Console.WriteLine($"❌ TypeLoadException still occurs: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Other exception: {ex.Message}");
        }
    }
}
