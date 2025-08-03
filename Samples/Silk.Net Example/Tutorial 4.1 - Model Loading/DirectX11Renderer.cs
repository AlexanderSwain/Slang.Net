using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Windowing;

namespace Tutorial
{
    public class DirectX11Renderer : IRenderer
    {
        private readonly IWindow _window;
        private D3D11 _d3d11;
        private IDXGISwapChain _swapChain;
        private ComPtr<ID3D11Device> _device;
        private ComPtr<ID3D11DeviceContext> _deviceContext;
        private ComPtr<ID3D11RenderTargetView> _renderTargetView;
        private ComPtr<ID3D11DepthStencilView> _depthStencilView;
        private ComPtr<ID3D11VertexShader> _vertexShader;
        private ComPtr<ID3D11PixelShader> _pixelShader;
        private ComPtr<ID3D11InputLayout> _inputLayout;
        private ComPtr<ID3D11Buffer> _constantBuffer;
        private ComPtr<ID3D11Texture2D> _texture;
        private ComPtr<ID3D11ShaderResourceView> _textureView;
        private ComPtr<ID3D11SamplerState> _samplerState;

        public DirectX11Renderer(IWindow window)
        {
            _window = window;
        }

        public override void Initialize()
        {
            _d3d11 = D3D11.GetApi(_window);
            
            // Create device and swap chain
            var swapChainDesc = new SwapChainDesc
            {
                BufferDesc = new ModeDesc
                {
                    Width = (uint)_window.Size.X,
                    Height = (uint)_window.Size.Y,
                    Format = Format.FormatR8G8B8A8Unorm
                },
                SampleDesc = new SampleDesc(1, 0),
                BufferUsage = DXGI.UsageRenderTargetOutput,
                BufferCount = 1,
                OutputWindow = _window.Native!.Win32!.Value.Hwnd,
                Windowed = true
            };

            var featureLevel = D3DFeatureLevel.Level111;
            
            SilkMarshal.ThrowHResult(_d3d11.CreateDeviceAndSwapChain(
                null,
                D3DDriverType.Hardware,
                0,
                CreateDeviceFlag.None,
                &featureLevel,
                1,
                D3D11.SdkVersion,
                &swapChainDesc,
                ref _swapChain,
                ref _device,
                null,
                ref _deviceContext));

            // Create render target view
            ComPtr<ID3D11Texture2D> backBuffer = default;
            SilkMarshal.ThrowHResult(_swapChain.GetBuffer(0, ref backBuffer));
            SilkMarshal.ThrowHResult(_device.Get().CreateRenderTargetView(backBuffer, null, ref _renderTargetView));

            // Create depth stencil buffer
            var depthDesc = new Texture2DDesc
            {
                Width = (uint)_window.Size.X,
                Height = (uint)_window.Size.Y,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.FormatD24UnormS8Uint,
                SampleDesc = new SampleDesc(1, 0),
                Usage = Usage.Default,
                BindFlags = (uint)BindFlag.DepthStencil
            };

            ComPtr<ID3D11Texture2D> depthStencilBuffer = default;
            SilkMarshal.ThrowHResult(_device.Get().CreateTexture2D(&depthDesc, null, ref depthStencilBuffer));

            var depthViewDesc = new DepthStencilViewDesc
            {
                Format = Format.FormatD24UnormS8Uint,
                ViewDimension = DsvDimension.Texture2D
            };

            SilkMarshal.ThrowHResult(_device.Get().CreateDepthStencilView(depthStencilBuffer, &depthViewDesc, ref _depthStencilView));

            // Set render targets
            var rtv = _renderTargetView.GetPinnableReference();
            var dsv = _depthStencilView.GetPinnableReference();
            _deviceContext.Get().OMSetRenderTargets(1, &rtv, dsv);

            // Create constant buffer
            var cbDesc = new BufferDesc
            {
                ByteWidth = (uint)Marshal.SizeOf<UniformsBuffer>(),
                Usage = Usage.Dynamic,
                BindFlags = (uint)BindFlag.ConstantBuffer,
                CPUAccessFlags = (uint)CpuAccessFlag.Write
            };

            SilkMarshal.ThrowHResult(_device.Get().CreateBuffer(&cbDesc, null, ref _constantBuffer));

            // Create sampler state
            var samplerDesc = new SamplerDesc
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                ComparisonFunc = ComparisonFunc.Never,
                MinLod = 0,
                MaxLod = float.MaxValue
            };

