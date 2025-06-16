using System;
// Using global using directive

public class EqualityTest
{
    public static void TestEntryPointReflectionEquality()
    {
        Console.WriteLine("Testing EntryPointReflection equality...");

        try
        {
            // Create two EntryPointReflection objects with the same native pointer
            // Note: This is a synthetic test since we're not going through the full compilation
            // In a real scenario, you would get these from ShaderReflection.GetEntryPointByIndex()

            // For now, let's just test the null equality operations which should work
            EntryPointReflection? nullReflection1 = null;
            EntryPointReflection? nullReflection2 = null;
            
            Console.WriteLine("Testing null equality:");
            bool bothNull = (nullReflection1 == nullReflection2);
            bool notEqualNull = (nullReflection1 != nullReflection2);
            Console.WriteLine($"null == null: {bothNull}");
            Console.WriteLine($"null != null: {notEqualNull}");

            Console.WriteLine("EntryPointReflection equality test completed successfully!");
            Console.WriteLine("Note: Full equality testing requires a working shader compilation pipeline.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
