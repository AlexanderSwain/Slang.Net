using Slang;

namespace Slang.Net.Samples.DirectXSilkNet;

/// <summary>
/// A simplified shader manager that demonstrates how to use Slang.Net to compile shaders
/// and prepare them for DirectX integration. This version focuses on the compilation
/// pipeline without requiring actual DirectX device creation.
/// </summary>
public class DirectXShaderManager : IDisposable
{    private readonly Session _vertexSession;
    private readonly Session _pixelSession;
    private bool _disposed;

    public DirectXShaderManager(string shaderDirectory)
    {
        // Create separate sessions for different shader types to avoid profile conflicts
        _vertexSession = new SessionBuilder()
            .AddSearchPath(shaderDirectory)
            .AddShaderModel(CompileTarget.SLANG_HLSL, "vs_5_0")
            .Create();
            
        _pixelSession = new SessionBuilder()
            .AddSearchPath(shaderDirectory)
            .AddShaderModel(CompileTarget.SLANG_HLSL, "ps_5_0")
            .Create();
    }    /// <summary>
    /// Compiles a vertex shader to HLSL code
    /// </summary>
    /// <param name="shaderPath">Path to the .slang file</param>
    /// <param name="entryPoint">Entry point function name</param>
    /// <returns>Generated HLSL code</returns>
    public string CompileVertexShader(string shaderPath, string entryPoint)
    {
        return CompileShader(_vertexSession, shaderPath, entryPoint, "vertex");
    }

    /// <summary>
    /// Compiles a pixel shader to HLSL code
    /// </summary>
    /// <param name="shaderPath">Path to the .slang file</param>
    /// <param name="entryPoint">Entry point function name</param>
    /// <returns>Generated HLSL code</returns>
    public string CompilePixelShader(string shaderPath, string entryPoint)
    {
        return CompileShader(_pixelSession, shaderPath, entryPoint, "pixel");
    }

    private string CompileShader(Session session, string shaderPath, string entryPoint, string shaderType)
    {
        try
        {
            var module = session.LoadModule(shaderPath);
            var entry = module.Program.EntryPoints.FirstOrDefault(e => e.Name == entryPoint);
            
            if (entry == null)
            {
                throw new Exception($"Entry point '{entryPoint}' not found in {shaderType} shader '{shaderPath}'");
            }
            
            return entry.Compile();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to compile {shaderType} shader '{shaderPath}' entry point '{entryPoint}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Compiles a Slang shader to HLSL code that can be used with DirectX
    /// </summary>
    /// <param name="shaderPath">Path to the .slang file</param>
    /// <param name="entryPoint">Entry point function name</param>
    /// <returns>Generated HLSL code</returns>
    [Obsolete("Use CompileVertexShader or CompilePixelShader instead")]
    public string CompileSlangToHlsl(string shaderPath, string entryPoint)
    {
        // Try vertex shader first, then pixel shader
        try
        {
            return CompileVertexShader(shaderPath, entryPoint);
        }
        catch
        {
            return CompilePixelShader(shaderPath, entryPoint);
        }
    }

    /// <summary>
    /// Gets information about available entry points in a shader module
    /// </summary>
    /// <param name="shaderPath">Path to the .slang file</param>
    /// <returns>List of entry point names</returns>    
    public string[] GetEntryPoints(string shaderPath)
    {
        try
        {
            // Try vertex session first, fallback to pixel session
            try
            {
                var module = _vertexSession.LoadModule(shaderPath);
                return module.Program.EntryPoints.Select(ep => ep.Name).ToArray();
            }
            catch
            {
                var module = _pixelSession.LoadModule(shaderPath);
                return module.Program.EntryPoints.Select(ep => ep.Name).ToArray();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to load shader module '{shaderPath}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Demonstrates how this HLSL code would be used with DirectX
    /// This method shows the conceptual pipeline without requiring actual DirectX APIs
    /// </summary>
    /// <param name="hlslCode">Generated HLSL code</param>
    /// <param name="entryPoint">Entry point name</param>
    /// <param name="profile">Shader profile (vs_5_0, ps_5_0, etc.)</param>
    /// <returns>Information about the next steps for DirectX integration</returns>
    public string GetDirectXIntegrationSteps(string hlslCode, string entryPoint, string profile)
    {
        return $@"DirectX Integration Steps for {entryPoint} ({profile}):

1. Compile HLSL to Bytecode:
   - Use D3DCompiler API to compile the HLSL code to bytecode
   - Handle compilation errors and warnings

2. Create DirectX Shader Object:
   - For vertex shaders: ID3D11Device.CreateVertexShader()
   - For pixel shaders: ID3D11Device.CreatePixelShader()
   - For compute shaders: ID3D11Device.CreateComputeShader()

3. Set Up Input Layout (for vertex shaders):
   - Define input element descriptions
   - Create ID3D11InputLayout using vertex shader bytecode

4. Bind During Rendering:
   - Use ID3D11DeviceContext.VSSetShader(), PSSetShader(), etc.
   - Bind constant buffers, textures, and other resources

HLSL Code Length: {hlslCode.Length} characters
Entry Point: {entryPoint}
Target Profile: {profile}";
    }    public void Dispose()
    {
        if (!_disposed)
        {
            _vertexSession?.Dispose();
            _pixelSession?.Dispose();
            _disposed = true;
        }
    }
}