            SilkMarshal.ThrowHResult(_device.Get().CreateSamplerState(&samplerDesc, ref _samplerState));
        }

        public override void Clear()
        {
            var clearColor = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            _deviceContext.Get().ClearRenderTargetView(_renderTargetView, clearColor);
            _deviceContext.Get().ClearDepthStencilView(_depthStencilView, (uint)ClearFlag.Depth, 1.0f, 0);
        }

        public override void SetViewport(int width, int height)
        {
            var viewport = new Viewport
            {
                Width = width,
                Height = height,
                MinDepth = 0.0f,
                MaxDepth = 1.0f,
                TopLeftX = 0,
                TopLeftY = 0
            };

            _deviceContext.Get().RSSetViewports(1, &viewport);
        }

        public unsafe override void SetShader(string vertexSource, string fragmentSource)
        {
            // Compile vertex shader
            var vsBlob = CompileShader(vertexSource, "vs_5_0", "vertexMain");
            SilkMarshal.ThrowHResult(_device.Get().CreateVertexShader(
                vsBlob.GetBufferPointer(),
                vsBlob.GetBufferSize(),
                null,
                ref _vertexShader));

            // Compile pixel shader  
            var psBlob = CompileShader(fragmentSource, "ps_5_0", "fragmentMain");
            SilkMarshal.ThrowHResult(_device.Get().CreatePixelShader(
                psBlob.GetBufferPointer(),
                psBlob.GetBufferSize(),
                null,
                ref _pixelShader));

            // Create input layout
            var inputElements = new InputElementDesc[]
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
                    AlignedByteOffset = 12,
                    InputSlotClass = InputClassification.PerVertexData,
                    InstanceDataStepRate = 0
                }
            };

            fixed (InputElementDesc* pInputElements = inputElements)
            {
                SilkMarshal.ThrowHResult(_device.Get().CreateInputLayout(
                    pInputElements,
                    (uint)inputElements.Length,
                    vsBlob.GetBufferPointer(),
                    vsBlob.GetBufferSize(),
                    ref _inputLayout));
            }

            // Set shaders
            _deviceContext.Get().VSSetShader(_vertexShader, null, 0);
            _deviceContext.Get().PSSetShader(_pixelShader, null, 0);
            _deviceContext.Get().IASetInputLayout(_inputLayout);
        }

        private unsafe ComPtr<ID3DBlob> CompileShader(string source, string target, string entryPoint)
        {
            ComPtr<ID3DBlob> blob = default;
            ComPtr<ID3DBlob> errors = default;

            // This is a simplified version - you'd want to use D3DCompile from d3dcompiler.dll
            // For now, we'll assume the shaders are already compiled HLSL
            var sourceBytes = System.Text.Encoding.UTF8.GetBytes(source);
            fixed (byte* pSource = sourceBytes)
            {
                // Note: This would need actual D3DCompile integration
                // For this example, we're simplifying
                throw new NotImplementedException("D3DCompile integration needed");
            }
        }

        public override void SetTexture(Texture texture)
        {
            // This would need to convert the OpenGL texture to a DirectX texture
            // For now, we'll skip this implementation
        }

        public override void SetUniform(string name, int value)
        {
            // DirectX uses constant buffers instead of individual uniforms
            // This would need to update the constant buffer
        }

        public override void SetUniform(string name, float value)
        {
            // DirectX uses constant buffers instead of individual uniforms
        }

        public override void SetUniform(string name, Matrix4x4 value)
        {
            // Update the constant buffer with the matrix
            // This is a simplified implementation
        }

        public override void DrawMesh(Mesh mesh)
        {
            // This would need to convert the OpenGL mesh to DirectX vertex/index buffers
        }

        public override void Present()
        {
            _swapChain.Present(1, 0);
        }

        public override void Dispose()
        {
            _renderTargetView.Dispose();
            _depthStencilView.Dispose();
            _vertexShader.Dispose();
            _pixelShader.Dispose();
            _inputLayout.Dispose();
            _constantBuffer.Dispose();
            _texture.Dispose();
            _textureView.Dispose();
            _samplerState.Dispose();
            _deviceContext.Dispose();
            _device.Dispose();
            _swapChain?.Release();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct UniformsBuffer
        {
            public Matrix4x4 uModel;
            public Matrix4x4 uView;
            public Matrix4x4 uProjection;
        }
    }
}
