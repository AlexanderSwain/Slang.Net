using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using Silk.NET.Direct3D.Compilers;
using System;
using System.Numerics;
using Tutorial;

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

        CreateShaders();
        CreateInputLayout();
        CreateConstantBuffer();

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
    }    private void CreateConstantBuffer()
    {
        // Create constant buffer for matrices
        var bufferDesc = new BufferDesc
        {
            ByteWidth = (uint)sizeof(Matrix4x4) * 3, // Model, View, Projection matrices
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

    public void SetUniform(string name, Matrix4x4 value)
    {
        Console.WriteLine($"DirectX11Shader: Setting uniform {name} = Matrix4x4");

        // Track which matrix is being set
        switch (name)
        {
            case "uModel":
                _modelMatrix = value;
                break;
            case "uView":
                _viewMatrix = value;
                break;
            case "uProjection":
                _projectionMatrix = value;
                break;
            default:
                Console.WriteLine($"DirectX11Shader: Unknown uniform matrix {name}");
                return;
        }

        // Update the constant buffer with all three matrices
        UpdateConstantBuffer();
    }

    private void UpdateConstantBuffer()
    {
        if (_constantBuffer != null)
        {
            // Map the constant buffer and update all matrices
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
                Console.WriteLine("DirectX11Shader: Updated constant buffer with all matrices");
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