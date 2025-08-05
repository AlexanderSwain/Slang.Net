using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tutorial
{
    public class BufferObject<TDataType> : IDisposable
            where TDataType : unmanaged
    {
        private uint _handle;
        private BufferTargetARB _bufferType;
        private GL _gl;

        public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType)
        {
            _gl = gl;
            _bufferType = bufferType;

            _handle = _gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(bufferType, (nuint)(data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            _gl.BindBuffer(_bufferType, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_handle);
        }
    }
    public class Mesh : IMesh, IDisposable
    {
        public Mesh(OpenGLRenderer renderer, Shader shader, Texture texture, Model model)
        {
            Renderer = renderer;
            Shader = shader;
            Texture = texture;
            Model = model;
            SetupMesh();
        }

        private OpenGLRenderer Renderer { get; }
        public Texture Texture { get; private set; }
        public Shader Shader { get; private set; }
        public Model Model { get; private set; }
        public VertexArrayObject<float, uint> VAO { get; set; }
        public BufferObject<float> VBO { get; set; }
        public BufferObject<uint> EBO { get; set; }

        public unsafe void SetupMesh()
        {
            var _gl = Renderer._gl;
            EBO = new BufferObject<uint>(_gl, Model.Indices, BufferTargetARB.ElementArrayBuffer);
            VBO = new BufferObject<float>(gl: _gl, Model.Vertices, BufferTargetARB.ArrayBuffer);
            VAO = new VertexArrayObject<float, uint>(_gl, VBO, EBO);
            VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            VAO.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);
        }

        public void Draw(IRenderer renderer, TransformBuffer transformBuffer)
        {
            if (renderer == null)
                throw new ArgumentException("Renderer is null", nameof(renderer));

            if (renderer is OpenGLRenderer glRenderer)
            {
                Shader.Use();
                Texture.Bind();
                Shader.SetUniform("uTexture0_0", 0);

                Shader.SetUniform("TransformBuffer", transformBuffer);

                Bind();
                Renderer._gl.DrawArrays(Silk.NET.OpenGL.PrimitiveType.Triangles, 0, (uint)Model.Vertices.Length);
            }
        }

        public void Bind()
        {
            VAO.Bind();
        }

        public void Dispose()
        {
            Texture?.Dispose();
            Shader?.Dispose();
            Model?.Dispose();
            VAO?.Dispose();
            VBO?.Dispose();
            EBO?.Dispose();
        }
    }

    public class Model : IDisposable
    {
        public float[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public Model(GL gl, string path, bool gamma = false)
        {
            var assimp = Silk.NET.Assimp.Assimp.GetApi();
            _assimp = assimp;
            _gl = gl;
            LoadModel(path);
        }

        private readonly GL _gl;
        private Assimp _assimp;
        private List<Texture> _texturesLoaded = new List<Texture>();
        public string Directory { get; protected set; } = string.Empty;
        public List<Mesh> Meshes { get; protected set; } = new List<Mesh>();

        private unsafe void LoadModel(string path)
        {
            var scene = _assimp.ImportFile(path, (uint)PostProcessSteps.Triangulate);

            if (scene == null || scene->MFlags == Silk.NET.Assimp.Assimp.SceneFlagsIncomplete || scene->MRootNode == null)
            {
                var error = _assimp.GetErrorStringS();
                throw new Exception(error);
            }

            Directory = path;

            ProcessNode(scene->MRootNode, scene);
        }

        private unsafe void ProcessNode(Node* node, Scene* scene)
        {
            for (var i = 0; i < node->MNumMeshes; i++)
            {
                var mesh = scene->MMeshes[node->MMeshes[i]];
                ProcessMesh(mesh, scene);

            }

            for (var i = 0; i < node->MNumChildren; i++)
            {
                ProcessNode(node->MChildren[i], scene);
            }
        }

        private unsafe void ProcessMesh(AssimpMesh* mesh, Scene* scene)
        {
            // data to fill
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            List<Texture> textures = new List<Texture>();

            // walk through each of the mesh's vertices
            for (uint i = 0; i < mesh->MNumVertices; i++)
            {
                Vertex vertex = new Vertex();
                vertex.BoneIds = new int[Vertex.MAX_BONE_INFLUENCE];
                vertex.Weights = new float[Vertex.MAX_BONE_INFLUENCE];

                vertex.Position = mesh->MVertices[i];

                // normals
                if (mesh->MNormals != null)
                    vertex.Normal = mesh->MNormals[i];
                // tangent
                if (mesh->MTangents != null)
                    vertex.Tangent = mesh->MTangents[i];
                // bitangent
                if (mesh->MBitangents != null)
                    vertex.Bitangent = mesh->MBitangents[i];

                // texture coordinates
                if (mesh->MTextureCoords[0] != null) // does the mesh contain texture coordinates?
                {
                    // a vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
                    // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
                    Vector3 texcoord3 = mesh->MTextureCoords[0][i];
                    vertex.TexCoords = new Vector2(texcoord3.X, texcoord3.Y);
                }

                vertices.Add(vertex);
            }

            // now wak through each of the mesh's faces (a face is a mesh its triangle) and retrieve the corresponding vertex indices.
            for (uint i = 0; i < mesh->MNumFaces; i++)
            {
                Face face = mesh->MFaces[i];
                // retrieve all indices of the face and store them in the indices vector
                for (uint j = 0; j < face.MNumIndices; j++)
                    indices.Add(face.MIndices[j]);
            }

            // process materials
            Material* material = scene->MMaterials[mesh->MMaterialIndex];
            // we assume a convention for sampler names in the shaders. Each diffuse texture should be named
            // as 'texture_diffuseN' where N is a sequential number ranging from 1 to MAX_SAMPLER_NUMBER. 
            // Same applies to other texture as the following list summarizes:
            // diffuse: texture_diffuseN
            // specular: texture_specularN
            // normal: texture_normalN

            // 1. diffuse maps
            var diffuseMaps = LoadMaterialTextures(material, TextureType.Diffuse, "texture_diffuse");
            if (diffuseMaps.Any())
                textures.AddRange(diffuseMaps);
            // 2. specular maps
            var specularMaps = LoadMaterialTextures(material, TextureType.Specular, "texture_specular");
            if (specularMaps.Any())
                textures.AddRange(specularMaps);
            // 3. normal maps
            var normalMaps = LoadMaterialTextures(material, TextureType.Height, "texture_normal");
            if (normalMaps.Any())
                textures.AddRange(normalMaps);
            // 4. height maps
            var heightMaps = LoadMaterialTextures(material, TextureType.Ambient, "texture_height");
            if (heightMaps.Any())
                textures.AddRange(heightMaps);

            Vertices = BuildVertices(vertices);
            Indices = BuildIndices(indices);

            // return a mesh object created from the extracted mesh data
            //var result = new Mesh(_gl, , , textures);
        }

        private unsafe List<Texture> LoadMaterialTextures(Material* mat, TextureType type, string typeName)
        {
            var textureCount = _assimp.GetMaterialTextureCount(mat, type);
            List<Texture> textures = new List<Texture>();
            for (uint i = 0; i < textureCount; i++)
            {
                AssimpString path;
                _assimp.GetMaterialTexture(mat, type, i, &path, null, null, null, null, null, null);
                bool skip = false;
                for (int j = 0; j < _texturesLoaded.Count; j++)
                {
                    if (_texturesLoaded[j].Path == path)
                    {
                        textures.Add(_texturesLoaded[j]);
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    var texture = new Texture(_gl, Directory, type);
                    texture.Path = path;
                    textures.Add(texture);
                    _texturesLoaded.Add(texture);
                }
            }
            return textures;
        }

        private float[] BuildVertices(List<Vertex> vertexCollection)
        {
            var vertices = new List<float>();

            foreach (var vertex in vertexCollection)
            {
                vertices.Add(vertex.Position.X);
                vertices.Add(vertex.Position.Y);
                vertices.Add(vertex.Position.Z);
                vertices.Add(vertex.TexCoords.X);
                vertices.Add(vertex.TexCoords.Y);
            }

            return vertices.ToArray();
        }

        private uint[] BuildIndices(List<uint> indices)
        {
            return indices.ToArray();
        }

        public void Dispose()
        {
            foreach (var mesh in Meshes)
            {
                mesh.Dispose();
            }

            _texturesLoaded = null;
        }
    }
    public class OpenGLRenderer : IRenderer
    {
        public readonly GL _gl;
        private Shader _currentShader;
        private Texture _currentTexture;

        public OpenGLRenderer(IWindow window)
        {
            _gl = GL.GetApi(window);
        }

        public override void Clear(Vector4 color)
        {
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void SetViewport(int width, int height)
        {
            _gl.Viewport(0, 0, (uint)width, (uint)height);
        }

        public override void Present()
        {
            // OpenGL presents automatically with swap buffers
        }

        public override void Dispose()
        {
            _currentShader?.Dispose();
        }

        public override void SetRenderTargets()
        {
            // Do Nothing for OpenGL as it uses the default framebuffer
        }

        public override void EnableDepth()
        {
            _gl.Enable(EnableCap.DepthTest);
        }

        public override void DisableCulling()
        {
            _gl.Disable(EnableCap.CullFace);
        }

        public override IMesh CreateCubeMesh(string vertexSource, string fragmentSource, string texturePath, string modelPath)
        {
            var shader = new Shader(_gl, vertexSource, fragmentSource);
            var texture = new Texture(_gl, "Resources\\silk.png");
            var model = new Model(_gl, "Resources\\cube.model");

            return new Mesh(this, shader, texture, model);
        }
    }
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
    public class Texture : IDisposable
    {
        private uint _handle;
        private GL _gl;

        public string Path { get; set; }
        public TextureType Type { get; }

        public unsafe Texture(GL gl, string path, TextureType type = TextureType.None)
        {
            _gl = gl;
            Path = path;
            Type = type;
            _handle = _gl.GenTexture();
            Bind();

            using (var img = Image.Load<Rgba32>(path))
            {
                gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);

                img.ProcessPixelRows(accessor =>
                {
                    for (int y = 0; y < accessor.Height; y++)
                    {
                        fixed (void* data = accessor.GetRowSpan(y))
                        {
                            gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                        }
                    }
                });
            }

            SetParameters();
        }

        public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
        {
            _gl = gl;

            _handle = _gl.GenTexture();
            Bind();

            fixed (void* d = &data[0])
            {
                _gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, d);
                SetParameters();
            }
        }

        private void SetParameters()
        {
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            _gl.ActiveTexture(textureSlot);
            _gl.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteTexture(_handle);
        }
    }
    public class Transform
    {
        //A transform abstraction.
        //For a transform we need to have a position, a scale, and a rotation,
        //depending on what application you are creating, the type for these may vary.

        //Here we have chosen a vec3 for position, float for scale and quaternion for rotation,
        //as that is the most normal to go with.
        //Another example could have been vec3, vec3, vec4, so the rotation is an axis angle instead of a quaternion

        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        public float Scale { get; set; } = 1f;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        //Note: The order here does matter.
        public Matrix4x4 ViewMatrix => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);
    }
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector2 TexCoords;
        public Vector3 Bitangent;

        public const int MAX_BONE_INFLUENCE = 4;
        public int[] BoneIds;
        public float[] Weights;
    }
    public class VertexArrayObject<TVertexType, TIndexType> : IDisposable
        where TVertexType : unmanaged
        where TIndexType : unmanaged
    {
        private uint _handle;
        private GL _gl;

        public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo, BufferObject<TIndexType> ebo)
        {
            _gl = gl;

            _handle = _gl.GenVertexArray();
            Bind();
            vbo.Bind();
            ebo.Bind();
        }

        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
        {
            _gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType), (void*)(offSet * sizeof(TVertexType)));
            _gl.EnableVertexAttribArray(index);
        }

        public void Bind()
        {
            _gl.BindVertexArray(_handle);
        }

        public void Dispose()
        {
            _gl.DeleteVertexArray(_handle);
        }
    }
}
