// Vertex shader entry point
[shader("vertex")]
void vertexMain(
    float3 vPos : POSITION,
    float2 vUv : TEXCOORD0,
    uniform float4x4 uModel,
    uniform float4x4 uView,
    uniform float4x4 uProjection,
    out float4 position : SV_Position,
    out float2 fUv : TEXCOORD0)
{
    // Transform vertex position
    float4 worldPos = mul(uModel, float4(vPos, 1.0));
    float4 viewPos = mul(uView, worldPos);
    position = mul(uProjection, viewPos);
    
    // Pass through texture coordinates
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float2 fUv : TEXCOORD0,
    uniform Texture2D uTexture0,
    uniform SamplerState textureSampler
) : SV_Target
{
    return uTexture0.Sample(textureSampler, fUv);
}
