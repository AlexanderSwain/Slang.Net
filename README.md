# Slang.Net

A comprehensive .NET wrapper for the Slang Shader Language compiler, providing seamless integration of shader compilation and reflection capabilities into .NET applications.

## Why Slang.Net?

**Slang** is a modern shader language developed by NVIDIA that brings advanced features like generics, interfaces, and modules to shader programming. **Slang.Net** makes this powerful technology accessible to .NET developers with:

- 🚀 **Zero Configuration**: Install via NuGet and start using immediately
- 🎯 **Type-Safe APIs**: Strongly-typed C# interfaces for all Slang functionality
- 📦 **Self-Contained**: All dependencies included - no external SDK required
- 🔄 **Automatic Management**: Native resources handled automatically with proper disposal
- 🌐 **Multi-Platform**: Works on Windows x64, x86, and ARM64 architectures
- ⚡ **High Performance**: Direct native interop with minimal overhead

## Why Choose Slang Over Traditional Shaders?

Traditional HLSL/GLSL shaders have limitations that Slang addresses:

| Traditional Shaders | Slang Advantages |
|-------------------|------------------|
| ❌ No code reuse between targets | ✅ Write once, compile to HLSL, GLSL, Metal, etc. |
| ❌ Limited modularity | ✅ True modules and interfaces |
| ❌ No generic programming | ✅ Generics and template metaprogramming |
| ❌ Manual resource binding | ✅ Automatic binding generation |
| ❌ Platform-specific code | ✅ Cross-platform shader source |

## Installation

Install via NuGet Package Manager:

```bash
Install-Package Slang.Net
```

Or via .NET CLI:

```bash
dotnet add package Slang.Net
```

Or add to your project file:

```xml
<PackageReference Include="Slang.Net" Version="1.0.0" />
```

## Quick Start

### Basic Shader Compilation

```csharp
using Slang;

// Create a session - the main entry point for Slang operations
using var session = new SessionBuilder()
    .AddSearchPath(@"C:\MyShaders")
    .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
    .Create();

// Load and compile a shader module
var module = session.LoadModule("MyShader.slang");
var entryPoint = module.Program.EntryPoints.First(e => e.Name == "main");
var compiledShader = entryPoint.Compile();

Console.WriteLine("Compiled HLSL:");
Console.WriteLine(compiledShader);
```

### Requirements

- **.NET 6.0** or later
- **Windows** (x64, x86, or ARM64)
- **No external dependencies** - everything is included in the NuGet package

## Complete Example

Create a simple compute shader file `AverageColor.slang`:

```hlsl
// The texture to sample
Texture2D<float4> inputImage : register(t0);

// The output buffer
RWStructuredBuffer<uint4> outputInt : register(u0);

[shader("compute")]
[numthreads(32, 32, 1)]
void CS(uint3 dispatchThreadID : SV_DispatchThreadID, 
        uint3 groupThreadID : SV_GroupThreadID, 
        uint3 groupID : SV_GroupID)
{
    // Get the dimensions of the image
    uint width, height;
    inputImage.GetDimensions(width, height);

    // Sample the color at the current pixel
    float4 color = inputImage.Load(int3(dispatchThreadID.xy, 0));

    // Convert the color to integers in the range 0-255
    uint4 colorInt = uint4(color * 255);

    // Add the color to the output buffer using atomic operations
    InterlockedAdd(outputInt[0].x, colorInt.r);
    InterlockedAdd(outputInt[0].y, colorInt.g);
    InterlockedAdd(outputInt[0].z, colorInt.b);
    InterlockedAdd(outputInt[0].w, colorInt.a);
}
```

Use the shader in your C# application:

```csharp
using Slang;
using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // Create a session with compiler options and search paths
        using var session = new SessionBuilder()
            .AddCompilerOption(CompilerOptionName.WarningsAsErrors, 
                new CompilerOptionValue(CompilerOptionValueKind.Int, 0, 0, "all", null))
            .AddCompilerOption(CompilerOptionName.Obfuscate, 
                new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null))
            .AddPreprocessorMacro("LIGHTING_SCALER", "12")
            .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
            .AddSearchPath(AppDomain.CurrentDomain.BaseDirectory)
            .Create();

        // Load the module from the specified file
        Module module = session.LoadModule("AverageColor.slang");

        // Access the shader program from the module
        ShaderProgram program = module.Program;

        // Find the compute shader entry point
        var entryPoint = program.EntryPoints.First(x => x.Name == "CS");

        // Compile the shader program using the entry point
        var source = entryPoint.Compile();

        // Print the generated source code
        Console.WriteLine("Generated HLSL:");
        Console.WriteLine(source);
    }
}
```

