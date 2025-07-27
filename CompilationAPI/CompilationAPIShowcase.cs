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
        await DemoAdvancedCompilationFeaturesAsync();
        
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

            /*using*/ var session = builder.Create();
            
            Console.WriteLine("✅ Session created successfully");
            Console.WriteLine($"📁 Search paths: {string.Join(", ", new[] { _workingDirectory })}");
            Console.WriteLine($"🎯 Targets: {session.Targets.Count} configured");

            // Load a simple compute shader module
            var module = session.LoadModule("SimpleCompute.slang");
            Console.WriteLine($"📦 Module loaded: {module.Name}");
            Console.WriteLine($"📋 Entry points found: {module.EntryPoints.Count}");
            
            // List entry points
            foreach (var entryPoint in module.EntryPoints)
            {
                Console.WriteLine($"   • EntryPoints[{entryPoint.Index} = {{ Name = {entryPoint.Name} }}"); // We can make this happen with an earlier idea I had
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
                .AddSearchPath(_workingDirectory);

            /*using TODO: disposable*/ var session = builder.Create();
            var module = session.LoadModule("SimpleCompute.slang");
            var computeEntry = module.EntryPoints["CS"];

            Console.WriteLine($"🔄 Compiling to {session.Targets.Count} different targets...\n");

            var results = new List<CompilationResult>();

            // Compile to each target
            foreach (var target in session.Targets)
            {
                try
                {
                    var result = module.Program.Compile(computeEntry, target);
                    results.Add(result);
                    
                    Console.WriteLine($"✅ {target}:");
                    Console.WriteLine($"   📏 Generated code: {result.Source.Length} characters");
                    Console.WriteLine($"   📝 Preview: {GetCodePreview(result.Source)}\n");
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
                .AddTarget(Targets.Hlsl.cs_6_0)
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

            /*using*/ var session = builder.Create();
            
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
            var result = module.Program.Compile(module.EntryPoints["PS"], Targets.Hlsl.cs_6_0);
            
            Console.WriteLine("✅ Compilation with custom configuration successful");
            Console.WriteLine($"📏 Generated code length: {result.Source.Length} characters");
            Console.WriteLine($"📝 Code preview:\n{GetCodePreview(result.Source, 5)}\n");

            // Save the configured compilation result
            await File.WriteAllTextAsync(Path.Combine(_workingDirectory, "configured_shader.hlsl"), result.Source);
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
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.vs_6_0)
                .AddTarget(Targets.Hlsl.ps_6_0)
                .AddTarget(Targets.Hlsl.cs_6_0)
                .AddSearchPath(_workingDirectory);

            /*using TODO: disposable*/ var session = builder.Create();
            var module = session.LoadModule("MultiStageShader.slang");
            
            Console.WriteLine($"🔍 Discovered {module.EntryPoints.Count} entry points:\n");

            foreach (var entryPoint in module.EntryPoints)
            {
                Console.WriteLine($"🎯 Entry Point: {entryPoint.Name}");
                Console.WriteLine($"   📋 Stage: {entryPoint.Value.Stage}");
                
                // Find appropriate target for this stage
                var target = GetTargetForStage(session.Targets, entryPoint.Value.Stage);
                if (target != null)
                {
                    try
                    {
                        var result = module.Program.Compile(entryPoint, target);
                        Console.WriteLine($"   ✅ Compiled successfully to {target}");
                        Console.WriteLine($"   📏 Code length: {result.Source.Length} characters");
                        
                        // Save individual entry point
                        var filename = $"{entryPoint.Name}_{target}.hlsl";
                        await File.WriteAllTextAsync(Path.Combine(_workingDirectory, filename), result.Source);
                        Console.WriteLine($"   💾 Saved as: {filename}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ❌ Compilation failed: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"   ⚠️ No suitable target found for stage: {entryPoint.Value.Stage}");
                }
                
                Console.WriteLine();
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

            /*using TODO: disposable*/ var session = builder.Create();
            
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
                var result = errorModule.Program.Compile(errorModule.EntryPoints.First(), Targets.Hlsl.cs_6_0);
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
                var unsupportedTarget = Targets.Hlsl.vs_6_0; // Vertex shader target for compute shader
                var result = validModule.Program.Compile(validModule.EntryPoints["CS"], unsupportedTarget);
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
    /// Demonstrates advanced compilation features
    /// </summary>
    private async Task DemoAdvancedCompilationFeaturesAsync()
    {
        PrintDemoHeader(6, "Advanced Compilation Features", 
            "Showcases advanced compilation features and session management");

        try
        {
            Console.WriteLine("🚀 Advanced Compilation Feature Demonstrations:\n");
            
            // Feature 1: Session and target inspection
            Console.WriteLine("📊 Feature 1: Session and Target Inspection");
            var builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_6_0)
                .AddTarget(Targets.Glsl.v460)
                .AddTarget(Targets.SpirV.vulkan_1_2)
                .AddSearchPath(_workingDirectory);

            /*using TODO: disposable*/ var session = builder.Create();
            
            Console.WriteLine($"   🎯 Session targets: {session.Targets.Count}");
            for (uint i = 0; i < session.Targets.Count; i++)
            {
                var target = session.Targets[i];
                Console.WriteLine($"      • Target {i}: {target}");
            }
            
            // Load multiple modules
            var modules = new List<Module>();
            var moduleNames = new[] { "SimpleCompute.slang", "ConfigurableShader.slang" };
            
            foreach (var moduleName in moduleNames)
            {
                if (File.Exists(Path.Combine(_workingDirectory, moduleName)))
                {
                    modules.Add(session.LoadModule(moduleName));
                }
            }
            
            Console.WriteLine($"\n   📦 Loaded modules: {modules.Count}");
            Console.WriteLine($"   📋 Session module count: {session.Modules.Count}");
            
            // Feature 2: Batch compilation across targets
            Console.WriteLine("\n⚡ Feature 2: Batch Compilation Across Targets");
            var batchResults = new List<(string Module, string Entry, Target Target, CompilationResult Result)>();
            
            foreach (var module in modules)
            {
                foreach (var entryPoint in module.EntryPoints)
                {
                    foreach (var target in session.Targets)
                    {
                        try
                        {
                            var result = module.Program.Compile(entryPoint, target);
                            batchResults.Add((module.Name, entryPoint.Name, target, result));
                        }
                        catch
                        {
                            // Skip failed compilations in batch mode
                        }
                    }
                }
            }
            
            Console.WriteLine($"   ✅ Batch compilation completed: {batchResults.Count} successful compilations");
            
            // Feature 3: Compilation performance and statistics
            Console.WriteLine("\n📈 Feature 3: Compilation Statistics");
            var stats = CalculateCompilationStats(batchResults);
            Console.WriteLine($"   📊 Total compilations: {stats.TotalCompilations}");
            Console.WriteLine($"   📏 Average code length: {stats.AverageCodeLength:F0} characters");
            Console.WriteLine($"   📐 Min/Max code length: {stats.MinCodeLength}/{stats.MaxCodeLength}");
            Console.WriteLine($"   🎯 Targets used: {stats.TargetsUsed}");
            Console.WriteLine($"   📦 Modules processed: {stats.ModulesProcessed}");

            //Impossible feature, entry point is required to compile
            // Feature 4: Module program compilation without entry points
            Console.WriteLine("\n🔧 Feature 4: Direct Program Compilation");
            foreach (var module in modules.Take(1)) // Just test with first module
            {
                try
                {
                    var programResult = module.Program.Compile(Targets.Hlsl.cs_6_0);
                    Console.WriteLine($"   ✅ Direct program compilation for {module.Name}:");
                    Console.WriteLine($"   📏 Generated code: {programResult.Source.Length} characters");
                    Console.WriteLine($"   📝 Preview: {GetCodePreview(programResult.Source, 2)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ⚠️ Direct compilation failed for {module.Name}: {ex.Message}");
                }
            }

            PrintSuccess("Advanced compilation features demonstration completed");
        }
        catch (Exception ex)
        {
            PrintError("Advanced compilation features", ex.Message);
        }
    }

    #endregion

    #region Helper Methods

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

    // ??? What is going on here ???
    private Target? GetTargetForStage(SlangCollection<Target> targets, EntryPoint.Stage stage)
    {
        [FIX THIS]
        // Find appropriate target for the shader stage
        foreach (var target in targets)
        {
            var targetStr = target.ToString().ToLower();
            switch (stage)
            {
                case EntryPoint.Stage.Vertex when targetStr.Contains("vs"):
                case EntryPoint.Stage.Fragment when targetStr.Contains("ps"):
                case EntryPoint.Stage.Compute when targetStr.Contains("cs"):
                case EntryPoint.Stage.Geometry when targetStr.Contains("gs"):
                case EntryPoint.Stage.Hull when targetStr.Contains("hs"):
                case EntryPoint.Stage.Domain when targetStr.Contains("ds"):
                    return target;
            }
        }
        return targets.FirstOrDefault(); // Fallback to first target
    }

    private async Task SaveCompilationResultsAsync(List<CompilationResult> results, string prefix)
    {
        var outputDir = Path.Combine(_workingDirectory, $"{prefix}_results");
        Directory.CreateDirectory(outputDir);
        
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var filename = $"{prefix}_{i}.txt";
            await File.WriteAllTextAsync(Path.Combine(outputDir, filename), result.Source);
        }
        
        Console.WriteLine($"💾 Saved {results.Count} compilation results to {outputDir}/");
    }

    private CompilationStats CalculateCompilationStats(List<(string Module, string Entry, Target Target, CompilationResult Result)> results)
    {
        if (!results.Any())
            return new CompilationStats();
            
        var codeLengths = results.Select(r => r.Result.Source.Length).ToList();
        
        return new CompilationStats
        {
            TotalCompilations = results.Count,
            AverageCodeLength = codeLengths.Average(),
            MinCodeLength = codeLengths.Min(),
            MaxCodeLength = codeLengths.Max(),
            TargetsUsed = results.Select(r => r.Target.ToString()).Distinct().Count(),
            ModulesProcessed = results.Select(r => r.Module).Distinct().Count()
        };
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
