# Vulkan Integration with Silk.NET Sample

This sample demonstrates how to use Slang.Net with Vulkan via Silk.NET.

## Overview

This sample shows:

1. How to compile Slang shaders to GLSL 450 for Vulkan
2. The process of converting GLSL to SPIR-V (simulated)
3. How to integrate with Vulkan shader modules
4. Best practices for managing Vulkan shaders with Slang.Net

## Sample Files

- **VulkanShaderManager.cs**: A manager class for loading and compiling Slang shaders for Vulkan
- **SimpleVertexVulkan.slang**: A simple vertex shader written in Slang
- **SimpleFragmentVulkan.slang**: A simple fragment shader written in Slang  
- **SimpleComputeVulkan.slang**: A simple compute shader written in Slang
- **Program.cs**: The main program that demonstrates the shader compilation process

## Usage

For a complete Vulkan application, you would:

1. Use the `VulkanShaderManager` to compile your Slang shaders to GLSL
2. Convert the GLSL to SPIR-V using a library like Shaderc
3. Create Vulkan shader modules from the SPIR-V bytecode
4. Use these shader modules in your Vulkan pipelines

This sample demonstrates the first step and shows how you would implement the remaining steps.

## Notes

In a real application, you would need to:

- Add a GLSL to SPIR-V compiler dependency (like Shaderc)
- Properly set up the Vulkan instance, device, and other resources
- Handle shader module creation and cleanup
- Manage pipeline creation with the shader modules

The explicit conversion to SPIR-V is necessary because Vulkan requires SPIR-V bytecode for its shader modules, unlike OpenGL which can use GLSL directly.
