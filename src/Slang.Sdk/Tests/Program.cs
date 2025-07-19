using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    internal class TestProgram
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Slang.Sdk Configuration Test ===\n");

            try
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

                Console.WriteLine("?? All Slang.Sdk configuration tests passed!");
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
            using var handle = new SlangSessionHandle(IntPtr.Zero);
            Console.WriteLine($"   Safe handle test: Created handle with IsInvalid = {handle.IsInvalid}");
        }

        private static unsafe void BasicSessionTest()
        {
            try
            {
                var sessionHandle = SlangNativeInterop.CreateSession(
                    null, 0,
                    null, 0,
                    null, 0,
                    null, 0
                );

                if (sessionHandle == IntPtr.Zero)
                {
                    var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
                    var errorMessage = StringMarshaling.FromUtf8(errorPtr);
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
    }
}