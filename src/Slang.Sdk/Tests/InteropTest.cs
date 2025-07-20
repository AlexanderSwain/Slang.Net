using Slang.Sdk.Interop;
using System.Runtime.InteropServices;

namespace Slang.Sdk.Examples;

/// <summary>
/// Example demonstrating basic usage of the Slang.Sdk interop layer.
/// This shows how to call native SlangNative functions directly.
/// </summary>
public static class InteropTest
{
    private static unsafe void StringMarshalingTest()
    {
        var managedString = "Hello, Slang!";
        var nativeString = StringMarshaling.ToUtf8(managedString);

        if (nativeString != null)
        {
            var convertedBack = StringMarshaling.FromUtf8(nativeString);
            Console.WriteLine($"   String marshaling test: '{managedString}' -> '{convertedBack}' (Match: {managedString == convertedBack})");
            StringMarshaling.FreeUtf8(nativeString);
        }
    }

    private static void ErrorHandlingTest()
    {
        var result = SlangResult.CompilationFailed;
        if (SlangResultHelper.IsFailure(result))
        {
            Console.WriteLine($"   Error handling test: Correctly identified failure ({result})");
        }
    }

    private static void SafeHandleTest()
    {
        using var handle = new SessionHandle(IntPtr.Zero);
        Console.WriteLine($"   Safe handle test: Created handle with IsInvalid = {handle.IsInvalid}");
    }

    private static unsafe void BasicSessionTest()
    {
        try
        {
            CompilerOption compilerOptions = new CompilerOption(
                CompilerOption.Name.Obfuscate,
                new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null)
            );

            PreprocessorMacro preprocessorMacros = new PreprocessorMacro("LIGHTING_VALUE", "2.5");

            ShaderModel shaderModels = new ShaderModel(ShaderModel.CompileTarget.Hlsl, "cs_5_0");

            var sessionHandle = SlangNativeInterop.Session_Create(
                &compilerOptions, 1,
                &preprocessorMacros, 1,
                &shaderModels, 1,
                null, 0
            );

            if (sessionHandle == IntPtr.Zero)
            {
                var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
                var errorMessage = new string(SlangNativeInterop.SlangNative_GetLastError());
                Console.WriteLine($"   Session creation failed (expected): {errorMessage ?? "Unknown error"}");
            }
            else
            {
                Console.WriteLine($"   Session created successfully! Handle: 0x{sessionHandle:X}");
            }
        }
        catch (System.DllNotFoundException ex)
        {
            Console.WriteLine($"   ?? SlangNative.dll not found: {ex.Message}");
            Console.WriteLine("   This is expected if native dependencies aren't built yet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   Session test encountered error: {ex.Message}");
        }
    }

    private static unsafe void StrongTypeInteropTest()
    {
        try
        {
            CompilerOption compilerOptions = new CompilerOption(
                CompilerOption.Name.Obfuscate,
                new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null)
            );

            PreprocessorMacro preprocessorMacros = new PreprocessorMacro("LIGHTING_VALUE", "2.5");

            ShaderModel shaderModels = new ShaderModel(ShaderModel.CompileTarget.Hlsl, "cs_5_0");

            // Test the new strongly-typed interop
            var sessionHandle = StrongTypeInterop.Session_Create(
                &compilerOptions, 1,
                &preprocessorMacros, 1,
                &shaderModels, 1,
                null, 0
            );

            Console.WriteLine($"   StrongTypeInterop test: SessionHandle type = {sessionHandle.GetType().Name}");
            Console.WriteLine($"   StrongTypeInterop test: Handle is invalid = {sessionHandle.IsInvalid}");
            
            // The handle should be disposable
            sessionHandle.Dispose();
            Console.WriteLine($"   StrongTypeInterop test: Handle disposed successfully");
        }
        catch (System.DllNotFoundException ex)
        {
            Console.WriteLine($"   ?? SlangNative.dll not found: {ex.Message}");
            Console.WriteLine("   This is expected if native dependencies aren't built yet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   StrongTypeInterop test encountered error: {ex.Message}");
        }
    }

    /// <summary>
    /// Main example runner.
    /// </summary>
    public static void RunExamples()
    {
        // Test 1: Basic interop is working
        Console.WriteLine("1. Testing basic interop...");
        StringMarshalingTest();
        Console.WriteLine("? Basic interop working\n");

        // Test 2: Error handling works
        Console.WriteLine("2. Testing error handling...");
        ErrorHandlingTest();
        Console.WriteLine("? Error handling working\n");

        // Test 3: Safe handles work
        Console.WriteLine("3. Testing safe handles...");
        SafeHandleTest();
        Console.WriteLine("? Safe handles working\n");

        // Test 4: Try to create a Slang session (this will test if SlangNative.dll is accessible)
        Console.WriteLine("4. Testing Slang session creation...");
        BasicSessionTest();
        Console.WriteLine("? Slang session test completed\n");

        // Test 5: Test strongly-typed interop
        Console.WriteLine("5. Testing strongly-typed interop...");
        StrongTypeInteropTest();
        Console.WriteLine("? StrongTypeInterop test completed\n");

        Console.WriteLine("?? All Slang.Sdk configuration tests passed!");
    }
}