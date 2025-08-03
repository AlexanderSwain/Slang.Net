using System;
using System.Numerics;

namespace Tutorial
{
    public abstract class IRenderer : IDisposable
    {
        public abstract void Initialize();
        public abstract void Clear();
        public abstract void SetViewport(int width, int height);
        public abstract void SetShader(string vertexSource, string fragmentSource);
        public abstract void SetTexture(Texture texture);
        public abstract void SetUniform(string name, int value);
        public abstract void SetUniform(string name, float value);
        public abstract void SetUniform(string name, Matrix4x4 value);
        public abstract void DrawMesh(Mesh mesh);
        public abstract void Present();
        public abstract void Dispose();
    }
}
