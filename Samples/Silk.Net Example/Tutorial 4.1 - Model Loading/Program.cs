using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Silk.NET.Maths;

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
        private static GraphicsBackend SelectedBackend = GraphicsBackend.OpenGL; // Try DirectX11 first
        
        // Backend-specific resources
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
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = $"Slang.Net Demo - {SelectedBackend} Backend (Press B to switch, R to reload shaders)";
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
            Gl = GL.GetApi(window);

            // Initialize Slang shader compiler for OpenGL
            slangCompiler = new SlangShaderCompiler(GraphicsBackend.OpenGL);
            
            try
            {
                // Compile Slang shaders to OpenGL
                var (vertexShader, fragmentShader) = slangCompiler.CompileShaders("Shaders/shader.slang");
                
                Console.WriteLine("Successfully compiled Slang shaders for OpenGL!");
                Console.WriteLine($"Vertex shader length: {vertexShader.Length}");
                Console.WriteLine($"Fragment shader length: {fragmentShader.Length}");
                
                // Print a preview of the compiled shaders
                Console.WriteLine("\n--- Compiled OpenGL Vertex Shader Preview ---");
                Console.WriteLine(vertexShader.Length > 200 ? vertexShader.Substring(0, 200) + "..." : vertexShader);
                Console.WriteLine("\n--- Compiled OpenGL Fragment Shader Preview ---");
                Console.WriteLine(fragmentShader.Length > 200 ? fragmentShader.Substring(0, 200) + "..." : fragmentShader);
                
                // Create shader with compiled sources
                Shader = new Shader(Gl, vertexShader, fragmentShader);
                
                // Get reflection information
                var reflection = slangCompiler.GetReflection("Shaders/shader.slang");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error compiling Slang shaders for OpenGL: {ex.Message}");
                Console.WriteLine("Falling back to hardcoded GLSL shaders");
                
                // Fallback to original GLSL shaders
                string vertexShaderSource = File.ReadAllText("Shaders/shader.vert");
                string fragmentShaderSource = File.ReadAllText("Shaders/shader.frag");
                Shader = new Shader(Gl, vertexShaderSource, fragmentShaderSource);
            }
            
            Texture = new Texture(Gl, "Resources\\silk.png");
            Model = new Model(Gl, "Resources\\cube.model");
        }

        private static void InitializeDirectX11()
        {
            Console.WriteLine("DirectX11: Initializing backend (demo mode)");
            
            // Initialize Slang shader compiler for DirectX11
            slangCompiler = new SlangShaderCompiler(GraphicsBackend.DirectX11);
            
            try
            {
                // Compile Slang shaders to HLSL
                var (vertexShader, fragmentShader) = slangCompiler.CompileShaders("Shaders/shader.slang");
                
                Console.WriteLine("Successfully compiled Slang shaders for DirectX11!");
                Console.WriteLine($"Vertex shader length: {vertexShader.Length}");
                Console.WriteLine($"Fragment shader length: {fragmentShader.Length}");
                
                // Print a preview of the compiled shaders
                Console.WriteLine("\n--- Compiled DirectX11 Vertex Shader Preview ---");
                Console.WriteLine(vertexShader.Length > 300 ? vertexShader.Substring(0, 300) + "..." : vertexShader);
                Console.WriteLine("\n--- Compiled DirectX11 Fragment Shader Preview ---");
                Console.WriteLine(fragmentShader.Length > 300 ? fragmentShader.Substring(0, 300) + "..." : fragmentShader);
                
                // Create DirectX shader with compiled sources
                DirectXShader = new DirectX11Shader(vertexShader, fragmentShader);
                
                // Get reflection information
                var reflection = slangCompiler.GetReflection("Shaders/shader.slang");
                Console.WriteLine("\n--- DirectX11 Shader Reflection ---");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error compiling Slang shaders for DirectX11: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // For DirectX demo, we'll fallback to OpenGL
                Console.WriteLine("Falling back to OpenGL backend due to DirectX compilation error");
                SelectedBackend = GraphicsBackend.OpenGL;
                InitializeOpenGL();
                return;
            }
            
            DirectXTexture = new DirectX11Texture("Resources\\silk.png");
            DirectXModel = new DirectX11Model("Resources\\cube.model");
            
            // Set up a minimal OpenGL context for display (since we're in demo mode)
            Gl = GL.GetApi(window);
            
            // Create fallback OpenGL resources for actual rendering
            string fallbackVS = File.ReadAllText("Shaders/shader.vert");
            string fallbackFS = File.ReadAllText("Shaders/shader.frag");
            Shader = new Shader(Gl, fallbackVS, fallbackFS);
            Texture = new Texture(Gl, "Resources\\silk.png");
            Model = new Model(Gl, "Resources\\cube.model");
        }

        private static unsafe void OnUpdate(double deltaTime)
        {
            var moveSpeed = 2.5f * (float) deltaTime;

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
            var difference = (float) (window.Time * 100);

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
            // DirectX11 rendering demo
            Console.WriteLine($"DirectX11: Rendering frame at time {deltaTime:F2}s");
            
            // Simulate DirectX rendering calls
            DirectXTexture?.Bind();
            DirectXShader?.Use();
            DirectXShader?.SetUniform("uTexture0", 0);

            //Use elapsed time to convert to radians to allow our cube to rotate over time
            var difference = (float) (window.Time * 100);
            var size = window.FramebufferSize;

            var model = Matrix4x4.CreateRotationY(MathHelper.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathHelper.DegreesToRadians(difference));
            var view = Matrix4x4.CreateLookAt(CameraPosition, CameraPosition + CameraFront, CameraUp);
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(CameraZoom), (float)size.X / size.Y, 0.1f, 100.0f);

            foreach (var mesh in DirectXModel?.Meshes ?? new DirectX11Mesh[0])
            {
                mesh.Bind();
                DirectXShader?.Use();
                DirectXTexture?.Bind();
                DirectXShader?.SetUniform("uTexture0", 0);
                DirectXShader?.SetUniform("uModel", model);
                DirectXShader?.SetUniform("uView", view);
                DirectXShader?.SetUniform("uProjection", projection);

                Console.WriteLine($"DirectX11: Drawing mesh with {mesh.Vertices.Length} vertices");
            }
            
            // Fall back to OpenGL for actual display since this is a demo
            RenderOpenGL(deltaTime);
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
                    var (vertexShader, fragmentShader) = slangCompiler.CompileShaders("Shaders/shader.slang");
                    
                    if (SelectedBackend == GraphicsBackend.OpenGL)
                    {
                        Shader?.Dispose();
                        Shader = new Shader(Gl, vertexShader, fragmentShader);
                        Console.WriteLine("OpenGL shaders reloaded successfully!");
                    }
                    else if (SelectedBackend == GraphicsBackend.DirectX11)
                    {
                        DirectXShader?.Dispose();
                        DirectXShader = new DirectX11Shader(vertexShader, fragmentShader);
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
