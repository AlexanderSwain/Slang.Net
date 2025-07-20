using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongTypeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents a Slang compilation session that manages modules and compilation.
/// </summary>
internal unsafe sealed class Session
{
    internal SessionHandle Handle { get; }

    internal IReadOnlyCollection<ShaderModel> Targets { get; }

    /// <summary>
    /// Creates a new Slang session with the specified configuration.
    /// </summary>
    /// <param name="configuration">The session configuration.</param>
    /// <exception cref="SlangException">Thrown if session creation fails.</exception>
    internal Session(CompilerOption[] options, PreprocessorMacro[] macros, ShaderModel[] models, string[] searchPaths)
    {
        Targets = models;

        fixed (CompilerOption* optionsPtr = options)
        fixed (PreprocessorMacro* macrosPtr = macros)
        fixed (ShaderModel* modelsPtr = models)
        {
            Handle = Session_Create(
                optionsPtr, options.Length,
                macrosPtr, macros.Length,
                modelsPtr, models.Length,
                searchPaths.To_CStrArr(), searchPaths.Length);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang session: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
    }

    ~Session()
    {
        Handle?.Dispose();
    }
}