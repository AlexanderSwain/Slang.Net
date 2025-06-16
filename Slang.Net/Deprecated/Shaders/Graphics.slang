// Graphics pipeline shaders
struct VSInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
};

struct PSInput
{
    float4 position : SV_Position;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
    float3 worldPos : TEXCOORD1;
};

cbuffer SceneConstants : register(b0)
{
    float4x4 viewMatrix;
    float4x4 projMatrix;
    float4x4 worldMatrix;
    float3 lightPos;
    float lightIntensity;
}

Texture2D<float4> diffuseTexture : register(t0);
SamplerState textureSampler : register(s0);

[shader("vertex")]
PSInput vertexMain(VSInput input)
{
    PSInput output;
    
    float4 worldPos = mul(worldMatrix, float4(input.position, 1.0));
    output.worldPos = worldPos.xyz;
    
    float4 viewPos = mul(viewMatrix, worldPos);
    output.position = mul(projMatrix, viewPos);
    
    output.normal = normalize(mul((float3x3)worldMatrix, input.normal));
    output.texCoord = input.texCoord;
    
    return output;
}

[shader("pixel")]
float4 pixelMain(PSInput input) : SV_Target
{
    float3 lightDir = normalize(lightPos - input.worldPos);
    float ndotl = max(0.0, dot(input.normal, lightDir));
    
    float4 albedo = diffuseTexture.Sample(textureSampler, input.texCoord);
    
    return albedo * ndotl * lightIntensity;
}
