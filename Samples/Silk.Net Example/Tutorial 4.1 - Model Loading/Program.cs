using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Direct3D11;
using Silk.NET.Windowing;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Silk.NET.Maths;
using Slang.Sdk;

namespace Tutorial
{
    class Program
    {
        private static IWindow window;
        private static GL Gl;
        private static IKeyboard primaryKeyboard;

        private static Texture Texture;
        private static Shader Shader;
        private static Model Model;
        private static SlangShaderCompiler slangCompiler;

        // Choose the graphics backend - switch between OpenGL and DirectX11
        private static GraphicsBackend SelectedBackend = GraphicsBackend.None;

        // Backend-specific resources
        private static D3D11 D3D11;
        private static DirectX11Renderer DirectXRenderer;
        private static DirectX11Shader DirectXShader;
        private static DirectX11Texture DirectXTexture;
        private static DirectX11Model DirectXModel;

        //Setup the camera's location, directions, and movement speed
        private static Vector3 CameraPosition = new Vector3(0.0f, 0.0f, 3.0f);
        private static Vector3 CameraFront = new Vector3(0.0f, 0.0f, -1.0f);
        private static Vector3 CameraUp = Vector3.UnitY;
        private static Vector3 CameraDirection = Vector3.Zero;
        private static float CameraYaw = -90f;
        private static float CameraPitch = 0f;
        private static float CameraZoom = 45f;

        //Used to track change in mouse movement to allow for moving of the Camera
        private static Vector2 LastMousePosition;

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Slang.Net Demo!");
            Console.WriteLine("Please select a graphics backend:");
            Console.WriteLine("1. OpenGL");
            Console.WriteLine("2. DirectX11");

            while (SelectedBackend == GraphicsBackend.None)
            {
                Console.Write("Enter your choice (1 or 2): ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SelectedBackend = GraphicsBackend.OpenGL;
                        Console.WriteLine("Selected OpenGL backend.");
                        break;
                    case "2":
                        SelectedBackend = GraphicsBackend.DirectX11;
                        Console.WriteLine("Selected DirectX11 backend.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice, defaulting to OpenGL.");
                        SelectedBackend = GraphicsBackend.None;
                        break;
                }
            }

            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = $"Slang.Net Demo - {SelectedBackend} Backend (Press B to switch, R to reload shaders)";
            options.API = GraphicsAPI.None;
            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.FramebufferResize += OnFramebufferResize;
            window.Closing += OnClose;

            window.Run();

            window.Dispose();
        }

        private static void OnLoad()
        {
            IInputContext input = window.CreateInput();
            primaryKeyboard = input.Keyboards.FirstOrDefault();
            if (primaryKeyboard != null)
            {
                primaryKeyboard.KeyDown += KeyDown;
            }
            for (int i = 0; i < input.Mice.Count; i++)
            {
                input.Mice[i].Cursor.CursorMode = CursorMode.Raw;
                input.Mice[i].MouseMove += OnMouseMove;
                input.Mice[i].Scroll += OnMouseWheel;
            }

            // Initialize based on selected backend
            Console.WriteLine($"Initializing {SelectedBackend} backend...");

            if (SelectedBackend == GraphicsBackend.OpenGL)
            {
                InitializeOpenGL();
            }
            else if (SelectedBackend == GraphicsBackend.DirectX11)
            {
                InitializeDirectX11();
            }
        }

        private static void InitializeOpenGL()
        {
            Console.WriteLine("OpenGL: Initializing backend (demo mode)");

            Gl = GL.GetApi(window);

            // Initialize Slang shader compiler for OpenGL
            slangCompiler = new SlangShaderCompiler("Shaders/shader.slang");

            // Compile Slang shaders to OpenGL
            var (vertexSource, fragmentSource) = slangCompiler.CompileShaders(GraphicsBackend.OpenGL);

            Console.WriteLine("Successfully compiled Slang shaders for OpenGL!");
            Console.WriteLine($"Vertex shader length: {vertexSource.Length}");
            Console.WriteLine($"Fragment shader length: {fragmentSource.Length}");

            // Print a preview of the compiled shaders
            Console.WriteLine("\n--- Compiled OpenGL Vertex Shader Preview ---");
            Console.WriteLine(vertexSource.Length > 200 ? vertexSource.Substring(0, 200) + "..." : vertexSource);
            Console.WriteLine("\n--- Compiled OpenGL Fragment Shader Preview ---");
            Console.WriteLine(fragmentSource.Length > 200 ? fragmentSource.Substring(0, 200) + "..." : fragmentSource);

            // Create shader with compiled sources
            Shader = new Shader(Gl, vertexSource, fragmentSource);

            // Get reflection information
            var reflection = slangCompiler.GetVSReflection(Targets.Glsl.es_320);

            Console.WriteLine("\n--- OpenGL Shader Reflection ---");
            Console.WriteLine($"Parameters: {reflection.Parameters.Count}");
            foreach (var param in reflection.Parameters)
            {
                Console.WriteLine($"  Parameter: {param.Name} (Kind: {param.Type.Kind})");
            }
            Console.WriteLine($"Entry Points: {reflection.EntryPoints.Count}");
            foreach (var ep in reflection.EntryPoints)
            {
                Console.WriteLine($"  Entry Point: {ep.Name} (Stage: {ep.Stage})");
            }

            Texture = new Texture(Gl, "Resources\\silk.png");
            Model = new Model(Gl, "Resources\\cube.model");
        }

