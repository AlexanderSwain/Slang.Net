using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace Tutorial
{
    public class OpenGLRenderer : IRenderer
    {
        private readonly GL _gl;
        private Shader _currentShader;
        private Texture _currentTexture;

        public OpenGLRenderer(GL gl)
        {
            _gl = gl;
        }

        public override void Initialize()
        {
            _gl.Enable(EnableCap.DepthTest);
        }

        public override void Clear()
        {
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void SetViewport(int width, int height)
        {
            _gl.Viewport(0, 0, (uint)width, (uint)height);
        }

        public override void SetShader(string vertexSource, string fragmentSource)
        {
            _currentShader?.Dispose();
            _currentShader = new Shader(_gl, vertexSource, fragmentSource);
            _currentShader.Use();
        }

        public override void SetTexture(Texture texture)
        {
            _currentTexture = texture;
            texture.Bind();
        }

        public override void SetUniform(string name, int value)
        {
            _currentShader?.SetUniform(name, value);
        }

        public override void SetUniform(string name, float value)
        {
            _currentShader?.SetUniform(name, value);
        }

        public override void SetUniform(string name, TransformBuffer value)
        {
            _currentShader?.SetUniform(name, value);
        }

        public override void DrawMesh(Mesh mesh)
        {
            mesh.Bind();
            _gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
        }

        public override void Present()
        {
            // OpenGL presents automatically with swap buffers
        }

        public override void Dispose()
        {
            _currentShader?.Dispose();
        }
    }
}
