namespace Native
{
    enum class CompilerOptionNameCLI
    {
        CompilerOptionNameCLI_MacroDefine, // stringValue0: macro name;  stringValue1: macro value
        CompilerOptionNameCLI_DepFile,
        CompilerOptionNameCLI_EntryPointName,
        CompilerOptionNameCLI_Specialize,
        CompilerOptionNameCLI_Help,
        CompilerOptionNameCLI_HelpStyle,
        CompilerOptionNameCLI_Include, // stringValue: additional include path.
        CompilerOptionNameCLI_Language,
        CompilerOptionNameCLI_MatrixLayoutColumn,         // bool
        CompilerOptionNameCLI_MatrixLayoutRow,            // bool
        CompilerOptionNameCLI_ZeroInitialize,             // bool
        CompilerOptionNameCLI_IgnoreCapabilities,         // bool
        CompilerOptionNameCLI_RestrictiveCapabilityCheck, // bool
        CompilerOptionNameCLI_ModuleName,                 // stringValue0: module name.
        CompilerOptionNameCLI_Output,
        CompilerOptionNameCLI_Profile, // intValue0: profile
        CompilerOptionNameCLI_Stage,   // intValue0: stage
        CompilerOptionNameCLI_Target,  // intValue0: CodeGenTarget
        CompilerOptionNameCLI_Version,
        CompilerOptionNameCLI_WarningsAsErrors, // stringValue0: "all" or comma separated list of warning codes or names.
        CompilerOptionNameCLI_DisableWarnings,  // stringValue0: comma separated list of warning codes or names.
        CompilerOptionNameCLI_EnableWarning,    // stringValue0: warning code or name.
        CompilerOptionNameCLI_DisableWarning,   // stringValue0: warning code or name.
        CompilerOptionNameCLI_DumpWarningDiagnostics,
        CompilerOptionNameCLI_InputFilesRemain,
        CompilerOptionNameCLI_EmitIr,                        // bool
        CompilerOptionNameCLI_ReportDownstreamTime,          // bool
        CompilerOptionNameCLI_ReportPerfBenchmark,           // bool
        CompilerOptionNameCLI_ReportCheckpointIntermediates, // bool
        CompilerOptionNameCLI_SkipSPIRVValidation,           // bool
        CompilerOptionNameCLI_SourceEmbedStyle,
        CompilerOptionNameCLI_SourceEmbedName,
        CompilerOptionNameCLI_SourceEmbedLanguage,
        CompilerOptionNameCLI_DisableShortCircuit,            // bool
        CompilerOptionNameCLI_MinimumSlangOptimization,       // bool
        CompilerOptionNameCLI_DisableNonEssentialValidations, // bool
        CompilerOptionNameCLI_DisableSourceMap,               // bool
        CompilerOptionNameCLI_UnscopedEnum,                   // bool
        CompilerOptionNameCLI_PreserveParameters, // bool: preserve all resource parameters in the output code.

        // Target

        CompilerOptionNameCLI_Capability,                // intValue0: CapabilityName
        CompilerOptionNameCLI_DefaultImageFormatUnknown, // bool
        CompilerOptionNameCLI_DisableDynamicDispatch,    // bool
        CompilerOptionNameCLI_DisableSpecialization,     // bool
        CompilerOptionNameCLI_FloatingPointMode,         // intValue0: FloatingPointMode
        CompilerOptionNameCLI_DebugInformation,          // intValue0: DebugInfoLevel
        CompilerOptionNameCLI_LineDirectiveMode,
        CompilerOptionNameCLI_Optimization, // intValue0: OptimizationLevel
        CompilerOptionNameCLI_Obfuscate,    // bool

        CompilerOptionNameCLI_VulkanBindShift, // intValue0 (higher 8 bits): kind; intValue0(lower bits): set; intValue1:
        // shift
        CompilerOptionNameCLI_VulkanBindGlobals,       // intValue0: index; intValue1: set
        CompilerOptionNameCLI_VulkanInvertY,           // bool
        CompilerOptionNameCLI_VulkanUseDxPositionW,    // bool
        CompilerOptionNameCLI_VulkanUseEntryPointName, // bool
        CompilerOptionNameCLI_VulkanUseGLLayout,       // bool
        CompilerOptionNameCLI_VulkanEmitReflection,    // bool

        CompilerOptionNameCLI_GLSLForceScalarLayout,   // bool
        CompilerOptionNameCLI_EnableEffectAnnotations, // bool

