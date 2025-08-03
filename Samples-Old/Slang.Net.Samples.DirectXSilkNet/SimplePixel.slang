// Simple pixel shader that outputs interpolated vertex color
struct VertexOutput
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

[shader("pixel")]
float4 PS(VertexOutput input) : SV_TARGET
{
    return input.color;
}
