// The texture to sample
Texture2D<float4> inputImage : register(t0);

// The output buffer
RWStructuredBuffer<uint4> outputInt : register(u0);
//RWStructuredBuffer<float4> outputFloat : register(u1);

[numthreads(32, 32, 1)]
void CS(uint3 dispatchThreadID : SV_DispatchThreadID, uint3 groupThreadID : SV_GroupThreadID, uint3 groupID : SV_GroupID)
{
    // Get the dimensions of the image
    uint width, height;
    inputImage.GetDimensions(width, height);

    // Calculate the total number of pixels
    uint totalPixels = width * height;

    // Sample the color at the current pixel
    float4 color = inputImage.Load(int3(dispatchThreadID.xy, 0));

    // Convert the color to integers in the range 0-255
    uint4 colorInt = uint4(color * 255);

    // Add the color to the output buffer using atomic operations
    InterlockedAdd(outputInt[0].x, colorInt.r);
    InterlockedAdd(outputInt[0].y, colorInt.g);
    InterlockedAdd(outputInt[0].z, colorInt.b);
    InterlockedAdd(outputInt[0].w, colorInt.a);
}
