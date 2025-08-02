using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents the result of a Slang compilation operation.
/// </summary>
internal class CompilationResult
{
    /// <summary>
    /// The compiled byte code.
    /// </summary>
    internal byte[] Compiled { get; }

    /// <summary>
    /// The target compilation format.
    /// </summary>
    internal uint TargetIndex { get; }

    /// <summary>
    /// The entry point that was compiled.
    /// </summary>
    internal uint? EntryPointIndex { get; }

    /// <summary>
    /// Whether the compilation was successful.
    /// </summary>
    internal SlangResult Result { get; }

    /// <summary>
    /// Any diagnostics or error messages from compilation.
    /// </summary>
    internal string? Diagnostics { get; }

    internal CompilationResult(byte[] compiled, uint targetIndex, uint? entryPointIndex, SlangResult result, string? diagnostics = null)
    {
        Compiled = compiled;
        TargetIndex = targetIndex;
        EntryPointIndex = entryPointIndex;
        Result = result;
        Diagnostics = diagnostics;
    }
}