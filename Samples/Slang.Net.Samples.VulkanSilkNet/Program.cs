using Silk.NET.Vulkan;
using System.Drawing;

namespace Slang.Net.Samples.VulkanSilkNet;

/// <summary>
/// Simple demonstration program that shows how to use Slang.Net with Vulkan via Silk.NET
/// </summary>
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Vulkan Integration with Silk.NET and Slang.Net Sample");
        Console.WriteLine("===================================================");

        // We'll compile the shaders but not create an actual Vulkan instance for simplicity
        SimulateVulkanCompilation();

        Console.WriteLine("✨ Sample completed successfully!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void SimulateVulkanCompilation()
    {
        Console.WriteLine("🔧 Creating Vulkan shader manager...");
        // Create shader manager with current directory as the shader directory
        // We're using null for Vk and Device instances since this is just a simulation
        var shaderManager = new VulkanShaderManager(null!, default, AppDomain.CurrentDomain.BaseDirectory);

        // Compile vertex shader
        Console.WriteLine("📝 Compiling vertex shader...");
        string vertexGLSL;
        try
        {
            vertexGLSL = shaderManager.CompileShaderToGLSL("SimpleVertexVulkan.slang", "VS");
            Console.WriteLine("✅ Vertex shader compiled successfully!");
            Console.WriteLine("Generated Vulkan-compatible GLSL:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(vertexGLSL);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to compile vertex shader: {ex.Message}");
            return;
        }

        // Compile fragment shader
        Console.WriteLine("📝 Compiling fragment shader...");
        string fragmentGLSL;
        try
        {
            fragmentGLSL = shaderManager.CompileShaderToGLSL("SimpleFragmentVulkan.slang", "FS");
            Console.WriteLine("✅ Fragment shader compiled successfully!");
            Console.WriteLine("Generated Vulkan-compatible GLSL:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(fragmentGLSL);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to compile fragment shader: {ex.Message}");
            return;
        }

        // Compile compute shader
        Console.WriteLine("📝 Compiling compute shader...");
        string computeGLSL;
        try
        {
            computeGLSL = shaderManager.CompileShaderToGLSL("SimpleComputeVulkan.slang", "CS");
            Console.WriteLine("✅ Compute shader compiled successfully!");
            Console.WriteLine("Generated Vulkan-compatible GLSL:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(computeGLSL);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to compile compute shader: {ex.Message}");
            return;
        }

        // Display integration information
        Console.WriteLine("🎯 Integration Overview:");
        Console.WriteLine("  1. Slang source files were loaded from disk");
        Console.WriteLine("  2. Slang.Net compiled them to Vulkan-compatible GLSL");
        Console.WriteLine("  3. In a real application, the GLSL would be converted to SPIR-V");
        Console.WriteLine("  4. SPIR-V bytecode would be used to create Vulkan shader modules");
        Console.WriteLine();

        // Show integration process details
        Console.WriteLine("📋 Vulkan Integration Details:");
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetVulkanIntegrationInfo(vertexGLSL, "VS", "VertexShader"));
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetVulkanIntegrationInfo(fragmentGLSL, "FS", "FragmentShader"));
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetVulkanIntegrationInfo(computeGLSL, "CS", "ComputeShader"));
    }
}
