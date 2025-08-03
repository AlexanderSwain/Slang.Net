using System;
using System.Numerics;

namespace Tutorial
{
    // Simplified DirectX wrapper classes for demonstration
    // In a real implementation, these would use actual DirectX types
    
    public class DirectX11Shader : IDisposable
    {
        private readonly string _vertexSource;
        private readonly string _fragmentSource;

        public DirectX11Shader(string vertexSource, string fragmentSource)
        {
            _vertexSource = vertexSource;
            _fragmentSource = fragmentSource;
            Console.WriteLine("DirectX11: Created shader from HLSL sources");
            Console.WriteLine($"DirectX11: VS length: {vertexSource.Length}");
            Console.WriteLine($"DirectX11: PS length: {fragmentSource.Length}");
        }

        public void Use()
        {
            Console.WriteLine("DirectX11: Using shader");
        }

        public void SetUniform(string name, int value)
        {
            Console.WriteLine($"DirectX11: Setting uniform {name} = {value}");
        }

        public void SetUniform(string name, float value)
        {
            Console.WriteLine($"DirectX11: Setting uniform {name} = {value}");
        }

        public void SetUniform(string name, Matrix4x4 value)
        {
            Console.WriteLine($"DirectX11: Setting uniform {name} = Matrix4x4");
        }

        public void Dispose()
        {
            Console.WriteLine("DirectX11: Disposing shader");
        }
    }

    public class DirectX11Texture : IDisposable
    {
        public DirectX11Texture(string path)
        {
            Console.WriteLine($"DirectX11: Loading texture {path}");
        }

        public void Bind()
        {
            Console.WriteLine("DirectX11: Binding texture");
        }

        public void Dispose()
        {
            Console.WriteLine("DirectX11: Disposing texture");
        }
    }

    public class DirectX11Mesh : IDisposable
    {
        public Vertex[] Vertices { get; }
        
        public DirectX11Mesh(Vertex[] vertices)
        {
            Vertices = vertices;
            Console.WriteLine($"DirectX11: Creating mesh with {vertices.Length} vertices");
        }

        public void Bind()
        {
            Console.WriteLine("DirectX11: Binding mesh");
        }

        public void Dispose()
        {
            Console.WriteLine("DirectX11: Disposing mesh");
        }
    }

    public class DirectX11Model : IDisposable
    {
        public DirectX11Mesh[] Meshes { get; }

        public DirectX11Model(string path)
        {
            Console.WriteLine($"DirectX11: Loading model {path}");
            // For demo, create a dummy mesh array
            Meshes = new DirectX11Mesh[] { new DirectX11Mesh(new Vertex[36]) }; // Cube vertices
        }

        public void Dispose()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Dispose();
            }
        }
    }
}
