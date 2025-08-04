// Vertex shader entry point
[shader("vertex")]
void vertexMain(
    float3 vPos : POSITION,
    float2 vUv : TEXCOORD0,
    out float4 position : SV_Position,
    out float2 fUv : TEXCOORD0)
{
    // For testing: render vertices directly in clip space without any transformations
    // This should show geometry if the basic pipeline works
    position = float4(vPos * 0.5, 1.0); // Scale down to fit in clip space
    
    // Pass through texture coordinates
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float2 fUv : TEXCOORD0
) : SV_Target
{
    return float4(1.0, 0.0, 0.0, 1.0); // Return solid red color for testing
}
