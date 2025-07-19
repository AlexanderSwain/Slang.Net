using System.Runtime.InteropServices;
using System.Text;

namespace Slang.Sdk.Interop;

/// <summary>
/// Provides utility methods for marshaling strings between managed and native code.
/// </summary>
internal static class StringMarshaling
{
    /// <summary>
    /// Converts a managed string to a UTF-8 byte pointer for native interop.
    /// </summary>
    /// <param name="managedString">The managed string to convert.</param>
    /// <returns>A pointer to the UTF-8 encoded string, or null if the input is null.</returns>
    public static unsafe byte* ToUtf8(string? managedString)
    {
        if (managedString == null)
            return null;

        var bytes = Encoding.UTF8.GetBytes(managedString);
        var ptr = (byte*)Marshal.AllocHGlobal(bytes.Length + 1);
        Marshal.Copy(bytes, 0, (nint)ptr, bytes.Length);
        ptr[bytes.Length] = 0; // Null terminator
        return ptr;
    }

    /// <summary>
    /// Converts a UTF-8 byte pointer to a managed string.
    /// </summary>
    /// <param name="utf8Ptr">The UTF-8 byte pointer.</param>
    /// <returns>A managed string, or null if the pointer is null.</returns>
    public static unsafe string? FromUtf8(byte* utf8Ptr)
    {
        if (utf8Ptr == null)
            return null;

        // Find the null terminator
        var length = 0;
        while (utf8Ptr[length] != 0)
            length++;

        return Encoding.UTF8.GetString(utf8Ptr, length);
    }

    /// <summary>
    /// Frees a UTF-8 string pointer that was allocated by ToUtf8.
    /// </summary>
    /// <param name="utf8Ptr">The pointer to free.</param>
    public static unsafe void FreeUtf8(byte* utf8Ptr)
    {
        if (utf8Ptr != null)
            Marshal.FreeHGlobal((nint)utf8Ptr);
    }

    /// <summary>
    /// Converts an array of managed strings to an array of UTF-8 byte pointers.
    /// </summary>
    /// <param name="strings">The array of managed strings.</param>
    /// <returns>A pointer to an array of UTF-8 string pointers.</returns>
    public static unsafe byte** ToUtf8Array(string[]? strings)
    {
        if (strings == null || strings.Length == 0)
            return null;

        var ptrs = (byte**)Marshal.AllocHGlobal(strings.Length * sizeof(byte*));
        for (int i = 0; i < strings.Length; i++)
        {
            ptrs[i] = ToUtf8(strings[i]);
        }
        return ptrs;
    }

    /// <summary>
    /// Frees an array of UTF-8 string pointers that was allocated by ToUtf8Array.
    /// </summary>
    /// <param name="utf8Array">The array pointer to free.</param>
    /// <param name="length">The number of strings in the array.</param>
    public static unsafe void FreeUtf8Array(byte** utf8Array, int length)
    {
        if (utf8Array == null)
            return;

        for (int i = 0; i < length; i++)
        {
            FreeUtf8(utf8Array[i]);
        }
        Marshal.FreeHGlobal((nint)utf8Array);
    }
}