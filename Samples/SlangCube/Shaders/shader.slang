// This can probably be improved using: https://shader-slang.org/docs/coming-from-glsl/

// Global constant buffer for transforms
cbuffer TransformBuffer : register(b0)
{
    float4x4 uModel;
    float4x4 uView;
    float4x4 uProjection;
}

// Use combined sampler for GLSL compatibility
uniform sampler2D uTexture0;

// Vertex shader entry point
[shader("vertex")]
void vertexMain(
    float3 vPos : POSITION,
    float2 vUv : TEXCOORD0,
    out float4 position : SV_Position,
    out float2 fUv : TEXCOORD0)
{
    float4 pos = float4(vPos, 1.0);
    pos = mul(pos, uModel);
    pos = mul(pos, uView);
    position = mul(pos, uProjection);
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float4 sv_position : SV_Position,
    float2 fUv : TEXCOORD0) : SV_Target
{
    return texture(uTexture0, fUv);
}
