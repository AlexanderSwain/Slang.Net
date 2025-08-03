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
        private readonly Session _session;
        private readonly Module _module;
        private readonly Slang.Sdk.Program _program;

        /// <summary>
        /// Creates a new Slang shader compiler for the specified graphics backend
        /// </summary>
        /// <param name="backend">The graphics backend to target (OpenGL or DirectX11)</param>
        public SlangShaderCompiler(string slangFilePath)
        {
            // Create the session
            _session = new Session.Builder()
                .AddTarget(Targets.Hlsl.vs_5_0)
                .AddTarget(Targets.Hlsl.ps_5_0)
                .AddTarget(Targets.Glsl.v330)
                .Create();

            // Create the module from the specified source file with the given targets
            _module = new Module.Builder(_session)
                .AddTranslationUnit(SourceLanguage.Slang, "shaderUnit", out var translationIndex)
                .AddTranslationUnitSourceFile(translationIndex, slangFilePath)
                .AddEntryPoint(translationIndex, "vertexMain", Stage.Vertex)
                .AddEntryPoint(translationIndex, "fragmentMain", Stage.Fragment)
                .Create();

            // Set the program
            _program = _module.Program;
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
                var program = _module.Program;

                var compileResults = backend switch
                {
                    GraphicsBackend.OpenGL => CompileForOpenGL(program),
                    GraphicsBackend.DirectX11 => CompileForDirectX11(program),
                    _ => throw new ArgumentException($"Unsupported backend: {backend}")
                };

                return (compileResults.vsCompileResult.SourceCode!, compileResults.psCompileResult.SourceCode!);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to compile Slang shader for {backend}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Compiles Slang shader for OpenGL target (GLSL 3.30)
        /// Note: Currently uses fallback GLSL due to compatibility issues with generated code
        /// </summary>
        private (CompilationResult vsCompileResult, CompilationResult psCompileResult) CompileForOpenGL(Slang.Sdk.Program program)
        {
            var programTarget = program.Targets[Targets.Glsl.v330];

            var vertexEntry = programTarget.EntryPoints["vertexMain"];
            var fragmentEntry = programTarget.EntryPoints["fragmentMain"];

            var vertexResult = vertexEntry.Compile();
            var fragmentResult = fragmentEntry.Compile();

            return (vertexResult, fragmentResult);
        }

        /// <summary>
        /// Compiles Slang shader for DirectX11 target (HLSL 5.0)
        /// </summary>
        private (CompilationResult vsCompileResult, CompilationResult psCompileResult) CompileForDirectX11(Slang.Sdk.Program program)
        {
            var vsProgramTarget = program.Targets[Targets.Hlsl.vs_5_0];
            var psProgramTarget = program.Targets[Targets.Hlsl.ps_5_0];
            
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
        public ShaderReflection GetReflection(Target target)
        {
            return _module.Program.Targets[target].GetReflection();
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
