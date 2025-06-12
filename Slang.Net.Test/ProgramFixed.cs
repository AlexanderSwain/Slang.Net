//using System;
//using Slang;
//using System.IO;
//using System.Linq;
//using System.Collections.Generic;

///// <summary>
///// CORRECTED VERSION: This version addresses the "Failed to create the Slang module" issues
///// Key fixes:
///// 1. Added AddCompileTarget() call
///// 2. Simplified session configuration
///// 3. Better error handling
///// 4. Proper search path configuration
///// </summary>
//public class ProgramFixed
//{
//    public static void Main(string[] args)
//    {
//        Console.WriteLine("=== Slang.Net Multiple Pipeline Compilation Demo (FIXED) ===\n");
        
//        // Approach 1: Single session for multiple modules (RECOMMENDED)
//        Console.WriteLine("1. Single Session Approach (Recommended) - FIXED VERSION");
//        DemonstratesSingleSessionMultipleModulesFixed();
        
//        Console.WriteLine("\n" + new string('=', 60) + "\n");
        
//        // Approach 2: Multiple sessions for different shader types
//        Console.WriteLine("2. Multiple Sessions Approach - FIXED VERSION");
//        DemonstrateMultipleSessionsFixed();
//    }

//    /// <summary>
//    /// FIXED VERSION: Demonstrates using a single session to manage multiple modules with different pipelines.
//    /// Key fixes applied to resolve "Failed to create the Slang module" error.
//    /// </summary>
//    private static void DemonstratesSingleSessionMultipleModulesFixed()
//    {
//        Console.WriteLine("Creating a single session that supports all shader model targets...");
        
//        try
//        {
//            // STEP 1: Build session configuration properly
//            Console.WriteLine("  Building session configuration...");
            
//            SessionBuilder builder = new SessionBuilder();
            
//            // FIX 1: Add compile target FIRST - this was missing!
//            Console.WriteLine("  Adding compile target...");
//            builder.AddCompileTarget(CompileTarget.SLANG_HLSL);
            
//            // FIX 2: Add search paths before other configuration
//            var shaderPath = Path.GetFullPath("./Shaders/");
//            Console.WriteLine($"  Adding search path: {shaderPath}");
//            builder.AddSearchPath(shaderPath);
            
//            // FIX 3: Add shader models (simplified)
//            Console.WriteLine("  Adding shader models...");
//            builder.AddShaderModel(CompileTarget.SLANG_HLSL, "vs_5_0");
//            builder.AddShaderModel(CompileTarget.SLANG_HLSL, "ps_5_0");
//            builder.AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0");
            
//            // FIX 4: Remove potentially problematic compiler options for now
//            // Note: These can be added back once basic loading works
//            // builder.AddCompilerOption(CompilerOptionName.WarningsAsErrors, ...);
//            // builder.AddPreprocessorMacro("ENABLE_ADVANCED_FEATURES", "1");
            
//            Console.WriteLine("  Creating session...");
//            Session session = builder.Create();
//            Console.WriteLine("  ✓ Session created successfully!");
        
//            // Dictionary to store compiled programs for different pipelines
//            Dictionary<string, ShaderProgram> programs = new Dictionary<string, ShaderProgram>();
        
//            // STEP 2: Load modules with proper error handling
//            LoadModuleWithErrorHandling(session, "Graphics", programs);
//            LoadModuleWithErrorHandling(session, "PostProcess", programs);
//            LoadModuleWithErrorHandling(session, "Particles", programs);
            
//            // STEP 3: Demonstrate pipeline management
//            if (programs.Count > 0)
//            {
//                Console.WriteLine("\n  ✅ SUCCESS: Single session successfully managed multiple pipeline types!");
//                DemonstrateCompiledPipelineUsage(programs);
//            }
//            else
//            {
//                Console.WriteLine("\n  ⚠️ No modules loaded successfully. Check shader files and paths.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"    ❌ Session creation error: {ex.Message}");
//            Console.WriteLine($"    Stack trace: {ex.StackTrace}");
//        }
//    }
    
