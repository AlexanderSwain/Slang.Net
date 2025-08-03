// Simple vertex shader with basic transformation
struct VertexInput
{
    float3 position : POSITION;
    float4 color : COLOR;
};

struct VertexOutput
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

cbuffer Constants : register(b0)
{
    float4x4 viewProjectionMatrix;
};

[shader("vertex")]
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    output.position = mul(float4(input.position, 1.0), viewProjectionMatrix);
    output.color = input.color;
    return output;
}
