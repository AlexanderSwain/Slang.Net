// Simple compute shader for OpenGL
[shader("compute")]
[numthreads(16, 16, 1)]
void CS(uint3 threadID : SV_DispatchThreadID)
{
    // Simple compute shader
}
