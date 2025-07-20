using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents the result of a Slang compilation operation.
/// </summary>
public class CompilationResult
{
    /// <summary>
    /// The compiled source code.
    /// </summary>
    public string Source { get; }

    /// <summary>
    /// The target compilation format.
    /// </summary>
    public ShaderModel Target { get; }

    /// <summary>
    /// The entry point that was compiled.
    /// </summary>
    public string EntryPoint { get; }

    /// <summary>
    /// Whether the compilation was successful.
    /// </summary>
    public SlangResult Result { get; }

    /// <summary>
    /// Any diagnostics or error messages from compilation.
    /// </summary>
    public string? Diagnostics { get; }

    public CompilationResult(string source, ShaderModel target, string entryPoint, SlangResult result, string? diagnostics = null)
    {
        Source = source;
        Target = target;
        EntryPoint = entryPoint;
        Result = result;
        Diagnostics = diagnostics;
    }
}