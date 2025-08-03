using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Drawing;

namespace Slang.Net.Samples.OpenGLSilkNet;

/// <summary>
/// Simple demonstration program that shows how to use Slang.Net with OpenGL via Silk.NET
/// </summary>
public class Program
{
    public static void Main()
    {
        Console.WriteLine("OpenGL Integration with Silk.NET and Slang.Net Sample");
        Console.WriteLine("====================================================");

        // We'll compile the shaders but not create an actual OpenGL window for simplicity
        SimulateOpenGLCompilation();

        Console.WriteLine("✨ Sample completed successfully!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static void SimulateOpenGLCompilation()
    {
        Console.WriteLine("🔧 Creating OpenGL shader manager...");
        // Create shader manager with current directory as the shader directory
        var shaderManager = new OpenGLShaderManager(null!, AppDomain.CurrentDomain.BaseDirectory);

        // Compile vertex shader
        Console.WriteLine("📝 Compiling vertex shader...");
        string vertexGLSL;
        try
        {
            vertexGLSL = shaderManager.CompileShaderToGLSL("SimpleVertexGL.slang", "VS");
            Console.WriteLine("✅ Vertex shader compiled successfully!");
            Console.WriteLine("Generated GLSL:");
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
            fragmentGLSL = shaderManager.CompileShaderToGLSL("SimpleFragmentGL.slang", "FS");
            Console.WriteLine("✅ Fragment shader compiled successfully!");
            Console.WriteLine("Generated GLSL:");
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
            computeGLSL = shaderManager.CompileShaderToGLSL("SimpleComputeGL.slang", "CS");
            Console.WriteLine("✅ Compute shader compiled successfully!");
            Console.WriteLine("Generated GLSL:");
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
        Console.WriteLine("  2. Slang.Net compiled them to GLSL");
        Console.WriteLine("  3. Generated GLSL can be used with OpenGL shader objects");
        Console.WriteLine();

        // Show integration process details
        Console.WriteLine("📋 OpenGL Integration Details:");
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetOpenGLIntegrationInfo(vertexGLSL, "VS", "VertexShader"));
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetOpenGLIntegrationInfo(fragmentGLSL, "FS", "FragmentShader"));
        Console.WriteLine();
        Console.WriteLine(shaderManager.GetOpenGLIntegrationInfo(computeGLSL, "CS", "ComputeShader"));
        Console.WriteLine();

        // Show example usage in full application
        Console.WriteLine("🔍 Example Usage in Full OpenGL Application:");
        Console.WriteLine(@"
// Create window with OpenGL context
var window = Window.Create(WindowOptions.Default);
window.Load += () => 
{
    // Get OpenGL API instance
    var gl = GL.GetApi(window);

    // Initialize shader manager with shader directory
    var shaderManager = new OpenGLShaderManager(gl, ""shaders"");

    // Create shader program from vertex and fragment shaders
    uint shaderProgram = shaderManager.CreateShaderProgram(
        ""SimpleVertexGL.slang"", 
        ""SimpleFragmentGL.slang""
    );

    // Create compute shader program
    uint computeProgram = shaderManager.CreateComputeShader(
        ""SimpleComputeGL.slang"",
        ""CS""
    );

    // Use the programs in rendering or computation
    gl.UseProgram(shaderProgram);
    // ... set up geometry and render

    gl.UseProgram(computeProgram);
    gl.DispatchCompute(8, 8, 1);
};

// Run the application
window.Run();
");

        // Clean up resources
        shaderManager.Dispose();
    }
}
