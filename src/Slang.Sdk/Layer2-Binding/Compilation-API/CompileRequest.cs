using Slang.Sdk.Interop;

namespace Slang.Sdk.Binding;

/// <summary>
/// Represents a Slang compile request for building shaders and performing compilation operations.
/// </summary>
internal unsafe sealed class CompileRequest : CompilationBinding
{
    internal Session Parent { get; }
    internal override CompileRequestHandle Handle { get; }
    internal override CompileRequestHandle NativeHandle => new(StrongInterop.CompileRequest.GetNative(Handle, out var _));

    /// <summary>
    /// Creates a new compile request associated with the specified session.
    /// </summary>
    /// <param name="parentSession">The session to create the compile request in.</param>
    /// <exception cref="SlangException">Thrown if compile request creation fails.</exception>
    internal CompileRequest(Session parentSession)
    {
        Parent = parentSession;

        // Create the compile request using the strongly-typed interop
        Handle = StrongInterop.CompileRequest.Create(Parent.Handle, out var error);

        if (Handle.IsInvalid)
            throw new SlangException(SlangResult.Fail, $"Failed to create Slang compile request: {error ?? "<No error was returned from Slang>"}");
    }

    ~CompileRequest()
    {
        Handle?.Dispose();
    }

    /// <summary>
    /// Adds a code generation target to the compile request.
    /// </summary>
    /// <param name="target">The target format (e.g., HLSL, SPIRV, etc.)</param>
    internal void AddCodeGenTarget(Target.CompileTarget target)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddCodeGenTarget(Handle, (int)target, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add code generation target: {error}");
    }

    /// <summary>
    /// Adds an entry point to the compile request.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit containing the entry point.</param>
    /// <param name="name">Name of the entry point function.</param>
    /// <param name="stage">Shader stage of the entry point.</param>
    /// <returns>Index of the added entry point.</returns>
    internal int AddEntryPoint(int translationUnitIndex, string name, Stage stage)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        int result = StrongInterop.CompileRequest.AddEntryPoint(Handle, translationUnitIndex, name, (int)stage, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add entry point: {error}");
        return result;
    }

    /// <summary>
    /// Adds an entry point with generic arguments to the compile request.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit containing the entry point.</param>
    /// <param name="name">Name of the entry point function.</param>
    /// <param name="stage">Shader stage of the entry point.</param>
    /// <param name="genericArgs">Generic type arguments for the entry point.</param>
    /// <returns>Index of the added entry point.</returns>
    internal int AddEntryPointEx(int translationUnitIndex, string name, Stage stage, string[] genericArgs)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        int result = StrongInterop.CompileRequest.AddEntryPointEx(Handle, translationUnitIndex, name, (int)stage, genericArgs, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add entry point with generic arguments: {error}");
        return result;
    }

    /// <summary>
    /// Adds a library reference to the compile request.
    /// </summary>
    /// <param name="baseRequest">Base compile request to reference.</param>
    /// <param name="libName">Name of the library to reference.</param>
    internal void AddLibraryReference(CompileRequest baseRequest, string libName)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddLibraryReference(Handle, baseRequest.Handle, libName, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add library reference: {error}");
    }

    /// <summary>
    /// Adds a preprocessor define to the compile request.
    /// </summary>
    /// <param name="key">Name of the preprocessor macro.</param>
    /// <param name="value">Value of the preprocessor macro.</param>
    internal void AddPreprocessorDefine(string key, string value)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddPreprocessorDefine(Handle, key, value, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add preprocessor define: {error}");
    }

    /// <summary>
    /// Adds a reference to the compile request.
    /// </summary>
    internal void AddRef()
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddRef(Handle, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add reference: {error}");
    }

    /// <summary>
    /// Adds a search path for include files.
    /// </summary>
    /// <param name="searchDir">Directory path to search for includes.</param>
    internal void AddSearchPath(string searchDir)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddSearchPath(Handle, searchDir, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add search path: {error}");
    }

    /// <summary>
    /// Adds a target capability to the compile request.
    /// </summary>
    /// <param name="targetIndex">Index of the target to add capability to.</param>
    /// <param name="capability">Capability to add to the target.</param>
    internal void AddTargetCapability(int targetIndex, CapabilityID capability)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTargetCapability(Handle, targetIndex, (int)capability, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add target capability: {error}");
    }

    /// <summary>
    /// Adds a translation unit to the compile request.
    /// </summary>
    /// <param name="language">Source language of the translation unit.</param>
    /// <param name="name">Name of the translation unit.</param>
    /// <returns>Index of the added translation unit.</returns>
    internal int AddTranslationUnit(SourceLanguage language, string name)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        int result = StrongInterop.CompileRequest.AddTranslationUnit(Handle, (int)language, name, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit: {error}");
        return result;
    }

    /// <summary>
    /// Adds a preprocessor define for a specific translation unit.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit.</param>
    /// <param name="key">Name of the preprocessor macro.</param>
    /// <param name="value">Value of the preprocessor macro.</param>
    internal void AddTranslationUnitPreprocessorDefine(int translationUnitIndex, string key, string value)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTranslationUnitPreprocessorDefine(Handle, translationUnitIndex, key, value, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit preprocessor define: {error}");
    }

    /// <summary>
    /// Adds source from a blob to a translation unit.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit.</param>
    /// <param name="path">Path identifier for the source.</param>
    /// <param name="sourceBlob">Native handle to the source blob.</param>
    internal void AddTranslationUnitSourceBlob(int translationUnitIndex, string path, nint sourceBlob)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTranslationUnitSourceBlob(Handle, translationUnitIndex, path, sourceBlob, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit source blob: {error}");
    }

    /// <summary>
    /// Adds source from a file to a translation unit.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit.</param>
    /// <param name="path">Path to the source file.</param>
    internal void AddTranslationUnitSourceFile(int translationUnitIndex, string path)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTranslationUnitSourceFile(Handle, translationUnitIndex, path, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit source file: {error}");
    }

    /// <summary>
    /// Adds source from a string to a translation unit.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit.</param>
    /// <param name="path">Path identifier for the source.</param>
    /// <param name="source">Source code as a string.</param>
    internal void AddTranslationUnitSourceString(int translationUnitIndex, string path, string source)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTranslationUnitSourceString(Handle, translationUnitIndex, path, source, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit source string: {error}");
    }

    /// <summary>
    /// Adds source from a string span to a translation unit.
    /// </summary>
    /// <param name="translationUnitIndex">Index of the translation unit.</param>
    /// <param name="path">Path identifier for the source.</param>
    /// <param name="sourceBegin">Beginning of the source string.</param>
    /// <param name="sourceEnd">End of the source string.</param>
    internal void AddTranslationUnitSourceStringSpan(int translationUnitIndex, string path, string sourceBegin, string sourceEnd)
    {
        ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
        StrongInterop.CompileRequest.AddTranslationUnitSourceStringSpan(Handle, translationUnitIndex, path, sourceBegin, sourceEnd, out var error);
        if (error != null)
            throw new SlangException(SlangResult.Fail, $"Failed to add translation unit source string span: {error}");
    }
}