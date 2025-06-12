// Compute pipeline shader for post-processing
RWTexture2D<float4> inputTexture : register(u0);
RWTexture2D<float4> outputTexture : register(u1);

cbuffer PostProcessConstants : register(b0)
{
    float brightness;
    float contrast;
    float saturation;
    float gamma;
}

// Convert RGB to HSV
float3 rgb2hsv(float3 rgb)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(rgb.bg, K.wz), float4(rgb.gb, K.xy), step(rgb.b, rgb.g));
    float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));
    
    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

// Convert HSV to RGB
float3 hsv2rgb(float3 hsv)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(hsv.xxx + K.xyz) * 6.0 - K.www);
    return hsv.z * lerp(K.xxx, saturate(p - K.xxx), hsv.y);
}

[shader("compute")]
[numthreads(8, 8, 1)]
void postProcessMain(uint3 id : SV_DispatchThreadID)
{
    uint width, height;
    outputTexture.GetDimensions(width, height);
    
    if (id.x >= width || id.y >= height) return;
    
    float4 color = inputTexture[id.xy];
    
    // Apply brightness
    color.rgb += brightness;
    
    // Apply contrast
    color.rgb = ((color.rgb - 0.5) * contrast) + 0.5;
    
    // Apply saturation
    float3 hsv = rgb2hsv(color.rgb);
    hsv.y *= saturation;
    color.rgb = hsv2rgb(hsv);
    
    // Apply gamma correction
    color.rgb = pow(abs(color.rgb), gamma);
    
    outputTexture[id.xy] = color;
}
