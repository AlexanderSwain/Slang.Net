using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.StrongInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents a Slang compilation session that manages modules and compilation.
/// </summary>
internal unsafe sealed class Session : CompilationBinding
{
    internal override SessionHandle Handle { get; }
    internal override SessionHandle NativeHandle => new(StrongInterop.Session.GetNative(Handle, out var _));


    internal IReadOnlyCollection<Target> Targets { get; }
    internal string[] SearchPaths { get; }

    /// <summary>
    /// Creates a new Slang session with the specified configuration.
    /// </summary>
    /// <param name="configuration">The session configuration.</param>
    /// <exception cref="SlangException">Thrown if session creation fails.</exception>
    internal Session(CompilerOption[] options, PreprocessorMacro[] macros, Target[] models, string[] searchPaths)
    {
        Targets = models;
        SearchPaths = searchPaths;

        fixed (CompilerOption* optionsPtr = options)
        fixed (PreprocessorMacro* macrosPtr = macros)
        fixed (Target* modelsPtr = models)
        {
            Handle = StrongInterop.Session.Create(
                optionsPtr, options.Length,
                macrosPtr, macros.Length,
                modelsPtr, models.Length,
                searchPaths, searchPaths.Length,
                out var error);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang session: {error ?? "<No error was returned from Slang>"}");
        }
    }

    internal uint GetModuleCount()
    {
        string? error = null;
        return Call(() => StrongInterop.Session.GetModuleCount(Handle, out error), () => error);
    }

    internal Module GetModuleByIndex(uint index)
    {
        string? error = null;
        return new Module(this, Call(() => StrongInterop.Session.GetModuleByIndex(Handle, index, out error), () => error));
    }

    ~Session()
    {
        Handle?.Dispose();
    }
}