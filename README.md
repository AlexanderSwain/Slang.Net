# Slang.Sdk

A comprehensive .NET wrapper for the Slang Shader Language Sdk, providing seamless integration of shader compilation and reflection capabilities into .NET applications.

Slang.Sdk Includes the following:
-**Compilation API**
-**Reflection API**
-**NEW: 'slangc' CLI API**

> **Update:** Release roadmap is now live (please see below). This provides info on releases until v1.0.0.

> **Note:** Slang.Sdk is currently in early development and will remain in alpha until v1.0.0. For experimental use only. Do not include this package for production builds.

> **Disclaimer:** We are the developers and maintainers of this C# wrapper library only, not of the Slang shader language itself. Slang is developed and maintained by NVIDIA. This project provides .NET bindings to make Slang accessible to C# developers.

## Supported Platforms

- **Windows x64** - Full support
- **Windows x86** - Unsuported due to missing support from the native slang api 
- **Windows ARM64** - Full support
- **Linux** - Planned for future release. Sponsorships can make this a priority.
- **macOS** - Planned for future release. Sponsorships can make this a priority.

## Release Roadmap:
We will use the latest `Slang api version: 2025.13.2` as the underlying native version until after v1.0.0 release. Expect at least one release at the end of every month after v0.5.0:

- **Slang.Sdk v0.0.1** (6/27/2025 Release): Proof of Concept. Limited testing. Has basic functionality but came with many known issues such as bugs and memory leaks. Windows (x64, ARM64).
- **Slang.Sdk v0.5.0** (8/06/2025 Release): [Massive Update] Most features are implemented. Most bugs and memory leaks fixed. Basic testing. Windows (x64, ARM64).
- **Slang.Sdk v0.9.0** (9/06/2025 Release): All features are implemented. This release may not exist if all test cases are already passing. Windows (x64, ARM64).
- **Slang.Sdk v1.0.0** (early October Release): All native slang features implemented and elegantly abstracted. No known bugs or memory leaks. Intensively tested. We're aiming for Linux, macOS compatibility for this release. Please help us reach this goal.
- **Slang.Sdk v1.X.X** (November Beyond Release): Updates to latest slang version. Fix breaking changes. Fix reported issues. Improving Abstractions.

> **Note:** The release roadmap may change in the future.

**Sponsoring Development:**

Slang.Sdk is an open-source project maintained by volunteers. Your sponsorships directly support:

- **Cross-platform support** - Help us bring Slang.Net to Linux and macOS
- **New features** - Fund development of additional functionality
- **Elegant .NET Abstractions** - Experience Slang through idiomatic .NET APIs
- **Maintenance** - Support regular updates and bug fixes
- **Documentation** - Improve guides and examples

If Slang.Sdk adds value to your project or organization, please consider sponsoring our work. Even small contributions make a significant difference.

[Become a Sponsor](#) <!-- Replace with your sponsorship link -->

❤️Make sure to also support the slang team directly so that they can continue to improve the underlying api: https://github.com/shader-slang/slang

## Why Slang.Sdk?

Slang is purpose-built to bring **modularity**, **scalability**, and **developer-friendliness** to modern GPU programming. Whether you're building apps, games, or ML pipelines, here's why it stands out:

### 🚀 Zero Configuration: 
- Install via NuGet and start using immediately

### 🎯 Type-Safe APIs: 
- Strongly-typed C# interfaces for all Slang functionality

### 🌐 Multi-Platform: 
- Works on Windows x64, and ARM64 architectures

### ✨ Modular Shader Development
- Write reusable shader libraries with clean interfaces
- Compose shaders like software modules—no more monolithic .hlsl files!

### ⚡ Performance Without Compromise
- Targets DirectX, Vulkan, CUDA, and more via cross-compilation
- Optimized for real-time rendering and compute workloads

### 🧠 Ideal for ML and Custom Pipelines
- Cleanly integrate with custom backends or accelerators
- Write compute shaders that scale with data and hardware

### 🔧 Designed for Tooling and Extensibility
- Structured reflection for debugging, UI integration, and dynamic dispatch
- Supports custom code generation and build-time tools

### 🌐 Open Source and Industry-Ready
- Actively developed with real-world use cases in mind
- Bridges the gap between high-level app logic and GPU execution

Slang doesn’t just let you write shaders—it empowers you to design **systems**. Clean, composable, and powerful. It's the shader language for developers who think architecturally.


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
Install-Package Slang.Sdk
```

Or via .NET CLI:

```bash
dotnet add package Slang.Sdk
```

Or add to your project file:

```xml
<PackageReference Include="Slang.Sdk" Version="0.5.0" />
```

## slangc CLI Invocation
If you just want to call slangc CLI tooling directly from C#, we got you covered:

### Set Working Directory
- Default directory is:
    ```csharp
    AppDomain.CurrentDomain.BaseDirectory;
    ```

- It can be changed like this:
    ```csharp
    CLI.WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\Shaders");
    ```

### Compile to StdOut
```csharp
using Slang.Sdk;

// Call slangc from C#
var result = CLI.slangc(
    target: "hlsl",
    profile: "cs_5_0",
    entry: "CS",
    inputFiles: ["AverageColor.slang"]);

// Prints the compiled shader source
Console.WriteLine(result.StdOut);
```

### Compile to output file
```csharp
using Slang.Sdk;

// Output file will be in the WorkingDirectory
var result = CLI.slangc(
    target: "hlsl",
    profile: "cs_5_0",
    entry: "CS",
    outputPath: "output1.hlsl",
    inputFiles: ["AverageColor.slang"]);
```

### Too many parameters, use a builder instead
```csharp
SlangC_Options.Builder paramsBuilder = new SlangC_Options.Builder()
    .SetTarget("hlsl")
    .SetProfile("cs_5_0")
    .SetEntry("CS")
    .AddIncludePaths(Path.Combine(CLI.WorkingDirectory, "AverageColor.slang"));
    
var cliResult = CLI.slangc(paramsBuilder.Build());
```

### Using experimental parameters not yet supported by SlangC_Options
- Pass raw parameters
    ```csharp
    string args = "-target spirv -profile sm_6_6 -stage compute -entry main -O 3 -g -source-embed-style auto -source-embed-language hlsl -source-embed-name MyShader -conformance myType:myInterface=myID -- 'AverageColor.slang'";

    // -source-embed-style -source-embed-name flags are not currently suppored SlangC_Options, but can still be used as a raw string
    var result = CLI.slangc(args);
    ```

## Compilation API

### Finding EntryPoints and compiling them
```csharp
using Slang.Sdk;
using Slang.Sdk.Interop;

// Create a session - the main entry point for Slang operations also supports macros and compile options
Session session = new Session.Builder()
    .AddTarget(Targets.Hlsl.cs_5_0)
    .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}\Shaders\")
    .Create();

