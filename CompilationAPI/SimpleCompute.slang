// Simple compute shader for basic compilation testing
// Demonstrates basic compute shader structure and cross-platform compilation

// Input texture (read-only)
Texture2D<float4> inputTexture;

// Output buffer (read-write)
RWStructuredBuffer<float4> outputBuffer;

// Compute shader entry point
[shader("compute")]
[numthreads(16, 16, 1)]
void CS(uint3 dispatchThreadID : SV_DispatchThreadID)
{
    // Get texture dimensions
    uint width, height;
    inputTexture.GetDimensions(width, height);
    
    // Bounds checking
    if (dispatchThreadID.x >= width || dispatchThreadID.y >= height)
        return;
    
    // Sample input texture
    float4 color = inputTexture.Load(int3(dispatchThreadID.xy, 0));
    
    // Simple processing: convert to grayscale
    float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
    
    // Calculate linear index
    uint index = dispatchThreadID.y * width + dispatchThreadID.x;
    
    // Write to output buffer
    outputBuffer[index] = float4(gray, gray, gray, color.a);
}
