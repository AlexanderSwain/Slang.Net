using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Windowing;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Tutorial
{
    /// <summary>
    /// Demo wrapper classes for DirectX11 rendering.
    /// These classes provide a simplified interface for demonstrating DirectX11 integration
    /// while showing the compiled HLSL shader output from Slang.
    /// </summary>

    public unsafe class DirectX11Renderer : IDisposable
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
            SetupViewport(window);
            SetupRasterizerState();
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

        private void SetupViewport(IWindow window)
        {
            var viewport = new Viewport
            {
                TopLeftX = 0,
                TopLeftY = 0,
                Width = window.Size.X,
                Height = window.Size.Y,
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

        public void Clear(Vector4 clearColor)
        {
            Console.WriteLine($"DirectX11: Clearing with color ({clearColor.X:F2}, {clearColor.Y:F2}, {clearColor.Z:F2}, {clearColor.W:F2})");
            var color = stackalloc float[] { clearColor.X, clearColor.Y, clearColor.Z, clearColor.W };
            DeviceContext->ClearRenderTargetView(RenderTargetView, color);
            DeviceContext->ClearDepthStencilView(DepthStencilView, (uint)ClearFlag.Depth | (uint)ClearFlag.Stencil, 1.0f, 0);
            Console.WriteLine("DirectX11: Clear completed");
        }

        public void SetRenderTargets()
        {
            var rtv = RenderTargetView;
            DeviceContext->OMSetRenderTargets(1, &rtv, DepthStencilView);
        }

        public void Present()
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

        public void Dispose()
        {
            DepthStencilView->Release();
            DepthStencilBuffer->Release();
            RenderTargetView->Release();
            SwapChain->Release();
            DeviceContext->Release();
            Device->Release();
        }
    }
    //public unsafe class DirectX11Shader : IDisposable
    //{
    //    private readonly DirectX11Renderer _renderer;
    //    private ID3D11VertexShader* _vertexShader;
    //    private ID3D11PixelShader* _pixelShader;
    //    private ID3D11InputLayout* _inputLayout;
    //    private ID3D11Buffer* _constantBuffer;

    //    public string VertexShader { get; }
    //    public string PixelShader { get; }

    //    // Constructor for simple demo usage (no actual DirectX implementation)
    //    public DirectX11Shader(string vertexSource, string fragmentSource)
    //    {
    //        VertexShader = vertexSource ?? throw new ArgumentNullException(nameof(vertexSource));
    //        PixelShader = fragmentSource ?? throw new ArgumentNullException(nameof(fragmentSource));
    //        _renderer = null; // No renderer - demo mode

    //        Console.WriteLine("DirectX11Shader: Created in demo mode (no actual DirectX objects)");
    //        Console.WriteLine($"DirectX11Shader: VS length: {vertexSource.Length}");
    //        Console.WriteLine($"DirectX11Shader: PS length: {fragmentSource.Length}");
    //    }

    //    // Constructor for full DirectX implementation
    //    public DirectX11Shader(DirectX11Renderer renderer, string vertexSource, string fragmentSource)
    //    {
    //        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    //        VertexShader = vertexSource ?? throw new ArgumentNullException(nameof(vertexSource));
    //        PixelShader = fragmentSource ?? throw new ArgumentNullException(nameof(fragmentSource));

    //        CreateShaders();
    //        CreateInputLayout();
    //        CreateConstantBuffer();

    //        Console.WriteLine("DirectX11Shader: Created actual DirectX11 shaders");
    //        Console.WriteLine($"DirectX11Shader: VS length: {vertexSource.Length}");
    //        Console.WriteLine($"DirectX11Shader: PS length: {fragmentSource.Length}");
    //    }

    //    private void CreateShaders()
    //    {
    //        if (_renderer == null) return; // Demo mode - no actual DirectX objects

    //        var d3d11 = _renderer._d3d11;

    //        // Compile vertex shader
    //        var vertexShaderBytes = System.Text.Encoding.UTF8.GetBytes(VertexShader);
    //        fixed (byte* vsPtr = vertexShaderBytes)
    //        {
    //            ID3D11VertexShader* vs = null;
    //            var result = _renderer.Device->CreateVertexShader(vsPtr, (nuint)vertexShaderBytes.Length, null, ref vs);
    //            if (result < 0)
    //            {
    //                Console.WriteLine($"DirectX11Shader: Failed to create vertex shader: 0x{result:X}");
    //                // For demo purposes, we'll continue without throwing
    //            }
    //            _vertexShader = vs;
    //        }

    //        // Compile pixel shader  
    //        var pixelShaderBytes = System.Text.Encoding.UTF8.GetBytes(PixelShader);
    //        fixed (byte* psPtr = pixelShaderBytes)
    //        {
    //            ID3D11PixelShader* ps = null;
    //            var result = _renderer.Device->CreatePixelShader(psPtr, (nuint)pixelShaderBytes.Length, null, ref ps);
    //            if (result < 0)
    //            {
    //                Console.WriteLine($"DirectX11Shader: Failed to create pixel shader: 0x{result:X}");
    //                // For demo purposes, we'll continue without throwing
    //            }
    //            _pixelShader = ps;
    //        }
    //    }

    //    private void CreateInputLayout()
    //    {
    //        if (_renderer == null) return; // Demo mode - no actual DirectX objects

    //        // Define input layout for our vertex structure
    //        var inputElements = stackalloc InputElementDesc[]
    //        {
    //            new InputElementDesc
    //            {
    //                SemanticName = (byte*)SilkMarshal.StringToPtr("POSITION"),
    //                SemanticIndex = 0,
    //                Format = Format.FormatR32G32B32Float,
    //                InputSlot = 0,
    //                AlignedByteOffset = 0,
    //                InputSlotClass = InputClassification.PerVertexData,
    //                InstanceDataStepRate = 0
    //            },
    //            new InputElementDesc
    //            {
    //                SemanticName = (byte*)SilkMarshal.StringToPtr("TEXCOORD"),
    //                SemanticIndex = 0,
    //                Format = Format.FormatR32G32Float,
    //                InputSlot = 0,
    //                AlignedByteOffset = 12, // 3 floats for position
    //                InputSlotClass = InputClassification.PerVertexData,
    //                InstanceDataStepRate = 0
    //            }
    //        };

    //        // Note: In a real implementation, you'd need the compiled vertex shader bytecode here
    //        // For demo purposes, we'll create a simple layout
    //        var vertexShaderBytes = System.Text.Encoding.UTF8.GetBytes(VertexShader);
    //        fixed (byte* vsPtr = vertexShaderBytes)
    //        {
    //            ID3D11InputLayout* layout = null;
    //            var result = _renderer.Device->CreateInputLayout(inputElements, 2, vsPtr, (nuint)vertexShaderBytes.Length, ref layout);
    //            if (result < 0)
    //            {
    //                Console.WriteLine($"DirectX11Shader: Failed to create input layout: 0x{result:X}");
    //            }
    //            _inputLayout = layout;
    //        }
    //    }

    //    private void CreateConstantBuffer()
    //    {
    //        if (_renderer == null) return; // Demo mode - no actual DirectX objects

    //        // Create constant buffer for matrices
    //        var bufferDesc = new BufferDesc
    //        {
    //            ByteWidth = (uint)sizeof(Matrix4x4) * 3, // Model, View, Projection matrices
    //            Usage = Usage.Dynamic,
    //            BindFlags = (uint)BindFlag.ConstantBuffer,
    //            CPUAccessFlags = (uint)CpuAccessFlag.Write,
    //            MiscFlags = 0,
    //            StructureByteStride = 0
    //        };

    //        ID3D11Buffer* buffer = null;
    //        var result = _renderer.Device->CreateBuffer(&bufferDesc, null, ref buffer);
    //        if (result < 0)
    //        {
    //            Console.WriteLine($"DirectX11Shader: Failed to create constant buffer: 0x{result:X}");
    //        }
    //        _constantBuffer = buffer;
    //    }

    //    public void Use()
    //    {
    //        if (_renderer == null)
    //        {
    //            Console.WriteLine("DirectX11Shader: Using shader (demo mode)");
    //            return;
    //        }

    //        if (_vertexShader != null)
    //        {
    //            _renderer.DeviceContext->VSSetShader(_vertexShader, null, 0);
    //            Console.WriteLine("DirectX11Shader: Set vertex shader");
    //        }

    //        if (_pixelShader != null)
    //        {
    //            _renderer.DeviceContext->PSSetShader(_pixelShader, null, 0);
    //            Console.WriteLine("DirectX11Shader: Set pixel shader");
    //        }

    //        if (_inputLayout != null)
    //        {
    //            _renderer.DeviceContext->IASetInputLayout(_inputLayout);
    //            Console.WriteLine("DirectX11Shader: Set input layout");
    //        }

    //        if (_constantBuffer != null)
    //        {
    //            var cb = _constantBuffer;
    //            _renderer.DeviceContext->VSSetConstantBuffers(0, 1, &cb);
    //            Console.WriteLine("DirectX11Shader: Set constant buffer");
    //        }
    //    }

    //    public void SetUniform(string name, int value)
    //    {
    //        Console.WriteLine($"DirectX11Shader: Setting uniform {name} = {value}");
    //        // In a real implementation, you'd update the constant buffer here
    //    }

    //    public void SetUniform(string name, float value)
    //    {
    //        Console.WriteLine($"DirectX11Shader: Setting uniform {name} = {value}");
    //        // In a real implementation, you'd update the constant buffer here
    //    }

    //    public void SetUniform(string name, Matrix4x4 value)
    //    {
    //        Console.WriteLine($"DirectX11Shader: Setting uniform {name} = Matrix4x4");

    //        if (_renderer == null || _constantBuffer == null) return; // Demo mode or no constant buffer

    //        // Map the constant buffer and update the matrix
    //        MappedSubresource mappedResource;
    //        var result = _renderer.DeviceContext->Map((ID3D11Resource*)_constantBuffer, 0, Map.WriteDiscard, 0, &mappedResource);
    //        if (result >= 0)
    //        {
    //            // In a real implementation, you'd copy the matrix data here based on the uniform name
    //            // For now, just copy the first matrix slot
    //            var matrixPtr = (Matrix4x4*)mappedResource.PData;
    //            *matrixPtr = value;
    //            _renderer.DeviceContext->Unmap((ID3D11Resource*)_constantBuffer, 0);
    //            Console.WriteLine($"DirectX11Shader: Updated constant buffer for {name}");
    //        }
    //        else
    //        {
    //            Console.WriteLine($"DirectX11Shader: Failed to map constant buffer: 0x{result:X}");
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (_constantBuffer != null)
    //        {
    //            _constantBuffer->Release();
    //            _constantBuffer = null;
    //        }

    //        if (_inputLayout != null)
    //        {
    //            _inputLayout->Release();
    //            _inputLayout = null;
    //        }

    //        if (_pixelShader != null)
    //        {
    //            _pixelShader->Release();
    //            _pixelShader = null;
    //        }

    //        if (_vertexShader != null)
    //        {
    //            _vertexShader->Release();
    //            _vertexShader = null;
    //        }

    //        Console.WriteLine("DirectX11Shader: Disposed actual DirectX11 resources");
    //    }
    //}

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
            if (_renderer == null) return; // Demo mode

            try
            {
                // For simplicity, create a 1x1 white texture as a placeholder
                // In a real implementation, you'd load the actual image file
                var textureDesc = new Texture2DDesc
                {
                    Width = 1,
                    Height = 1,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.FormatR8G8B8A8Unorm,
                    SampleDesc = new SampleDesc(1, 0),
                    Usage = Usage.Default,
                    BindFlags = (uint)BindFlag.ShaderResource,
                    CPUAccessFlags = 0,
                    MiscFlags = 0
                };

                // Create a 1x1 white pixel
                var whitePixel = stackalloc byte[] { 255, 255, 255, 255 }; // RGBA
                var initialData = new SubresourceData
                {
                    PSysMem = whitePixel,
                    SysMemPitch = 4, // 4 bytes per pixel
                    SysMemSlicePitch = 4
                };

                ID3D11Texture2D* texture = null;
                var result = _renderer.Device->CreateTexture2D(&textureDesc, &initialData, ref texture);
                if (result < 0)
                {
                    Console.WriteLine($"DirectX11Texture: Failed to create texture: 0x{result:X}");
                    return;
                }

                _texture = texture;
                Console.WriteLine("DirectX11Texture: Created 1x1 white texture (placeholder)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DirectX11Texture: Error creating texture: {ex.Message}");
            }
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

    public unsafe class DirectX11Mesh : IDisposable
    {
        private readonly DirectX11Renderer _renderer;
        private ID3D11Buffer* _vertexBuffer;
        private ID3D11Buffer* _indexBuffer;
        private uint _indexCount;

        public Vertex[] Vertices { get; }
        public uint[] Indices { get; } = [
                // Front face (Z+)
                4, 5, 6,
                6, 7, 4,

                // Back face (Z-)
                0, 3, 2,
                2, 1, 0,

                // Left face (X-)
                0, 4, 7,
                7, 3, 0,

                // Right face (X+)
                1, 2, 6,
                6, 5, 1,

                // Top face (Y+)
                3, 7, 6,
                6, 2, 3,

                // Bottom face (Y-)
                0, 1, 5,
                5, 4, 0
            ];

        public DirectX11Mesh(DirectX11Renderer renderer, Vertex[] vertices, uint[] indices = null)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));

            CreateVertexBuffer();
            CreateIndexBuffer();

            Console.WriteLine($"DirectX11Mesh: Created mesh with {vertices.Length} vertices, {Indices.Length} indices");
        }

        // Constructor for compatibility with existing code (demo mode)
        public DirectX11Mesh(Vertex[] vertices)
        {
            _renderer = null;
            Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            Console.WriteLine($"DirectX11Mesh: Creating mesh with {vertices.Length} vertices (demo mode)");
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

        public void Draw()
        {
            if (_renderer == null)
            {
                Console.WriteLine("DirectX11Mesh: Drawing mesh (demo mode)");
                return;
            }

            if (_indexBuffer != null && _indexCount > 0)
            {
                _renderer.DeviceContext->DrawIndexed(_indexCount, 0, 0);
                Console.WriteLine($"DirectX11Mesh: Drew {_indexCount} indices");
            }
            else if (_vertexBuffer != null)
            {
                _renderer.DeviceContext->Draw((uint)Vertices.Length, 0);
                Console.WriteLine($"DirectX11Mesh: Drew {Vertices.Length} vertices");
            }
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
        public DirectX11Mesh[] Meshes { get; }

        public DirectX11Model(DirectX11Renderer renderer, string path)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            Console.WriteLine($"DirectX11Model: Loading model from '{path}'");

            // Create some demo cube vertices
            var cubeVertices = CreateCubeVertices();
            Meshes = new DirectX11Mesh[] { new DirectX11Mesh(renderer, cubeVertices) };
        }

        // Constructor for compatibility with existing code (demo mode)
        public DirectX11Model(string path)
        {
            _renderer = null;
            Console.WriteLine($"DirectX11Model: Loading model from '{path}' (demo mode)");
            // For demo, create a dummy mesh array
            Meshes = new DirectX11Mesh[] { new DirectX11Mesh(new Vertex[36]) }; // Cube vertices
        }

        private Vertex[] CreateCubeVertices()
        {
            // Create a simple cube with texture coordinates
            return new Vertex[]
            {
                new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), TexCoords = new Vector2(0.0f, 0.0f) }, // 0
                new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), TexCoords = new Vector2(1.0f, 0.0f) }, // 1
                new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), TexCoords = new Vector2(1.0f, 1.0f) }, // 2
                new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), TexCoords = new Vector2(0.0f, 1.0f) }, // 3
                new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), TexCoords = new Vector2(0.0f, 0.0f) }, // 4
                new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), TexCoords = new Vector2(1.0f, 0.0f) }, // 5
                new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), TexCoords = new Vector2(1.0f, 1.0f) }, // 6
                new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), TexCoords = new Vector2(0.0f, 1.0f) }  // 7
            };
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
