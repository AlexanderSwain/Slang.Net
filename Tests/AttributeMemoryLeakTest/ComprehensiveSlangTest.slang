// Comprehensive test shader for SlangNative reflection API
// This shader includes various features to test all aspects of the reflection system

// ============================================================================
// INTERFACES AND GENERICS
// ============================================================================

// Generic interface for testing generic reflection
interface ILightModel<T>
{
    T calculateLighting(T input, float3 lightDir);
}

// Implementation of generic interface
struct LambertianModel : ILightModel<float3>
{
    float3 calculateLighting(float3 input, float3 lightDir)
    {
        return input * max(0.0, dot(input, lightDir));
    }
}

// Generic struct for testing type parameters
struct GenericBuffer<T, uint N>
{
    T data[N];
    uint count;
    
    T get(uint index) { return data[index]; }
    // Fixed: Remove assignment since struct fields cannot be modified in methods
    void set(uint index, T value) { 
        // Note: This is a simplified version for testing reflection
        // In a real implementation, you would need a different approach
        // or use a different pattern for mutable operations
    }
}

// ============================================================================
// CUSTOM ATTRIBUTES
// ============================================================================

// Custom attributes for testing attribute reflection
[MyCustomAttribute("test_value", 42)]
struct MyAttributeStruct
{
    float x;
    int y;
}

// Function with custom attributes
[shader("vertex")]
[MyCustomAttribute("vertex_shader", 1)]
[numthreads(1, 1, 1)]
void TestAttributeFunction()
{
    // Empty for attribute testing
}

// ============================================================================
// STRUCTURED TYPES
// ============================================================================

// Nested struct for testing field reflection
struct NestedStruct
{
    float2 position;
    float3 normal;
    float4 color;
}

// Complex struct with various field types
struct ComplexStruct
{
    // Basic types
    float scalar;
    float2 vector2;
    float3 vector3;
    float4 vector4;
    
    // Matrix types
    float3x3 matrix3x3;
    float4x4 matrix4x4;
    
    // Arrays
    float fixedArray[8];
    float dynamicArray[16];
    
    // Nested structures
    NestedStruct nested;
    
    // Generic instantiation
    GenericBuffer<float, 4> genericBuffer;
}

// ============================================================================
// CONSTANT BUFFERS AND BINDINGS
// ============================================================================

// Constant buffer for testing parameter reflection
cbuffer SceneConstants : register(b0)
{
    float4x4 viewMatrix;
    float4x4 projMatrix;
    float3 lightPosition;
    float lightIntensity;
    ComplexStruct complexData;
}

// Material constant buffer
cbuffer MaterialConstants : register(b1)
{
    float4 baseColor;
    float metallic;
    float roughness;
    float2 textureScale;
    MyAttributeStruct attributeData;
}

// ============================================================================
// RESOURCES AND TEXTURES
// ============================================================================

// Various texture types for resource reflection
Texture2D<float4> diffuseTexture : register(t0);
Texture2D<float3> normalTexture : register(t1);
Texture2D<float2> metallicRoughnessTexture : register(t2);
TextureCube<float4> environmentTexture : register(t3);
Texture2DArray<float4> shadowMaps : register(t4);

// Samplers
SamplerState linearSampler : register(s0);
SamplerState pointSampler : register(s1);
SamplerComparisonState shadowSampler : register(s2);

// Structured buffers
StructuredBuffer<ComplexStruct> inputBuffer : register(t5);
RWStructuredBuffer<ComplexStruct> outputBuffer : register(u0);

// Append/Consume buffers
AppendStructuredBuffer<float4> appendBuffer : register(u1);
ConsumeStructuredBuffer<float4> consumeBuffer : register(u2);

// Global shared memory for compute shader
groupshared float4 globalSharedData[64];

// ============================================================================
// VERTEX SHADER ENTRY POINT
// ============================================================================

struct VertexInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR0;
}

struct VertexOutput
{
    float4 position : SV_Position;
    float3 worldPosition : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR0;
}

[shader("vertex")]
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    
    // Transform position
    float4 worldPos = float4(input.position, 1.0);
    output.worldPosition = worldPos.xyz;
    output.position = mul(mul(worldPos, viewMatrix), projMatrix);
    
    // Pass through other attributes
    output.normal = input.normal;
    output.texCoord = input.texCoord * textureScale;
    output.color = input.color * baseColor;
    
    // Test generic function call
    LambertianModel lambertian;
    output.color.rgb = lambertian.calculateLighting(output.color.rgb, normalize(lightPosition - worldPos.xyz));
    
    return output;
}

// ============================================================================
// PIXEL SHADER ENTRY POINT
// ============================================================================

struct PixelOutput
{
    float4 color : SV_Target0;
    float depth : SV_Depth;
}

[shader("pixel")]
PixelOutput PS(VertexOutput input)
{
    PixelOutput output;
    
    // Sample textures
    float4 diffuse = diffuseTexture.Sample(linearSampler, input.texCoord);
    float3 normal = normalTexture.Sample(linearSampler, input.texCoord);
    float2 metallicRoughness = metallicRoughnessTexture.Sample(linearSampler, input.texCoord);
    
    // Calculate lighting
    float3 lightDir = normalize(lightPosition - input.worldPosition);
    float lambert = max(0.0, dot(input.normal, lightDir));
    
    // Combine with material properties
    output.color = diffuse * input.color;
    output.color.rgb *= lambert * lightIntensity;
    output.color.a *= metallicRoughness.x; // Use metallic as alpha modifier
    
    // Test complex struct access
    output.color.rgb += complexData.scalar * complexData.vector3;
    
    // Custom depth output
    output.depth = input.position.z;
    
    return output;
}

// ============================================================================
// GEOMETRY SHADER ENTRY POINT
// ============================================================================

