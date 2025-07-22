using System.Runtime.InteropServices;

namespace Slang.Sdk.Interop;

/// <summary>
/// Result codes returned by Slang native functions.
/// Corresponds to SlangResult in the native code.
/// </summary>
public enum SlangResult : int
{
    /// <summary>
    /// Success.
    /// </summary>
    Ok = 0,

    /// <summary>
    /// Generic failure.
    /// </summary>
    Fail = unchecked((int)0x80004005),

    /// <summary>
    /// Interface not supported.
    /// </summary>
    NoInterface = unchecked((int)0x80004002),

    /// <summary>
    /// Operation aborted.
    /// </summary>
    Abort = unchecked((int)0x80004004),

    /// <summary>
    /// Invalid argument.
    /// </summary>
    InvalidArg = unchecked((int)0x80070057),

    /// <summary>
    /// Not implemented.
    /// </summary>
    NotImplemented = unchecked((int)0x80004001),

    /// <summary>
    /// Out of memory.
    /// </summary>
    OutOfMemory = unchecked((int)0x8007000E),

    /// <summary>
    /// Invalid pointer.
    /// </summary>
    Pointer = unchecked((int)0x80004003),

    /// <summary>
    /// Invalid handle.
    /// </summary>
    Handle = unchecked((int)0x80070006),

    /// <summary>
    /// Compilation error.
    /// </summary>
    CompilationFailed = -1,

    /// <summary>
    /// Internal compiler error.
    /// </summary>
    InternalError = -2
}

/// <summary>
/// Shader stage types.
/// </summary>
public enum ShaderStage
{
    Unknown,
    Vertex,
    Hull,
    Domain,
    Geometry,
    Fragment,
    Compute,
    RayGeneration,
    Intersection,
    AnyHit,
    ClosestHit,
    Miss,
    Callable,
    Mesh,
    Amplification,
    Pixel = Fragment // Alias for DirectX terminology
}



/// <summary>
/// Parameter categories for layout calculations.
/// </summary>
public enum ParameterCategory
{
    None,
    Mixed,
    ConstantBuffer,
    ShaderResource,
    UnorderedAccess,
    VaryingInput,
    VaryingOutput,
    SamplerState,
    Uniform,
    DescriptorTableSlot,
    SpecializationConstant,
    PushConstantBuffer,
    RegisterSpace,
    GenericResource,
    ExistentialTypeParam,
    ExistentialObjectParam,
    SubElementRegisterSpace,
    InputAttachmentIndex,
    MetalBuffer,
    MetalTexture,
    MetalSampler,
    MetalArgumentBuffer,
    MetalAttribute,
    VertexInput,
    FragmentOutput,
    RayPayload,
    HitAttributes,
    CallablePayload,
    ShaderRecord,
    Metal,
    VulkanStorageClass,
    InlineUniformData
}

/// <summary>
/// Scalar types.
/// </summary>
public enum ScalarType
{
    None,
    Void,
    Bool,
    Int8,
    Int16,
    Int32,
    Int64,
    UInt8,
    UInt16,
    UInt32,
    UInt64,
    Float16,
    Float32,
    Float64,
    IntPtr,
    UIntPtr
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CompilerOption
{
    public Name name;
    public Value value;

    public CompilerOption(Name name, Value value)
    {
        this.name = name;
        this.value = value;
    }

    public enum Name
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
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Value : IDisposable
    {
        public Kind kind = Kind.Int;
        public int intValue0 = 0;
        public int intValue1 = 0;
        public IntPtr stringValue0 = IntPtr.Zero;
        public IntPtr stringValue1 = IntPtr.Zero;

        public Value(Kind kind, int intValue0, int intValue1, string? stringValue0, string? stringValue1)
        {
            this.kind = kind;
            this.intValue0 = intValue0;
            this.intValue1 = intValue1;
            this.stringValue0 = Marshal.StringToHGlobalAnsi(stringValue0);
            this.stringValue1 = Marshal.StringToHGlobalAnsi(stringValue1);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(stringValue0);
            Marshal.FreeHGlobal(stringValue1);
        }

        public enum Kind
        {
            Int,
            String
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct PreprocessorMacro : IDisposable
{
    IntPtr name;
    IntPtr value;

    public PreprocessorMacro(string name, string value)
    {
        this.name = Marshal.StringToHGlobalAnsi(name);
        this.value = Marshal.StringToHGlobalAnsi(value);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(name);
        Marshal.FreeHGlobal(value);
    }
}

public struct Target : IDisposable
{
    public CompileTarget target;
    public IntPtr profile;

    internal Target(CompileTarget target, string profile)
    {
        this.target = target;
        this.profile = Marshal.StringToHGlobalAnsi(profile);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(profile);
    }

    /// <summary>
    /// Compilation target formats.
    /// </summary>
    public enum CompileTarget
    {
        Unknown,
        Hlsl,
        Glsl,
        Metal,
        SpirV,
        DxBytecode,
        DxIl,
        Cpp,
        C,
        Cuda,
        Dxil,
        DxilAssembly,
        SpirVAssembly,
        PyTorch,
        Host,
        PTX,
        HostCpp,
        HostCuda,
        HostHostCallable,
        CSource,
        CppSource,
        HostCppSource,
        CudaSource,
        ObjectCode,
        ShaderSharedLibrary,
        ShaderHostCallable,
        Executable,
        CSharpSource,
        Wgsl
    }
}

#region Reflection Slang Types

/// <summary>
/// Type kinds for reflection.
/// </summary>
public enum TypeKind
{
    None,
    Struct,
    Basic,
    Vector,
    Matrix,
    Array,
    GenericTypeParameter,
    Interface,
    ConstantBuffer,
    Resource,
    SamplerState,
    TextureBuffer,
    ShaderStorageBuffer,
    ParameterBlock,
    GenericDeclRef,
    Pointer,
    NativePtr,
    NativeRef,
    Dynamic,
    Specialized,
    Feedback,
    Extension
}

#endregion