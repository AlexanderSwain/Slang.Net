namespace Slang.Net.Samples.CrossPlatform;

/// <summary>
/// Demonstration program for the Cross-Platform Shader Pipeline
/// </summary>
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Cross-Platform Shader Pipeline with Slang.Net Sample");
        Console.WriteLine("===================================================");
        Console.WriteLine();

        // Create a cross-platform shader manager
        using var shaderManager = new CrossPlatformShaderManager(AppDomain.CurrentDomain.BaseDirectory);

        // Define the shader file and entry points we want to compile
        var shaderPath = "UniversalShader.slang";
        var vertexEntryPoint = "VS";
        var fragmentEntryPoint = "FS";

        // Create output directory
        var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeneratedShaders");
        Directory.CreateDirectory(outputDir);
        
        Console.WriteLine("🔍 Compiling vertex shader for all platforms...");
        Console.WriteLine();

        // Compile vertex shader for all platforms
        var vertexResults = shaderManager.CompileForAllTargets(shaderPath, vertexEntryPoint);
        foreach (var (api, result) in vertexResults)
        {
            Console.WriteLine($"✅ {shaderManager.GetAPIDescription(api)}");
            
            // Save the compiled shader to a file
            result.SaveToFile(outputDir);
            
            // Print a preview of the generated code
            Console.WriteLine("Preview:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(result.GetSourceCodePreview(10));
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();
        }

        Console.WriteLine("🔍 Compiling fragment shader for all platforms...");
        Console.WriteLine();

        // Compile fragment shader for all platforms
        var fragmentResults = shaderManager.CompileForAllTargets(shaderPath, fragmentEntryPoint);
        foreach (var (api, result) in fragmentResults)
        {
            Console.WriteLine($"✅ {shaderManager.GetAPIDescription(api)}");
            
            // Save the compiled shader to a file
            result.SaveToFile(outputDir);
            
            // Print a preview of the generated code
            Console.WriteLine("Preview:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(result.GetSourceCodePreview(10));
            Console.WriteLine(new string('-', 60));
            Console.WriteLine();
        }

        Console.WriteLine($"📁 All compiled shaders have been saved to: {outputDir}");
        Console.WriteLine();
        
        // Show integration information
        Console.WriteLine("🔄 Cross-Platform Integration:");
        Console.WriteLine("  1. Write one Slang shader that works everywhere");
        Console.WriteLine("  2. Compile to target-specific formats at runtime");
        Console.WriteLine("  3. Integrate with platform-specific graphics APIs");
        Console.WriteLine("  4. Automatically handle binding differences between platforms");
        Console.WriteLine();

        // Example of targeted compilation for a specific platform with custom settings
        Console.WriteLine("🎯 Example of targeted compilation:");
        Console.WriteLine();
        
        var customDirectX = shaderManager.CompileForTarget(
            shaderPath, 
            vertexEntryPoint, 
            GraphicsAPI.DirectX12,
            "6_5" // Using a specific shader model
        );
        
        Console.WriteLine($"✅ Custom DirectX 12 (Shader Model 6.5)");
        Console.WriteLine("Preview:");
        Console.WriteLine(new string('-', 60));
        Console.WriteLine(customDirectX.GetSourceCodePreview(10));
        Console.WriteLine(new string('-', 60));
        Console.WriteLine();
        
        customDirectX.SaveToFile(outputDir);
        
        Console.WriteLine("✨ Sample completed successfully!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