[shader("geometry")]
[maxvertexcount(3)]
void GS(triangle VertexOutput input[3], inout TriangleStream<VertexOutput> outputStream)
{
    // Pass through triangle with potential modifications
    for (int i = 0; i < 3; i++)
    {
        VertexOutput output = input[i];
        
        // Test buffer access - Fixed: GetDimensions requires output parameters
        uint bufferSize, bufferStride;
        inputBuffer.GetDimensions(bufferSize, bufferStride);
        if (i < bufferSize)
        {
            ComplexStruct bufferData = inputBuffer[i];
            output.color.rgb += bufferData.nested.color.rgb;
        }
        
        outputStream.Append(output);
    }
    outputStream.RestartStrip();
}

// ============================================================================
// COMPUTE SHADER ENTRY POINT
// ============================================================================

[shader("compute")]
[numthreads(8, 8, 1)]
void CS(uint3 dispatchThreadID : SV_DispatchThreadID,
        uint3 groupThreadID : SV_GroupThreadID,
        uint3 groupID : SV_GroupID,
        uint groupIndex : SV_GroupIndex)
{
    uint index = dispatchThreadID.x + dispatchThreadID.y * 64; // Assuming 64 width
    
    // Test structured buffer read/write - Fixed: GetDimensions requires output parameters
    uint bufferSize, bufferStride;
    outputBuffer.GetDimensions(bufferSize, bufferStride);
    if (index < bufferSize)
    {
        ComplexStruct data = inputBuffer[index];
        
        // Modify the data
        data.scalar += 1.0;
        data.vector3 = normalize(data.vector3);
        data.nested.position = data.nested.position * 2.0;
        
        // Test generic buffer operations
        data.genericBuffer.set(0, data.scalar);
        data.genericBuffer.set(1, data.vector3.x);
        data.genericBuffer.count = 2;
        
        // Write back
        outputBuffer[index] = data;
        
        // Test append buffer
        if (data.scalar > 0.5)
        {
            appendBuffer.Append(float4(data.vector3, data.scalar));
        }
    }
    
    // Use global shared data
    globalSharedData[groupIndex] = float4(groupThreadID, 1.0);
    
    GroupMemoryBarrierWithGroupSync();
    
    // Use shared data
    if (groupIndex == 0)
    {
        float4 sum = float4(0, 0, 0, 0);
        for (uint i = 0; i < 64; i++)
        {
            sum += globalSharedData[i];
        }
        
        if (groupID.x == 0 && groupID.y == 0)
        {
            ComplexStruct result;
            result.scalar = sum.w;
            result.vector3 = sum.xyz;
            result.nested.color = sum;
            outputBuffer[0] = result;
        }
    }
}

// ============================================================================
// HULL SHADER ENTRY POINT (Tessellation Control)
// ============================================================================

struct HullOutput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
}

struct PatchConstantOutput
{
    float edges[3] : SV_TessFactor;
    float inside : SV_InsideTessFactor;
}

PatchConstantOutput PatchConstantFunction(InputPatch<VertexOutput, 3> patch)
{
    PatchConstantOutput output;
    
    // Simple tessellation factors
    output.edges[0] = output.edges[1] = output.edges[2] = 4.0;
    output.inside = 4.0;
    
    return output;
}

[shader("hull")]
[domain("tri")]
[partitioning("fractional_odd")]
[outputtopology("triangle_cw")]
[outputcontrolpoints(3)]
[patchconstantfunc("PatchConstantFunction")]
HullOutput HS(InputPatch<VertexOutput, 3> patch, uint id : SV_OutputControlPointID)
{
    HullOutput output;
    output.position = patch[id].worldPosition;
    output.normal = patch[id].normal;
    output.texCoord = patch[id].texCoord;
    return output;
}

// ============================================================================
// DOMAIN SHADER ENTRY POINT (Tessellation Evaluation)
// ============================================================================

[shader("domain")]
[domain("tri")]
VertexOutput DS(PatchConstantOutput patchConstant,
                float3 barycentrics : SV_DomainLocation,
                const OutputPatch<HullOutput, 3> patch)
{
    VertexOutput output;
    
    // Barycentric interpolation
    output.worldPosition = barycentrics.x * patch[0].position +
                          barycentrics.y * patch[1].position +
                          barycentrics.z * patch[2].position;
    
    output.normal = normalize(barycentrics.x * patch[0].normal +
                             barycentrics.y * patch[1].normal +
                             barycentrics.z * patch[2].normal);
    
    output.texCoord = barycentrics.x * patch[0].texCoord +
                     barycentrics.y * patch[1].texCoord +
                     barycentrics.z * patch[2].texCoord;
    
    output.color = float4(1, 1, 1, 1);
    
    // Transform to clip space
    output.position = mul(mul(float4(output.worldPosition, 1.0), viewMatrix), projMatrix);
    
    return output;
}

// ============================================================================
// ADDITIONAL FUNCTIONS FOR TESTING
// ============================================================================

// Generic function - Fixed: Remove unsupported constraint and comparison
T genericMax<T>(T a, T b)
{
    // Simple return without comparison since constraints are not supported
    return a;
}

// Function with modifiers and attributes
[MyCustomAttribute("utility_function", 999)]
static inline float4 utilityFunction(float3 input, [MyCustomAttribute("param", 1)] float scale)
{
    return float4(input * scale, 1.0);
}

// Overloaded functions for testing function overload reflection
float overloadedFunction(float x) { return x * 2.0; }
float2 overloadedFunction(float2 x) { return x * 2.0; }
float3 overloadedFunction(float3 x) { return x * 2.0; }

// Function with default parameters
float functionWithDefaults(float x, float y = 1.0, float z = 2.0)
{
    return x + y + z;
}