        CompilerOptionNameCLI_EmitSpirvViaGLSL,     // bool (will be deprecated)
        CompilerOptionNameCLI_EmitSpirvDirectly,    // bool (will be deprecated)
        CompilerOptionNameCLI_SPIRVCoreGrammarJSON, // stringValue0: json path
        CompilerOptionNameCLI_IncompleteLibrary,    // bool, when set, will not issue an error when the linked program has
        // unresolved extern function symbols.

    // Downstream

        CompilerOptionNameCLI_CompilerPath,
        CompilerOptionNameCLI_DefaultDownstreamCompiler,
        CompilerOptionNameCLI_DownstreamArgs, // stringValue0: downstream compiler name. stringValue1: argument list, one
        // per line.
        CompilerOptionNameCLI_PassThrough,

        // Repro

        CompilerOptionNameCLI_DumpRepro,
        CompilerOptionNameCLI_DumpReproOnError,
        CompilerOptionNameCLI_ExtractRepro,
        CompilerOptionNameCLI_LoadRepro,
        CompilerOptionNameCLI_LoadReproDirectory,
        CompilerOptionNameCLI_ReproFallbackDirectory,

        // Debugging

        CompilerOptionNameCLI_DumpAst,
        CompilerOptionNameCLI_DumpIntermediatePrefix,
        CompilerOptionNameCLI_DumpIntermediates, // bool
        CompilerOptionNameCLI_DumpIr,            // bool
        CompilerOptionNameCLI_DumpIrIds,
        CompilerOptionNameCLI_PreprocessorOutput,
        CompilerOptionNameCLI_OutputIncludes,
        CompilerOptionNameCLI_ReproFileSystem,
        CompilerOptionNameCLI_SerialIr,    // bool
        CompilerOptionNameCLI_SkipCodeGen, // bool
        CompilerOptionNameCLI_ValidateIr,  // bool
        CompilerOptionNameCLI_VerbosePaths,
        CompilerOptionNameCLI_VerifyDebugSerialIr,
        CompilerOptionNameCLI_NoCodeGen, // Not used.

        // Experimental

        CompilerOptionNameCLI_FileSystem,
        CompilerOptionNameCLI_Heterogeneous,
        CompilerOptionNameCLI_NoMangle,
        CompilerOptionNameCLI_NoHLSLBinding,
        CompilerOptionNameCLI_NoHLSLPackConstantBufferElements,
        CompilerOptionNameCLI_ValidateUniformity,
        CompilerOptionNameCLI_AllowGLSL,
        CompilerOptionNameCLI_EnableExperimentalPasses,
        CompilerOptionNameCLI_BindlessSpaceIndex, // int

        // Internal

        CompilerOptionNameCLI_ArchiveType,
        CompilerOptionNameCLI_CompileCoreModule,
        CompilerOptionNameCLI_Doc,
        CompilerOptionNameCLI_IrCompression,
        CompilerOptionNameCLI_LoadCoreModule,
        CompilerOptionNameCLI_ReferenceModule,
        CompilerOptionNameCLI_SaveCoreModule,
        CompilerOptionNameCLI_SaveCoreModuleBinSource,
        CompilerOptionNameCLI_TrackLiveness,
        CompilerOptionNameCLI_LoopInversion, // bool, enable loop inversion optimization

        // Deprecated
        CompilerOptionNameCLI_ParameterBlocksUseRegisterSpaces,

        CompilerOptionNameCLI_CountOfParsableOptions,

        // Used in parsed options only.
        CompilerOptionNameCLI_DebugInformationFormat,  // intValue0: DebugInfoFormat
        CompilerOptionNameCLI_VulkanBindShiftAll,      // intValue0: kind; intValue1: shift
        CompilerOptionNameCLI_GenerateWholeProgram,    // bool
        CompilerOptionNameCLI_UseUpToDateBinaryModule, // bool, when set, will only load
        // precompiled modules if it is up-to-date with its source.
        CompilerOptionNameCLI_EmbedDownstreamIR,       // bool
        CompilerOptionNameCLI_ForceDXLayout,           // bool

        // Add this new option to the end of the list to avoid breaking ABI as much as possible.
        // Setting of EmitSpirvDirectly or EmitSpirvViaGLSL will turn into this option internally.
        CompilerOptionNameCLI_EmitSpirvMethod, // enum SlangEmitSpirvMethod

        CompilerOptionNameCLI_EmitReflectionJSON, // bool
        CompilerOptionNameCLI_SaveGLSLModuleBinSource,

        CompilerOptionNameCLI_SkipDownstreamLinking, // bool, experimental
        CompilerOptionNameCLI_DumpModule,
        CompilerOptionNameCLI_CountOf,
    };
}