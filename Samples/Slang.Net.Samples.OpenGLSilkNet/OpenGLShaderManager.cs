using Silk.NET.OpenGL;
using Slang;

namespace Slang.Net.Samples.OpenGLSilkNet
{
    /// <summary>
    /// A class that manages the compilation and usage of Slang shaders with OpenGL through Silk.NET
    /// </summary>
    public class OpenGLShaderManager : IDisposable
    {
        private readonly GL _gl;
        private readonly Session _slangSession;
        private bool _disposed;

        public OpenGLShaderManager(GL gl, string shaderDirectory)
        {
            _gl = gl;
            _slangSession = new SessionBuilder()
                .AddSearchPath(shaderDirectory)
                .AddShaderModel(CompileTarget.SLANG_GLSL, "460")
                .AddPreprocessorMacro("GL_CORE_PROFILE", "1")
                .Create();
        }

        /// <summary>
        /// Creates a shader program from vertex and fragment shader files
        /// </summary>
        public uint CreateShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            var vertexShader = CreateShader(vertexShaderPath, "VS", ShaderType.VertexShader);
            var fragmentShader = CreateShader(fragmentShaderPath, "FS", ShaderType.FragmentShader);

            var program = _gl.CreateProgram();
            _gl.AttachShader(program, vertexShader);
            _gl.AttachShader(program, fragmentShader);
            _gl.LinkProgram(program);

            // Check for linking errors
            _gl.GetProgram(program, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = _gl.GetProgramInfoLog(program);
                throw new Exception($"Shader program linking failed: {log}");
            }

            // Cleanup individual shaders
            _gl.DeleteShader(vertexShader);
            _gl.DeleteShader(fragmentShader);

            return program;
        }

        /// <summary>
        /// Creates a compute shader program
        /// </summary>
        public uint CreateComputeShader(string shaderPath, string entryPoint = "CS")
        {
            var computeShader = CreateShader(shaderPath, entryPoint, ShaderType.ComputeShader);

            var program = _gl.CreateProgram();
            _gl.AttachShader(program, computeShader);
            _gl.LinkProgram(program);

            // Check for linking errors
            _gl.GetProgram(program, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = _gl.GetProgramInfoLog(program);
                throw new Exception($"Compute shader linking failed: {log}");
            }

            _gl.DeleteShader(computeShader);
            return program;
        }

        /// <summary>
        /// Compiles a Slang shader file to GLSL and creates an OpenGL shader object
        /// </summary>
        private uint CreateShader(string shaderPath, string entryPoint, ShaderType type)
        {
            try
            {
                var module = _slangSession.LoadModule(shaderPath);
                var entry = module.Program.EntryPoints.First(e => e.Name == entryPoint);
                var glslCode = entry.Compile();

                var shader = _gl.CreateShader(type);
                _gl.ShaderSource(shader, glslCode);
                _gl.CompileShader(shader);

                // Check for compilation errors
                _gl.GetShader(shader, GLEnum.CompileStatus, out var status);
                if (status == 0)
                {
                    var log = _gl.GetShaderInfoLog(shader);
                    throw new Exception($"Shader compilation failed: {log}");
                }

                return shader;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create {type} shader from '{shaderPath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Compiles a Slang shader file to GLSL without creating an OpenGL shader object
        /// </summary>
        public string CompileShaderToGLSL(string shaderPath, string entryPoint)
        {
            try
            {
                var module = _slangSession.LoadModule(shaderPath);
                var entry = module.Program.EntryPoints.First(e => e.Name == entryPoint);
                return entry.Compile();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to compile shader '{shaderPath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets information about the shader compilation process for debugging purposes
        /// </summary>
        public string GetOpenGLIntegrationInfo(string glslCode, string entryPoint, string shaderType)
        {
            return $@"OpenGL Integration Process for {entryPoint} ({shaderType}):

1. Compile Slang to GLSL:
   - Slang.Net processes the code and outputs GLSL
   - Target GLSL version: 4.60

2. Create OpenGL Shader:
   - gl.CreateShader({shaderType})
   - gl.ShaderSource(shader, glslCode)
   - gl.CompileShader(shader)

3. Link Shader Program:
   - gl.CreateProgram()
   - gl.AttachShader(program, shader)
   - gl.LinkProgram(program)
   - Check for linking errors

4. Use in Render Loop:
   - gl.UseProgram(program)
   - Set uniforms and bind resources
   - Draw or dispatch compute

GLSL Code Length: {glslCode.Length} characters
Entry Point: {entryPoint}
Shader Type: {shaderType}";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _slangSession?.Dispose();
                _disposed = true;
            }
        }
    }
}
