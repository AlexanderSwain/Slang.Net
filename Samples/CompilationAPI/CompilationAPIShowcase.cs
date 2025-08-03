using Slang.Sdk;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;

namespace CompilationAPI;

/// <summary>
/// Comprehensive showcase of the Slang.Net Compilation API features.
/// This sample demonstrates:
/// - Session creation and configuration
/// - Multi-target compilation (HLSL, GLSL, SPIR-V, Metal, WGSL)
/// - Entry point compilation
/// - Error handling and diagnostics
/// - Different shader types (vertex, pixel, compute, geometry)
/// - Preprocessor macros and compiler options
/// - Module loading and compilation
/// Note: Reflection API features are demonstrated in a separate ReflectionAPI sample project.
/// </summary>
public class CompilationAPIShowcase
{
    private readonly string _workingDirectory;

    public CompilationAPIShowcase()
    {
        _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// Runs all compilation API demonstrations
    /// </summary>
    public async Task RunAllDemosAsync()
    {
        PrintHeader("SLANG.NET COMPILATION API SHOWCASE");

        // Demo 1: Basic Session and Module Loading
        await DemoBasicSessionAndModuleAsync();

        // Demo 2: Multi-Target Compilation
        await DemoMultiTargetCompilationAsync();

        // Demo 3: Compiler Options and Preprocessor Macros
        await DemoCompilerOptionsAndMacrosAsync();

        // Demo 4: Entry Point Discovery and Compilation
        await DemoEntryPointDiscoveryAsync();

        // Demo 5: Error Handling and Diagnostics
        await DemoErrorHandlingAsync();

        // Demo 6: Advanced Compilation Features
        await DemoAdvancedModuleBuildingAsync();

        PrintSummary();
    }

    #region Demo Methods

    /// <summary>
    /// Demonstrates basic session creation and module loading
    /// </summary>
    private async Task DemoBasicSessionAndModuleAsync()
    {
        PrintDemoHeader(1, "Basic Session and Module Loading",
            "Shows how to create sessions, configure search paths, and load Slang modules");

        try
        {
            // Create a simple session with basic configuration
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_5_0)
                .AddSearchPath(_workingDirectory);

            var session = builder.Create();

            Console.WriteLine("✅ Session created successfully");
            Console.WriteLine($"📁 Search paths: {string.Join(", ", new[] { _workingDirectory })}");
            Console.WriteLine($"🎯 Targets: {session.Targets.Count} configured");

            // Load a simple compute shader module
            var module = session.LoadModule("SimpleCompute.slang");
            var program = module.Program;
            Console.WriteLine($"📦 Module loaded: {module.Name}");
            Console.WriteLine($"📋 Entry points found: {program.Targets[Targets.Hlsl.cs_5_0].EntryPoints.Count}");

            // List entry points (note: stages require reflection, which needs a target)
            foreach (var entryPoint in program.Targets[Targets.Hlsl.cs_5_0].EntryPoints)
            {
                Console.WriteLine($"   • {entryPoint.Name}");
            }

            PrintSuccess("Basic session and module loading completed");
        }
        catch (Exception ex)
        {
            PrintError("Basic session demo", ex.Message);
        }
    }

