# OpenGL Integration with Silk.NET Sample

This sample demonstrates how to use Slang.Net with Silk.NET for OpenGL integration, showing how to compile Slang shaders to GLSL and use them in an OpenGL application.

## What This Sample Shows

- **Slang Shader Compilation**: Compiling Slang shaders to GLSL using Slang.Net
- **OpenGL Integration**: Demonstrating the compilation process and integration details
- **Shader Management**: Creating a reusable shader manager class for OpenGL
- **GLSL Generation**: Viewing the generated GLSL code from Slang shaders

## Features

- ✅ **Slang to GLSL Compilation**: Converting Slang shaders to GLSL 4.60
- ✅ **OpenGL Shader Creation**: Pattern for creating OpenGL shader objects
- ✅ **Shader Program Linking**: Linking shaders into programs
- ✅ **Compute Shader Support**: Support for compute shaders
- ✅ **Error Handling**: Proper error handling for both Slang and OpenGL operations

## Files

- **`Program.cs`**: Main application with shader compilation demonstration
- **`OpenGLShaderManager.cs`**: Shader manager class for OpenGL integration
- **`SimpleVertexGL.slang`**: Simple vertex shader in Slang
- **`SimpleFragmentGL.slang`**: Simple fragment shader in Slang
- **`SimpleComputeGL.slang`**: Simple compute shader in Slang

## Shader Pipeline

1. **Slang Source** → **Slang.Net** → **GLSL Code**
2. **GLSL Code** → **OpenGL Shader Object** → **OpenGL Program Object**

## How It Works

### 1. Shader Manager Initialization

```csharp
var shaderManager = new OpenGLShaderManager(gl, shaderDirectory);
```

### 2. Slang Compilation

The `OpenGLShaderManager` loads Slang modules and compiles them to GLSL:

```csharp
var module = _slangSession.LoadModule(shaderPath);
var entry = module.Program.EntryPoints.First(e => e.Name == entryPoint);
var glslCode = entry.Compile();
```

### 3. OpenGL Shader Creation

The generated GLSL is used to create OpenGL shader objects:

```csharp
var shader = _gl.CreateShader(type);
_gl.ShaderSource(shader, glslCode);
_gl.CompileShader(shader);
```

### 4. Program Linking

Finally, shaders are linked into a program:

```csharp
var program = _gl.CreateProgram();
_gl.AttachShader(program, shader);
_gl.LinkProgram(program);
```

## Prerequisites

- .NET 9.0 or later
- Silk.NET.OpenGL package
- Slang.Net package

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
   - Console shows compilation process
   - Generated GLSL code is displayed
   - Integration details are explained

## Key Concepts

- **Session Configuration**: Setting up Slang compilation for OpenGL GLSL
- **Shader Types**: Creating different shader types (vertex, fragment, compute)
- **Program Linking**: Creating shader programs by linking multiple shaders
- **Error Handling**: Handling compilation and linking errors

This sample provides a foundation for integrating Slang shaders into OpenGL applications using Silk.NET.
