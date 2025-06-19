// Using global using directive

public class CompilerOption
{
    public CompilerOptionName Name { get; }
    public CompilerOptionValue Value { get; }
    public CompilerOption(CompilerOptionName name, CompilerOptionValue value)
    {
        Name = name;
        Value = value;
    }
}

public class CompilerOptionValue
{
    public CompilerOptionValueKind Kind { get; }
    public int IntValue0 { get; }
    public int IntValue1 { get; }
    public string? StringValue0 { get; }
    public string? StringValue1 { get; }
    public CompilerOptionValue(CompilerOptionValueKind kind, int intValue0 = 0, int intValue1 = 0, string? stringValue0 = null, string? stringValue1 = null)
    {
        Kind = kind;
        IntValue0 = intValue0;
        IntValue1 = intValue1;
        StringValue0 = stringValue0;
        StringValue1 = stringValue1;
    }
}

public enum CompilerOptionValueKind
{
    Int,
    String,
}
public enum CompilerOptionName
{
    MacroDefine, // stringValue0: macro name;  stringValue1: macro value
    DepFile,
    EntryPointName,
    Specialize,
    Help,
    HelpStyle,
    Include, // stringValue: additional include path.
    Language,
    MatrixLayoutColumn,         // bool
    MatrixLayoutRow,            // bool
    ZeroInitialize,             // bool
    IgnoreCapabilities,         // bool
    RestrictiveCapabilityCheck, // bool
    ModuleName,                 // stringValue0: module name.
    Output,
    Profile, // intValue0: profile
    Stage,   // intValue0: stage
    Target,  // intValue0: CodeGenTarget
    Version,
    WarningsAsErrors, // stringValue0: "all" or comma separated list of warning codes or names.
    DisableWarnings,  // stringValue0: comma separated list of warning codes or names.
    EnableWarning,    // stringValue0: warning code or name.
    DisableWarning,   // stringValue0: warning code or name.
    DumpWarningDiagnostics,
    InputFilesRemain,
    EmitIr,                        // bool
    ReportDownstreamTime,          // bool
    ReportPerfBenchmark,           // bool
    ReportCheckpointIntermediates, // bool
    SkipSPIRVValidation,           // bool
    SourceEmbedStyle,
    SourceEmbedName,
    SourceEmbedLanguage,
    DisableShortCircuit,            // bool
    MinimumSlangOptimization,       // bool
    DisableNonEssentialValidations, // bool
    DisableSourceMap,               // bool
    UnscopedEnum,                   // bool
    PreserveParameters, // bool: preserve all resource parameters in the output code.

    // Target

    Capability,                // intValue0: CapabilityName
    DefaultImageFormatUnknown, // bool
    DisableDynamicDispatch,    // bool
    DisableSpecialization,     // bool
    FloatingPointMode,         // intValue0: FloatingPointMode
    DebugInformation,          // intValue0: DebugInfoLevel
    LineDirectiveMode,
    Optimization, // intValue0: OptimizationLevel
    Obfuscate,    // bool

    VulkanBindShift, // intValue0 (higher 8 bits): kind; intValue0(lower bits): set; intValue1:
    // shift
    VulkanBindGlobals,       // intValue0: index; intValue1: set
    VulkanInvertY,           // bool
    VulkanUseDxPositionW,    // bool
    VulkanUseEntryPointName, // bool
    VulkanUseGLLayout,       // bool
    VulkanEmitReflection,    // bool

    GLSLForceScalarLayout,   // bool
    EnableEffectAnnotations, // bool

    EmitSpirvViaGLSL,     // bool (will be deprecated)
    EmitSpirvDirectly,    // bool (will be deprecated)
    SPIRVCoreGrammarJSON, // stringValue0: json path
    IncompleteLibrary,    // bool, when set, will not issue an error when the linked program has
    // unresolved extern function symbols.

    // Downstream

    CompilerPath,
    DefaultDownstreamCompiler,
    DownstreamArgs, // stringValue0: downstream compiler name. stringValue1: argument list, one
    // per line.
    PassThrough,

    // Repro

    DumpRepro,
    DumpReproOnError,
    ExtractRepro,
    LoadRepro,
    LoadReproDirectory,
    ReproFallbackDirectory,

    // Debugging

    DumpAst,
    DumpIntermediatePrefix,
    DumpIntermediates, // bool
    DumpIr,            // bool
    DumpIrIds,
    PreprocessorOutput,
    OutputIncludes,
    ReproFileSystem,
    SerialIr,    // bool
    SkipCodeGen, // bool
    ValidateIr,  // bool
    VerbosePaths,
    VerifyDebugSerialIr,
    NoCodeGen, // Not used.

    // Experimental

    FileSystem,
    Heterogeneous,
    NoMangle,
    NoHLSLBinding,
    NoHLSLPackConstantBufferElements,
    ValidateUniformity,
    AllowGLSL,
    EnableExperimentalPasses,
    BindlessSpaceIndex, // int

    // Internal

    ArchiveType,
    CompileCoreModule,
    Doc,
    IrCompression,
    LoadCoreModule,
    ReferenceModule,
    SaveCoreModule,
    SaveCoreModuleBinSource,
    TrackLiveness,
    LoopInversion, // bool, enable loop inversion optimization

    // Deprecated
    ParameterBlocksUseRegisterSpaces,

    CountOfParsableOptions,

