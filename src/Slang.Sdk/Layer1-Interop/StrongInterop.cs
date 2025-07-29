using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop;

/// <summary>
/// Strongly-typed wrapper methods for SlangNative.dll functions.
/// These provide type-safe access with proper handle types and string marshalling.
/// </summary>
internal static unsafe partial class StrongInterop
{
    // This class is split across multiple files for better organization:
    // - StrongTypeInterop.Session.cs - Session management APIs
    // - StrongTypeInterop.Module.cs - Module compilation APIs  
    // - StrongTypeInterop.Reflection.cs - Reflection APIs
    // - StrongTypeInterop.Attributes.cs - Attribute and modifier APIs
}