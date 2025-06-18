using Silk.NET.Vulkan;
using Slang;
using System.Runtime.InteropServices;
using System.Text;

namespace Slang.Net.Samples.VulkanSilkNet;

/// <summary>
/// Manages shaders for Vulkan using Slang.Net
/// </summary>
public unsafe class VulkanShaderManager : IDisposable
{
    private readonly Vk _vk;
    private readonly Device _device;
    private readonly Session _slangSession;
    private bool _disposed;
    
    /// <summary>
    /// Creates a new Vulkan shader manager
    /// </summary>
    /// <param name="vk">Vulkan API instance</param>
    /// <param name="device">Vulkan device</param>
    /// <param name="shaderDirectory">Directory containing shader files</param>
    public VulkanShaderManager(Vk vk, Device device, string shaderDirectory)
    {
        _vk = vk;
        _device = device;
        _slangSession = new SessionBuilder()
            .AddSearchPath(shaderDirectory)
            .AddShaderModel(CompileTarget.SLANG_GLSL, "450")
            .AddPreprocessorMacro("VULKAN", "1")
            .Create();
    }
    
    /// <summary>
    /// Compiles a Slang shader to GLSL for Vulkan
    /// </summary>
    /// <param name="shaderPath">Path to the shader file</param>
    /// <param name="entryPoint">Entry point name</param>
    /// <returns>Compiled GLSL code</returns>
    public string CompileShaderToGLSL(string shaderPath, string entryPoint)
    {
        try
        {
            var module = _slangSession.LoadModule(shaderPath);
            var entry = module.Program.EntryPoints.FirstOrDefault(e => e.Name == entryPoint) 
                ?? throw new InvalidOperationException($"Entry point '{entryPoint}' not found in shader '{shaderPath}'");
            
            return entry.Compile();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to compile shader '{shaderPath}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Creates a Vulkan shader module from a compiled GLSL shader
    /// </summary>
    /// <param name="glslCode">Compiled GLSL shader code</param>
    /// <returns>Vulkan shader module handle</returns>
    /// <remarks>
    /// This is a simulation only. In a real implementation, you would use
    /// a GLSL to SPIR-V compiler like Shaderc or glslang.
    /// </remarks>
    public ShaderModule SimulateCreateShaderModule(string glslCode)
    {
        Console.WriteLine("Note: In a real implementation, you would use a GLSL to SPIR-V compiler");
        Console.WriteLine("      like Shaderc or glslang to convert GLSL to SPIR-V bytecode.");
        
        // In a real implementation, this would actually compile GLSL to SPIR-V
        // and create a real shader module. We're simulating it here.
        return new ShaderModule(1234); // Simulated handle
    }
    
    /// <summary>
    /// Gets information about the Vulkan shader module creation process for documentation purposes
    /// </summary>
    /// <param name="glslCode">The compiled GLSL code</param>
    /// <param name="entryPoint">The shader entry point</param>
    /// <param name="shaderType">The type of shader</param>
    /// <returns>Formatted information about the integration</returns>
    public string GetVulkanIntegrationInfo(string glslCode, string entryPoint, string shaderType)
    {
        StringBuilder sb = new();
        sb.AppendLine($"Integrating {shaderType} with Vulkan:");
        sb.AppendLine($"1. Compile Slang to GLSL 450 for Vulkan (done)");
        sb.AppendLine($"2. Convert GLSL to SPIR-V bytecode:");
        sb.AppendLine("   ```csharp");
        sb.AppendLine("   // Using Shaderc (via Silk.NET.Shaderc or similar)");
        sb.AppendLine("   var compiler = new Shaderc.Shaderc();");
        sb.AppendLine($"   var kind = shaderType switch {{");
        sb.AppendLine($"       \"VertexShader\" => Shaderc.ShaderKind.VertexShader,");
        sb.AppendLine($"       \"FragmentShader\" => Shaderc.ShaderKind.FragmentShader,");
        sb.AppendLine($"       \"ComputeShader\" => Shaderc.ShaderKind.ComputeShader,");
        sb.AppendLine($"       _ => throw new NotSupportedException($\"Shader type {{shaderType}} not supported\")");
        sb.AppendLine($"   }};");
        sb.AppendLine($"   var result = compiler.CompileGlslToSpv(glslCode, kind, \"shader.glsl\");");
        sb.AppendLine($"   var spirvCode = result.GetBytes();");
        sb.AppendLine("   ```");
        sb.AppendLine("3. Create Vulkan shader module from SPIR-V bytecode:");
        sb.AppendLine("   ```csharp");
        sb.AppendLine("   fixed (byte* pCode = spirvCode) {");
        sb.AppendLine("       var createInfo = new ShaderModuleCreateInfo {");
        sb.AppendLine("           SType = StructureType.ShaderModuleCreateInfo,");
        sb.AppendLine("           CodeSize = (nuint)spirvCode.Length,");
        sb.AppendLine("           PCode = (uint*)pCode");
        sb.AppendLine("       };");
        sb.AppendLine("       vk.CreateShaderModule(device, createInfo, null, out var shaderModule);");
        sb.AppendLine("   }");
        sb.AppendLine("   ```");
        sb.AppendLine("4. Use in pipeline creation:");
        sb.AppendLine("   ```csharp");
        sb.AppendLine("   var shaderStageInfo = new PipelineShaderStageCreateInfo {");
        sb.AppendLine("       SType = StructureType.PipelineShaderStageCreateInfo,");
        sb.AppendLine($"       Stage = ShaderStageFlags.{shaderType.Replace("Shader", "")}Bit,");
        sb.AppendLine("       Module = shaderModule,");
        sb.AppendLine($"       PName = Marshal.StringToHGlobalAnsi(\"{entryPoint}\")");
        sb.AppendLine("   };");
        sb.AppendLine("   // Add to pipeline create info");
        sb.AppendLine("   ```");
        return sb.ToString();
    }
    
    /// <summary>
    /// Clean up resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _slangSession?.Dispose();
            _disposed = true;
        }
        
        GC.SuppressFinalize(this);
    }
}
