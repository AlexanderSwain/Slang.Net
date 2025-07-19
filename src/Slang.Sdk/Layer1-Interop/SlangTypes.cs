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