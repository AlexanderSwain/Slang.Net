using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop
{
    internal static class Utilities
    {
        internal unsafe static string? GetLastError()
        {
            var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
            return new string(errorPtr);
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

        //Delete later
        //internal unsafe static IntPtr ConvertStringArrayToCharPointerArray(string[] stringArray)
        //{
        //    // Allocate memory for the array of pointers
        //    IntPtr pointerArray = Marshal.AllocHGlobal(stringArray.Length * IntPtr.Size);
        //
        //    for (int i = 0; i < stringArray.Length; i++)
        //    {
        //        // Convert each string to a null-terminated ANSI string
        //        IntPtr stringPointer = Marshal.StringToHGlobalAnsi(stringArray[i]);
        //
        //        // Store the pointer in the array
        //        Marshal.WriteIntPtr(pointerArray, i * IntPtr.Size, stringPointer);
        //    }
        //
        //    return pointerArray;
        //}
        //
        //internal unsafe static void FreeCharPointerArray(IntPtr pointerArray, int length)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        // Free each string pointer
        //        IntPtr stringPointer = Marshal.ReadIntPtr(pointerArray, i * IntPtr.Size);
        //        Marshal.FreeHGlobal(stringPointer);
        //    }
        //
        //    // Free the array of pointers
        //    Marshal.FreeHGlobal(pointerArray);
        //}
    }
}
