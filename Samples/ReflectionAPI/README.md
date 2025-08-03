# Slang.Net Reflection API - Comprehensive Sample

This sample demonstrates comprehensive usage of the Slang.Net Reflection API, showcasing real-world scenarios from basic to advanced usage patterns.

## What This Sample Demonstrates

### Basic Reflection Features (Samples 1-3)
1. **Basic Shader Introspection** - Getting fundamental information about compiled shaders
2. **Entry Point Analysis** - Detailed examination of shader entry points
3. **Parameter Reflection** - Exploring shader parameters and their properties

### Intermediate Reflection Features (Samples 4-6)
4. **Type System Exploration** - Deep dive into Slang's type system
5. **Resource Binding Analysis** - Understanding how resources are bound
6. **Attribute Introspection** - Examining shader attributes and metadata

### Advanced Reflection Features (Samples 7-10)
7. **Compute Shader Analysis** - Detailed analysis of compute shader characteristics
8. **Shader Optimization Insights** - Analyzing shaders for optimization opportunities
9. **Cross-Compilation Reflection** - Comparing reflection across different targets
10. **Advanced Type Layout Analysis** - Memory layout and alignment analysis

## Real-World Applications

This sample shows how to use reflection for:

- **Shader Debugging** - Understanding shader structure and parameters
- **Dynamic Resource Binding** - Automatically binding resources based on reflection
- **Shader Optimization** - Identifying performance bottlenecks and optimization opportunities
- **Cross-Platform Development** - Understanding differences between compilation targets
- **Tool Development** - Building shader editors, inspectors, and profilers
- **Runtime Validation** - Validating shader inputs and resource compatibility

## Key Features Demonstrated

### Shader Analysis
- Entry point enumeration and analysis
- Parameter type and binding inspection
- Resource usage analysis
- Memory layout examination

### Performance Insights
- Compute shader thread group optimization
- Constant buffer efficiency analysis
- Texture usage patterns
- Memory access pattern analysis

### Cross-Platform Support
- HLSL, GLSL, and SPIR-V reflection comparison
- Target-specific binding differences
- Platform optimization recommendations

### Attribute System
- Custom attribute inspection
- Metadata extraction
- Semantic analysis

## Running the Sample

```bash
cd ReflectionAPI
dotnet run
```

The sample will execute all 10 demonstration scenarios, showing progressively more advanced reflection capabilities.

## Expected Output

Each sample will display:
- ?? Basic information and statistics
- ?? Entry point details
- ?? Parameter and type information
- ?? Resource binding analysis
- ? Performance insights and recommendations
- ?? Detailed technical information

## File Structure

- `Program.cs` - Main program with 10 comprehensive reflection samples
- `ReflectionSamples.slang` - Comprehensive shader for demonstration
- `README.md` - This documentation

## Prerequisites

- .NET 9.0 or later
- Slang.Net package
- Windows (for HLSL compilation)

## Sample Shader Features

The `ReflectionSamples.slang` shader includes:

- Multiple entry points (vertex, pixel, compute)
- Various resource types (textures, buffers, samplers)
- Complex data structures
- Custom attributes
- Different shader stages
- Real-world shader techniques

This provides a comprehensive test bed for all reflection API features.