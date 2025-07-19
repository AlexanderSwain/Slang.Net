using Slang.Sdk.Interop;
using System.Runtime.InteropServices;

namespace Slang.Sdk.Examples;

/// <summary>
/// Example demonstrating basic usage of the Slang.Sdk interop layer.
/// This shows how to call native SlangNative functions directly.
/// </summary>
public static class InteropExample
{
    /// <summary>
    /// Example of creating a session and getting last error.
    /// </summary>
    public static unsafe void BasicSessionExample()
    {
        try
        {
            Console.WriteLine("Creating Slang session...");

            // Create arrays for parameters (empty for this example)
            var options = IntPtr.Zero;
            var macros = IntPtr.Zero;
            var models = IntPtr.Zero;
            var searchPaths = (byte**)IntPtr.Zero;

            // Create the session
            var sessionHandle = SlangNativeInterop.CreateSession(
                (void*)options, 0,     // No compiler options
                (void*)macros, 0,      // No preprocessor macros  
                (void*)models, 0,      // No shader models
                searchPaths, 0         // No search paths
            );

            if (sessionHandle == IntPtr.Zero)
            {
                // Get the last error if session creation failed
                var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
                var errorMessage = StringMarshaling.FromUtf8(errorPtr);
                Console.WriteLine($"Session creation failed: {errorMessage ?? "Unknown error"}");
            }
            else
            {
                Console.WriteLine($"Session created successfully! Handle: 0x{sessionHandle:X}");
                
                // The session will be cleaned up automatically by Slang
                // In a real application, you would use this session to load modules
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during session creation: {ex.Message}");
        }
    }

    /// <summary>
    /// Example of using safe handles for resource management.
    /// </summary>
    public static void SafeHandleExample()
    {
        try
        {
            Console.WriteLine("Creating session with safe handle...");

            // Create a session (simplified for example)
            var sessionPtr = IntPtr.Zero; // In real usage, you'd call CreateSession

            if (sessionPtr != IntPtr.Zero)
            {
                using var session = new SlangSessionHandle(sessionPtr);
                Console.WriteLine($"Session handle: {session.Handle}");
                
                // The session will be automatically disposed when the using block exits
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    /// <summary>
    /// Example of error handling with SlangResult.
    /// </summary>
    public static void ErrorHandlingExample()
    {
        try
        {
            // Simulate a Slang operation that might fail
            var result = SlangResult.CompilationFailed;

            // Check if the operation was successful
            if (SlangResultHelper.IsFailure(result))
            {
                Console.WriteLine($"Operation failed with result: {result}");
                
                // This would throw an exception
                // SlangResultHelper.ThrowOnFailure(result, "Custom error message");
            }
            else
            {
                Console.WriteLine("Operation succeeded!");
            }
        }
        catch (SlangException ex)
        {
            Console.WriteLine($"Slang exception: {ex.Message} (Result: {ex.Result})");
        }
    }

    /// <summary>
    /// Example of string marshaling between managed and native code.
    /// </summary>
    public static unsafe void StringMarshalingExample()
    {
        try
        {
            Console.WriteLine("Testing string marshaling...");

            // Convert a managed string to native UTF-8
            var managedString = "Hello, Slang!";
            var nativeString = StringMarshaling.ToUtf8(managedString);

            if (nativeString != null)
            {
                // Convert back to managed string
                var convertedBack = StringMarshaling.FromUtf8(nativeString);
                Console.WriteLine($"Original: '{managedString}'");
                Console.WriteLine($"Converted back: '{convertedBack}'");
                Console.WriteLine($"Strings match: {managedString == convertedBack}");

                // Clean up the native string
                StringMarshaling.FreeUtf8(nativeString);
            }

            // Example with string arrays
            var stringArray = new[] { "path1", "path2", "path3" };
            var nativeArray = StringMarshaling.ToUtf8Array(stringArray);

            if (nativeArray != null)
            {
                Console.WriteLine($"Created native array with {stringArray.Length} strings");
                
                // Clean up the native array
                StringMarshaling.FreeUtf8Array(nativeArray, stringArray.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during string marshaling: {ex.Message}");
        }
    }

    /// <summary>
    /// Main example runner.
    /// </summary>
    public static void RunExamples()
    {
        Console.WriteLine("=== Slang.Sdk Interop Examples ===\n");

        Console.WriteLine("1. Basic Session Example:");
        BasicSessionExample();
        Console.WriteLine();

        Console.WriteLine("2. Safe Handle Example:");
        SafeHandleExample();
        Console.WriteLine();

        Console.WriteLine("3. Error Handling Example:");
        ErrorHandlingExample();
        Console.WriteLine();

        Console.WriteLine("4. String Marshaling Example:");
        StringMarshalingExample();
        Console.WriteLine();

        Console.WriteLine("=== Examples Complete ===");
    }
}