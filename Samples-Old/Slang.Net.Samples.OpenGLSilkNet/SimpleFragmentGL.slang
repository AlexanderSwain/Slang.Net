// Simple fragment shader for OpenGL
[shader("fragment")]
void FS(
    in float4 input_color : COLOR,
    out float4 output_color : SV_Target0
)
{
    output_color = input_color;
}
