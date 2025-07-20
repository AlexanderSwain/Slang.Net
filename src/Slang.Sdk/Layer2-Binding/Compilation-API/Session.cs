using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.SlangNativeInterop;
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
    internal Session(
        CompilerOption[] options,
        PreprocessorMacro[] macros,
        ShaderModel[] models,
        string[] searchPaths)
    {
        Targets = models;

        fixed (CompilerOption* optionsPtr = options)
        fixed (PreprocessorMacro* macrosPtr = macros)
        fixed (ShaderModel* modelsPtr = models)
        {
            Handle = new SessionHandle(
                Session_Create(
                optionsPtr, options.Length,  // No compiler options for now
                macrosPtr, macros.Length,  // No preprocessor macros for now
                modelsPtr, models.Length,  // No shader models for now
                searchPaths.To_CStrArr(), searchPaths.Length));   // No search paths for now

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang session: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
    }

    ~Session()
    {
        Handle?.Dispose();
    }

    // Delete later
    ///// <summary>
    ///// Loads a Slang module from a file.
    ///// </summary>
    ///// <param name="modulePath">The path to the .slang file.</param>
    ///// <param name="moduleName">Optional module name. If null, derived from file path.</param>
    ///// <returns>The loaded module.</returns>
    ///// <exception cref="SlangException">Thrown if module loading fails.</exception>
    //public Module LoadModule(string modulePath, string? moduleName = null)
    //{
    //    ObjectDisposedException.ThrowIf(_disposed, this);
    //    ArgumentNullException.ThrowIfNull(modulePath);
    //
    //    if (!Path.GetExtension(modulePath).Equals(".slang", StringComparison.OrdinalIgnoreCase))
    //        throw new ArgumentException("Module path must have a .slang extension.", nameof(modulePath));
    //
    //    if (!File.Exists(modulePath))
    //        throw new FileNotFoundException($"Slang module file not found: {modulePath}");
    //
    //    moduleName ??= Path.GetFileNameWithoutExtension(modulePath);
    //
    //    var moduleHandle = CreateNativeModule(modulePath, moduleName);
    //    return new Module(this, moduleHandle, moduleName, modulePath);
    //}
    //
    ///// <summary>
    ///// Loads a Slang module from source code.
    ///// </summary>
    ///// <param name="source">The Slang source code.</param>
    ///// <param name="moduleName">The name for the module.</param>
    ///// <param name="fileName">Optional file name for diagnostics.</param>
    ///// <returns>The loaded module.</returns>
    ///// <exception cref="SlangException">Thrown if module loading fails.</exception>
    //public Module LoadModuleFromSource(string source, string moduleName, string? fileName = null)
    //{
    //    ObjectDisposedException.ThrowIf(_disposed, this);
    //    ArgumentNullException.ThrowIfNull(source);
    //    ArgumentNullException.ThrowIfNull(moduleName);
    //
    //    fileName ??= $"{moduleName}.slang";
    //
    //    var moduleHandle = CreateNativeModuleFromSource(source, moduleName, fileName);
    //    return new Module(this, moduleHandle, moduleName, fileName);
    //}

    /// <summary>
    /// Gets the last error message from the native Slang library.
    /// </summary>
    /// <returns>The error message, or null if no error.</returns>


    //private unsafe SlangModuleHandle CreateNativeModule(string modulePath, string moduleName)
    //{
    //    var moduleNamePtr = StringMarshaling.ToUtf8(moduleName);
    //    var modulePathPtr = StringMarshaling.ToUtf8(modulePath);
    //
    //    try
    //    {
    //        var modulePtr = SlangNativeInterop.CreateModule(_sessionHandle, moduleNamePtr, modulePathPtr, null);
    //
    //        if (modulePtr == IntPtr.Zero)
    //        {
    //            var errorMessage = GetLastError();
    //            throw new SlangException(SlangResult.Fail, $"Failed to load module '{moduleName}': {errorMessage ?? "Unknown error"}");
    //        }
    //
    //        return new SlangModuleHandle(modulePtr);
    //    }
    //    finally
    //    {
    //        StringMarshaling.FreeUtf8(moduleNamePtr);
    //        StringMarshaling.FreeUtf8(modulePathPtr);
    //    }
    //}

    //private unsafe SlangModuleHandle CreateNativeModuleFromSource(string source, string moduleName, string fileName)
    //{
    //    var moduleNamePtr = StringMarshaling.ToUtf8(moduleName);
    //    var fileNamePtr = StringMarshaling.ToUtf8(fileName);
    //    var sourcePtr = StringMarshaling.ToUtf8(source);
    //
    //    try
    //    {
    //        var modulePtr = SlangNativeInterop.CreateModule(_sessionHandle, moduleNamePtr, fileNamePtr, sourcePtr);
    //
    //        if (modulePtr == IntPtr.Zero)
    //        {
    //            var errorMessage = GetLastError();
    //            throw new SlangException(SlangResult.Fail, $"Failed to load module '{moduleName}' from source: {errorMessage ?? "Unknown error"}");
    //        }
    //
    //        return new SlangModuleHandle(modulePtr);
    //    }
    //    finally
    //    {
    //        StringMarshaling.FreeUtf8(moduleNamePtr);
    //        StringMarshaling.FreeUtf8(fileNamePtr);
    //        StringMarshaling.FreeUtf8(sourcePtr);
    //    }
    //}

}