    /// <summary>
    /// Demonstrates compilation to multiple target languages
    /// </summary>
    private async Task DemoMultiTargetCompilationAsync()
    {
        PrintDemoHeader(2, "Multi-Target Compilation",
            "Compiles the same shader to multiple target languages (HLSL, GLSL, SPIR-V, Metal, WGSL)");

        try
        {
            // Create session with multiple targets
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_6_0)          // DirectX 12
                .AddTarget(Targets.Glsl.v460)            // OpenGL 4.6
                .AddTarget(Targets.SpirV.vulkan_1_2)     // Vulkan 1.2
                .AddTarget(Targets.Metal.v2_4)           // Metal 2.4
                .AddTarget(Targets.Wgsl.v1_0)            // WebGPU
                .AddCompilerOption(CompilerOption.Name.EmitSpirvDirectly, new(CompilerOption.Value.Kind.Int, 1, 0, null, null))
                .AddSearchPath(_workingDirectory);

            var session = builder.Create();
            var module = session.LoadModule("SimpleCompute.slang");
            var program = module.Program;

            Console.WriteLine($"🔄 Compiling to {session.Targets.Count} different targets...\n");

            var results = new List<CompilationResult>();

            // Compile to each target
            foreach (var target in session.Targets)
            {
                try
                {
                    var result = program.Targets[target].Compile();
                    results.Add(result);

                    Console.WriteLine($"✅ {target}:");
                    if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
                    {
                        Console.WriteLine($"   📏 Generated code: {result.SourceCode!.Length} characters");
                        Console.WriteLine($"   📝 Preview: {GetCodePreview(result.SourceCode)}\n");
                    }
                    else if (result.CompileOutputType == Target.CompileOutputType.ByteCode)
                    {
                        Console.WriteLine($"   📦 Binary size: {result.ByteCode!.Length} bytes");
                        Console.WriteLine($"   📝 Binary preview: {BitConverter.ToString(result.ByteCode.Take(16).ToArray())}\n");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ {target}: {ex.Message}\n");
                }
            }

            // Save results to files
            await SaveCompilationResultsAsync(results, "multi_target");

            PrintSuccess($"Multi-target compilation completed ({results.Count} successful)");
        }
        catch (Exception ex)
        {
            PrintError("Multi-target compilation", ex.Message);
        }
    }

    /// <summary>
    /// Demonstrates compiler options and preprocessor macros
    /// </summary>
    private async Task DemoCompilerOptionsAndMacrosAsync()
    {
        PrintDemoHeader(3, "Compiler Options and Preprocessor Macros",
            "Shows how to configure compilation with various options and preprocessor definitions");

        try
        {
            // Create session with various compiler options and macros
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.vs_6_0)
                .AddSearchPath(_workingDirectory)
                // Compiler options
                .AddCompilerOption(CompilerOption.Name.Optimization,
                    new CompilerOption.Value(CompilerOption.Value.Kind.Int, 2, 0, null, null)) // O2 optimization
                .AddCompilerOption(CompilerOption.Name.WarningsAsErrors,
                    new CompilerOption.Value(CompilerOption.Value.Kind.String, 0, 0, "all", null))
                .AddCompilerOption(CompilerOption.Name.MatrixLayoutColumn,
                    new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null))
                // Preprocessor macros
                .AddPreprocessorMacro("ENABLE_LIGHTING", "1")
                .AddPreprocessorMacro("MAX_LIGHTS", "8")
                .AddPreprocessorMacro("LIGHTING_MODEL", "PHONG")
                .AddPreprocessorMacro("DEBUG_MODE", "0");

            var session = builder.Create();

            Console.WriteLine("⚙️ Compilation Configuration:");
            Console.WriteLine("   🎛️ Optimization Level: O2");
            Console.WriteLine("   ⚠️ Warnings as Errors: Enabled");
            Console.WriteLine("   📐 Matrix Layout: Column-major");
            Console.WriteLine("   🔧 Preprocessor Macros:");
            Console.WriteLine("      • ENABLE_LIGHTING = 1");
            Console.WriteLine("      • MAX_LIGHTS = 8");
            Console.WriteLine("      • LIGHTING_MODEL = PHONG");
            Console.WriteLine("      • DEBUG_MODE = 0\n");

            var module = session.LoadModule("ConfigurableShader.slang");
            var program = module.Program;
            var result = program.Targets[Targets.Hlsl.vs_6_0].EntryPoints["VS"].Compile();

            Console.WriteLine("✅ Compilation with custom configuration successful");

            if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
            {
                Console.WriteLine($"   📏 Generated code: {result.SourceCode!.Length} characters");
                Console.WriteLine($"   📝 Preview: {GetCodePreview(result.SourceCode)}\n");
            }
            else if (result.CompileOutputType == Target.CompileOutputType.ByteCode)
            {
                Console.WriteLine($"   📦 Binary size: {result.ByteCode!.Length} bytes");
                Console.WriteLine($"   📝 Binary preview: {BitConverter.ToString(result.ByteCode.Take(16).ToArray())}\n");
            }

            // Save the configured compilation result
            if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
                await File.WriteAllTextAsync(Path.Combine(_workingDirectory, "configured_shader.hlsl"), result.SourceCode);
            else if (result.CompileOutputType == Target.CompileOutputType.ByteCode)
                await File.WriteAllBytesAsync(Path.Combine(_workingDirectory, "configured_shader.spv"), result.ByteCode!);

