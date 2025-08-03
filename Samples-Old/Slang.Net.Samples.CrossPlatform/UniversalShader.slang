// Universal cross-platform shader for DirectX, OpenGL, Vulkan, Metal
// This shader can be compiled to all target platforms

// Shader parameters
struct VertexInput
{
    float3 position : POSITION;
    float2 texCoord : TEXCOORD0;
    float3 normal : NORMAL;
    float4 color : COLOR0;
};

struct VertexOutput
{
    float4 position : SV_Position;
    float2 texCoord : TEXCOORD0;
    float3 normal : NORMAL;
    float4 color : COLOR0;
};

// Uniform buffers with cross-platform declarations
cbuffer Constants
{
    float4x4 worldMatrix;
    float4x4 viewMatrix;
    float4x4 projectionMatrix;
    float3 lightDirection;
    float lightIntensity;
    float4 ambientColor;
};

// Main texture sampler
Texture2D diffuseTexture;
SamplerState textureSampler;

// Vertex shader entry point
[shader("vertex")]
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    
    // Transform the position from model space to clip space
    float4 worldPosition = mul(float4(input.position, 1.0), worldMatrix);
    float4 viewPosition = mul(worldPosition, viewMatrix);
    output.position = mul(viewPosition, projectionMatrix);
    
    // Pass-through other attributes
    output.texCoord = input.texCoord;
    output.normal = mul(input.normal, (float3x3)worldMatrix);
    output.color = input.color;
    
    return output;
}

// Fragment/Pixel shader entry point
[shader("fragment")]
float4 FS(VertexOutput input) : SV_Target0
{
    // Sample the diffuse texture
    float4 texColor = diffuseTexture.Sample(textureSampler, input.texCoord);
    
    // Basic lighting calculation
    float3 normalizedNormal = normalize(input.normal);
    float3 normalizedLightDir = normalize(-lightDirection);
    float diffuseFactor = max(dot(normalizedNormal, normalizedLightDir), 0.0);
    
    float3 diffuseColor = texColor.rgb * input.color.rgb * diffuseFactor * lightIntensity;
    float3 ambient = texColor.rgb * input.color.rgb * ambientColor.rgb;
    
    // Final color
    float3 finalColor = ambient + diffuseColor;
    return float4(finalColor, texColor.a * input.color.a);
}
