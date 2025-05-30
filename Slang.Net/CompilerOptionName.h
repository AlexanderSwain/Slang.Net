namespace Slang
{
    enum class CompilerOptionName
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
}