// Load the module from the specified file, other overloads exists to expose most features from the native slang api
Module module = session.LoadModule("AverageColor.slang");

// Find an entry point by name
EntryPoint entryPoint = module.EntryPoints["CS"];

// Compiles that entry point for the specified target
var result = entryPoint.Compile(Targets.Hlsl.cs_5_0);

// Gets the source code from the compilation result
Console.WriteLine(result.Source);
```
```csharp
// Or you can also compile with the module's program object
var result2 = module.Program.Compile(entryPoint, Targets.Hlsl.cs_5_0);
```

### Slang Collections
- Native slang interface methods has been abstracted into Slang Collections
- Slang Collections extend from IEnumerable and behaves as you'd expect in .Net
    ```csharp
    foreach (var entryPoint in module.EntryPoints)
    {
        Console.WriteLine($"Index: {entryPoint.Index}, Name: {entryPoint.Name}");
    }
    ```
    ```csharp
    module.EntryPoints.Where(entryPoint => entryPoint.Name == "CS").FirstOrDefault();
    ```
- SlangDictionaries expose Dictionary-like abstractions
    - Indexer
    ```csharp
    EntryPoint entryPoint = module.EntryPoints["CS"];
    ```
    - TryGetValue
    ```csharp
    if (module.EntryPoints.TryGetValue("CS", out EntryPoint? entryPoint))
    {
        Console.WriteLine($"Found entry point: {entryPoint?.Name} at index {entryPoint?.Index}");
    }
    else
    {
        Console.WriteLine("Entry point 'CS' not found.");
        return;
    }
    ```
- SlangNamedCollection combines the best of both worlds from Slang Collections and Dictionaries

### Reflection API

- Need to programmatically bind shader parameters? No problem, just use the Reflection API.

> **Note** As of v0.5.0, all slang reflection types (with the exception of DeclReflection and a few other minor things) has been completed and elegantly abstracted

- Reflection Example

    ```csharp
    using Slang.Sdk;
    using Slang.Sdk.Interop;

    // Same as above to load the module

    // Get the shader reflection for the specified target
    ShaderReflection reflection = module.Program.GetReflection(Targets.Hlsl.cs_5_0);

    // Get the shader reflection for the specified target
    var parameters = reflection.Parameters;

    // Access the shader reflection parameters
    var shaderInputParameters = reflection.Parameters;
    foreach (var parameter in shaderInputParameters)
    {
        Console.WriteLine($"Parameter Name: {parameter.Name}");
        Console.WriteLine($"Type: {parameter.Type.Name}");
        Console.WriteLine($"BindingIndex: {parameter.BindingIndex}, BindingSpace: {parameter.BindingSpace}");
    }
    ```

### Using Compiler Options

Control compiler options:

```csharp
var session = new Session.Builder()
    .AddCompilerOption(CompilerOption.Name.WarningsAsErrors, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 0, 0, "all", null))
    .AddCompilerOption(CompilerOption.Name.Obfuscate, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null))
    .AddTarget(Targets.Hlsl.cs_5_0)
    .Create();
```

### Using Preprocessor Macros

Control preprocessor definitions:

```csharp
var session = new Session.Builder()
    .AddPreprocessorMacro("ENABLE_LIGHTING", "1")
    .AddPreprocessorMacro("MAX_LIGHTS", "16")
    .AddPreprocessorMacro("QUALITY_LEVEL", "HIGH")
    .AddTarget(Targets.Hlsl.cs_5_0)
    .Create();
```

### Complete Examples
// Samples links and descriptions here

## Contributing

As an early-stage project, we welcome community contributions to help Slang.Net grow. Found a bug or want to contribute? Visit our [GitHub repository](https://github.com/Aqqorn/Slang.Sdk) to:

- Report issues
- Submit pull requests
- Request new features
- View the source code
- Join discussions about future development

We especially welcome contributions in these areas:
- Cross-platform support
- Performance improvements
- Documentation and examples
- Test coverage

## License

This project is licensed under the same liscense as the project it is wrapping: Apache-2.0 WITH LLVM-exception. See the LICENSE file for details.

## Additional Resources

- **Slang Documentation**: [https://shader-slang.org/](https://shader-slang.org/)
- **Slang GitHub**: [https://github.com/shader-slang/slang](https://github.com/shader-slang/slang)
- **Sample Projects**: [Available in the GitHub repository](https://github.com/Aqqorn/Slang.Sdk/tree/main/Samples)
