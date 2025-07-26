# Comprehensive Slang Test Shader - Multiple Entry Points

This shader has been rewritten to provide comprehensive testing capabilities for your Slang.Net wrapper. It includes multiple entry points across different shader stages to thoroughly test the reflection and compilation functionality.

## Entry Points Overview

### Vertex Shaders
1. **BasicVertexShader** - Standard vertex transformation with lighting setup
2. **AnimatedVertexShader** - Includes time-based vertex animation
3. **InstancedVertexShader** - Supports instanced rendering with per-instance offsets

### Pixel Shaders
1. **BasicPixelShader** - Standard texturing and lighting
2. **MultiTargetPixelShader** - Multiple render targets (color, normal, depth)
3. **ProcceduralPixelShader** - Procedural pattern generation
4. **MultiTexturePixelShader** - Blends multiple textures

### Compute Shaders
1. **ImageProcessingCompute** - Image processing and color averaging
2. **ParticleSimulationCompute** - Simple particle physics simulation
3. **ConvolutionFilterCompute** - 3x3 Gaussian blur filter

### Geometry Shaders
1. **BasicGeometryShader** - Pass-through geometry processing
2. **WireframeGeometryShader** - Converts triangles to wireframe lines

### Tessellation Shaders
1. **HullShader** - Tessellation control with distance-based factors
2. **DomainShader** - Tessellation evaluation with displacement mapping

## Resources Used

### Textures
- `inputTexture` (t0) - Main input texture
- `secondaryTexture` (t1) - Secondary texture for blending
- `cubeTexture` (t2) - Cube map texture
- `inputVertexBuffer` (t3) - Vertex data buffer

### Samplers
- `linearSampler` (s0) - Linear filtering sampler
- `pointSampler` (s1) - Point filtering sampler

### Buffers
- `outputIntBuffer` (u0) - Integer output buffer
- `outputFloatBuffer` (u1) - Float output buffer
- `debugBuffer` (u2) - Debug data buffer

### Constant Buffers
- `ViewProjectionMatrix` (b0) - Camera and lighting data
- `WorldTransforms` (b1) - Transform and timing data
- `MaterialProperties` (b2) - Material properties

## Testing with Slang.Net Wrapper

Here's how you can test each entry point:

```csharp
using Slang.Net;

var session = new Session();
var module = session.LoadModule("ComprehensiveSlangTest.slang");

// Test vertex shaders
var basicVS = module.GetEntryPoint("BasicVertexShader");
var animatedVS = module.GetEntryPoint("AnimatedVertexShader");
var instancedVS = module.GetEntryPoint("InstancedVertexShader");

// Test pixel shaders
var basicPS = module.GetEntryPoint("BasicPixelShader");
var multiTargetPS = module.GetEntryPoint("MultiTargetPixelShader");
var proceduralPS = module.GetEntryPoint("ProcceduralPixelShader");
var multiTexturePS = module.GetEntryPoint("MultiTexturePixelShader");

// Test compute shaders
var imageCS = module.GetEntryPoint("ImageProcessingCompute");
var particleCS = module.GetEntryPoint("ParticleSimulationCompute");
var convolutionCS = module.GetEntryPoint("ConvolutionFilterCompute");

// Test geometry shaders
var basicGS = module.GetEntryPoint("BasicGeometryShader");
var wireframeGS = module.GetEntryPoint("WireframeGeometryShader");

// Test tessellation shaders
var hullHS = module.GetEntryPoint("HullShader");
var domainDS = module.GetEntryPoint("DomainShader");

// Compile each entry point to HLSL
foreach (var entryPoint in module.EntryPoints)
{
    var hlslCode = entryPoint.Compile();
    Console.WriteLine($"Entry Point: {entryPoint.Name}");
    Console.WriteLine($"Shader Stage: {entryPoint.Stage}");
    Console.WriteLine($"Generated HLSL Length: {hlslCode.Length} characters");
    Console.WriteLine();
}
```

## Key Features for Testing

1. **Multiple Shader Stages** - Tests vertex, pixel, compute, geometry, and tessellation shaders
2. **Complex Resource Binding** - Multiple textures, samplers, and buffers
3. **Advanced Features** - Instancing, tessellation, multiple render targets
4. **Error Handling** - Bounds checking and safe buffer access
5. **Real-world Scenarios** - Practical shader techniques like lighting, animation, and image processing

## Expected Outputs

When testing with your wrapper, you should be able to:

- ? Enumerate all 14 entry points
- ? Get reflection data for each entry point (parameters, resources, etc.)
- ? Compile each entry point to target languages (HLSL, GLSL, etc.)
- ? Validate resource bindings and signatures
- ? Handle different shader stages appropriately

This comprehensive shader provides an excellent test bed for validating the complete functionality of your Slang.Net wrapper across all supported shader types and features.