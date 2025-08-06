using SharpDX.WIC;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D.Compilers;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Windowing;
using Silk.NET.WindowsCodecs;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SlangCube
{
    /// <summary>
    /// Demo wrapper classes for DirectX11 rendering.
    /// These classes provide a simplified interface for demonstrating DirectX11 integration
    /// while showing the compiled HLSL shader output from Slang.
    /// </summary>

    public unsafe class DirectX11Renderer : IRenderer, IDisposable
    {
        public readonly D3D11 _d3d11;
        private readonly DXGI _dxgi;

        public ID3D11Device* Device;
        public ID3D11DeviceContext* DeviceContext;
        public IDXGISwapChain* SwapChain;
        public ID3D11RenderTargetView* RenderTargetView;
        public ID3D11DepthStencilView* DepthStencilView;
        public ID3D11Texture2D* DepthStencilBuffer;

        public DirectX11Renderer(IWindow window)
        {
            _d3d11 = D3D11.GetApi(window);
            _dxgi = DXGI.GetApi(window);

            CreateDeviceAndSwapChain(window);
            CreateRenderTargetView();
            CreateDepthStencilView(window);
            SetViewport(window.Size.X, window.Size.Y);
            SetupRasterizerState();
        }

        public override IMesh CreateCubeMesh(string vertexSource, string fragmentSource, string texturePath, string modelPath)
        {
            var shader = new DirectX11Shader(this, vertexSource, fragmentSource);
            var texture = new DirectX11Texture(this, texturePath);
            var model = new DirectX11Model(this, modelPath);

            return new DirectX11Mesh(this, shader, texture, model);
        }

        private void CreateDeviceAndSwapChain(IWindow window)
        {
            var swapChainDesc = new SwapChainDesc
            {
                BufferDesc = new ModeDesc
                {
                    Width = (uint)window.Size.X,
                    Height = (uint)window.Size.Y,
                    RefreshRate = new Silk.NET.DXGI.Rational(60, 1),
                    Format = Format.FormatR8G8B8A8Unorm
                },
                SampleDesc = new SampleDesc(1, 0),
                BufferUsage = DXGI.UsageRenderTargetOutput,
                BufferCount = 1,
                OutputWindow = window.Native!.Win32!.Value.Hwnd,
                Windowed = true,
                SwapEffect = SwapEffect.Discard
            };

            D3DFeatureLevel* featureLevels = stackalloc D3DFeatureLevel[]
            {
                D3DFeatureLevel.Level111,
                D3DFeatureLevel.Level110,
                D3DFeatureLevel.Level101,
                D3DFeatureLevel.Level100
            };

            IDXGISwapChain* swapChain = null;
            ID3D11Device* device = null;
            ID3D11DeviceContext* deviceContext = null;
            D3DFeatureLevel featureLevel;

            SilkMarshal.ThrowHResult(_d3d11.CreateDeviceAndSwapChain(
                null,
                D3DDriverType.Hardware,
                0,
                0,
                featureLevels,
                4,
                D3D11.SdkVersion,
                &swapChainDesc,
                ref swapChain,
                ref device,
                &featureLevel,
                ref deviceContext));

            Device = device;
            DeviceContext = deviceContext;
            SwapChain = swapChain;
        }

        private void CreateRenderTargetView()
        {
            ComPtr<ID3D11Texture2D> backBuffer = null;
            SilkMarshal.ThrowHResult(SwapChain->GetBuffer<ID3D11Texture2D>(0, out backBuffer));

            ID3D11RenderTargetView* renderTargetView = null;
            SilkMarshal.ThrowHResult(Device->CreateRenderTargetView(backBuffer.QueryInterface<ID3D11Resource>(), null, ref renderTargetView));

            RenderTargetView = renderTargetView;
            backBuffer.Dispose();
        }

        private void CreateDepthStencilView(IWindow window)
        {
            var depthBufferDesc = new Texture2DDesc
            {
                Width = (uint)window.Size.X,
                Height = (uint)window.Size.Y,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.FormatD24UnormS8Uint,
                SampleDesc = new SampleDesc(1, 0),
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.DepthStencil,
                CPUAccessFlags = 0,
                MiscFlags = 0
            };

            ID3D11Texture2D* depthStencilBuffer = null;
            SilkMarshal.ThrowHResult(Device->CreateTexture2D(&depthBufferDesc, null, ref depthStencilBuffer));

            var depthStencilViewDesc = new DepthStencilViewDesc
            {
                Format = Format.FormatD24UnormS8Uint,
                ViewDimension = DsvDimension.Texture2D,
                Texture2D = new Tex2DDsv { MipSlice = 0 }
            };

            ComPtr<ID3D11DepthStencilView> depthStencilView = null;
            SilkMarshal.ThrowHResult(Device->CreateDepthStencilView(depthStencilBuffer->QueryInterface<ID3D11Resource>(), &depthStencilViewDesc, ref depthStencilView));

            DepthStencilBuffer = depthStencilBuffer;
            DepthStencilView = depthStencilView;
        }

        public override void SetViewport(int width, int height)
        {
            var viewport = new Viewport
            {
                TopLeftX = 0,
                TopLeftY = 0,
                Width = width,
                Height = height,
                MinDepth = 0.0f,
                MaxDepth = 1.0f
            };

            DeviceContext->RSSetViewports(1, &viewport);
        }

        private void SetupRasterizerState()
        {
            Console.WriteLine("DirectX11: Setting up rasterizer state");

            var rasterizerDesc = new RasterizerDesc
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None, // Disable culling to see all triangles
                FrontCounterClockwise = false,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                SlopeScaledDepthBias = 0.0f,
                DepthClipEnable = true,
                ScissorEnable = false,
                MultisampleEnable = false,
                AntialiasedLineEnable = false
            };

            ID3D11RasterizerState* rasterizerState;
            var result = Device->CreateRasterizerState(&rasterizerDesc, &rasterizerState);
            if (result < 0)
            {
                Console.WriteLine($"DirectX11: Failed to create rasterizer state: {result:X}");
                return;
            }

            DeviceContext->RSSetState(rasterizerState);
            Console.WriteLine("DirectX11: Rasterizer state set (no culling)");
        }

        public override void Clear(Vector4 clearColor)
        {
            Console.WriteLine($"DirectX11: Clearing with color ({clearColor.X:F2}, {clearColor.Y:F2}, {clearColor.Z:F2}, {clearColor.W:F2})");
            var color = stackalloc float[] { clearColor.X, clearColor.Y, clearColor.Z, clearColor.W };
            DeviceContext->ClearRenderTargetView(RenderTargetView, color);
            DeviceContext->ClearDepthStencilView(DepthStencilView, (uint)ClearFlag.Depth | (uint)ClearFlag.Stencil, 1.0f, 0);
            Console.WriteLine("DirectX11: Clear completed");
        }

        public override void SetRenderTargets()
        {
            var rtv = RenderTargetView;
            DeviceContext->OMSetRenderTargets(1, &rtv, DepthStencilView);
        }

        public override void Present()
        {
            var result = SwapChain->Present(1, 0);
            if (result < 0)
            {
                Console.WriteLine($"DirectX11: Present failed with HRESULT 0x{result:X}");
            }
            else
            {
                Console.WriteLine("DirectX11: Present succeeded");
            }
        }

        public override void EnableDepth()
        {
            // Do nothing for DirectX11
        }

        public override void DisableCulling()
        {
            // Do nothing for DirectX11
        }

        public override void Dispose()
        {
            DepthStencilView->Release();
            DepthStencilBuffer->Release();
            RenderTargetView->Release();
            SwapChain->Release();
            DeviceContext->Release();
            Device->Release();
        }
    }

    public unsafe class DirectX11Shader : IDisposable
    {
        private readonly DirectX11Renderer _renderer;
        private ID3D11VertexShader* _vertexShader;
        private ID3D11PixelShader* _pixelShader;
        private ID3D11InputLayout* _inputLayout;
        private ID3D11Buffer* _constantBuffer;
        private ComPtr<ID3D10Blob> _vertexShaderBytecode;

        // Track matrices for proper constant buffer updates
        private Matrix4x4 _modelMatrix;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;

        public string VertexShader { get; }
        public string PixelShader { get; }

        public DirectX11Shader(DirectX11Renderer renderer, string vertexSource, string fragmentSource)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            VertexShader = vertexSource ?? throw new ArgumentNullException(nameof(vertexSource));
            PixelShader = fragmentSource ?? throw new ArgumentNullException(nameof(fragmentSource));

            // Initialize matrices to identity
            _modelMatrix = Matrix4x4.Identity;
            _viewMatrix = Matrix4x4.Identity;
            _projectionMatrix = Matrix4x4.Identity;

            CreateShaders();
            CreateInputLayout();
            CreateConstantBuffer(); // Skip for simple test - no uniforms needed

            // Initialize the constant buffer with identity matrices
            UpdateConstantBuffer();

            Console.WriteLine("DirectX11Shader: Created actual DirectX11 shaders");
            Console.WriteLine($"DirectX11Shader: VS length: {vertexSource.Length}");
            Console.WriteLine($"DirectX11Shader: PS length: {fragmentSource.Length}");
        }

        private void CreateShaders()
        {
            try
            {
                var d3dCompiler = D3DCompiler.GetApi();

                // Compile vertex shader from HLSL source
                ComPtr<ID3D10Blob> vertexBlob = default;
                ComPtr<ID3D10Blob> errorBlob = default;

                var vertexSource = SilkMarshal.StringToPtr(VertexShader);
                var result = d3dCompiler.Compile(
                    (void*)vertexSource,
                    (nuint)VertexShader.Length,
                    (byte*)SilkMarshal.StringToPtr("vertex_shader"),
                    null,
                    null,
                    (byte*)SilkMarshal.StringToPtr("vertexMain"),
                    (byte*)SilkMarshal.StringToPtr("vs_5_0"),
                    0,
                    0,
                    vertexBlob.GetAddressOf(),
                    errorBlob.GetAddressOf());

                if (result < 0)
                {
                    if (errorBlob.Handle != null)
                    {
                        var errorString = SilkMarshal.PtrToString((nint)errorBlob.GetBufferPointer());
                        Console.WriteLine($"DirectX11Shader: Vertex shader compilation error: {errorString}");
                    }
                    Console.WriteLine($"DirectX11Shader: Failed to compile vertex shader: 0x{result:X}");
                }
                else
                {
                    // Create vertex shader from compiled bytecode
                    ID3D11VertexShader* vs = null;
                    result = _renderer.Device->CreateVertexShader(
                        vertexBlob.GetBufferPointer(),
                        vertexBlob.GetBufferSize(),
                        null,
                        ref vs);
                    if (result < 0)
                    {
                        Console.WriteLine($"DirectX11Shader: Failed to create vertex shader: 0x{result:X}");
                    }
                    else
                    {
                        _vertexShader = vs;
                        _vertexShaderBytecode = vertexBlob; // Store for input layout creation
                        Console.WriteLine("DirectX11Shader: Successfully created vertex shader");
                    }
                }

                // Compile pixel shader from HLSL source
                ComPtr<ID3D10Blob> pixelBlob = default;
                errorBlob = default;

                var pixelSource = SilkMarshal.StringToPtr(PixelShader);
                result = d3dCompiler.Compile(
                    (void*)pixelSource,
                    (nuint)PixelShader.Length,
                    (byte*)SilkMarshal.StringToPtr("pixel_shader"),
                    null,
                    null,
                    (byte*)SilkMarshal.StringToPtr("fragmentMain"),
                    (byte*)SilkMarshal.StringToPtr("ps_5_0"),
                    0,
                    0,
                    pixelBlob.GetAddressOf(),
                    errorBlob.GetAddressOf());

                if (result < 0)
                {
                    if (errorBlob.Handle != null)
                    {
                        var errorString = SilkMarshal.PtrToString((nint)errorBlob.GetBufferPointer());
                        Console.WriteLine($"DirectX11Shader: Pixel shader compilation error: {errorString}");
                    }
                    Console.WriteLine($"DirectX11Shader: Failed to compile pixel shader: 0x{result:X}");
                }
                else
                {
                    // Create pixel shader from compiled bytecode
                    ID3D11PixelShader* ps = null;
                    result = _renderer.Device->CreatePixelShader(
                        pixelBlob.GetBufferPointer(),
                        pixelBlob.GetBufferSize(),
                        null,
                        ref ps);
                    if (result < 0)
                    {
                        Console.WriteLine($"DirectX11Shader: Failed to create pixel shader: 0x{result:X}");
                    }
                    else
                    {
                        _pixelShader = ps;
                        Console.WriteLine("DirectX11Shader: Successfully created pixel shader");
                    }
                }

                // Free allocated strings
                SilkMarshal.Free(vertexSource);
                SilkMarshal.Free(pixelSource);
                pixelBlob.Dispose();
                errorBlob.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DirectX11Shader: Exception during shader compilation: {ex.Message}");
            }
        }

        private void CreateInputLayout()
        {
            if (_vertexShaderBytecode.Handle == null)
            {
                Console.WriteLine("DirectX11Shader: No vertex shader bytecode available for input layout");
                return;
            }

            var inputElements = stackalloc InputElementDesc[]
            {
            new InputElementDesc
            {
                SemanticName = (byte*)SilkMarshal.StringToPtr("POSITION"),
                SemanticIndex = 0,
                Format = Format.FormatR32G32B32Float,
                InputSlot = 0,
                AlignedByteOffset = 0,
                InputSlotClass = InputClassification.PerVertexData,
                InstanceDataStepRate = 0
            },
            new InputElementDesc
            {
                SemanticName = (byte*)SilkMarshal.StringToPtr("TEXCOORD"),
                SemanticIndex = 0,
                Format = Format.FormatR32G32Float,
                InputSlot = 0,
                AlignedByteOffset = 12, // 3 floats for position
                InputSlotClass = InputClassification.PerVertexData,
                InstanceDataStepRate = 0
            }
        };

            ID3D11InputLayout* layout = null;
            var result = _renderer.Device->CreateInputLayout(
                inputElements,
                2,
                _vertexShaderBytecode.GetBufferPointer(),
                _vertexShaderBytecode.GetBufferSize(),
                ref layout);

            if (result < 0)
            {
                Console.WriteLine($"DirectX11Shader: Failed to create input layout: 0x{result:X}");
            }
            else
            {
                _inputLayout = layout;
                Console.WriteLine("DirectX11Shader: Successfully created input layout");
            }

            // Free the allocated strings
            SilkMarshal.Free((nint)inputElements[0].SemanticName);
            SilkMarshal.Free((nint)inputElements[1].SemanticName);
        }
        private void CreateConstantBuffer()
        {
            // Create constant buffer for matrices
            var bufferDesc = new BufferDesc
            {
                ByteWidth = (uint)(sizeof(Matrix4x4) * 3), // Model, View, Projection matrices
                Usage = Usage.Dynamic,
                BindFlags = (uint)BindFlag.ConstantBuffer,
                CPUAccessFlags = (uint)CpuAccessFlag.Write,
                MiscFlags = 0,
                StructureByteStride = 0
            };

            ID3D11Buffer* buffer = null;
            var result = _renderer.Device->CreateBuffer(&bufferDesc, null, ref buffer);
            if (result < 0)
            {
                Console.WriteLine($"DirectX11Shader: Failed to create constant buffer: 0x{result:X}");
            }
            _constantBuffer = buffer;
        }

        public void Use()
        {
            if (_vertexShader != null)
            {
                _renderer.DeviceContext->VSSetShader(_vertexShader, null, 0);
                Console.WriteLine("DirectX11Shader: Set vertex shader");
            }
            else
            {
                Console.WriteLine("DirectX11Shader: No vertex shader available");
            }

            if (_pixelShader != null)
            {
                _renderer.DeviceContext->PSSetShader(_pixelShader, null, 0);
                Console.WriteLine("DirectX11Shader: Set pixel shader");
            }
            else
            {
                Console.WriteLine("DirectX11Shader: No pixel shader available");
            }

            if (_inputLayout != null)
            {
                _renderer.DeviceContext->IASetInputLayout(_inputLayout);
                Console.WriteLine("DirectX11Shader: Set input layout");
            }
            else
            {
                Console.WriteLine("DirectX11Shader: No input layout available");
            }

            // Constant buffer disabled for simplified test
            if (_constantBuffer != null)
            {
                var cb = _constantBuffer;
                _renderer.DeviceContext->VSSetConstantBuffers(0, 1, &cb);
                Console.WriteLine("DirectX11Shader: Set constant buffer");
            }
        }

        public void SetUniform(string name, int value)
        {
            Console.WriteLine($"DirectX11Shader: Setting uniform {name} = {value}");
            // In a real implementation, you'd update the constant buffer here
        }

        public void SetUniform(string name, float value)
        {
            Console.WriteLine($"DirectX11Shader: Setting uniform {name} = {value}");
            // In a real implementation, you'd update the constant buffer here
        }

        public void SetUniform(string name, TransformBuffer value)
        {
            Console.WriteLine($"DirectX11Shader: Setting uniform {name} = Matrix4x4");

            _modelMatrix = value.uModel;
            _viewMatrix = value.uView;
            _projectionMatrix = value.uProjection;

            // Update the constant buffer with all three matrices
            UpdateConstantBuffer();
        }

        private void UpdateConstantBuffer()
        {
            if (_constantBuffer != null)
            {
                // Map the constant buffer and update all three matrices
                MappedSubresource mappedResource;
                var resource = _constantBuffer->QueryInterface<ID3D11Resource>();
                var result = _renderer.DeviceContext->Map(resource, 0, Map.WriteDiscard, 0, &mappedResource);
                if (result >= 0)
                {
                    // Copy all three matrices to the constant buffer
                    var matrixPtr = (Matrix4x4*)mappedResource.PData;
                    matrixPtr[0] = _modelMatrix;      // uModel
                    matrixPtr[1] = _viewMatrix;       // uView
                    matrixPtr[2] = _projectionMatrix; // uProjection

                    _renderer.DeviceContext->Unmap(resource, 0);
                    Console.WriteLine("DirectX11Shader: Updated constant buffer with all three matrices");
                }
                else
                {
                    Console.WriteLine($"DirectX11Shader: Failed to map constant buffer: {result}");
                }
            }
        }

        public void Dispose()
        {
            if (_constantBuffer != null)
            {
                _constantBuffer->Release();
                _constantBuffer = null;
            }

            if (_inputLayout != null)
            {
                _inputLayout->Release();
                _inputLayout = null;
            }

            if (_pixelShader != null)
            {
                _pixelShader->Release();
                _pixelShader = null;
            }

            if (_vertexShader != null)
            {
                _vertexShader->Release();
                _vertexShader = null;
            }

            Console.WriteLine("DirectX11Shader: Disposed actual DirectX11 resources");
        }
    }

    public unsafe class DirectX11Texture : IDisposable
    {
        private readonly DirectX11Renderer _renderer;
        private ID3D11Texture2D* _texture;
        private ID3D11ShaderResourceView* _shaderResourceView;
        private ID3D11SamplerState* _samplerState;

        public DirectX11Texture(DirectX11Renderer renderer, string path)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            Console.WriteLine($"DirectX11Texture: Loading texture from '{path}'");

            CreateTexture(path);
            CreateShaderResourceView();
            CreateSamplerState();
        }

        // Constructor for compatibility with existing code (demo mode)
        public DirectX11Texture(string path)
        {
            _renderer = null;
            Console.WriteLine($"DirectX11Texture: Loading texture from '{path}' (demo mode)");
        }

        private void CreateTexture(string path)
        {
            // Load texture from file
            _texture = TextureLoader.Read(_renderer.Device, path);

            // Test texture code commented out:
            // CreateTestTexture();
        }

        private void CreateTestTexture()
        {
            // Create a simple 4x4 red texture to test if texture sampling works
            const uint width = 4;
            const uint height = 4;
            const uint pixelSize = 4; // RGBA

            byte[] pixels = new byte[width * height * pixelSize];

            // Fill with red color (255, 0, 0, 255)
            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 255;     // Red
                pixels[i + 1] = 0;   // Green
                pixels[i + 2] = 0;   // Blue
                pixels[i + 3] = 255; // Alpha
            }

            var desc = new Texture2DDesc
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.FormatR8G8B8A8Unorm,
                SampleDesc = new SampleDesc(1, 0),
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.ShaderResource,
                CPUAccessFlags = 0,
                MiscFlags = 0
            };

            ID3D11Texture2D* texture = null;
            fixed (byte* pPixels = pixels)
            {
                var subResourceData = new SubresourceData
                {
                    PSysMem = pPixels,
                    SysMemPitch = width * pixelSize,
                    SysMemSlicePitch = 0
                };

                var result = _renderer.Device->CreateTexture2D(&desc, &subResourceData, &texture);
                Console.WriteLine($"DirectX11Texture: CreateTestTexture result: 0x{result:X}");
            }

            _texture = texture;
            Console.WriteLine("DirectX11Texture: Created simple red test texture (4x4)");
        }

        private void CreateShaderResourceView()
        {
            if (_renderer == null || _texture == null) return; // Demo mode or no texture

            var srvDesc = new ShaderResourceViewDesc
            {
                Format = Format.FormatR8G8B8A8Unorm,
                ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexture2D,
                Texture2D = new Tex2DSrv { MostDetailedMip = 0, MipLevels = 1 }
            };

            ID3D11ShaderResourceView* srv = null;
            var result = _renderer.Device->CreateShaderResourceView((ID3D11Resource*)_texture, &srvDesc, &srv);
            if (result < 0)
            {
                Console.WriteLine($"DirectX11Texture: Failed to create shader resource view: 0x{result:X}");
                return;
            }

            _shaderResourceView = srv;
            Console.WriteLine("DirectX11Texture: Created shader resource view");
        }

        private void CreateSamplerState()
        {
            if (_renderer == null) return; // Demo mode

            var samplerDesc = new SamplerDesc
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                ComparisonFunc = ComparisonFunc.Never,
                MinLOD = 0,
                MaxLOD = float.MaxValue
            };

            ID3D11SamplerState* sampler = null;
            var result = _renderer.Device->CreateSamplerState(&samplerDesc, ref sampler);
            if (result < 0)
            {
                Console.WriteLine($"DirectX11Texture: Failed to create sampler state: 0x{result:X}");
                return;
            }

            _samplerState = sampler;
            Console.WriteLine("DirectX11Texture: Created sampler state");
        }

        public void Bind(uint slot = 0)
        {
            if (_renderer == null)
            {
                Console.WriteLine($"DirectX11Texture: Binding to slot {slot} (demo mode)");
                return;
            }

            if (_shaderResourceView != null)
            {
                var srv = _shaderResourceView;
                _renderer.DeviceContext->PSSetShaderResources(slot, 1, &srv);
                Console.WriteLine($"DirectX11Texture: Bound shader resource view to slot {slot}");
            }

            if (_samplerState != null)
            {
                var sampler = _samplerState;
                _renderer.DeviceContext->PSSetSamplers(slot, 1, &sampler);
                Console.WriteLine($"DirectX11Texture: Bound sampler state to slot {slot}");
            }
        }

        public void Dispose()
        {
            if (_samplerState != null)
            {
                _samplerState->Release();
                _samplerState = null;
            }

            if (_shaderResourceView != null)
            {
                _shaderResourceView->Release();
                _shaderResourceView = null;
            }

            if (_texture != null)
            {
                _texture->Release();
                _texture = null;
            }

            Console.WriteLine("DirectX11Texture: Disposed DirectX11 texture resources");
        }
    }

    public unsafe class DirectX11Mesh : IMesh, IDisposable
    {
        private DirectX11Renderer _renderer;
        private ID3D11Buffer* _vertexBuffer;
        private ID3D11Buffer* _indexBuffer;
        private uint _indexCount;

        public DirectX11Shader DirectXShader { get; private set; }
        public DirectX11Texture DirectXTexture { get; private set; }
        public DirectX11Model DirectXModel { get; private set; }

        public Vertex[] Vertices { get; }
        public uint[] Indices { get; } = [
                // Front face (vertices 0-3)
                0, 1, 2,  2, 3, 0,
                
                // Back face (vertices 4-7)
                4, 5, 6,  6, 7, 4,
                
                // Left face (vertices 8-11)
                8, 9, 10,  10, 11, 8,
                
                // Right face (vertices 12-15)
                12, 13, 14,  14, 15, 12,
                
                // Top face (vertices 16-19)
                16, 17, 18,  18, 19, 16,
                
                // Bottom face (vertices 20-23)
                20, 21, 22,  22, 23, 20
            ];

        public DirectX11Mesh(DirectX11Renderer renderer, DirectX11Shader directXShader, DirectX11Texture directXTexture, DirectX11Model directXModel)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(directXShader), "DirectX11Shader must be provided");
            DirectXShader = directXShader ?? throw new ArgumentNullException(nameof(directXShader));
            DirectXTexture = directXTexture ?? throw new ArgumentNullException(nameof(directXTexture));
            DirectXModel = directXModel ?? throw new ArgumentNullException(nameof(directXModel));
            Vertices = DirectXModel.GetVertices();

            CreateVertexBuffer();
            CreateIndexBuffer();

            Console.WriteLine($"DirectX11Mesh: Created mesh with {Vertices.Length} vertices, {Indices.Length} indices");
        }

        private void CreateVertexBuffer()
        {
            if (_renderer == null) return; // Demo mode

            // Since Vertex is a managed type, we'll create a simplified vertex buffer
            // For a real implementation, you'd create a proper vertex structure
            var vertexSize = 5 * sizeof(float); // 3 floats for position + 2 floats for texcoords
            var bufferDesc = new BufferDesc
            {
                ByteWidth = (uint)(vertexSize * Vertices.Length),
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.VertexBuffer,
                CPUAccessFlags = 0,
                MiscFlags = 0,
                StructureByteStride = 0
            };

            // Create simplified vertex data: position (xyz) + texcoords (uv)
            var vertexData = new float[Vertices.Length * 5];
            for (int i = 0; i < Vertices.Length; i++)
            {
                vertexData[i * 5 + 0] = Vertices[i].Position.X;
                vertexData[i * 5 + 1] = Vertices[i].Position.Y;
                vertexData[i * 5 + 2] = Vertices[i].Position.Z;
                vertexData[i * 5 + 3] = Vertices[i].TexCoords.X;
                vertexData[i * 5 + 4] = Vertices[i].TexCoords.Y;
            }

            fixed (float* vertexPtr = vertexData)
            {
                var initialData = new SubresourceData
                {
                    PSysMem = vertexPtr,
                    SysMemPitch = 0,
                    SysMemSlicePitch = 0
                };

                ID3D11Buffer* buffer = null;
                var result = _renderer.Device->CreateBuffer(&bufferDesc, &initialData, &buffer);
                if (result < 0)
                {
                    Console.WriteLine($"DirectX11Mesh: Failed to create vertex buffer: 0x{result:X}");
                    return;
                }

                _vertexBuffer = buffer;
                Console.WriteLine($"DirectX11Mesh: Created vertex buffer ({bufferDesc.ByteWidth} bytes)");
            }
        }

        private void CreateIndexBuffer()
        {
            if (_renderer == null || Indices == null) return; // Demo mode or no indices

            var bufferDesc = new BufferDesc
            {
                ByteWidth = (uint)(sizeof(uint) * Indices.Length),
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.IndexBuffer,
                CPUAccessFlags = 0,
                MiscFlags = 0,
                StructureByteStride = 0
            };

            fixed (uint* indexPtr = Indices)
            {
                var initialData = new SubresourceData
                {
                    PSysMem = indexPtr,
                    SysMemPitch = 0,
                    SysMemSlicePitch = 0
                };

                ID3D11Buffer* buffer = null;
                var result = _renderer.Device->CreateBuffer(&bufferDesc, &initialData, &buffer);
                if (result < 0)
                {
                    Console.WriteLine($"DirectX11Mesh: Failed to create index buffer: 0x{result:X}");
                    return;
                }

                _indexBuffer = buffer;
                _indexCount = (uint)Indices.Length;
                Console.WriteLine($"DirectX11Mesh: Created index buffer ({bufferDesc.ByteWidth} bytes)");
            }
        }

        public void Bind()
        {
            if (_renderer == null)
            {
                Console.WriteLine("DirectX11Mesh: Binding mesh (demo mode)");
                return;
            }

            if (_vertexBuffer != null)
            {
                var buffer = _vertexBuffer;
                var stride = 5 * sizeof(float); // 3 floats for position + 2 floats for texcoords
                var offset = 0u;
                _renderer.DeviceContext->IASetVertexBuffers(0, 1, &buffer, (uint*)&stride, &offset);
                Console.WriteLine("DirectX11Mesh: Bound vertex buffer");
            }

            if (_indexBuffer != null)
            {
                _renderer.DeviceContext->IASetIndexBuffer(_indexBuffer, Format.FormatR32Uint, 0);
                Console.WriteLine("DirectX11Mesh: Bound index buffer");
            }

            // Set primitive topology
            _renderer.DeviceContext->IASetPrimitiveTopology(D3DPrimitiveTopology.D3D11PrimitiveTopologyTrianglelist);
        }

        public void Draw(IRenderer renderer, TransformBuffer transformBuffer)
        {
            if (_renderer == null)
                throw new ArgumentException("Renderer is null", nameof(renderer));

            if (renderer is DirectX11Renderer dxRenderer)
            {
                Bind();
                DirectXShader?.Use();
                DirectXTexture?.Bind();
                DirectXShader?.SetUniform("uTexture0", 0);
                DirectXShader?.SetUniform("uTransformBuffer", transformBuffer);

                dxRenderer.DeviceContext->DrawIndexed(_indexCount, 0, 0);
            }
            else
                throw new InvalidOperationException("Renderer must be a DirectX11Renderer to draw DirectX11Mesh");
        }

        public void Dispose()
        {
            if (_indexBuffer != null)
            {
                _indexBuffer->Release();
                _indexBuffer = null;
            }

            if (_vertexBuffer != null)
            {
                _vertexBuffer->Release();
                _vertexBuffer = null;
            }

            Console.WriteLine("DirectX11Mesh: Disposed DirectX11 mesh resources");
        }
    }

    public class DirectX11Model : IDisposable
    {
        private readonly DirectX11Renderer _renderer;

        public DirectX11Model(DirectX11Renderer renderer, string path)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            Console.WriteLine($"DirectX11Model: Loading model from '{path}'");
        }

        // Constructor for compatibility with existing code (demo mode)
        public DirectX11Model(string path)
        {
            _renderer = null;
            Console.WriteLine($"DirectX11Model: Loading model from '{path}' (demo mode)");
        }

        public Vertex[] GetVertices()
        {
            // Create cube with proper UV mapping - 24 vertices (4 per face)
            // Each face gets its own vertices with UVs from (0,0) to (1,1)
            return new Vertex[]
            {
                // Front face (Z+)
                new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), TexCoords = new Vector2(0.0f, 1.0f) },

                // Back face (Z-)
                new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), TexCoords = new Vector2(0.0f, 1.0f) },

                // Left face (X-)
                new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), TexCoords = new Vector2(0.0f, 1.0f) },

                // Right face (X+)
                new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), TexCoords = new Vector2(0.0f, 1.0f) },

                // Top face (Y+)
                new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), TexCoords = new Vector2(0.0f, 1.0f) },

                // Bottom face (Y-)
                new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), TexCoords = new Vector2(0.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), TexCoords = new Vector2(1.0f, 0.0f) },
                new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), TexCoords = new Vector2(1.0f, 1.0f) },
                new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), TexCoords = new Vector2(0.0f, 1.0f) }
            };
        }

        public void Dispose()
        {
        }
    }

    public unsafe static class TextureLoader
    {
        static SharpDX.WIC.ImagingFactory2 WIC_Factory { get; }

        static TextureLoader()
        {
            WIC_Factory = new SharpDX.WIC.ImagingFactory2();
        }
        public static ID3D11Texture2D* Read(ID3D11Device* device, string fileName)
        {
            var texture = CreateTexture2DFromBitmap(device, LoadBitmap(WIC_Factory, File.OpenRead(fileName)));
            return texture;
        }

        /// <summary> 
        /// Loads a bitmap using WIC. 
        /// </summary> 
        /// <param name="deviceManager"></param> 
        /// <param name="filename"></param> 
        /// <returns></returns> 
        public static SharpDX.WIC.BitmapSource LoadBitmap(SharpDX.WIC.ImagingFactory2 factory, Stream stream)
        {
            var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                factory,
                stream,
                SharpDX.WIC.DecodeOptions.CacheOnDemand);


            var formatConverter = new SharpDX.WIC.FormatConverter(factory);

            //TODO: Implement GIF support using bitmapDecoder.FrameCount and bitmapDecoder.GetFrame
            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                SharpDX.WIC.BitmapDitherType.None,
                null,
                0.0,
                SharpDX.WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary> 
        /// Creates a <see cref="SharpDX.Direct3D11.Texture2D"/> from a WIC <see cref="SharpDX.WIC.BitmapSource"/> 
        /// </summary> 
        /// <param name="device">The Direct3D11 device</param> 
        /// <param name="bitmapSource">The WIC bitmap source</param> 
        /// <returns>A Texture2D</returns> 
        public static ID3D11Texture2D* CreateTexture2DFromBitmap(ID3D11Device* device, BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels 
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer 
                bitmapSource.CopyPixels(stride, buffer);

                ID3D11Texture2D* result;
                Texture2DDesc desc = new Texture2DDesc
                {
                    Width = (uint)bitmapSource.Size.Width,
                    Height = (uint)bitmapSource.Size.Height,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.FormatR8G8B8A8Unorm,
                    SampleDesc = new SampleDesc(1, 0),
                    Usage = Usage.Default,
                    BindFlags = (uint)BindFlag.ShaderResource,
                    CPUAccessFlags = 0,
                    MiscFlags = 0
                };

                // Create Silk.NET SubresourceData instead of SharpDX.DataRectangle
                var initialData = new SubresourceData
                {
                    PSysMem = (void*)buffer.DataPointer,
                    SysMemPitch = (uint)stride,
                    SysMemSlicePitch = (uint)(stride * bitmapSource.Size.Height)
                };

                var createResult = device->CreateTexture2D(&desc, &initialData, &result);

                if (createResult < 0)
                {
                    Console.WriteLine($"TextureLoader: Failed to create texture from bitmap: 0x{createResult:X}");
                    return null;
                }

                return result;
            }
        }
    }
}