    // Used in parsed options only.
    DebugInformationFormat,  // intValue0: DebugInfoFormat
    VulkanBindShiftAll,      // intValue0: kind; intValue1: shift
    GenerateWholeProgram,    // bool
    UseUpToDateBinaryModule, // bool, when set, will only load
    // precompiled modules if it is up-to-date with its source.
    EmbedDownstreamIR,       // bool
    ForceDXLayout,           // bool

    // Add this new option to the end of the list to avoid breaking ABI as much as possible.
    // Setting of EmitSpirvDirectly or EmitSpirvViaGLSL will turn into this option internally.
    EmitSpirvMethod, // enum SlangEmitSpirvMethod

    EmitReflectionJSON, // bool
    SaveGLSLModuleBinSource,

    SkipDownstreamLinking, // bool, experimental
    DumpModule,
    CountOf,
};
public enum CompileTarget
{
    SLANG_TARGET_UNKNOWN,
    SLANG_TARGET_NONE,
    SLANG_GLSL,
    SLANG_GLSL_VULKAN_DEPRECATED,          //< deprecated and removed: just use `SLANG_GLSL`.
    SLANG_GLSL_VULKAN_ONE_DESC_DEPRECATED, //< deprecated and removed.
    SLANG_HLSL,
    SLANG_SPIRV,
    SLANG_SPIRV_ASM,
    SLANG_DXBC,
    SLANG_DXBC_ASM,
    SLANG_DXIL,
    SLANG_DXIL_ASM,
    SLANG_C_SOURCE,              ///< The C language
    SLANG_CPP_SOURCE,            ///< C++ code for shader kernels.
    SLANG_HOST_EXECUTABLE,       ///< Standalone binary executable (for hosting CPU/OS)
    SLANG_SHADER_SHARED_LIBRARY, ///< A shared library/Dll for shader kernels (for hosting
                                 ///< CPU/OS)
    SLANG_SHADER_HOST_CALLABLE,  ///< A CPU target that makes the compiled shader code available
                                 ///< to be run immediately
    SLANG_CUDA_SOURCE,           ///< Cuda source
    SLANG_PTX,                   ///< PTX
    SLANG_CUDA_OBJECT_CODE,      ///< Object code that contains CUDA functions.
    SLANG_OBJECT_CODE,           ///< Object code that can be used for later linking
    SLANG_HOST_CPP_SOURCE,       ///< C++ code for host library or executable.
    SLANG_HOST_HOST_CALLABLE,    ///< Host callable host code (ie non kernel/shader)
    SLANG_CPP_PYTORCH_BINDING,   ///< C++ PyTorch binding code.
    SLANG_METAL,                 ///< Metal shading language
    SLANG_METAL_LIB,             ///< Metal library
    SLANG_METAL_LIB_ASM,         ///< Metal library assembly
    SLANG_HOST_SHARED_LIBRARY,   ///< A shared library/Dll for host code (for hosting CPU/OS)
    SLANG_WGSL,                  ///< WebGPU shading language
    SLANG_WGSL_SPIRV_ASM,        ///< SPIR-V assembly via WebGPU shading language
    SLANG_WGSL_SPIRV,            ///< SPIR-V via WebGPU shading language
    SLANG_TARGET_COUNT_OF,
};
public unsafe class Session
{
    internal Slang.Cpp.Session cppObj { get; }

    internal Session(Slang.Cpp.CompilerOption[] compilerOptions, Slang.Cpp.PreprocessorMacroDesc[] macros, Slang.Cpp.ShaderModel[] models, string[] searchPaths)
    {
        cppObj = new Slang.Cpp.Session(compilerOptions, macros, models, searchPaths);
    }

    public Module LoadModule(string fileName)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        if (extension != ".slang")
            throw new ArgumentException($"{(string.IsNullOrEmpty(extension) ? "<no extension>" : extension)} files are not supported. Only .slang files are supported.", nameof(fileName));

        return new Module(this, fileName);
    }
}

public class SessionBuilder
{
    public List<Slang.Cpp.CompilerOption> CompilerOptions { get; } = new();
    public List<Slang.Cpp.PreprocessorMacroDesc> PreprocessorMacroDesc { get; } = new();
    public List<Slang.Cpp.ShaderModel> ShaderModel { get; } = new();
    public List<string> SearchPaths { get; } = new();

    public SessionBuilder AddCompilerOption(CompilerOptionName name, CompilerOptionValue value)
    {
        CompilerOptions.Add(
            new Slang.Cpp.CompilerOption(
                (Slang.Cpp.CompilerOptionName)name, 
                new(
                    (Slang.Cpp.CompilerOptionValueKind)value.Kind,
                    value.IntValue0,
                    value.IntValue1,
                    value.StringValue0,
                    value.StringValue1
                )));
        return this;
    }

    public SessionBuilder AddPreprocessorMacro(string name, string value)
    {
        PreprocessorMacroDesc.Add(new Slang.Cpp.PreprocessorMacroDesc(name, value));
        return this;
    }

    public SessionBuilder AddShaderModel(CompileTarget target, string profile)
    {
        ShaderModel.Add(new Slang.Cpp.ShaderModel((Slang.Cpp.CompileTarget)target, profile));
        return this;
    }

    public SessionBuilder AddSearchPath(string absolutePath)
    {
        SearchPaths.Add(absolutePath);
        return this;
    }

    public Session Create()
    {
        return new Session(CompilerOptions.ToArray(), PreprocessorMacroDesc.ToArray(), ShaderModel.ToArray(), SearchPaths.ToArray());
    }
}