# Cross-Platform Shader Pipeline Sample

This sample demonstrates how to use Slang.Net to create a cross-platform shader pipeline that can compile the same shader source code to multiple graphics APIs.

## Overview

This sample shows:

1. How to compile a single Slang shader to multiple target platforms
2. The process of managing compiled shaders for different graphics APIs
3. How to handle platform-specific shader model versions
4. Best practices for cross-platform shader development

## Sample Files

- **CrossPlatformShaderManager.cs**: Manager class for cross-platform shader compilation
- **UniversalShader.slang**: A shader written once to work on all platforms
- **Program.cs**: Demo application that compiles the shader for all platforms

## Usage

The sample demonstrates:

1. Writing a single shader in Slang that can target all platforms
2. Compiling to multiple targets:
   - DirectX 11 (HLSL SM 5.0)
   - DirectX 12 (HLSL SM 6.0)
   - OpenGL (GLSL 450)
   - Vulkan (GLSL 450)
   - Metal (Metal 2.0)
3. Saving the compiled shaders to files for inspection
4. Customizing compilation parameters for specific targets

## Benefits

Cross-platform shader pipelines provide several benefits:

- **Write Once, Run Everywhere**: Write shaders once in Slang and deploy to any platform
- **Consistent Behavior**: Same visual results across different platforms
- **Maintainability**: Single source of truth for shader code
- **Flexibility**: Customize compilation parameters for specific platforms when needed

## Next Steps

In a real-world application, you would:

1. Integrate the compiled shaders with platform-specific rendering APIs
2. Implement runtime platform detection
3. Add shader hot-reloading for faster development
4. Cache compiled shaders for improved performance