        private static void InitializeDirectX11()
        {
            Console.WriteLine("DirectX11: Initializing backend");

            // Create DirectX11 renderer
            DirectXRenderer = new DirectX11Renderer(window);

            // Initialize Slang shader compiler for DirectX11
            slangCompiler = new SlangShaderCompiler("Shaders/shader.slang");

            // Compile Slang shaders to HLSL
            var (vertexSource, fragmentSource) = slangCompiler.CompileShaders(GraphicsBackend.DirectX11);

            Console.WriteLine("Successfully compiled Slang shaders for DirectX11!");
            Console.WriteLine($"Vertex shader length: {vertexSource.Length}");
            Console.WriteLine($"Fragment shader length: {fragmentSource.Length}");

            // Print a preview of the compiled shaders
            Console.WriteLine("\n--- Compiled DirectX11 Vertex Shader Preview ---");
            Console.WriteLine(vertexSource.Length > 300 ? vertexSource.Substring(0, 300) + "..." : vertexSource);
            Console.WriteLine("\n--- Compiled DirectX11 Fragment Shader Preview ---");
            Console.WriteLine(fragmentSource.Length > 300 ? fragmentSource.Substring(0, 300) + "..." : fragmentSource);

            // Create DirectX shader with compiled sources and renderer
            DirectXShader = new DirectX11Shader(DirectXRenderer, vertexSource, fragmentSource);

            // Get reflection information
            //var vsReflection = slangCompiler.GetVSReflection(Targets.Hlsl.vs_5_0);
            var psReflection = slangCompiler.GetPSReflection(Targets.Hlsl.ps_5_0);
            //Console.WriteLine(vsReflection.ToJson());
            Console.WriteLine(psReflection.ToJson());
            //var psReflection = slangCompiler.GetReflection(Targets.Hlsl.ps_5_0);

            //// Display reflection information for vs_5_0 target
            //Console.WriteLine("\n--- DirectX11 Shader Reflection ---");
            //Console.WriteLine($"Parameters: {vsReflection.Parameters.Count}");
            //foreach (var param in vsReflection.Parameters)
            //{
            //    Console.WriteLine($"  Parameter: {param.Name} (Kind: {param.Type.Kind})");
            //}
            //Console.WriteLine($"Entry Points: {vsReflection.EntryPoints.Count}");
            //foreach (var ep in vsReflection.EntryPoints)
            //{
            //    Console.WriteLine($"  Entry Point: {ep.Name} (Stage: {ep.Stage})");
            //}

            //// Display reflection information for ps_5_0 target
            //Console.WriteLine("\n--- DirectX11 Shader Reflection ---");
            //Console.WriteLine($"Parameters: {psReflection.Parameters.Count}");
            //foreach (var param in psReflection.Parameters)
            //{
            //    Console.WriteLine($"  Parameter: {param.Name} (Kind: {param.Type.Kind})");
            //}
            //Console.WriteLine($"Entry Points: {psReflection.EntryPoints.Count}");
            //foreach (var ep in psReflection.EntryPoints)
            //{
            //    Console.WriteLine($"  Entry Point: {ep.Name} (Stage: {ep.Stage})");
            //}
            //
            // Create DirectX11 texture and model
            DirectXTexture = new DirectX11Texture(DirectXRenderer, "Resources\\silk.png");
            DirectXModel = new DirectX11Model(DirectXRenderer, "Resources\\cube.model");
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            var moveSpeed = 2.5f * (float)deltaTime;

            if (primaryKeyboard.IsKeyPressed(Key.W))
            {
                //Move forwards
                CameraPosition += moveSpeed * CameraFront;
            }
            if (primaryKeyboard.IsKeyPressed(Key.S))
            {
                //Move backwards
                CameraPosition -= moveSpeed * CameraFront;
            }
            if (primaryKeyboard.IsKeyPressed(Key.A))
            {
                //Move left
                CameraPosition -= Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
            if (primaryKeyboard.IsKeyPressed(Key.D))
            {
                //Move right
                CameraPosition += Vector3.Normalize(Vector3.Cross(CameraFront, CameraUp)) * moveSpeed;
            }
        }

        private static unsafe void OnRender(double deltaTime)
        {
            if (SelectedBackend == GraphicsBackend.OpenGL)
            {
                RenderOpenGL(deltaTime);
            }
            else if (SelectedBackend == GraphicsBackend.DirectX11)
            {
                RenderDirectX11(deltaTime);
            }
        }

        private static unsafe void RenderOpenGL(double deltaTime)
        {
            Gl.Enable(EnableCap.DepthTest);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Texture.Bind();
            Shader.Use();
            Shader.SetUniform("uTexture0", 0);

            //Use elapsed time to convert to radians to allow our cube to rotate over time
            var difference = (float)(window.Time * 100);

            var size = window.FramebufferSize;

            var model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(difference));
            var view = Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            //Note that the apsect ratio calculation must be performed as a float, otherwise integer division will be performed (truncating the result).
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), (float)size.X / size.Y, 0.1f, 100.0f);

            foreach (var mesh in Model.Meshes)
            {
                mesh.Bind();
                Shader.Use();
                Texture.Bind();
                Shader.SetUniform("uTexture0", 0);
                Shader.SetUniform("uModel", model);
                Shader.SetUniform("uView", view);
                Shader.SetUniform("uProjection", projection);

                Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
            }
        }

        private static unsafe void RenderDirectX11(double deltaTime)
        {
            Console.WriteLine($"DirectX11: Rendering frame at time {deltaTime:F2}s");

            // Set render targets first, then clear
            DirectXRenderer?.SetRenderTargets();
            DirectXRenderer?.Clear(new Vector4(0.2f, 0.3f, 1.0f, 1.0f)); // Blue background

            // Use elapsed time to convert to radians to allow our cube to rotate over time
            var difference = (float)(window.Time * 100);
            var size = window.FramebufferSize;

            var model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(difference));
            var view = Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), (float)size.X / size.Y, 0.1f, 100.0f);

            // Set shader and texture
            DirectXShader?.Use();
            DirectXTexture?.Bind();
            DirectXShader?.SetUniform("uTexture0", 0);

            foreach (var mesh in DirectXModel?.Meshes ?? new DirectX11Mesh[0])
            {
                mesh.Bind();
                // Set all three matrices for proper 3D transformation
                DirectXShader?.SetUniform("uModel", model);
                DirectXShader?.SetUniform("uView", view);
                DirectXShader?.SetUniform("uProjection", projection);

                mesh.Draw(); // Actually draw the mesh
                Console.WriteLine($"DirectX11: Drawing mesh with {mesh.Vertices.Length} vertices");
            }

            // Present the frame
            DirectXRenderer?.Present();
        }

        private static void OnFramebufferResize(Vector2D<int> newSize)
        {
            Gl.Viewport(newSize);
        }

        private static unsafe void OnMouseMove(IMouse mouse, Vector2 position)
        {
            var lookSensitivity = 0.1f;
            if (LastMousePosition == default)
            {
                LastMousePosition = position;
            }
            else
            {
                var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                CameraYaw += xOffset;
                CameraPitch -= yOffset;

                //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
                CameraPitch = Math.Clamp(CameraPitch, -89.0f, 89.0f);

                CameraDirection.X = MathF.Cos(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
                CameraDirection.Y = MathF.Sin(MathHelper.DegreesToRadians(CameraPitch));
                CameraDirection.Z = MathF.Sin(MathHelper.DegreesToRadians(CameraYaw)) * MathF.Cos(MathHelper.DegreesToRadians(CameraPitch));
                CameraFront = Vector3.Normalize(CameraDirection);
            }
        }

        private static unsafe void OnMouseWheel(IMouse mouse, ScrollWheel scrollWheel)
        {
            //We don't want to be able to zoom in too close or too far away so clamp to these values
            CameraZoom = Math.Clamp(CameraZoom - scrollWheel.Y, 1.0f, 45f);
        }

        private static void OnClose()
        {
            // Dispose OpenGL resources
            Model?.Dispose();
            Shader?.Dispose();
            Texture?.Dispose();

            // Dispose DirectX resources
            DirectXModel?.Dispose();
            DirectXShader?.Dispose();
            DirectXTexture?.Dispose();
            DirectXRenderer?.Dispose();

            slangCompiler?.Dispose();
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                window.Close();
            }
            else if (key == Key.R)
            {
                // Reload shaders for testing
                Console.WriteLine($"Reloading shaders for {SelectedBackend} backend...");
                try
                {
                    var (vertexShader, fragmentShader) = slangCompiler.CompileShaders(SelectedBackend);

                    if (SelectedBackend == GraphicsBackend.OpenGL)
                    {
                        Shader?.Dispose();
                        Shader = new Shader(Gl, vertexShader, fragmentShader);
                        Console.WriteLine("OpenGL shaders reloaded successfully!");
                    }
                    else if (SelectedBackend == GraphicsBackend.DirectX11)
                    {
                        DirectXShader?.Dispose();
                        DirectXShader = new DirectX11Shader(DirectXRenderer, vertexShader, fragmentShader);
                        Console.WriteLine("DirectX11 shaders reloaded successfully!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reloading shaders: {ex.Message}");
                }
            }
            else if (key == Key.B)
            {
                // Switch between backends
                var oldBackend = SelectedBackend;
                SelectedBackend = SelectedBackend == GraphicsBackend.OpenGL ? GraphicsBackend.DirectX11 : GraphicsBackend.OpenGL;
                Console.WriteLine($"Switching from {oldBackend} to {SelectedBackend} backend...");

                // Note: In a real implementation, you'd reinitialize the entire graphics context
                Console.WriteLine("Backend switching requires restart in this demo");
            }
        }
    }
}
