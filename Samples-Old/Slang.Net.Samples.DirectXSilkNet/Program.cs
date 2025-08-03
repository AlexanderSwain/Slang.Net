using Slang;
using System.Text;

namespace Slang.Net.Samples.DirectXSilkNet;

/// <summary>
/// A simplified DirectX integration sample that demonstrates how to compile Slang shaders
/// and prepare them for use with DirectX. This sample focuses on the compilation pipeline
/// rather than full DirectX rendering to avoid complex platform dependencies.
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("DirectX Integration with Silk.NET and Slang.Net Sample");
        Console.WriteLine("========================================================");
        Console.WriteLine();

        try
        {
            // Initialize the shader manager
            var shaderDirectory = AppDomain.CurrentDomain.BaseDirectory;
            using var shaderManager = new DirectXShaderManager(shaderDirectory);

            Console.WriteLine("🔧 Initializing Slang session...");
              // Compile vertex shader
            Console.WriteLine("📝 Compiling vertex shader...");
            var vertexShaderHlsl = shaderManager.CompileVertexShader("SimpleVertex.slang", "VS");
            
            Console.WriteLine("✅ Vertex shader compiled successfully!");
            Console.WriteLine("Generated HLSL:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(vertexShaderHlsl);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine();

            // Compile pixel shader
            Console.WriteLine("📝 Compiling pixel shader...");
            var pixelShaderHlsl = shaderManager.CompilePixelShader("SimplePixel.slang", "PS");
            
            Console.WriteLine("✅ Pixel shader compiled successfully!");
            Console.WriteLine("Generated HLSL:");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(pixelShaderHlsl);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine();            // Demonstrate how these would be used with DirectX
            Console.WriteLine("🎯 Integration Overview:");
            Console.WriteLine("  1. Slang source files were loaded from disk");
            Console.WriteLine("  2. Slang.Net compiled them to HLSL");
            Console.WriteLine("  3. Generated HLSL can now be compiled to bytecode using D3DCompiler");
            Console.WriteLine("  4. Bytecode can be used to create DirectX shader objects");
            Console.WriteLine();

            // Show integration steps for each shader
            Console.WriteLine("📋 DirectX Integration Details:");
            Console.WriteLine();
            Console.WriteLine(shaderManager.GetDirectXIntegrationSteps(vertexShaderHlsl, "VS", "vs_5_0"));
            Console.WriteLine();
            Console.WriteLine(shaderManager.GetDirectXIntegrationSteps(pixelShaderHlsl, "PS", "ps_5_0"));
            Console.WriteLine();

            // Show available entry points
            Console.WriteLine("🔍 Available Entry Points:");
            var vertexEntryPoints = shaderManager.GetEntryPoints("SimpleVertex.slang");
            var pixelEntryPoints = shaderManager.GetEntryPoints("SimplePixel.slang");
            
            Console.WriteLine($"  SimpleVertex.slang: {string.Join(", ", vertexEntryPoints)}");
            Console.WriteLine($"  SimplePixel.slang: {string.Join(", ", pixelEntryPoints)}");
            Console.WriteLine();

            Console.WriteLine("📋 Next Steps for Full Integration:");
            Console.WriteLine("  • Use D3DCompiler to compile HLSL to bytecode");
            Console.WriteLine("  • Create ID3D11VertexShader/ID3D11PixelShader objects");
            Console.WriteLine("  • Set up input layouts matching the vertex structure");
            Console.WriteLine("  • Bind shaders during rendering");
            Console.WriteLine();

            Console.WriteLine("✨ Sample completed successfully!");
            Console.WriteLine("The generated HLSL code above can be used with any DirectX application.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"   Inner: {ex.InnerException.Message}");
            }
            Console.WriteLine();
            Console.WriteLine("💡 Troubleshooting:");
            Console.WriteLine("  • Ensure .slang files are in the output directory");
            Console.WriteLine("  • Check that Slang.Net package is properly installed");
            Console.WriteLine("  • Verify the shader syntax is correct");
            
            Environment.Exit(1);
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
