// Vertex shader entry point
[shader("vertex")]
void vertexMain(
    float3 vPos: POSITION,
    float2 vUv: TEXCOORD0,
    uniform float4x4 uModel,
    out float4 position : SV_Position,
    out float2 fUv : TEXCOORD0)
{
// Apply model transformation and use hardcoded view/projection for now
    float4 worldPos = mul(uModel, float4(vPos, 1.0));
    
    // Hardcoded view matrix (camera at 0,0,3 looking at origin)
    float4x4 view = float4x4(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, -3,
        0, 0, 0, 1
    );
    
    // Hardcoded projection matrix (perspective)
    float4x4 projection = float4x4(
        1.5, 0, 0, 0,
        0, 2, 0, 0,
        0, 0, -1.002, -0.2002,
        0, 0, -1, 0
    );
    
    float4 viewPos = mul(view, worldPos);
    position = mul(projection, viewPos);
    
    // Pass through texture coordinates
    fUv = vUv;
}

// Fragment shader entry point
[shader("fragment")]
float4 fragmentMain(
    float2 fUv: TEXCOORD0,
    uniform Texture2D uTexture0,
    uniform SamplerState textureSampler
) : SV_Target
{
    return uTexture0.Sample(textureSampler, fUv); // Return solid red color for testing
}

//// Vertex shader entry point
//[shader("vertex")]
//void vertexMain(
//    float3 vPos: POSITION,
//    float2 vUv: TEXCOORD0,
//    uniform float4x4 uModel,
//    uniform float4x4 uView,
//    uniform float4x4 uProjection,
//    out float4 position: SV_Position,
//    out float2 fUv: TEXCOORD0)
//{
//    // Transform vertex position
//    float4 worldPos = mul(uModel, float4(vPos, 1.0));
//    float4 viewPos = mul(uView, worldPos);
//    position = mul(uProjection, viewPos);

//    // Pass through texture coordinates
//    fUv = vUv;
//}

//// Fragment shader entry point
//[shader("fragment")]
//float4 fragmentMain(
//    float2 fUv: TEXCOORD0,
//    uniform Texture2D uTexture0,
//    uniform SamplerState textureSampler
//) : SV_Target
//{
//    return uTexture0.Sample(textureSampler, fUv);
//}