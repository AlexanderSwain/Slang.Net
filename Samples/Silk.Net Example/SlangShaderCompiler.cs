using Silk.NET.OpenGL;
using Slang.Sdk;
using Slang.Sdk.Interop;
using System;
using System.Linq;

namespace Tutorial
{
    /// <summary>
    /// Supported graphics backends for shader compilation
    /// </summary>
    public enum GraphicsBackend
    {
        None,
        OpenGL,
        DirectX11
    }

    /// <summary>
    /// Wrapper class for compiling Slang shaders to different graphics API targets.
    /// Demonstrates how to use Slang.Sdk for cross-platform shader development.
    /// </summary>
    public class SlangShaderCompiler : IDisposable
    {
        private readonly Session _vsSession;
        private readonly Session _psSession;
        private readonly Module _vsModule;
        private readonly Module _psModule;
        private readonly Slang.Sdk.Program _vsProgram;
        private readonly Slang.Sdk.Program _psProgram;

        /// <summary>
        /// Creates a new Slang shader compiler for the specified graphics backend
        /// </summary>
        /// <param name="backend">The graphics backend to target (OpenGL or DirectX11)</param>
        public SlangShaderCompiler(string slangFilePath)
        {
            // Create the sessions
            _vsSession = new Session.Builder()
                .AddTarget(Targets.Hlsl.vs_5_0)
                .AddTarget(Targets.Glsl.v330)
                .Create();
            _psSession = new Session.Builder()
                .AddTarget(Targets.Hlsl.ps_5_0)
                .AddTarget(Targets.Glsl.v330)
                .Create();

            // Create the modules
            _vsModule = new Module.Builder(_vsSession)
                .AddTranslationUnit(SourceLanguage.Slang, "vsUnit", out var vsUnitIndex)
                .AddTranslationUnitSourceFile(vsUnitIndex, slangFilePath)
                .AddEntryPoint(vsUnitIndex, "vertexMain", Stage.Vertex)
                .Create();
            _psModule = new Module.Builder(_psSession)
                .AddTranslationUnit(SourceLanguage.Slang, "psUnit", out var psUnitIndex)
                .AddTranslationUnitSourceFile(psUnitIndex, slangFilePath)
                .AddEntryPoint(psUnitIndex, "fragmentMain", Stage.Pixel)
                .Create();

            // Get the programs
            _vsProgram = _vsModule.Program;
            _psProgram = _psModule.Program;
        }

        /// <summary>
        /// Compiles a Slang shader file to vertex and fragment shader source code for the target backend
        /// </summary>
        /// <param name="slangFilePath">Path to the .slang shader file</param>
        /// <returns>Tuple containing (vertex shader source, fragment shader source)</returns>
        /// <exception cref="InvalidOperationException">Thrown when shader compilation fails</exception>
        public (string vertexShader, string fragmentShader) CompileShaders(GraphicsBackend backend)
        {
            try
            {
                var compileResults = backend switch
                {
                    GraphicsBackend.OpenGL => CompileForOpenGL(),
                    GraphicsBackend.DirectX11 => CompileForDirectX11(),
                    _ => throw new ArgumentException($"Unsupported backend: {backend}")
                };

                return (compileResults.vsCompileResult.SourceCode!, compileResults.psCompileResult.SourceCode!);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to compile Slang shader for {backend}: {ex.Message}", ex);
            }
        }

        private (CompilationResult vsCompileResult, CompilationResult psCompileResult) CompileForOpenGL()
        {
            var vsProgramTarget = _vsProgram.Targets[Targets.Glsl.es_320];
            var psProgramTarget = _psProgram.Targets[Targets.Glsl.es_320];

            var vertexEntry = vsProgramTarget.EntryPoints["vertexMain"];
            var fragmentEntry = psProgramTarget.EntryPoints["fragmentMain"];

            var vertexResult = vertexEntry.Compile();
            var fragmentResult = fragmentEntry.Compile();

            return (vertexResult, fragmentResult);
        }

        /// <summary>
        /// Compiles Slang shader for DirectX11 target (HLSL 5.0)
        /// </summary>
        private (CompilationResult vsCompileResult, CompilationResult psCompileResult) CompileForDirectX11()
        {
            var vsProgramTarget = _vsProgram.Targets[Targets.Hlsl.vs_5_0];
            var psProgramTarget = _psProgram.Targets[Targets.Hlsl.ps_5_0];
            
            var vertexEntry = vsProgramTarget.EntryPoints["vertexMain"];
            var fragmentEntry = psProgramTarget.EntryPoints["fragmentMain"];
            
            var vertexResult = vertexEntry.Compile();
            var fragmentResult = fragmentEntry.Compile();
            
            return (vertexResult, fragmentResult);
        }

        /// <summary>
        /// Gets reflection information for the compiled shader
        /// </summary>
        /// <param name="slangFilePath">Path to the .slang shader file</param>
        /// <returns>Shader reflection data containing entry points and parameters</returns>
        public ShaderReflection GetVSReflection(Target target)
        {
            return _vsProgram.Targets[target].GetReflection();
        }

        /// <summary>
        /// Gets reflection information for the compiled shader
        /// </summary>
        /// <param name="slangFilePath">Path to the .slang shader file</param>
        /// <returns>Shader reflection data containing entry points and parameters</returns>
        public ShaderReflection GetPSReflection(Target target)
        {
            return _psProgram.Targets[target].GetReflection();
        }

        /// <summary>
        /// Disposes of the Slang session resources
        /// </summary>
        public void Dispose()
        {
            // Slang sessions are automatically managed
            // No explicit disposal needed for current Slang.Sdk version
        }
    }
}