## Advanced Usage

### Compiling to Different Targets

Slang.Net supports compilation to multiple target languages:

```csharp
using var session = new SessionBuilder()
    .AddSearchPath(@"C:\MyShaders")
    .Create();

var module = session.LoadModule("MyShader.slang");
var entryPoint = module.Program.EntryPoints.First();

// Compile to HLSL (DirectX)
session.AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0");
var hlslCode = entryPoint.Compile();

// Compile to GLSL (OpenGL/Vulkan) 
session.AddShaderModel(CompileTarget.SLANG_GLSL, "430");
var glslCode = entryPoint.Compile();

// Compile to Metal (macOS/iOS)
session.AddShaderModel(CompileTarget.SLANG_METAL, "metal2.0");
var metalCode = entryPoint.Compile();
```

### Using Preprocessor Macros

Control compilation with preprocessor definitions:

```csharp
var session = new SessionBuilder()
    .AddPreprocessorMacro("ENABLE_LIGHTING", "1")
    .AddPreprocessorMacro("MAX_LIGHTS", "16")
    .AddPreprocessorMacro("QUALITY_LEVEL", "HIGH")
    .Create();
```

### Reflection and Introspection

Examine shader structure and parameters:

```csharp
var module = session.LoadModule("MyShader.slang");
var program = module.Program;

// Inspect entry points
foreach (var entryPoint in program.EntryPoints)
{
    Console.WriteLine($"Entry Point: {entryPoint.Name}");
    
    // Get parameter information
    var parameters = entryPoint.Parameters;
    foreach (var param in parameters)
    {
        Console.WriteLine($"  Parameter: {param.Name}, Type: {param.Type}");
    }
}

// Examine module layout
var layout = program.Layout;
Console.WriteLine($"Global parameters: {layout.GlobalParamsVarLayout?.TypeLayout?.Size ?? 0} bytes");
```

### Error Handling

Handle compilation errors gracefully:

```csharp
try
{
    var session = new SessionBuilder()
        .AddSearchPath(@"C:\MyShaders")
        .Create();
        
    var module = session.LoadModule("MyShader.slang");
    var compiledShader = module.Program.EntryPoints.First().Compile();
}
catch (SlangCompilationException ex)
{
    Console.WriteLine($"Compilation failed: {ex.Message}");
    foreach (var diagnostic in ex.Diagnostics)
    {
        Console.WriteLine($"  {diagnostic.Severity}: {diagnostic.Message}");
    }
}
```

## API Reference

### Core Classes

#### `SessionBuilder`
Factory for creating Slang compilation sessions with specific configurations.

**Key Methods:**
- `AddSearchPath(string path)` - Add directory to search for shader files
- `AddShaderModel(CompileTarget target, string profile)` - Set compilation target
- `AddPreprocessorMacro(string name, string value)` - Define preprocessor macro
- `AddCompilerOption(CompilerOptionName name, CompilerOptionValue value)` - Set compiler option
- `Create()` - Build the configured session

#### `Session`
Main interface for shader compilation operations.

**Key Methods:**
- `LoadModule(string path)` - Load a shader module from file
- `LoadModuleFromSource(string source, string path)` - Load module from string
- `Dispose()` - Clean up native resources

#### `Module`
Represents a compiled shader module.

**Key Properties:**
- `Program` - Access to the shader program and its entry points
- `Name` - Module name
- `Session` - Parent session

#### `ShaderProgram`
Contains compiled shader code and metadata.

**Key Properties:**
- `EntryPoints` - Collection of shader entry points
- `Layout` - Program layout information for resource binding

#### `EntryPoint`
Represents a shader entry point (vertex, pixel, compute, etc.).

**Key Methods:**
- `Compile()` - Generate target language code
- `GetCompilationResult()` - Get detailed compilation results

**Key Properties:**
- `Name` - Entry point function name
- `Stage` - Shader stage (vertex, pixel, compute, etc.)
- `Parameters` - Input parameters

## Best Practices

### 1. Use `using` Statements
Always dispose of sessions to free native resources:

```csharp
using var session = new SessionBuilder().Create();
// Session automatically disposed when leaving scope
```

### 2. Configure Search Paths Early
Set up search paths before loading modules:

