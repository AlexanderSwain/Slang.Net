using Silk.NET.Assimp;
using Silk.NET.Windowing;
using System;
using System.Numerics;
using static Tutorial.Shader;

namespace Tutorial
{
    public abstract class IRenderer : IDisposable
    {
        public abstract void Clear(Vector4 clearColor);
        public abstract void SetViewport(int width, int height);
        public abstract void SetRenderTargets();
        public abstract void Present();
        public abstract void EnableDepth();
        public abstract void DisableCulling();
        public abstract IMesh CreateCubeMesh(string vertexSource, string fragmentSource, string texturePath, string modelPath);

        public void Render(IWindow window, IMesh mesh, Camera camera)
        {
            EnableDepth();
            DisableCulling();
            SetRenderTargets();
            Clear(new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            // Use elapsed time to convert to radians to allow our cube to rotate over time
            var difference = (float)(window.Time * 100);
            var size = window.FramebufferSize;

            var model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(difference));
            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + camera.Front, camera.Up);
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.Zoom), (float)size.X / size.Y, 0.1f, 100.0f);

            // Draw mesh
            {
                TransformBuffer transformBuffer = new TransformBuffer
                {
                    uModel = model,
                    uView = view,
                    uProjection = projection
                };
                mesh.Draw(this, transformBuffer);
            }

            Present();
        }

        public abstract void Dispose();
    }
}
