using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents the result of a Slang compilation operation.
/// </summary>
internal class CompilationResult
{
    /// <summary>
    /// The compiled source code.
    /// </summary>
    internal string Source { get; }

    /// <summary>
    /// The target compilation format.
    /// </summary>
    internal Target Target { get; }

    /// <summary>
    /// The entry point that was compiled.
    /// </summary>
    internal Binding.EntryPoint EntryPoint { get; }

    /// <summary>
    /// Whether the compilation was successful.
    /// </summary>
    internal SlangResult Result { get; }

    /// <summary>
    /// Any diagnostics or error messages from compilation.
    /// </summary>
    internal string? Diagnostics { get; }

    internal CompilationResult(string source, Target target, Binding.EntryPoint entryPoint, SlangResult result, string? diagnostics = null)
    {
        Source = source;
        Target = target;
        EntryPoint = entryPoint;
        Result = result;
        Diagnostics = diagnostics;
    }
}