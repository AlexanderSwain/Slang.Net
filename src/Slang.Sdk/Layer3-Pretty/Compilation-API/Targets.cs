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
            
            /// <summary>
            /// Vertex shader, Shader Model 4.0 (DirectX 10). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target vs_4_0 { get; }

            /// <summary>
            /// Pixel shader, Shader Model 4.0 (DirectX 10). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target ps_4_0 { get; }

            /// <summary>
            /// Geometry shader, Shader Model 4.0 (DirectX 10). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target gs_4_0 { get; }

            // Shader Model 4.1 - DirectX 10.1
            
            /// <summary>
            /// Vertex shader, Shader Model 4.1 (DirectX 10.1). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target vs_4_1 { get; }

            /// <summary>
            /// Pixel shader, Shader Model 4.1 (DirectX 10.1). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target ps_4_1 { get; }

            /// <summary>
            /// Geometry shader, Shader Model 4.1 (DirectX 10.1). [Deprecated]
            /// </summary>
            [Obsolete]
            public static Target gs_4_1 { get; }

            // Shader Model 5.0 - DirectX 11
            
            /// <summary>
            /// Vertex shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target vs_5_0 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target ps_5_0 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target gs_5_0 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target cs_5_0 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target hs_5_0 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 5.0 (DirectX 11).
            /// </summary>
            public static Target ds_5_0 { get; }

            // Shader Model 5.1 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target vs_5_1 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target ps_5_1 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target gs_5_1 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target cs_5_1 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target hs_5_1 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 5.1 (DirectX 12).
            /// </summary>
            public static Target ds_5_1 { get; }

            // Shader Model 6.0 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target vs_6_0 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target ps_6_0 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target gs_6_0 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target cs_6_0 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target hs_6_0 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.0 (DirectX 12).
            /// </summary>
            public static Target ds_6_0 { get; }

            // Shader Model 6.1 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target vs_6_1 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target ps_6_1 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target gs_6_1 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target cs_6_1 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target hs_6_1 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.1 (DirectX 12).
            /// </summary>
            public static Target ds_6_1 { get; }

            // Shader Model 6.2 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target vs_6_2 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target ps_6_2 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target gs_6_2 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target cs_6_2 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target hs_6_2 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.2 (DirectX 12).
            /// </summary>
            public static Target ds_6_2 { get; }

            // Shader Model 6.3 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target vs_6_3 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target ps_6_3 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target gs_6_3 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target cs_6_3 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target hs_6_3 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.3 (DirectX 12).
            /// </summary>
            public static Target ds_6_3 { get; }

            // Shader Model 6.4 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target vs_6_4 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target ps_6_4 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target gs_6_4 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target cs_6_4 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target hs_6_4 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.4 (DirectX 12).
            /// </summary>
            public static Target ds_6_4 { get; }

            // Shader Model 6.5 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target vs_6_5 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target ps_6_5 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target gs_6_5 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target cs_6_5 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target hs_6_5 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.5 (DirectX 12).
            /// </summary>
            public static Target ds_6_5 { get; }

            // Shader Model 6.6 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target vs_6_6 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target ps_6_6 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target gs_6_6 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target cs_6_6 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target hs_6_6 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.6 (DirectX 12).
            /// </summary>
            public static Target ds_6_6 { get; }

            // Shader Model 6.7 - DirectX 12
            
            /// <summary>
            /// Vertex shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target vs_6_7 { get; }
            
            /// <summary>
            /// Pixel shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target ps_6_7 { get; }
            
            /// <summary>
            /// Geometry shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target gs_6_7 { get; }
            
            /// <summary>
            /// Compute shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target cs_6_7 { get; }
            
            /// <summary>
            /// Hull shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target hs_6_7 { get; }
            
            /// <summary>
            /// Domain shader, Shader Model 6.7 (DirectX 12).
            /// </summary>
            public static Target ds_6_7 { get; }

            // Mesh Shaders (Shader Model 6.5+)
            
            /// <summary>
            /// Mesh shader, Shader Model 6.5 (DirectX 12 Ultimate - new pipeline).
            /// </summary>
            public static Target ms_6_5 { get; }
            
            /// <summary>
            /// Amplification shader (task shader), Shader Model 6.5 (DirectX 12 Ultimate).
            /// </summary>
            public static Target as_6_5 { get; }
            
            /// <summary>
            /// Mesh shader, Shader Model 6.6 (DirectX 12 Ultimate).
            /// </summary>
            public static Target ms_6_6 { get; }
            
            /// <summary>
            /// Amplification shader, Shader Model 6.6 (DirectX 12 Ultimate).
            /// </summary>
            public static Target as_6_6 { get; }
            
            /// <summary>
            /// Mesh shader, Shader Model 6.7 (DirectX 12 Ultimate).
            /// </summary>
            public static Target ms_6_7 { get; }
            
            /// <summary>
            /// Amplification shader, Shader Model 6.7 (DirectX 12 Ultimate).
            /// </summary>
            public static Target as_6_7 { get; }

            static Hlsl()
            {
                // Shader Model 4.0 - DirectX 10
                vs_4_0 = new Target(Target.CompileTarget.Hlsl, "vs_4_0");
                ps_4_0 = new Target(Target.CompileTarget.Hlsl, "ps_4_0");
                gs_4_0 = new Target(Target.CompileTarget.Hlsl, "gs_4_0");

                vs_4_1 = new Target(Target.CompileTarget.Hlsl, "vs_4_1");
                ps_4_1 = new Target(Target.CompileTarget.Hlsl, "ps_4_1");
                gs_4_1 = new Target(Target.CompileTarget.Hlsl, "gs_4_1");

                vs_5_0 = new Target(Target.CompileTarget.Hlsl, "vs_5_0");
                ps_5_0 = new Target(Target.CompileTarget.Hlsl, "ps_5_0");
                gs_5_0 = new Target(Target.CompileTarget.Hlsl, "gs_5_0");
                cs_5_0 = new Target(Target.CompileTarget.Hlsl, "cs_5_0");
                hs_5_0 = new Target(Target.CompileTarget.Hlsl, "hs_5_0");
                ds_5_0 = new Target(Target.CompileTarget.Hlsl, "ds_5_0");

                vs_5_1 = new Target(Target.CompileTarget.Hlsl, "vs_5_1");
                ps_5_1 = new Target(Target.CompileTarget.Hlsl, "ps_5_1");
                gs_5_1 = new Target(Target.CompileTarget.Hlsl, "gs_5_1");
                cs_5_1 = new Target(Target.CompileTarget.Hlsl, "cs_5_1");
                hs_5_1 = new Target(Target.CompileTarget.Hlsl, "hs_5_1");
                ds_5_1 = new Target(Target.CompileTarget.Hlsl, "ds_5_1");

                vs_6_0 = new Target(Target.CompileTarget.Hlsl, "vs_6_0");
                ps_6_0 = new Target(Target.CompileTarget.Hlsl, "ps_6_0");
                gs_6_0 = new Target(Target.CompileTarget.Hlsl, "gs_6_0");
                cs_6_0 = new Target(Target.CompileTarget.Hlsl, "cs_6_0");
                hs_6_0 = new Target(Target.CompileTarget.Hlsl, "hs_6_0");
                ds_6_0 = new Target(Target.CompileTarget.Hlsl, "ds_6_0");

                vs_6_1 = new Target(Target.CompileTarget.Hlsl, "vs_6_1");
                ps_6_1 = new Target(Target.CompileTarget.Hlsl, "ps_6_1");
                gs_6_1 = new Target(Target.CompileTarget.Hlsl, "gs_6_1");
                cs_6_1 = new Target(Target.CompileTarget.Hlsl, "cs_6_1");
                hs_6_1 = new Target(Target.CompileTarget.Hlsl, "hs_6_1");
                ds_6_1 = new Target(Target.CompileTarget.Hlsl, "ds_6_1");

                vs_6_2 = new Target(Target.CompileTarget.Hlsl, "vs_6_2");
                ps_6_2 = new Target(Target.CompileTarget.Hlsl, "ps_6_2");
                gs_6_2 = new Target(Target.CompileTarget.Hlsl, "gs_6_2");
                cs_6_2 = new Target(Target.CompileTarget.Hlsl, "cs_6_2");
                hs_6_2 = new Target(Target.CompileTarget.Hlsl, "hs_6_2");
                ds_6_2 = new Target(Target.CompileTarget.Hlsl, "ds_6_2");

                vs_6_3 = new Target(Target.CompileTarget.Hlsl, "vs_6_3");
                ps_6_3 = new Target(Target.CompileTarget.Hlsl, "ps_6_3");
                gs_6_3 = new Target(Target.CompileTarget.Hlsl, "gs_6_3");
                cs_6_3 = new Target(Target.CompileTarget.Hlsl, "cs_6_3");
                hs_6_3 = new Target(Target.CompileTarget.Hlsl, "hs_6_3");
                ds_6_3 = new Target(Target.CompileTarget.Hlsl, "ds_6_3");

                vs_6_4 = new Target(Target.CompileTarget.Hlsl, "vs_6_4");
                ps_6_4 = new Target(Target.CompileTarget.Hlsl, "ps_6_4");
                gs_6_4 = new Target(Target.CompileTarget.Hlsl, "gs_6_4");
                cs_6_4 = new Target(Target.CompileTarget.Hlsl, "cs_6_4");
                hs_6_4 = new Target(Target.CompileTarget.Hlsl, "hs_6_4");
                ds_6_4 = new Target(Target.CompileTarget.Hlsl, "ds_6_4");

                vs_6_5 = new Target(Target.CompileTarget.Hlsl, "vs_6_5");
                ps_6_5 = new Target(Target.CompileTarget.Hlsl, "ps_6_5");
                gs_6_5 = new Target(Target.CompileTarget.Hlsl, "gs_6_5");
                cs_6_5 = new Target(Target.CompileTarget.Hlsl, "cs_6_5");
                hs_6_5 = new Target(Target.CompileTarget.Hlsl, "hs_6_5");
                ds_6_5 = new Target(Target.CompileTarget.Hlsl, "ds_6_5");

                vs_6_6 = new Target(Target.CompileTarget.Hlsl, "vs_6_6");
                ps_6_6 = new Target(Target.CompileTarget.Hlsl, "ps_6_6");
                gs_6_6 = new Target(Target.CompileTarget.Hlsl, "gs_6_6");
                cs_6_6 = new Target(Target.CompileTarget.Hlsl, "cs_6_6");
                hs_6_6 = new Target(Target.CompileTarget.Hlsl, "hs_6_6");
                ds_6_6 = new Target(Target.CompileTarget.Hlsl, "ds_6_6");

                vs_6_7 = new Target(Target.CompileTarget.Hlsl, "vs_6_7");
                ps_6_7 = new Target(Target.CompileTarget.Hlsl, "ps_6_7");
                gs_6_7 = new Target(Target.CompileTarget.Hlsl, "gs_6_7");
                cs_6_7 = new Target(Target.CompileTarget.Hlsl, "cs_6_7");
                hs_6_7 = new Target(Target.CompileTarget.Hlsl, "hs_6_7");
                ds_6_7 = new Target(Target.CompileTarget.Hlsl, "ds_6_7");

                ms_6_5 = new Target(Target.CompileTarget.Hlsl, "ms_6_5");
                as_6_5 = new Target(Target.CompileTarget.Hlsl, "as_6_5");
                ms_6_6 = new Target(Target.CompileTarget.Hlsl, "ms_6_6");
                as_6_6 = new Target(Target.CompileTarget.Hlsl, "as_6_6");
                ms_6_7 = new Target(Target.CompileTarget.Hlsl, "ms_6_7");
                as_6_7 = new Target(Target.CompileTarget.Hlsl, "as_6_7");
            }
        }

        /// <summary>
        /// GLSL (OpenGL Shading Language) compilation targets for OpenGL and Vulkan.
        /// Supports various GLSL versions from 330 to 460.
        /// </summary>
        public static class Glsl
        {
            // OpenGL 3.3 and later
            
            /// <summary>
            /// GLSL 3.30 (OpenGL 3.3).
            /// </summary>
            public static Target v330 { get; }
            
            /// <summary>
            /// GLSL 4.00 (OpenGL 4.0).
            /// </summary>
            public static Target v400 { get; }
            
            /// <summary>
            /// GLSL 4.10 (OpenGL 4.1).
            /// </summary>
            public static Target v410 { get; }
            
            /// <summary>
            /// GLSL 4.20 (OpenGL 4.2).
            /// </summary>
            public static Target v420 { get; }
            
            /// <summary>
            /// GLSL 4.30 (OpenGL 4.3).
            /// </summary>
            public static Target v430 { get; }
            
            /// <summary>
            /// GLSL 4.40 (OpenGL 4.4).
            /// </summary>
            public static Target v440 { get; }
            
            /// <summary>
            /// GLSL 4.50 (OpenGL 4.5).
            /// </summary>
            public static Target v450 { get; }
            
            /// <summary>
            /// GLSL 4.60 (OpenGL 4.6).
            /// </summary>
            public static Target v460 { get; }

            // Common Vulkan targets
            
            /// <summary>
            /// Alias for GLSL 4.50 (used for Vulkan 1.0/1.1).
            /// </summary>
            public static Target vulkan { get; }
            
            /// <summary>
            /// GLSL 4.50 (Vulkan 1.0).
            /// </summary>
            public static Target vulkan_1_0 { get; }
            
            /// <summary>
            /// GLSL 4.50 (Vulkan 1.1).
            /// </summary>
            public static Target vulkan_1_1 { get; }
            
            /// <summary>
            /// GLSL 4.60 (Vulkan 1.2).
            /// </summary>
            public static Target vulkan_1_2 { get; }
            
            /// <summary>
            /// GLSL 4.60 (Vulkan 1.3).
            /// </summary>
            public static Target vulkan_1_3 { get; }

            // OpenGL ES targets
            
            /// <summary>
            /// GLSL ES 3.00 (OpenGL ES 3.0).
            /// </summary>
            public static Target es_300 { get; }
            
            /// <summary>
            /// GLSL ES 3.10 (OpenGL ES 3.1).
            /// </summary>
            public static Target es_310 { get; }
            
            /// <summary>
            /// GLSL ES 3.20 (OpenGL ES 3.2).
            /// </summary>
            public static Target es_320 { get; }

            static Glsl()
            {
                v330 = new Target(Target.CompileTarget.Glsl, "330");
                v400 = new Target(Target.CompileTarget.Glsl, "400");
                v410 = new Target(Target.CompileTarget.Glsl, "410");
                v420 = new Target(Target.CompileTarget.Glsl, "420");
                v430 = new Target(Target.CompileTarget.Glsl, "430");
                v440 = new Target(Target.CompileTarget.Glsl, "440");
                v450 = new Target(Target.CompileTarget.Glsl, "450");
                v460 = new Target(Target.CompileTarget.Glsl, "460");

                vulkan = new Target(Target.CompileTarget.Glsl, "450");
                vulkan_1_0 = new Target(Target.CompileTarget.Glsl, "450");
                vulkan_1_1 = new Target(Target.CompileTarget.Glsl, "450");
                vulkan_1_2 = new Target(Target.CompileTarget.Glsl, "460");
                vulkan_1_3 = new Target(Target.CompileTarget.Glsl, "460");


                es_300 = new Target(Target.CompileTarget.Glsl, "300 es");
                es_310 = new Target(Target.CompileTarget.Glsl, "310 es");
                es_320 = new Target(Target.CompileTarget.Glsl, "320 es");
            }
        }

        /// <summary>
        /// Metal Shading Language compilation targets for macOS, iOS, and Apple platforms.
        /// Supports Metal versions from 1.0 to 3.1.
        /// </summary>
        public static class Metal
        {
            // Metal 1.x - iOS 8.0+, macOS 10.11+
            
            /// <summary>
            /// Metal Shading Language v1.0 (iOS 8.0+/macOS 10.11+) [Legacy].
            /// </summary>
            public static Target v1_0 { get; }
            
            /// <summary>
            /// Metal Shading Language v1.1 (iOS 9.0+/macOS 10.11+).
            /// </summary>
            public static Target v1_1 { get; }
            
            /// <summary>
            /// Metal Shading Language v1.2 (iOS 10.0+/macOS 10.12+).
            /// </summary>
            public static Target v1_2 { get; }

            // Metal 2.x - iOS 10.0+, macOS 10.12+
            
            /// <summary>
            /// Metal Shading Language v2.0 (iOS 10.0+/macOS 10.12+).
            /// </summary>
            public static Target v2_0 { get; }
            
            /// <summary>
            /// Metal Shading Language v2.1 (iOS 10.2+/macOS 10.13+).
            /// </summary>
            public static Target v2_1 { get; }
            
            /// <summary>
            /// Metal Shading Language v2.2 (iOS 11.0+/macOS 10.13+).
            /// </summary>
            public static Target v2_2 { get; }
            
            /// <summary>
            /// Metal Shading Language v2.3 (iOS 11.3+/macOS 10.13+).
            /// </summary>
            public static Target v2_3 { get; }
            
            /// <summary>
            /// Metal Shading Language v2.4 (iOS 12.0+/macOS 10.14+).
            /// </summary>
            public static Target v2_4 { get; }

            // Metal 3.x - iOS 16.0+, macOS 13.0+
            
            /// <summary>
            /// Metal Shading Language v3.0 (macOS 13.0+/iOS 16.0+).
            /// </summary>
            public static Target v3_0 { get; }
            
            /// <summary>
            /// Metal Shading Language v3.1 (macOS 13.1+/iOS 16.1+).
            /// </summary>
            public static Target v3_1 { get; }

            // Platform-specific convenience targets
            
            /// <summary>
            /// Convenience alias (Metal 2.0 target on macOS).
            /// </summary>
            public static Target macos { get; }
            
            /// <summary>
            /// Convenience alias (Metal 2.0 target on iOS).
            /// </summary>
            public static Target ios { get; }
            
            /// <summary>
            /// Convenience alias (Metal 2.0 target on tvOS).
            /// </summary>
            public static Target tvos { get; }
            
            /// <summary>
            /// Convenience alias (Metal 2.0 target on watchOS).
            /// </summary>
            public static Target watchos { get; }

            // Latest version
            
            /// <summary>
            /// Latest Metal version (v3.1 as of 2025).
            /// </summary>
            public static Target latest { get; }

            static Metal()
            {
                // Metal 1.x - iOS 8.0+, macOS 10.11+
                v1_0 = new Target(Target.CompileTarget.Metal, "1.0");
                v1_1 = new Target(Target.CompileTarget.Metal, "1.1");
                v1_2 = new Target(Target.CompileTarget.Metal, "1.2");

                v2_0 = new Target(Target.CompileTarget.Metal, "2.0");
                v2_1 = new Target(Target.CompileTarget.Metal, "2.1");
                v2_2 = new Target(Target.CompileTarget.Metal, "2.2");
                v2_3 = new Target(Target.CompileTarget.Metal, "2.3");
                v2_4 = new Target(Target.CompileTarget.Metal, "2.4");

                v3_0 = new Target(Target.CompileTarget.Metal, "3.0");
                v3_1 = new Target(Target.CompileTarget.Metal, "3.1");

                macos = new Target(Target.CompileTarget.Metal, "2.0");
                ios = new Target(Target.CompileTarget.Metal, "2.0");
                tvos = new Target(Target.CompileTarget.Metal, "2.0");
                watchos = new Target(Target.CompileTarget.Metal, "2.0");


                latest = new Target(Target.CompileTarget.Metal, "3.1");
            }
        }

        /// <summary>
        /// SPIR-V (Standard Portable Intermediate Representation for Vulkan) compilation targets.
        /// Primarily used with Vulkan, OpenCL, and other Khronos APIs.
        /// </summary>
        public static class SpirV
        {
            // SPIR-V binary output
            
            /// <summary>
            /// SPIR-V 1.0 (released 2015).
            /// </summary>
            public static Target v1_0 { get; }
            
            /// <summary>
            /// SPIR-V 1.1 (2016).
            /// </summary>
            public static Target v1_1 { get; }
            
            /// <summary>
            /// SPIR-V 1.2 (2017).
            /// </summary>
            public static Target v1_2 { get; }
            
            /// <summary>
            /// SPIR-V 1.3 (2018).
            /// </summary>
            public static Target v1_3 { get; }
            
            /// <summary>
            /// SPIR-V 1.4 (2019).
            /// </summary>
            public static Target v1_4 { get; }
            
            /// <summary>
            /// SPIR-V 1.5 (2020).
            /// </summary>
            public static Target v1_5 { get; }
            
            /// <summary>
            /// SPIR-V 1.6 (2021).
            /// </summary>
            public static Target v1_6 { get; }

            // Vulkan-specific SPIR-V targets
            
            /// <summary>
            /// Vulkan 1.0 target (SPIR-V 1.0).
            /// </summary>
            public static Target vulkan_1_0 { get; }
            
            /// <summary>
            /// Vulkan 1.1 target (SPIR-V 1.3).
            /// </summary>
            public static Target vulkan_1_1 { get; }
            
            /// <summary>
            /// Vulkan 1.2 target (SPIR-V 1.5).
            /// </summary>
            public static Target vulkan_1_2 { get; }
            
            /// <summary>
            /// Vulkan 1.3 target (SPIR-V 1.6).
            /// </summary>
            public static Target vulkan_1_3 { get; }

            // Convenience targets
            
            /// <summary>
            /// Raw SPIR-V binary (SPIR-V 1.6).
            /// </summary>
            public static Target binary { get; }
            
            /// <summary>
            /// Latest SPIR-V (alias for 1.6).
            /// </summary>
            public static Target latest { get; }

            static SpirV()
            {
                v1_0 = new Target(Target.CompileTarget.SpirV, "1.0");
                v1_1 = new Target(Target.CompileTarget.SpirV, "1.1");
                v1_2 = new Target(Target.CompileTarget.SpirV, "1.2");
                v1_3 = new Target(Target.CompileTarget.SpirV, "1.3");
                v1_4 = new Target(Target.CompileTarget.SpirV, "1.4");
                v1_5 = new Target(Target.CompileTarget.SpirV, "1.5");
                v1_6 = new Target(Target.CompileTarget.SpirV, "1.6");

                vulkan_1_0 = new Target(Target.CompileTarget.SpirV, "spirv_1_0");
                vulkan_1_1 = new Target(Target.CompileTarget.SpirV, "spirv_1_3");
                vulkan_1_2 = new Target(Target.CompileTarget.SpirV, "spirv_1_5");
                vulkan_1_3 = new Target(Target.CompileTarget.SpirV, "spirv_1_6");

                binary = new Target(Target.CompileTarget.SpirV, "1.6");
                latest = new Target(Target.CompileTarget.SpirV, "1.6");
            }
        }

        /// <summary>
        /// WGSL (WebGPU Shading Language) compilation targets for WebGPU.
        /// The emerging standard for web-based GPU programming.
        /// </summary>
        public static class Wgsl
        {
            // WebGPU shading language
            
            /// <summary>
            /// WGSL 1.0 (the current WebGPU Shading Language).
            /// </summary>
            public static Target v1_0 { get; }
            
            /// <summary>
            /// Alias for WGSL 1.0 (W3C/WebGPU standard).
            /// </summary>
            public static Target webgpu { get; }
            
            /// <summary>
            /// Latest WGSL (alias for 1.0).
            /// </summary>
            public static Target latest { get; }

            static Wgsl()
            {
                // WGSL 1.0 is the current version
                v1_0 = new Target(Target.CompileTarget.Wgsl, "1.0");
                webgpu = new Target(Target.CompileTarget.Wgsl, "1.0");
                latest = new Target(Target.CompileTarget.Wgsl, "1.0");
            }
        }

        /// <summary>
        /// Additional compilation targets for various purposes.
        /// </summary>
        public static class Other
        {
            // C/C++ targets for compute kernels
            
            /// <summary>
            /// Emit C99-compatible source (CPU compute).
            /// </summary>
            public static Target c_source { get; }
            
            /// <summary>
            /// Emit C++11 source (CPU compute).
            /// </summary>
            public static Target cpp_source { get; }
            
            /// <summary>
            /// Emit C++14 source (CPU compute).
            /// </summary>
            public static Target cpp14 { get; }
            
            /// <summary>
            /// Emit C++17 source (CPU compute).
            /// </summary>
            public static Target cpp17 { get; }
            
            /// <summary>
            /// Emit C++20 source (CPU compute).
            /// </summary>
            public static Target cpp20 { get; }

            // CUDA targets
            
            /// <summary>
            /// Alias for latest supported SM (default).
            /// </summary>
            public static Target cuda_source { get; }

            /// <summary>
            /// NVIDIA PTX SM 5.0 (Maxwell) [Deprecated].
            /// </summary>
            [Obsolete]
            public static Target cuda_sm_50 { get; }

            /// <summary>
            /// PTX SM 6.0 (Pascal) [Deprecated].
            /// </summary>
            [Obsolete]
            public static Target cuda_sm_60 { get; }

            /// <summary>
            /// PTX SM 7.0 (Volta).
            /// </summary>
            public static Target cuda_sm_70 { get; }
            
            /// <summary>
            /// PTX SM 7.5 (Turing).
            /// </summary>
            public static Target cuda_sm_75 { get; }
            
            /// <summary>
            /// PTX SM 8.0 (Ampere).
            /// </summary>
            public static Target cuda_sm_80 { get; }
            
            /// <summary>
            /// PTX SM 8.6 (Ampere Update).
            /// </summary>
            public static Target cuda_sm_86 { get; }
            
            /// <summary>
            /// PTX SM 8.7 (Ampere Ti).
            /// </summary>
            public static Target cuda_sm_87 { get; }
            
            /// <summary>
            /// PTX SM 8.9 (Hopper).
            /// </summary>
            public static Target cuda_sm_89 { get; }
            
            /// <summary>
            /// PTX SM 9.0 (Hopper GA100+).
            /// </summary>
            public static Target cuda_sm_90 { get; }

            // PTX targets
            
            /// <summary>
            /// Alias for latest PTX (v8.0).
            /// </summary>
            public static Target ptx { get; }
            
            /// <summary>
            /// PTX ISA version 6.0.
            /// </summary>
            public static Target ptx_6_0 { get; }
            
            /// <summary>
            /// PTX 7.0.
            /// </summary>
            public static Target ptx_7_0 { get; }
            
            /// <summary>
            /// PTX 8.0.
            /// </summary>
            public static Target ptx_8_0 { get; }

            // Host targets
            
            /// <summary>
            /// CPU host-callable (compute kernel style) execution.
            /// </summary>
            public static Target host_callable { get; }
            
            /// <summary>
            /// CPU host-executable (scalar code) execution.
            /// </summary>
            public static Target host_executable { get; }

            static Other()
            {
                // C/C++ targets
                c_source = new Target(Target.CompileTarget.CSource, "c99");
                cpp_source = new Target(Target.CompileTarget.CppSource, "c++11");
                cpp14 = new Target(Target.CompileTarget.CppSource, "c++14");
                cpp17 = new Target(Target.CompileTarget.CppSource, "c++17");
                cpp20 = new Target(Target.CompileTarget.CppSource, "c++20");
                // CUDA targets
                cuda_source = new Target(Target.CompileTarget.CudaSource, "cuda_source");
                cuda_sm_50 = new Target(Target.CompileTarget.CudaSource, "sm_50");
                cuda_sm_60 = new Target(Target.CompileTarget.CudaSource, "sm_60");
                cuda_sm_70 = new Target(Target.CompileTarget.CudaSource, "sm_70");
                cuda_sm_75 = new Target(Target.CompileTarget.CudaSource, "sm_75");
                cuda_sm_80 = new Target(Target.CompileTarget.CudaSource, "sm_80");
                cuda_sm_86 = new Target(Target.CompileTarget.CudaSource, "sm_86");
                cuda_sm_87 = new Target(Target.CompileTarget.CudaSource, "sm_87");
                cuda_sm_89 = new Target(Target.CompileTarget.CudaSource, "sm_89");
                cuda_sm_90 = new Target(Target.CompileTarget.CudaSource, "sm_90");
                // PTX targets
                ptx = new Target(Target.CompileTarget.Ptx, "ptx");
                ptx_6_0 = new Target(Target.CompileTarget.Ptx, "6.0");
                ptx_7_0 = new Target(Target.CompileTarget.Ptx, "7.0");
                ptx_8_0 = new Target(Target.CompileTarget.Ptx, "8.0");
                // Host targets
                host_callable = new Target(Target.CompileTarget.HostHostCallable, "host");
                host_executable = new Target(Target.CompileTarget.HostExecutable, "host");
            }
        }

        #region Custom
        // Internal for Target.ToString()
        internal static HashSet<Target> _CustomTargets = new();

        /// <summary>
        /// Creates a custom target with specified compile target and profile. If it already exists, returns the existing target.
        /// </summary>
        /// <param name="compileTarget">The compilation target type</param>
        /// <param name="profile">The shader model/version profile</param>
        /// <returns>A new Target instance</returns>
        public static Target Custom(Target.CompileTarget compileTarget, string profile)
        {
            var newTarget = new Target(compileTarget, profile);
            if (_CustomTargets.TryGetValue(newTarget, out var existingTarget))
            {
                return existingTarget; // Return existing target if it already exists
            }
            else
            {
                _CustomTargets.Add(newTarget);
                return newTarget; // Add and return the new target
            }
        } 
        #endregion

        /// <summary>
        /// Gets the compilation output type for this target (binary or source code).
        /// </summary>
        /// <param name="target">The target to get the output type for</param>
        /// <returns>The output type indicating whether this target produces binary code or source code</returns>
        public static Target.CompileOutputType CompileOutputType(this Target target)
        {
            return target.target switch
            {
                // Binary output formats
                Target.CompileTarget.Dxbc => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.DxbcAsm => Target.CompileOutputType.SourceCode, // Assembly is text
                Target.CompileTarget.Dxil => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.DxilAsm => Target.CompileOutputType.SourceCode, // Assembly is text
                Target.CompileTarget.SpirV => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.SpirVAsm => Target.CompileOutputType.SourceCode, // Assembly is text
                Target.CompileTarget.WgslSpirv => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.WgslSpirvAsm => Target.CompileOutputType.SourceCode, // Assembly is text
                Target.CompileTarget.Ptx => Target.CompileOutputType.ByteCode, // PTX is technically bytecode
                Target.CompileTarget.CudaObjectCode => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.ObjectCode => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.MetalLib => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.MetalLibAsm => Target.CompileOutputType.SourceCode, // Assembly is text
                Target.CompileTarget.ShaderSharedLibrary => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.HostSharedLibrary => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.HostExecutable => Target.CompileOutputType.ByteCode,
                Target.CompileTarget.HostVm => Target.CompileOutputType.ByteCode,

                // Source code output formats
                Target.CompileTarget.Hlsl => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.Glsl => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.GlslVulkanDeprecated => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.GlslVulkanOneDescDeprecated => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.Metal => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.Wgsl => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.CSource => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.CppSource => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.HostCppSource => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.CudaSource => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.CppPytorchBinding => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.ShaderHostCallable => Target.CompileOutputType.SourceCode, // Callable interfaces are typically source
                Target.CompileTarget.HostHostCallable => Target.CompileOutputType.SourceCode, // Callable interfaces are typically source

                // Unknown/None targets default to source code
                Target.CompileTarget.TargetUnknown => Target.CompileOutputType.SourceCode,
                Target.CompileTarget.TargetNone => Target.CompileOutputType.SourceCode,

                // Default case for any new targets
                _ => Target.CompileOutputType.SourceCode
            };
        }
    }
}