            Console.WriteLine("💾 Configured shader saved to configured_shader.hlsl");

            PrintSuccess("Compiler options and macros demo completed");
        }
        catch (Exception ex)
        {
            PrintError("Compiler options demo", ex.Message);
        }
    }

    /// <summary>
    /// Demonstrates entry point discovery and individual compilation
    /// </summary>
    private async Task DemoEntryPointDiscoveryAsync()
    {
        PrintDemoHeader(4, "Entry Point Discovery and Compilation",
            "Discovers all entry points in a module and compiles each individually");

        try
        {
            var session = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_6_0)
                .AddSearchPath(_workingDirectory)
                .Create();

            var module = session.LoadModule("MultiComputeShader.slang");
            var program = module.Program;

            Console.WriteLine($"🔍 Discovered {program.Targets.Count} targets:\n");

            // To get stage information, we need to use reflection with a target
            // Let's try each target to see what entry points are available
            foreach (var target in session.Targets)
            {
                try
                {
                    var programTarget = program.Targets[target];
                    Console.WriteLine($"🎯 Target: {target}");
                    Console.WriteLine($"   📋 Entry points found: {programTarget.EntryPoints.Count}");

                    foreach (var entryPoint in programTarget.EntryPoints)
                    {
                        Console.WriteLine($"   • {entryPoint.Name} (Stage: {entryPoint.Stage})");

                        // Try to find the corresponding entry point in the module
                        try
                        {
                            var result = entryPoint.Compile();
                            Console.WriteLine($"     ✅ Compiled successfully");
                            if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
                            {
                                Console.WriteLine($"   📏 Generated code: {result.SourceCode!.Length} characters");
                                Console.WriteLine($"   📝 Preview: {GetCodePreview(result.SourceCode)}\n");
                            }
                            else if (result.CompileOutputType == Target.CompileOutputType.ByteCode)
                            {
                                Console.WriteLine($"   📦 Binary size: {result.ByteCode!.Length} bytes");
                                Console.WriteLine($"   📝 Binary preview: {BitConverter.ToString(result.ByteCode.Take(16).ToArray())}\n");
                            }

                            // Save individual entry point
                            var filename = $"{entryPoint.Name}_{target.ToString().Replace(":", "_")}.hlsl";
                            if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
                                await File.WriteAllTextAsync(Path.Combine(_workingDirectory, filename), result.SourceCode!);
                            Console.WriteLine($"     💾 Saved as: {filename}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"     ❌ Compilation failed: {ex.Message}");
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Reflection failed for target {target}: {ex.Message}\n");
                }
            }

            PrintSuccess("Entry point discovery and compilation completed");
        }
        catch (Exception ex)
        {
            PrintError("Entry point discovery", ex.Message);
        }
    }

    /// <summary>
    /// Demonstrates error handling and diagnostic information
    /// </summary>
    private async Task DemoErrorHandlingAsync()
    {
        PrintDemoHeader(5, "Error Handling and Diagnostics",
            "Shows how to handle compilation errors and extract diagnostic information");

        try
        {
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_6_0)
                .AddSearchPath(_workingDirectory);

            var session = builder.Create();

            Console.WriteLine("🧪 Testing error handling scenarios:\n");

            // Test 1: Missing file
            Console.WriteLine("📂 Test 1: Loading non-existent module");
            try
            {
                var missingModule = session.LoadModule("NonExistentShader.slang");
                Console.WriteLine("   ❌ Expected error did not occur");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"   ✅ Correctly caught FileNotFoundException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ Unexpected exception type: {ex.GetType().Name}: {ex.Message}");
            }

            // Test 2: Shader with syntax errors
            Console.WriteLine("\n🔧 Test 2: Compiling shader with syntax errors");
            try
            {
                var errorModule = session.LoadModule("ErrorShader.slang");
                var target = errorModule.Program.Targets[Targets.Hlsl.cs_6_0];
                var result = target.EntryPoints.First().Compile();
                Console.WriteLine("   ❌ Expected compilation error did not occur");
            }
            catch (SlangException ex)
            {
                Console.WriteLine($"   ✅ Correctly caught SlangException:");
                Console.WriteLine($"   📋 Result: {ex.Result}");
                Console.WriteLine($"   💬 Message: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ⚠️ Unexpected exception: {ex.GetType().Name}: {ex.Message}");
            }

            // Test 3: Invalid target configuration
            Console.WriteLine("\n🎯 Test 3: Using unsupported target combination");
            try
            {
                var validModule = session.LoadModule("SimpleCompute.slang");
                // Try to compile compute shader with vertex shader target (should fail)
                var unsupportedTarget = validModule.Program.Targets[Targets.Hlsl.vs_6_0]; // Vertex shader target for compute shader
                var result = unsupportedTarget.EntryPoints["CS"].Compile();
                Console.WriteLine("   ❌ Expected target mismatch error did not occur");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✅ Correctly caught target mismatch: {ex.Message}");
            }

            PrintSuccess("Error handling and diagnostics demo completed");
        }
        catch (Exception ex)
        {
            PrintError("Error handling demo", ex.Message);
        }
    }

    /// <summary>
    /// Demonstrates advanced module building with compile requests
    /// </summary>
    private async Task DemoAdvancedModuleBuildingAsync()
    {
        PrintDemoHeader(6, "Advanced Module Building with Compile Requests",
            "Showcases Module.Builder abstraction over CompileRequest for flexible shader compilation");

        try
        {
            Console.WriteLine("🚀 Module.Builder Demonstrations:\n");

            var session = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_6_0)
                .AddSearchPath(_workingDirectory)
                .Create();

            // Demo 1: File-based compilation with Module.Builder
            Console.WriteLine("📦 Building module from MultiComputeShader.slang file");
            if (File.Exists(Path.Combine(_workingDirectory, "MultiComputeShader.slang")))
            {
                var fileBuilder = new Module.Builder(session)
                    .AddCodeGenTarget(Target.CompileTarget.Hlsl)
                    .AddPreprocessorDefine("MULTI_COMPUTE", "1")
                    .AddTranslationUnit(SourceLanguage.Slang, "MultiComputeShader", out int unitIndex)
                    .AddTranslationUnitSourceFile(unitIndex, Path.Combine(_workingDirectory, "MultiComputeShader.slang"));
                
                // Add some compute entry points
                fileBuilder.AddEntryPoint(0, "GaussianBlur", Stage.Compute);
                fileBuilder.AddEntryPoint(0, "GenerateNoise", Stage.Compute);

                var module = fileBuilder.Create();
                Console.WriteLine($"   ✅ Created module '{module.Name}' with {module.Program.Targets.First().EntryPoints.Count} entry points");
            }

            // Demo 2: Inline source compilation with fluent API
            Console.WriteLine("\n🔧 Building module from inline source with fluent API");
            string computeSource = @"
RWTexture2D<float4> outputTexture;

[shader(""compute"")]
[numthreads(8, 8, 1)]
void SimpleBlur(uint3 id : SV_DispatchThreadID)
{
    outputTexture[id.xy] = float4(0.5, 0.5, 0.5, 1.0);
}";

            var inlineModule = new Module.Builder(session)
                .AddCodeGenTarget(Target.CompileTarget.Hlsl)
                .AddPreprocessorDefine("INLINE_BUILD", "1")
                .AddTranslationUnit(SourceLanguage.Slang, "inline", out var index)
                .AddTranslationUnitSourceString(
                    translationUnitIndex: (int)session.Modules.Count, // Simple indexing
                    path: "inline.slang",
                    source: computeSource)
                .Create();

            Console.WriteLine($"   ✅ Created inline module with fluent API");

            // Demo 3: Multiple configurations
            Console.WriteLine("\n⚙️ Testing different build configurations");
            var configs = new[] { ("DEBUG", "1"), ("RELEASE", "1"), ("PROFILING", "1") };

            foreach (var (key, value) in configs)
            {
                try
                {
                    var configModule = new Module.Builder(session)
                        .AddCodeGenTarget(Target.CompileTarget.Hlsl)
                        .AddPreprocessorDefine(key, value)
                        .AddTranslationUnit(SourceLanguage.Slang, key, out var configIndex)
                        .Create();

                    Console.WriteLine($"   ✅ {key} configuration: Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ❌ {key} configuration: {ex.Message}");
                }
            }

            PrintSuccess("Advanced module building demonstration completed");
        }
        catch (Exception ex)
        {
            PrintError("Advanced module building", ex.Message);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Helper method to get entry point stage information.
    /// </summary>
    private async Task<List<(string Name, string Stage, Target Target)>> GetEntryPointStagesAsync(Module module, IEnumerable<Target> targets)
    {
        var entryPointInfo = new List<(string Name, string Stage, Target Target)>();

        foreach (var target in targets)
        {
            var programTarget = module.Program.Targets[target];
            try
            {
                foreach (var entryPoint in programTarget.EntryPoints)
                {
                    entryPointInfo.Add((entryPoint.Name, entryPoint.Stage.ToString(), target));
                }
            }
            catch
            {
                // Some targets may not be compatible with all modules
                continue;
            }
        }

        return entryPointInfo.GroupBy(x => x.Name).Select(g => g.First()).ToList();
    }

    private void PrintHeader(string title)
    {
        Console.WriteLine($"╔══════════════════════════════════════════════╗");
        Console.WriteLine($"║ {title,-44} ║");
        Console.WriteLine($"╚══════════════════════════════════════════════╝\n");
    }

    private void PrintDemoHeader(int number, string title, string description)
    {
        Console.WriteLine($"\n🎯 Demo {number}: {title}");
        Console.WriteLine(new string('─', Math.Max(title.Length + 10, 50)));
        Console.WriteLine($"📋 {description}\n");
    }

    private void PrintSuccess(string message)
    {
        Console.WriteLine($"\n✅ {message}\n");
    }

    private void PrintError(string context, string message)
    {
        Console.WriteLine($"\n❌ Error in {context}: {message}\n");
    }

    private void PrintSummary()
    {
        Console.WriteLine("📊 COMPILATION API SHOWCASE SUMMARY");
        Console.WriteLine("===================================");
        Console.WriteLine("✅ Demo 1: Basic Session and Module Loading");
        Console.WriteLine("✅ Demo 2: Multi-Target Compilation");
        Console.WriteLine("✅ Demo 3: Compiler Options and Preprocessor Macros");
        Console.WriteLine("✅ Demo 4: Entry Point Discovery and Compilation");
        Console.WriteLine("✅ Demo 5: Error Handling and Diagnostics");
        Console.WriteLine("✅ Demo 6: Advanced Compilation Features");
        Console.WriteLine("\n🎉 All compilation demonstrations completed successfully!");

        Console.WriteLine("\n📁 Generated Files:");
        var outputFiles = new[]
        {
            "configured_shader.hlsl",
            "multi_target_results/",
            "Various compiled shader outputs"
        };

        foreach (var file in outputFiles)
        {
            Console.WriteLine($"   📄 {file}");
        }

        Console.WriteLine("\n💡 Note: Reflection API features are demonstrated in a separate ReflectionAPI sample project.");
    }

    private string GetCodePreview(string code, int lines = 3)
    {
        if (string.IsNullOrEmpty(code)) return "[Empty]";

        var codeLines = code.Split('\n');
        var preview = string.Join('\n', codeLines.Take(lines));

        if (codeLines.Length > lines)
        {
            preview += $"\n... ({codeLines.Length - lines} more lines)";
        }

        return preview;
    }

    private async Task SaveCompilationResultsAsync(List<CompilationResult> results, string prefix)
    {
        var outputDir = Path.Combine(_workingDirectory, $"{prefix}_results");
        Directory.CreateDirectory(outputDir);

        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var filename = $"{prefix}_{i}.txt";
            if (result.CompileOutputType == Target.CompileOutputType.SourceCode)
                await File.WriteAllTextAsync(Path.Combine(_workingDirectory, "configured_shader.hlsl"), result.SourceCode);
            else if (result.CompileOutputType == Target.CompileOutputType.ByteCode)
                await File.WriteAllBytesAsync(Path.Combine(_workingDirectory, "configured_shader.spv"), result.ByteCode!);
        }

        Console.WriteLine($"💾 Saved {results.Count} compilation results to {outputDir}/");
    }

    #endregion
}

/// <summary>
/// Statistics about compilation results
/// </summary>
public record CompilationStats
{
    public int TotalCompilations { get; init; }
    public double AverageCodeLength { get; init; }
    public int MinCodeLength { get; init; }
    public int MaxCodeLength { get; init; }
    public int TargetsUsed { get; init; }
    public int ModulesProcessed { get; init; }
}
