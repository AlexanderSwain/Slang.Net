# DirectX Integration with Silk.NET Sample

This sample demonstrates how to use Slang.Net with Silk.NET for DirectX 11 integration, showcasing the complete pipeline from Slang shader compilation to DirectX rendering.

## What This Sample Shows

- **Slang Shader Compilation**: Compiling Slang shaders to HLSL using Slang.Net
- **DirectX Integration**: Using compiled shaders with DirectX 11 via Silk.NET
- **Shader Management**: Creating a reusable shader manager class
- **Real-time Rendering**: Rendering a simple colored triangle

## Features

- ✅ **Slang to HLSL Compilation**: Demonstrates converting Slang shaders to HLSL
- ✅ **DirectX 11 Integration**: Shows how to use compiled shaders with D3D11
- ✅ **Error Handling**: Proper error handling for both Slang and DirectX operations
- ✅ **Resource Management**: Proper disposal of native resources
- ✅ **Shader Manager Pattern**: Reusable pattern for managing shaders in DirectX applications

## Files

- **`Program.cs`**: Main application with DirectX setup and rendering loop
- **`DirectXShaderManager.cs`**: Shader manager class that bridges Slang.Net and DirectX
- **`SimpleVertex.slang`**: Simple vertex shader in Slang
- **`SimplePixel.slang`**: Simple pixel shader in Slang

## Shader Pipeline

1. **Slang Source** → **Slang.Net** → **HLSL Code**
2. **HLSL Code** → **D3DCompiler** → **Bytecode**
3. **Bytecode** → **DirectX** → **GPU Shader Objects**

## How It Works

### 1. Shader Manager Initialization

```csharp
var shaderManager = new DirectXShaderManager(device, shaderDirectory);
```

### 2. Slang Compilation

The `DirectXShaderManager` loads Slang modules and compiles them to HLSL:

```csharp
var module = _slangSession.LoadModule("SimpleVertex.slang");
var entry = module.Program.EntryPoints.First(e => e.Name == "VS");
var hlslCode = entry.Compile();
```

### 3. HLSL to Bytecode

The generated HLSL is then compiled to DirectX bytecode:

```csharp
var bytecode = CompileHlslToBytecode(hlslCode, "VS", "vs_5_0");
```

### 4. DirectX Shader Creation

Finally, the bytecode is used to create DirectX shader objects:

```csharp
device.CreateVertexShader(bytecode, ...);
```

## Prerequisites

- Windows with DirectX 11 support
- .NET 9.0 or later
- Visual Studio 2022 or later (recommended)

## Building and Running

1. **Build the project**:
   ```bash
   dotnet build
   ```

2. **Run the sample**:
   ```bash
   dotnet run
   ```

3. **Expected Output**:
   - A window opens showing a colored triangle
   - Console shows compilation and initialization messages
   - No errors should occur during shader compilation

## Troubleshooting

### Common Issues

1. **Shader Compilation Errors**:
   - Check that the `.slang` files are copied to the output directory
   - Verify the entry point names match between Slang files and code

2. **DirectX Initialization Errors**:
   - Ensure your system supports DirectX 11
   - Check that the graphics drivers are up to date

3. **Missing Dependencies**:
   - Restore NuGet packages: `dotnet restore`
   - Ensure all Silk.NET packages are properly installed

### Debug Output

The sample includes detailed console output showing:
- DirectX initialization progress
- Shader compilation status
- Any errors that occur during the process

## Extending This Sample

You can extend this sample by:

1. **Adding More Shaders**: Create additional `.slang` files for compute shaders, geometry shaders, etc.
2. **Advanced Features**: Add texture loading, lighting, or post-processing effects
3. **Input Handling**: Add keyboard/mouse input using Silk.NET.Input
4. **GUI Integration**: Add ImGui for runtime shader editing

## Key Concepts

- **Session Management**: How to set up and configure Slang compilation sessions
- **Module Loading**: Loading Slang modules from files
- **Entry Point Compilation**: Compiling specific entry points to target languages
- **Resource Disposal**: Proper cleanup of both managed and native resources
- **Error Handling**: Handling compilation and runtime errors gracefully

This sample provides a solid foundation for integrating Slang shaders into DirectX applications using modern .NET and Silk.NET.
