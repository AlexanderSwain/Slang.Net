using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;

namespace Tutorial
{
    public class Shader : IDisposable
    {
        private uint _handle;
        private GL _gl;
        private uint _transformUBO;

        public Shader(GL gl, string vertexSrc, string fragmentSrc)
        {
            _gl = gl;

            uint vertex = LoadShader(ShaderType.VertexShader, vertexSrc);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentSrc);
            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            _gl.LinkProgram(_handle);
            _gl.GetProgram(_handle, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_handle)}");
            }
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
            
            // Create and set up the UBO
            SetupUniformBuffer();
        }
        
        private unsafe void SetupUniformBuffer()
        {
            // Create a Uniform Buffer Object for the transform matrices
            _transformUBO = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, _transformUBO);
            
            // Allocate space for 3 Matrix4x4 (64 bytes each = 192 bytes total)
            _gl.BufferData(BufferTargetARB.UniformBuffer, 192, (void*)0, BufferUsageARB.DynamicDraw);
            
            // Bind the UBO to binding point 0 (which matches the shader's binding = 0)
            _gl.BindBufferBase(BufferTargetARB.UniformBuffer, 0, _transformUBO);
            
            // Find and bind the uniform block in the shader program
            uint blockIndex = _gl.GetUniformBlockIndex(_handle, "block_SLANG_ParameterGroup_TransformBuffer_std140_0");
            if (blockIndex != 0xFFFFFFFF) // GL_INVALID_INDEX
            {
                _gl.UniformBlockBinding(_handle, blockIndex, 0);
                Console.WriteLine($"OpenGL: Successfully bound uniform block to binding point 0");
            }
            else
            {
                Console.WriteLine($"OpenGL: Warning - Could not find uniform block 'block_SLANG_ParameterGroup_TransformBuffer_std140_0'");
            }
            
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);
        }

        public void Use()
        {
            _gl.UseProgram(_handle);
        }

        public void SetUniform(string name, int value)
        {
            
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public unsafe void SetUniform(string name, TransformBuffer value)
        {
            // For OpenGL with UBO, we need to update the uniform buffer object
            // The matrices need to be in std140 layout which means each Matrix4x4 is 64 bytes
    
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, _transformUBO);
    
            // Update the UBO with the transform data
            // Offset 0: uModel (64 bytes)
            // Offset 64: uView (64 bytes) 
            // Offset 128: uProjection (64 bytes)
    
            _gl.BufferSubData(BufferTargetARB.UniformBuffer, 0, 64, &value.uModel);
            _gl.BufferSubData(BufferTargetARB.UniformBuffer, 64, 64, &value.uView);
            _gl.BufferSubData(BufferTargetARB.UniformBuffer, 128, 64, &value.uProjection);
    
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, 0);
    
            Console.WriteLine("OpenGL: Updated UBO with transform matrices");
        }

        public void SetUniform(string name, float value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
            _gl.Uniform1(location, value);
        }

        public void Dispose()
        {
            if (_transformUBO != 0)
            {
                _gl.DeleteBuffer(_transformUBO);
                _transformUBO = 0;
            }
            _gl.DeleteProgram(_handle);
        }

        private uint LoadShader(ShaderType type, string src)
        {
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);
            string infoLog = _gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }
    }
}
