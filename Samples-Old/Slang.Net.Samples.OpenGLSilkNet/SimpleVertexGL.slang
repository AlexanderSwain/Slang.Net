// Simple vertex shader for OpenGL
[shader("vertex")]
void VS(
    in float3 position : POSITION,
    in float4 color : COLOR,
    out float4 gl_Position : SV_Position,
    out float4 output_color : COLOR
)
{
    gl_Position = float4(position, 1.0);
    output_color = color;
}