```csharp
var session = new SessionBuilder()
    .AddSearchPath(@"C:\CommonShaders")
    .AddSearchPath(@"C:\ProjectShaders")
    .Create();
```

### 3. Handle Compilation Errors
Always wrap compilation in try-catch blocks:

```csharp
try
{
    var compiledShader = entryPoint.Compile();
}
catch (SlangCompilationException ex)
{
    // Handle compilation errors
}
```

### 4. Cache Compiled Results
Avoid recompiling the same shaders repeatedly:

```csharp
private static readonly Dictionary<string, string> _compiledShaders = new();

string GetCompiledShader(string shaderPath)
{
    if (_compiledShaders.TryGetValue(shaderPath, out var cached))
        return cached;
        
    var compiled = CompileShader(shaderPath);
    _compiledShaders[shaderPath] = compiled;
    return compiled;
}
```

## Common Scenarios

### Game Engine Integration

```csharp
public class ShaderCompiler
{
    private readonly Session _session;
    
    public ShaderCompiler(string shaderDirectory)
    {
        _session = new SessionBuilder()
            .AddSearchPath(shaderDirectory)
            .AddShaderModel(CompileTarget.SLANG_HLSL, "vs_5_0")
            .AddShaderModel(CompileTarget.SLANG_HLSL, "ps_5_0")
            .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
            .Create();
    }
    
    public CompiledShader CompileVertexShader(string path) =>
        CompileShaderForStage(path, "VS");
        
    public CompiledShader CompilePixelShader(string path) =>
        CompileShaderForStage(path, "PS");
        
    private CompiledShader CompileShaderForStage(string path, string entryPoint)
    {
        var module = _session.LoadModule(path);
        var entry = module.Program.EntryPoints.First(e => e.Name == entryPoint);
        return new CompiledShader(entry.Compile(), entry.Stage);
    }
}
```

### Compute Shader Pipeline

```csharp
public class ComputePipeline
{
    public void ProcessTexture(string shaderPath, string entryPoint)
    {
        using var session = new SessionBuilder()
            .AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0")
            .AddPreprocessorMacro("THREAD_GROUP_SIZE", "32")
            .Create();
            
        var module = session.LoadModule(shaderPath);
        var computeShader = module.Program.EntryPoints
            .First(e => e.Name == entryPoint);
            
        var hlslCode = computeShader.Compile();
        
        // Use compiled HLSL with your graphics API
        // (DirectX, OpenGL, Vulkan, etc.)
    }
}
```

## Troubleshooting

### Common Issues

1. **"Module not found" errors**
   - Ensure the shader file exists in the specified search paths
   - Use absolute paths or verify the working directory is correct
   - Check that the file extension matches (`.slang`)

2. **Compilation errors**
   - Wrap compilation calls in try-catch blocks to see detailed error messages
   - Check that the target shader model is supported for your graphics API
   - Verify preprocessor macros are defined correctly

3. **Runtime exceptions**
   - Ensure you're using `using` statements to properly dispose resources
   - Check that your .NET version is 6.0 or later
   - Verify the NuGet package was installed correctly

4. **Performance issues**
   - Cache compiled shaders to avoid recompiling the same code
   - Use background threads for compilation when possible
   - Consider using incremental compilation for development scenarios

## Supported Platforms

- **Windows x64** - Full support
- **Windows x86** - Unsuported due to missing support from the native slang api 
- **Windows ARM64** - Full support
- **Linux** - Planned for future release. Sponsorships can make this a priority.
- **macOS** - Planned for future release. Sponsorships can make this a priority.

## Version History

### 1.0.0 (Current)
- Initial release with full Slang compilation support
- Support for HLSL, GLSL, and Metal output targets
- Comprehensive reflection and introspection APIs
- Windows multi-architecture support (x86, x64, ARM64)

## Contributing

Found a bug or want to contribute? Visit our [GitHub repository](https://github.com/your-repo/Slang.Net) to:

- Report issues
- Submit pull requests
- Request new features
- View the source code

## License

This project is licensed under the same liscense as it's parent project: SPDX-License-Identifier: Apache-2.0 WITH LLVM-exception. See the LICENSE file for details.

## Additional Resources

- **Slang Documentation**: [https://shader-slang.org/](https://shader-slang.org/)
- **Slang GitHub**: [https://github.com/shader-slang/slang](https://github.com/shader-slang/slang)
- **Sample Projects**: [Available in the GitHub repository](https://github.com/your-repo/Slang.Net/tree/main/Samples)
