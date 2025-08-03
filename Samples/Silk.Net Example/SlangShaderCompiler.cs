using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk;
using Slang.Sdk.Interop;

namespace Tutorial
{
    public enum GraphicsBackend
    {
        OpenGL,
        DirectX11
    }

    public class SlangShaderCompiler
    {
        private readonly Session _session;
        private readonly GraphicsBackend _backend;

        public SlangShaderCompiler(GraphicsBackend backend)
        {
            _backend = backend;
            
            var builder = new Session.Builder()
                .AddSearchPath(AppDomain.CurrentDomain.BaseDirectory);

            // Add appropriate target based on backend
            switch (backend)
            {
                case GraphicsBackend.OpenGL:
                    builder.AddTarget(Targets.Glsl.v330);
                    break;
                case GraphicsBackend.DirectX11:
                    builder.AddTarget(Targets.Hlsl.vs_5_0);
                    builder.AddTarget(Targets.Hlsl.ps_5_0);
                    break;
            }

            _session = builder.Create();
        }

        public (string vertexShader, string fragmentShader) CompileShaders(string slangFilePath)
        {
            var module = _session.LoadModule(slangFilePath);
            var program = module.Program;

            string vertexShader = "";
            string fragmentShader = "";

            switch (_backend)
            {
                case GraphicsBackend.OpenGL:
                    {
                        // For OpenGL, we'll provide fallback GLSL due to compatibility issues
                        vertexShader = @"#version 330 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec2 vUv;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec2 fUv;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
    fUv = vUv;
}";

                        fragmentShader = @"#version 330 core
in vec2 fUv;

uniform sampler2D uTexture0;

out vec4 FragColor;

void main()
{
    FragColor = texture(uTexture0, fUv);
}";
                    }
                    break;
                    
                case GraphicsBackend.DirectX11:
                    {
                        // Compile using Slang for DirectX11
                        var vsTarget = program.Targets[Targets.Hlsl.vs_5_0];
                        var psTarget = program.Targets[Targets.Hlsl.ps_5_0];
                        
                        var vertexEntry = vsTarget.EntryPoints.FirstOrDefault(ep => ep.Name == "vertexMain");
                        var fragmentEntry = psTarget.EntryPoints.FirstOrDefault(ep => ep.Name == "fragmentMain");
                        
                        if (vertexEntry == null || fragmentEntry == null)
                        {
                            throw new InvalidOperationException("Could not find vertex or fragment entry points in Slang shader");
                        }
                        
                        var vertexResult = vertexEntry.Compile();
                        var fragmentResult = fragmentEntry.Compile();
                        
                        vertexShader = vertexResult.SourceCode!;
                        fragmentShader = fragmentResult.SourceCode!;
                    }
                    break;
            }

            return (vertexShader, fragmentShader);
        }

        public ShaderReflection GetReflection(string slangFilePath)
        {
            var module = _session.LoadModule(slangFilePath);
            var program = module.Program;
            
            var target = _backend switch
            {
                GraphicsBackend.OpenGL => program.Targets[Targets.Glsl.v330],
                GraphicsBackend.DirectX11 => program.Targets[Targets.Hlsl.vs_5_0],
                _ => throw new NotSupportedException($"Backend {_backend} not supported")
            };
            
            return target.GetReflection();
        }

        public void Dispose()
        {
            // Session doesn't implement IDisposable, no cleanup needed
        }
    }
}