//    /// <summary>
//    /// Helper method to load a module with detailed error reporting
//    /// </summary>
//    private static void LoadModuleWithErrorHandling(Session session, string moduleName, Dictionary<string, ShaderProgram> programs)
//    {
//        try
//        {
//            Console.WriteLine($"\n  Loading {moduleName} module...");
            
//            // Check if file exists first
//            var shaderFile = Path.Combine("./Shaders/", $"{moduleName}.slang");
//            if (!File.Exists(shaderFile))
//            {
//                Console.WriteLine($"    ❌ Shader file not found: {shaderFile}");
//                return;
//            }
            
//            Console.WriteLine($"    Found shader file: {shaderFile}");
            
//            Module module = session.LoadModule(moduleName);
//            ShaderProgram program = module.Program;
//            programs[moduleName] = program;
            
//            Console.WriteLine($"    ✓ Module loaded successfully!");
//            Console.WriteLine($"    Entry points found: {program.EntryPoints.Count()}");
            
//            foreach (var ep in program.EntryPoints)
//            {
//                Console.WriteLine($"      - {ep.Name} (stage: {ep.Stage})");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"    ❌ Error loading {moduleName}: {ex.Message}");
//            // Continue loading other modules
//        }
//    }
    
//    /// <summary>
//    /// Demonstrate how to use compiled pipelines for different purposes
//    /// </summary>
//    private static void DemonstrateCompiledPipelineUsage(Dictionary<string, ShaderProgram> programs)
//    {
//        Console.WriteLine("\n  🎯 Pipeline Usage Demonstration:");
//        Console.WriteLine("     Benefits of single session approach:");
//        Console.WriteLine("     ✓ Shared session state and caching");
//        Console.WriteLine("     ✓ Consistent compiler options across all shaders");
//        Console.WriteLine("     ✓ Efficient memory usage");
//        Console.WriteLine("     ✓ Module dependency resolution across different pipelines");
        
//        // Graphics Pipeline
//        if (programs.ContainsKey("Graphics"))
//        {
//            var graphicsProgram = programs["Graphics"];
//            var vertexEntry = graphicsProgram.EntryPoints.FirstOrDefault(ep => ep.Name == "vertexMain");
//            var pixelEntry = graphicsProgram.EntryPoints.FirstOrDefault(ep => ep.Name == "pixelMain");
            
//            if (vertexEntry != null && pixelEntry != null)
//            {
//                Console.WriteLine("\n     🎨 Graphics Pipeline Ready:");
//                Console.WriteLine($"       Vertex Shader: {vertexEntry.Name} ({vertexEntry.Stage})");
//                Console.WriteLine($"       Pixel Shader: {pixelEntry.Name} ({pixelEntry.Stage})");
//                Console.WriteLine("       Usage: Render 3D geometry with lighting");
//            }
//        }
        
//        // Post-Processing Pipeline
//        if (programs.ContainsKey("PostProcess"))
//        {
//            var postProcessProgram = programs["PostProcess"];
//            var postProcessEntry = postProcessProgram.EntryPoints.FirstOrDefault(ep => ep.Name == "postProcessMain");
            
//            if (postProcessEntry != null)
//            {
//                Console.WriteLine("\n     🖼️ Post-Processing Pipeline Ready:");
//                Console.WriteLine($"       Compute Shader: {postProcessEntry.Name} ({postProcessEntry.Stage})");
//                Console.WriteLine("       Usage: Apply screen-space effects (brightness, contrast, etc.)");
//            }
//        }
        
//        // Particle Simulation Pipeline
//        if (programs.ContainsKey("Particles"))
//        {
//            var particlesProgram = programs["Particles"];
//            var simulateEntry = particlesProgram.EntryPoints.FirstOrDefault(ep => ep.Name == "simulateParticles");
//            var resetEntry = particlesProgram.EntryPoints.FirstOrDefault(ep => ep.Name == "resetParticles");
            
