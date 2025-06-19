namespace Slang::Cpp
{
    typedef int SlangCompileTargetIntegral;
    public enum class CompileTarget : SlangCompileTargetIntegral
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
}