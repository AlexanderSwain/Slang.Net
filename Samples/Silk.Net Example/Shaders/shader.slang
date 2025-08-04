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
    // Try different matrix multiplication order (row-major vs column-major)
    float4 pos = float4(vPos, 1.0);
    pos = mul(pos, uModel);
    pos = mul(pos, uView);
    position = mul(pos, uProjection);

    // Pass through texture coordinates
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float4 sv_position: SV_Position,
    float2 fUv : TEXCOORD0,
    uniform Texture2D uTexture0,
    uniform SamplerState textureSampler
) : SV_Target
{
    // Sample the texture at the given UV coordinates
    float4 texColor = uTexture0.Sample(textureSampler, fUv);
    return texColor;
}