//            if (simulateEntry != null && resetEntry != null)
//            {
//                Console.WriteLine("\n     ✨ Particle Simulation Pipeline Ready:");
//                Console.WriteLine($"       Simulation: {simulateEntry.Name} ({simulateEntry.Stage})");
//                Console.WriteLine($"       Reset: {resetEntry.Name} ({resetEntry.Stage})");
//                Console.WriteLine("       Usage: GPU-based particle physics simulation");
//            }
//        }
//    }

//    /// <summary>
//    /// FIXED VERSION: Demonstrates using multiple sessions for different types of shaders.
//    /// This approach might be useful when you need different compiler settings.
//    /// </summary>
//    private static void DemonstrateMultipleSessionsFixed()
//    {
//        Console.WriteLine("Creating separate sessions for different pipeline types...");
        
//        try
//        {
//            // Session for graphics shaders with specific optimizations
//            Console.WriteLine("\n  Creating graphics-optimized session...");
//            SessionBuilder graphicsBuilder = new SessionBuilder();
            
//            // FIX: Add compile target first
//            graphicsBuilder.AddCompileTarget(CompileTarget.SLANG_HLSL);
//            graphicsBuilder.AddSearchPath(Path.GetFullPath("./Shaders/"));
            
//            // Graphics-specific configuration
//            graphicsBuilder.AddShaderModel(CompileTarget.SLANG_HLSL, "vs_5_0");
//            graphicsBuilder.AddShaderModel(CompileTarget.SLANG_HLSL, "ps_5_0");
//            // graphicsBuilder.AddCompilerOption(CompilerOptionName.Optimization, 
//            //     new CompilerOptionValue(CompilerOptionValueKind.Int, 3, 0, null, null));
//            graphicsBuilder.AddPreprocessorMacro("GRAPHICS_PIPELINE", "1");
            
//            Session graphicsSession = graphicsBuilder.Create();
//            Console.WriteLine("    ✓ Graphics session created");
            
//            // Session for compute shaders
//            Console.WriteLine("  Creating compute-optimized session...");
//            SessionBuilder computeBuilder = new SessionBuilder();
            
//            // FIX: Add compile target first
//            computeBuilder.AddCompileTarget(CompileTarget.SLANG_HLSL);
//            computeBuilder.AddSearchPath(Path.GetFullPath("./Shaders/"));
            
//            // Compute-specific configuration
//            computeBuilder.AddShaderModel(CompileTarget.SLANG_HLSL, "cs_5_0");
//            // computeBuilder.AddCompilerOption(CompilerOptionName.Debug, 
//            //     new CompilerOptionValue(CompilerOptionValueKind.Int, 1, 0, null, null));
//            computeBuilder.AddPreprocessorMacro("COMPUTE_PIPELINE", "1");
            
//            Session computeSession = computeBuilder.Create();
//            Console.WriteLine("    ✓ Compute session created");

//            // Load modules in specialized sessions
//            Console.WriteLine("\n  Loading modules in specialized sessions...");
            
//            // Load graphics in graphics session
//            LoadModuleWithErrorHandling(graphicsSession, "Graphics", new Dictionary<string, ShaderProgram>());
            
//            // Load compute shaders in compute session
//            var computePrograms = new Dictionary<string, ShaderProgram>();
//            LoadModuleWithErrorHandling(computeSession, "PostProcess", computePrograms);
//            LoadModuleWithErrorHandling(computeSession, "Particles", computePrograms);
            
//            Console.WriteLine("\n  ✅ Multiple sessions approach demonstrated!");
//            Console.WriteLine("     Use cases:");
//            Console.WriteLine("     • Different optimization levels for different shader types");
//            Console.WriteLine("     • Platform-specific compilation settings");
//            Console.WriteLine("     • Debug vs Release configurations");
//            Console.WriteLine("     ⚠️ Trade-offs: Higher memory usage, no shared cache");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"    ❌ Error in multiple sessions: {ex.Message}");
//        }
//    }
//}
