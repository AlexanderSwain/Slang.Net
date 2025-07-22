using System.Linq;
using System.Reflection;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    /// <summary>
    /// Provides static access to all available compilation targets and shader models.
    /// This class contains pre-configured targets for HLSL, GLSL, Metal, SPIR-V, and WGSL.
    /// </summary>
    public static class Targets
    {
        /// <summary>
        /// HLSL (High-Level Shader Language) compilation targets for DirectX.
        /// Supports various shader models from 4.0 to 6.7.
        /// </summary>
        public static class Hlsl
        {
            // Shader Model 4.0 - DirectX 10
            public static Target vs_4_0 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_4_0");
            public static Target ps_4_0 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_4_0");
            public static Target gs_4_0 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_4_0");
            
            // Shader Model 4.1 - DirectX 10.1
            public static Target vs_4_1 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_4_1");
            public static Target ps_4_1 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_4_1");
            public static Target gs_4_1 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_4_1");
            
            // Shader Model 5.0 - DirectX 11
            public static Target vs_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_5_0");
            public static Target ps_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_5_0");
            public static Target gs_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_5_0");
            public static Target cs_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_5_0");
            public static Target hs_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_5_0");
            public static Target ds_5_0 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_5_0");
            
            // Shader Model 5.1 - DirectX 12
            public static Target vs_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_5_1");
            public static Target ps_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_5_1");
            public static Target gs_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_5_1");
            public static Target cs_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_5_1");
            public static Target hs_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_5_1");
            public static Target ds_5_1 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_5_1");
            
            // Shader Model 6.0 - DirectX 12
            public static Target vs_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_0");
            public static Target ps_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_0");
            public static Target gs_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_0");
            public static Target cs_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_0");
            public static Target hs_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_0");
            public static Target ds_6_0 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_0");
            
            // Shader Model 6.1 - DirectX 12
            public static Target vs_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_1");
            public static Target ps_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_1");
            public static Target gs_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_1");
            public static Target cs_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_1");
            public static Target hs_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_1");
            public static Target ds_6_1 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_1");
            
            // Shader Model 6.2 - DirectX 12
            public static Target vs_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_2");
            public static Target ps_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_2");
            public static Target gs_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_2");
            public static Target cs_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_2");
            public static Target hs_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_2");
            public static Target ds_6_2 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_2");
            
            // Shader Model 6.3 - DirectX 12
            public static Target vs_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_3");
            public static Target ps_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_3");
            public static Target gs_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_3");
            public static Target cs_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_3");
            public static Target hs_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_3");
            public static Target ds_6_3 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_3");
            
            // Shader Model 6.4 - DirectX 12
            public static Target vs_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_4");
            public static Target ps_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_4");
            public static Target gs_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_4");
            public static Target cs_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_4");
            public static Target hs_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_4");
            public static Target ds_6_4 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_4");
            
            // Shader Model 6.5 - DirectX 12
            public static Target vs_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_5");
            public static Target ps_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_5");
            public static Target gs_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_5");
            public static Target cs_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_5");
            public static Target hs_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_5");
            public static Target ds_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_5");
            
            // Shader Model 6.6 - DirectX 12
            public static Target vs_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_6");
            public static Target ps_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_6");
            public static Target gs_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_6");
            public static Target cs_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_6");
            public static Target hs_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_6");
            public static Target ds_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_6");
            
            // Shader Model 6.7 - DirectX 12 (Latest)
            public static Target vs_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "vs_6_7");
            public static Target ps_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "ps_6_7");
            public static Target gs_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "gs_6_7");
            public static Target cs_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "cs_6_7");
            public static Target hs_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "hs_6_7");
            public static Target ds_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "ds_6_7");
            
            // Mesh Shaders (Shader Model 6.5+)
            public static Target ms_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "ms_6_5");
            public static Target as_6_5 { get; } = new Target(Target.CompileTarget.Hlsl, "as_6_5");
            public static Target ms_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "ms_6_6");
            public static Target as_6_6 { get; } = new Target(Target.CompileTarget.Hlsl, "as_6_6");
            public static Target ms_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "ms_6_7");
            public static Target as_6_7 { get; } = new Target(Target.CompileTarget.Hlsl, "as_6_7");
        }

        /// <summary>
        /// GLSL (OpenGL Shading Language) compilation targets for OpenGL and Vulkan.
        /// Supports various GLSL versions from 330 to 460.
        /// </summary>
        public static class Glsl
        {
            // OpenGL 3.3 and later
            public static Target v330 { get; } = new Target(Target.CompileTarget.Glsl, "330");
            public static Target v400 { get; } = new Target(Target.CompileTarget.Glsl, "400");
            public static Target v410 { get; } = new Target(Target.CompileTarget.Glsl, "410");
            public static Target v420 { get; } = new Target(Target.CompileTarget.Glsl, "420");
            public static Target v430 { get; } = new Target(Target.CompileTarget.Glsl, "430");
            public static Target v440 { get; } = new Target(Target.CompileTarget.Glsl, "440");
            public static Target v450 { get; } = new Target(Target.CompileTarget.Glsl, "450");
            public static Target v460 { get; } = new Target(Target.CompileTarget.Glsl, "460");
            
            // Common Vulkan targets
            public static Target vulkan { get; } = new Target(Target.CompileTarget.Glsl, "450");
            public static Target vulkan_1_0 { get; } = new Target(Target.CompileTarget.Glsl, "450");
            public static Target vulkan_1_1 { get; } = new Target(Target.CompileTarget.Glsl, "450");
            public static Target vulkan_1_2 { get; } = new Target(Target.CompileTarget.Glsl, "460");
            public static Target vulkan_1_3 { get; } = new Target(Target.CompileTarget.Glsl, "460");
            
            // OpenGL ES targets
            public static Target es_300 { get; } = new Target(Target.CompileTarget.Glsl, "300 es");
            public static Target es_310 { get; } = new Target(Target.CompileTarget.Glsl, "310 es");
            public static Target es_320 { get; } = new Target(Target.CompileTarget.Glsl, "320 es");
        }

        /// <summary>
        /// Metal Shading Language compilation targets for macOS, iOS, and Apple platforms.
        /// Supports Metal versions from 1.0 to 3.1.
        /// </summary>
        public static class Metal
        {
            // Metal 1.x - iOS 8.0+, macOS 10.11+
            public static Target v1_0 { get; } = new Target(Target.CompileTarget.Metal, "1.0");
            public static Target v1_1 { get; } = new Target(Target.CompileTarget.Metal, "1.1");
            public static Target v1_2 { get; } = new Target(Target.CompileTarget.Metal, "1.2");
            
            // Metal 2.x - iOS 10.0+, macOS 10.12+
            public static Target v2_0 { get; } = new Target(Target.CompileTarget.Metal, "2.0");
            public static Target v2_1 { get; } = new Target(Target.CompileTarget.Metal, "2.1");
            public static Target v2_2 { get; } = new Target(Target.CompileTarget.Metal, "2.2");
            public static Target v2_3 { get; } = new Target(Target.CompileTarget.Metal, "2.3");
            public static Target v2_4 { get; } = new Target(Target.CompileTarget.Metal, "2.4");
            
            // Metal 3.x - iOS 16.0+, macOS 13.0+
            public static Target v3_0 { get; } = new Target(Target.CompileTarget.Metal, "3.0");
            public static Target v3_1 { get; } = new Target(Target.CompileTarget.Metal, "3.1");
            
            // Platform-specific convenience targets
            public static Target macos { get; } = new Target(Target.CompileTarget.Metal, "2.0");
            public static Target ios { get; } = new Target(Target.CompileTarget.Metal, "2.0");
            public static Target tvos { get; } = new Target(Target.CompileTarget.Metal, "2.0");
            public static Target watchos { get; } = new Target(Target.CompileTarget.Metal, "2.0");
            
            // Latest version
            public static Target latest { get; } = new Target(Target.CompileTarget.Metal, "3.1");
        }

        /// <summary>
        /// SPIR-V (Standard Portable Intermediate Representation for Vulkan) compilation targets.
        /// Primarily used with Vulkan, OpenCL, and other Khronos APIs.
        /// </summary>
        public static class SpirV
        {
            // SPIR-V binary output
            public static Target v1_0 { get; } = new Target(Target.CompileTarget.SpirV, "1.0");
            public static Target v1_1 { get; } = new Target(Target.CompileTarget.SpirV, "1.1");
            public static Target v1_2 { get; } = new Target(Target.CompileTarget.SpirV, "1.2");
            public static Target v1_3 { get; } = new Target(Target.CompileTarget.SpirV, "1.3");
            public static Target v1_4 { get; } = new Target(Target.CompileTarget.SpirV, "1.4");
            public static Target v1_5 { get; } = new Target(Target.CompileTarget.SpirV, "1.5");
            public static Target v1_6 { get; } = new Target(Target.CompileTarget.SpirV, "1.6");
            
            // Vulkan-specific SPIR-V targets
            public static Target vulkan_1_0 { get; } = new Target(Target.CompileTarget.SpirV, "spv1.0");
            public static Target vulkan_1_1 { get; } = new Target(Target.CompileTarget.SpirV, "spv1.3");
            public static Target vulkan_1_2 { get; } = new Target(Target.CompileTarget.SpirV, "spv1.5");
            public static Target vulkan_1_3 { get; } = new Target(Target.CompileTarget.SpirV, "spv1.6");
            
            // Convenience targets
            public static Target binary { get; } = new Target(Target.CompileTarget.SpirV, "1.6");
            public static Target latest { get; } = new Target(Target.CompileTarget.SpirV, "1.6");
        }

        /// <summary>
        /// WGSL (WebGPU Shading Language) compilation targets for WebGPU.
        /// The emerging standard for web-based GPU programming.
        /// </summary>
        public static class Wgsl
        {
            // WebGPU shading language
            public static Target v1_0 { get; } = new Target(Target.CompileTarget.Wgsl, "1.0");
            public static Target webgpu { get; } = new Target(Target.CompileTarget.Wgsl, "1.0");
            public static Target latest { get; } = new Target(Target.CompileTarget.Wgsl, "1.0");
        }

        /// <summary>
        /// Additional compilation targets for various purposes.
        /// </summary>
        public static class Other
        {
            // C/C++ targets for compute kernels
            public static Target c_source { get; } = new Target(Target.CompileTarget.C, "c99");
            public static Target cpp_source { get; } = new Target(Target.CompileTarget.Cpp, "c++11");
            public static Target cpp14 { get; } = new Target(Target.CompileTarget.Cpp, "c++14");
            public static Target cpp17 { get; } = new Target(Target.CompileTarget.Cpp, "c++17");
            public static Target cpp20 { get; } = new Target(Target.CompileTarget.Cpp, "c++20");
            
            // CUDA targets
            public static Target cuda_source { get; } = new Target(Target.CompileTarget.Cuda, "sm_50");
            public static Target cuda_sm_50 { get; } = new Target(Target.CompileTarget.Cuda, "sm_50");
            public static Target cuda_sm_60 { get; } = new Target(Target.CompileTarget.Cuda, "sm_60");
            public static Target cuda_sm_70 { get; } = new Target(Target.CompileTarget.Cuda, "sm_70");
            public static Target cuda_sm_75 { get; } = new Target(Target.CompileTarget.Cuda, "sm_75");
            public static Target cuda_sm_80 { get; } = new Target(Target.CompileTarget.Cuda, "sm_80");
            public static Target cuda_sm_86 { get; } = new Target(Target.CompileTarget.Cuda, "sm_86");
            public static Target cuda_sm_87 { get; } = new Target(Target.CompileTarget.Cuda, "sm_87");
            public static Target cuda_sm_89 { get; } = new Target(Target.CompileTarget.Cuda, "sm_89");
            public static Target cuda_sm_90 { get; } = new Target(Target.CompileTarget.Cuda, "sm_90");
            
            // PTX targets
            public static Target ptx { get; } = new Target(Target.CompileTarget.PTX, "6.0");
            public static Target ptx_6_0 { get; } = new Target(Target.CompileTarget.PTX, "6.0");
            public static Target ptx_7_0 { get; } = new Target(Target.CompileTarget.PTX, "7.0");
            public static Target ptx_8_0 { get; } = new Target(Target.CompileTarget.PTX, "8.0");
            
            // Host targets
            public static Target host_callable { get; } = new Target(Target.CompileTarget.HostHostCallable, "host");
            public static Target host_executable { get; } = new Target(Target.CompileTarget.Executable, "host");
        }

        /// <summary>
        /// Creates a custom target with specified compile target and profile.
        /// </summary>
        /// <param name="compileTarget">The compilation target type</param>
        /// <param name="profile">The shader model/version profile</param>
        /// <returns>A new Target instance</returns>
        public static Target Custom(Target.CompileTarget compileTarget, string profile)
        {
            return new Target(compileTarget, profile);
        }

        /// <summary>
        /// Gets all available HLSL targets as an array.
        /// </summary>
        /// <returns>Array of all HLSL Target instances</returns>
        public static Target[] GetAllHlslTargets()
        {
            return typeof(Hlsl).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                               .Where(p => p.PropertyType == typeof(Target))
                               .Select(p => (Target)p.GetValue(null)!)
                               .ToArray();
        }

        /// <summary>
        /// Gets all available GLSL targets as an array.
        /// </summary>
        /// <returns>Array of all GLSL Target instances</returns>
        public static Target[] GetAllGlslTargets()
        {
            return typeof(Glsl).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                               .Where(p => p.PropertyType == typeof(Target))
                               .Select(p => (Target)p.GetValue(null)!)
                               .ToArray();
        }

        /// <summary>
        /// Gets all available Metal targets as an array.
        /// </summary>
        /// <returns>Array of all Metal Target instances</returns>
        public static Target[] GetAllMetalTargets()
        {
            return typeof(Metal).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                .Where(p => p.PropertyType == typeof(Target))
                                .Select(p => (Target)p.GetValue(null)!)
                                .ToArray();
        }

        /// <summary>
        /// Gets all available SPIR-V targets as an array.
        /// </summary>
        /// <returns>Array of all SPIR-V Target instances</returns>
        public static Target[] GetAllSpirVTargets()
        {
            return typeof(SpirV).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                .Where(p => p.PropertyType == typeof(Target))
                                .Select(p => (Target)p.GetValue(null)!)
                                .ToArray();
        }

        /// <summary>
        /// Gets all available WGSL targets as an array.
        /// </summary>
        /// <returns>Array of all WGSL Target instances</returns>
        public static Target[] GetAllWgslTargets()
        {
            return typeof(Wgsl).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                               .Where(p => p.PropertyType == typeof(Target))
                               .Select(p => (Target)p.GetValue(null)!)
                               .ToArray();
        }

        /// <summary>
        /// Gets all available targets from all categories as an array.
        /// </summary>
        /// <returns>Array of all Target instances</returns>
        public static Target[] GetAllTargets()
        {
            return GetAllHlslTargets()
                   .Concat(GetAllGlslTargets())
                   .Concat(GetAllMetalTargets())
                   .Concat(GetAllSpirVTargets())
                   .Concat(GetAllWgslTargets())
                   .ToArray();
        }
    }
}
