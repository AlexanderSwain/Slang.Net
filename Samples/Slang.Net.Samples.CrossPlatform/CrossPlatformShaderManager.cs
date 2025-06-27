using Slang;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Slang.Net.Samples.CrossPlatform;

/// <summary>
/// A demonstration of the Cross-Platform Shader Pipeline using Slang.Net
/// This class implements the CrossPlatformShaderManager from the README
/// </summary>
public class CrossPlatformShaderManager : IDisposable
{
    private readonly Session _baseSession;
    private readonly string _shaderDirectory;
    private bool _disposed;
    
    /// <summary>
    /// Creates a new cross-platform shader manager
    /// </summary>
    /// <param name="shaderDirectory">Directory containing Slang shader files</param>
    public CrossPlatformShaderManager(string shaderDirectory)
    {
        _shaderDirectory = shaderDirectory;
        // Initialize the base session with minimal configuration - we'll create specific sessions for each target
        _baseSession = new SessionBuilder()
            .AddSearchPath(shaderDirectory)
            .AddShaderModel(CompileTarget.SLANG_HLSL, "5_0") // Add a default shader model to avoid errors
            .Create();
    }
    
    /// <summary>
    /// Compiles a shader for a specific graphics API target
    /// </summary>
    /// <param name="shaderPath">Path to the shader file</param>
    /// <param name="entryPoint">Entry point name (e.g., VS, FS)</param>
    /// <param name="targetAPI">Target graphics API</param>
    /// <param name="shaderModel">Optional specific shader model version</param>
    /// <returns>Compiled shader result containing source code and metadata</returns>
    public CompiledShaderResult CompileForTarget(
        string shaderPath, 
        string entryPoint,
        GraphicsAPI targetAPI,
        string? shaderModel = null)
    {
        var (target, profile) = GetTargetAndProfile(targetAPI, shaderModel);
        
        // Configure a new session specifically for this target
        /*using*/ var session = new SessionBuilder()
            .AddSearchPath(_shaderDirectory)
            .AddShaderModel(target, profile)
            .Create();
            
        var module = session.LoadModule(shaderPath);
        var entry = module.Program.EntryPoints.FirstOrDefault(e => e.Name == entryPoint)
            ?? throw new InvalidOperationException($"Entry point '{entryPoint}' not found in shader '{shaderPath}'");
            
        var compiledCode = entry.Compile();
        
        return new CompiledShaderResult
        {
            SourceCode = compiledCode,
            Target = target,
            Profile = profile,
            EntryPoint = entryPoint,
            ShaderPath = shaderPath,
            TargetAPI = targetAPI
        };
    }
    
    /// <summary>
    /// Compiles a shader for all supported graphics APIs
    /// </summary>
    /// <param name="shaderPath">Path to the shader file</param>
    /// <param name="entryPoint">Entry point name (e.g., VS, FS)</param>
    /// <returns>Dictionary mapping graphics APIs to their compiled shader results</returns>
    public Dictionary<GraphicsAPI, CompiledShaderResult> CompileForAllTargets(
        string shaderPath,
        string entryPoint)
    {
        var results = new Dictionary<GraphicsAPI, CompiledShaderResult>();
        
        foreach (GraphicsAPI api in Enum.GetValues(typeof(GraphicsAPI)))
        {
            try 
            {
                var result = CompileForTarget(shaderPath, entryPoint, api);
                results[api] = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to compile for {api}: {ex.Message}");
            }
        }
        
        return results;
    }
    
    /// <summary>
    /// Gets appropriate target and profile for the specified graphics API
    /// </summary>
    private (CompileTarget, string) GetTargetAndProfile(GraphicsAPI targetAPI, string? shaderModel)
    {
        return targetAPI switch
        {
            GraphicsAPI.DirectX11 => (CompileTarget.SLANG_HLSL, shaderModel ?? "5_0"),
            GraphicsAPI.DirectX12 => (CompileTarget.SLANG_HLSL, shaderModel ?? "6_0"),
            GraphicsAPI.OpenGL => (CompileTarget.SLANG_GLSL, shaderModel ?? "450"),
            GraphicsAPI.Vulkan => (CompileTarget.SLANG_GLSL, shaderModel ?? "450"),
            GraphicsAPI.Metal => (CompileTarget.SLANG_METAL, shaderModel ?? "2.0"),
            _ => throw new InvalidEnumArgumentException($"Unsupported graphics API: {targetAPI}")
        };
    }
    
    /// <summary>
    /// Get a descriptive string for the API target
    /// </summary>
    public string GetAPIDescription(GraphicsAPI api)
    {
        return api switch
        {
            GraphicsAPI.DirectX11 => "DirectX 11 (HLSL SM 5.0)",
            GraphicsAPI.DirectX12 => "DirectX 12 (HLSL SM 6.0)",
            GraphicsAPI.OpenGL => "OpenGL 4.5 (GLSL 450)",
            GraphicsAPI.Vulkan => "Vulkan (GLSL 450)",
            GraphicsAPI.Metal => "Metal 2.0",
            _ => api.ToString()
        };
    }
    
    /// <summary>
    /// Dispose of resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            // Uncomment this when fixing memory leak issues
            // Uncomment line 49 "using" statement
            //_baseSession?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Supported graphics APIs
/// </summary>
public enum GraphicsAPI
{
    DirectX11,
    DirectX12,
    OpenGL,
    Vulkan,
    Metal
}

/// <summary>
/// Results from a shader compilation
/// </summary>
public class CompiledShaderResult
{
    /// <summary>
    /// The compiled source code in the target language (HLSL, GLSL, Metal)
    /// </summary>
    public string SourceCode { get; init; } = string.Empty;
    
    /// <summary>
    /// The compile target used
    /// </summary>
    public CompileTarget Target { get; init; }
    
    /// <summary>
    /// The shader model/profile
    /// </summary>
    public string Profile { get; init; } = string.Empty;
    
    /// <summary>
    /// The entry point name
    /// </summary>
    public string EntryPoint { get; init; } = string.Empty;
    
    /// <summary>
    /// Path to the original shader file
    /// </summary>
    public string ShaderPath { get; init; } = string.Empty;
    
    /// <summary>
    /// The target graphics API
    /// </summary>
    public GraphicsAPI TargetAPI { get; init; }
    
    /// <summary>
    /// Get a shortened version of the source code for display
    /// </summary>
    public string GetSourceCodePreview(int maxLines = 20)
    {
        var lines = SourceCode.Split('\n');
        if (lines.Length <= maxLines)
            return SourceCode;
            
        var preview = string.Join('\n', lines.Take(maxLines));
        return preview + $"\n\n... ({lines.Length - maxLines} more lines) ...";
    }
    
    /// <summary>
    /// Saves the compiled shader to a file
    /// </summary>
    public void SaveToFile(string outputDirectory)
    {
        var extension = Target switch
        {
            CompileTarget.SLANG_HLSL => "hlsl",
            CompileTarget.SLANG_GLSL => "glsl",
            CompileTarget.SLANG_METAL => "metal",
            _ => "txt"
        };
        
        var baseFileName = Path.GetFileNameWithoutExtension(ShaderPath);
        var outputFileName = $"{baseFileName}_{EntryPoint}_{TargetAPI}.{extension}";
        var outputPath = Path.Combine(outputDirectory, outputFileName);
        
        Directory.CreateDirectory(outputDirectory);
        File.WriteAllText(outputPath, SourceCode);
    }
}
