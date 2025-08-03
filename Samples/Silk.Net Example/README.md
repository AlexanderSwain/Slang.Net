# Slang.Net with Silk.NET Demo

This sample demonstrates how to use **Slang.Net** with **Silk.NET** to create cross-platform graphics applications that can target multiple graphics APIs from a single Slang shader source.

## Features

- **Cross-Platform Shader Compilation**: Write shaders once in Slang, compile to multiple targets
- **Multiple Graphics Backend Support**: 
  - OpenGL (with GLSL output)
  - DirectX 11 (with HLSL output)
- **Interactive Backend Selection**: Choose your graphics backend at runtime
- **Shader Reflection**: Demonstrates Slang's reflection capabilities
- **Hot Shader Reloading**: Press 'R' to reload shaders during development

## Requirements

- .NET 9.0 or later
- Windows (for DirectX 11 support) or cross-platform (for OpenGL support)
- Slang.Sdk NuGet package
- Silk.NET packages for graphics and windowing

## Getting Started

1. **Build the project**:
   ```bash
   dotnet build "Tutorial 4.1 - Model Loading.csproj"
   ```

2. **Run the demo**:
   ```bash
   dotnet run
   ```

3. **Select your backend**:
   - Choose **1** for OpenGL
   - Choose **2** for DirectX 11

## Controls

- **WASD**: Move camera
- **Mouse**: Look around
- **R**: Reload shaders (useful for development)
- **B**: Switch backends (requires restart)
- **ESC**: Exit application

## How It Works

### Slang Shader

The demo uses a single Slang shader file (`Shaders/shader.slang`) that contains:

```hlsl
// Vertex shader entry point
[shader("vertex")]
void vertexMain(
    float3 vPos : POSITION,
    float2 vUv : TEXCOORD0,
    uniform float4x4 uModel,
    uniform float4x4 uView,
    uniform float4x4 uProjection,
    out float4 position : SV_Position,
    out float2 fUv : TEXCOORD0)
{
    // Transform vertex position
    float4 worldPos = mul(uModel, float4(vPos, 1.0));
    float4 viewPos = mul(uView, worldPos);
    position = mul(uProjection, viewPos);
    
    // Pass through texture coordinates
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float2 fUv : TEXCOORD0,
    uniform Texture2D uTexture0,
    uniform SamplerState textureSampler
) : SV_Target
{
    return uTexture0.Sample(textureSampler, fUv);
}
```

### Compilation Process

1. **Slang Compilation**: The `SlangShaderCompiler` class uses Slang.Sdk to compile the Slang shader to the target graphics API
2. **OpenGL Path**: Compiles to GLSL 3.30 (with fallback to hardcoded GLSL for compatibility)
3. **DirectX 11 Path**: Compiles to HLSL 5.0 for vertex and pixel shaders
4. **Reflection**: Uses Slang's reflection API to inspect shader parameters and entry points

### Project Structure

```
├── Tutorial 4.1 - Model Loading/
│   ├── Program.cs                    # Main application entry point
│   ├── SlangShaderCompiler.cs        # Slang compilation wrapper
│   ├── DirectX11Renderer.cs          # DirectX 11 demo classes
│   ├── OpenGLRenderer.cs             # OpenGL renderer wrapper
│   ├── IRenderer.cs                  # Abstract renderer interface
│   └── [Other Silk.NET classes...]   # Shader, Texture, Model, etc.
├── Shaders/
│   ├── shader.slang                  # Main Slang shader
│   ├── shader.vert                   # Fallback GLSL vertex shader
│   └── shader.frag                   # Fallback GLSL fragment shader
└── Resources/
    ├── cube.model                    # 3D model data
    └── silk.png                      # Texture
```

## Key Code Components

### SlangShaderCompiler

The `SlangShaderCompiler` class demonstrates:
- Creating Slang sessions with different targets
- Loading and compiling Slang modules
- Handling multiple shader stages (vertex/fragment)
- Using shader reflection to inspect compiled shaders

### Backend Abstraction

The demo shows how to:
- Abstract graphics backend differences
- Handle platform-specific compilation paths
- Provide graceful fallbacks for compatibility issues
- Support runtime backend switching

## Educational Value

This sample teaches:

1. **Slang Integration**: How to integrate Slang.Sdk into a .NET graphics application
2. **Cross-Platform Graphics**: Techniques for supporting multiple graphics APIs
3. **Shader Development Workflow**: Hot-reloading and development-friendly features
4. **Graphics API Abstraction**: Clean separation between rendering logic and API specifics

## Extending the Demo

Try these modifications to learn more:

1. **Add Compute Shaders**: Extend the Slang shader to include compute functionality
2. **More Graphics APIs**: Add Vulkan or Metal support
3. **Advanced Slang Features**: Use Slang interfaces, generics, or modules
4. **Shader Parameters**: Add more uniforms and demonstrate parameter binding

## Troubleshooting

- **Shader Compilation Errors**: Check the console output for detailed Slang error messages
- **DirectX Issues**: Ensure you're running on Windows with DirectX 11 support
- **OpenGL Issues**: Update your graphics drivers for OpenGL 3.3+ support

## Learn More

- [Slang Documentation](https://slang-lang.org/)
- [Silk.NET Documentation](https://dotnetfoundation.github.io/Silk.NET/)
- [Slang.Net Repository](https://github.com/Aqqorn/Slang.Net)
