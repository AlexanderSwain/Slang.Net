using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop
{
    internal static class Utilities
    {
        internal unsafe static string? GetLastError()
        {
            var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
            return FromCharPtr(errorPtr);
        }

        internal unsafe static string? FromCharPtr(char* ptr)
        {
            return Marshal.PtrToStringAnsi((IntPtr)ptr);
        }

        internal unsafe static char*[] To_CStrArr(this string[] arr)
        {
            // Allocate memory for char* array
            IntPtr[] charPointers = new IntPtr[arr.Length];

            char*[] result = new char*[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                fixed (char* strPtr = arr[i])
                {
                    result[i] = strPtr;
                }
            }

            return result;
        }
    }
}
