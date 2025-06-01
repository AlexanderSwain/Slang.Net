[format("rgba8")]
Texture2D<float4> myImage;

SamplerState sampler;

float4 imageMain(uint2 dispatchThreadID, int2 screenSize)
{
    float2 scaled = float2(dispatchThreadID.xy) / screenSize.y;
    float2 flipped = float2(scaled.x, 1 - scaled.y);

    float4 imageColor = myImage.SampleLevel(sampler, flipped, 0);
    return imageColor;
}