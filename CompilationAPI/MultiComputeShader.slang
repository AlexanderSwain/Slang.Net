// MultiComputeShader.slang - Multiple compute entry points for various GPU compute tasks

// Shared constant buffer for all compute shaders
cbuffer ComputeParams
{
    uint2 dimensions;
    float time;
    float deltaTime;
    uint frameCount;
    float4 params; // Generic parameters for different compute shaders
};

// Shared resources
RWTexture2D<float4> outputTexture;
RWTexture2D<float4> tempTexture;
Texture2D<float4> inputTexture;
RWStructuredBuffer<float4> particlePositions;
RWStructuredBuffer<float4> particleVelocities;
RWStructuredBuffer<float> computeResults;

// =============================================================================
// Entry Point 1: Image Processing - Gaussian Blur
// =============================================================================
[shader("compute")]
[numthreads(8, 8, 1)]
void GaussianBlur(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= dimensions.x || id.y >= dimensions.y)
        return;

    float4 color = float4(0, 0, 0, 0);
    float totalWeight = 0.0;
    
    // 5x5 Gaussian kernel
    const float sigma = 1.5;
    const int kernelSize = 2;
    
    for (int x = -kernelSize; x <= kernelSize; x++)
    {
        for (int y = -kernelSize; y <= kernelSize; y++)
        {
            int2 samplePos = clamp(int2(id.xy) + int2(x, y), int2(0, 0), int2(dimensions) - 1);
            
            float weight = exp(-(x*x + y*y) / (2.0 * sigma * sigma));
            color += inputTexture[samplePos] * weight;
            totalWeight += weight;
        }
    }
    
    outputTexture[id.xy] = color / totalWeight;
}

// =============================================================================
// Entry Point 2: Procedural Generation - Perlin Noise
// =============================================================================
float hash(float2 p)
{
    return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
}

float noise(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);
    
    float a = hash(i);
    float b = hash(i + float2(1.0, 0.0));
    float c = hash(i + float2(0.0, 1.0));
    float d = hash(i + float2(1.0, 1.0));
    
    float2 u = f * f * (3.0 - 2.0 * f);
    
    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

[shader("compute")]
[numthreads(8, 8, 1)]
void GenerateNoise(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= dimensions.x || id.y >= dimensions.y)
        return;

    float2 uv = float2(id.xy) / float2(dimensions);
    
    // Multi-octave noise
    float noiseValue = 0.0;
    float amplitude = 1.0;
    float frequency = 4.0;
    
    for (int i = 0; i < 4; i++)
    {
        noiseValue += noise(uv * frequency + time * 0.1) * amplitude;
        amplitude *= 0.5;
        frequency *= 2.0;
    }
    
    // Create colorful patterns based on noise
    float3 color = float3(
        0.5 + 0.5 * sin(noiseValue * 6.28 + time),
        0.5 + 0.5 * sin(noiseValue * 6.28 + time + 2.09),
        0.5 + 0.5 * sin(noiseValue * 6.28 + time + 4.18)
    );
    
    outputTexture[id.xy] = float4(color, 1.0);
}

// =============================================================================
// Entry Point 3: Particle Simulation
// =============================================================================
[shader("compute")]
[numthreads(64, 1, 1)]
void UpdateParticles(uint3 id : SV_DispatchThreadID)
{
    uint particleCount = min(1024, uint(params.x)); // Max particles from params
    if (id.x >= particleCount)
        return;

    uint index = id.x;
    float4 position = particlePositions[index];
    float4 velocity = particleVelocities[index];
    
    // Apply gravity
    velocity.y -= 9.81 * deltaTime;
    
    // Update position
    position.xyz += velocity.xyz * deltaTime;
    
    // Bounce off ground
    if (position.y < 0.0)
    {
        position.y = 0.0;
        velocity.y = abs(velocity.y) * 0.8; // Energy loss on bounce
    }
    
    // Add some turbulence based on noise
    float2 noisePos = position.xz * 0.1 + time * 0.05;
    float turbulence = noise(noisePos) * 2.0 - 1.0;
    velocity.x += turbulence * deltaTime * 5.0;
    velocity.z += noise(noisePos + float2(100, 100)) * deltaTime * 5.0;
    
    // Damping
    velocity.xyz *= 0.995;
    
    // Wrap around screen edges
    if (position.x < -10.0) position.x = 10.0;
    if (position.x > 10.0) position.x = -10.0;
    if (position.z < -10.0) position.z = 10.0;
    if (position.z > 10.0) position.z = -10.0;
    
    particlePositions[index] = position;
    particleVelocities[index] = velocity;
}

// =============================================================================
// Entry Point 4: Mathematical Computation - Parallel Reduction
// =============================================================================
groupshared float sharedData[256];

[shader("compute")]
[numthreads(256, 1, 1)]
void ParallelSum(uint3 id : SV_DispatchThreadID, uint3 groupId : SV_GroupID, uint groupIndex : SV_GroupIndex)
{
    uint globalIndex = id.x;
    uint maxElements = uint(params.y); // Number of elements to sum
    
    // Load data into shared memory
    sharedData[groupIndex] = (globalIndex < maxElements) ? sin(globalIndex * 0.1 + time) : 0.0;
    
    GroupMemoryBarrierWithGroupSync();
    
    // Parallel reduction
    for (uint stride = 128; stride > 0; stride >>= 1)
    {
        if (groupIndex < stride)
        {
            sharedData[groupIndex] += sharedData[groupIndex + stride];
        }
        GroupMemoryBarrierWithGroupSync();
    }
    
    // Write result to global memory
    if (groupIndex == 0)
    {
        computeResults[groupId.x] = sharedData[0];
    }
}

// =============================================================================
// Entry Point 5: Post-Processing - Edge Detection
// =============================================================================
[shader("compute")]
[numthreads(8, 8, 1)]
void EdgeDetection(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= dimensions.x || id.y >= dimensions.y)
        return;

    // Sobel edge detection kernels
    const float sobelX[9] = {
        -1, 0, 1,
        -2, 0, 2,
        -1, 0, 1
    };
    
    const float sobelY[9] = {
        -1, -2, -1,
         0,  0,  0,
         1,  2,  1
    };
    
    float gx = 0.0, gy = 0.0;
    
    for (int y = -1; y <= 1; y++)
    {
        for (int x = -1; x <= 1; x++)
        {
            int2 samplePos = clamp(int2(id.xy) + int2(x, y), int2(0, 0), int2(dimensions) - 1);
            float4 sample = inputTexture[samplePos];
            float luminance = dot(sample.rgb, float3(0.299, 0.587, 0.114));
            
            int kernelIndex = (y + 1) * 3 + (x + 1);
            gx += luminance * sobelX[kernelIndex];
            gy += luminance * sobelY[kernelIndex];
        }
    }
    
    float magnitude = sqrt(gx * gx + gy * gy);
    float3 edgeColor = float3(magnitude, magnitude, magnitude);
    
    // Blend with original image based on params.z
    float4 original = inputTexture[id.xy];
    float blendFactor = params.z;
    
    outputTexture[id.xy] = float4(lerp(original.rgb, edgeColor, blendFactor), 1.0);
}

// =============================================================================
// Entry Point 6: GPU Raymarching - Simple SDF Rendering
// =============================================================================
float sdSphere(float3 p, float r)
{
    return length(p) - r;
}

float sdBox(float3 p, float3 b)
{
    float3 q = abs(p) - b;
    return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
}

float sceneSDF(float3 p)
{
    // Animated scene with sphere and box
    float sphere = sdSphere(p - float3(sin(time) * 2.0, 0, 0), 1.0);
    float box = sdBox(p - float3(0, cos(time * 1.5) * 1.5, 2), float3(0.8, 0.8, 0.8));
    
    return min(sphere, box);
}

float3 calculateNormal(float3 p)
{
    const float eps = 0.001;
    return normalize(float3(
        sceneSDF(p + float3(eps, 0, 0)) - sceneSDF(p - float3(eps, 0, 0)),
        sceneSDF(p + float3(0, eps, 0)) - sceneSDF(p - float3(0, eps, 0)),
        sceneSDF(p + float3(0, 0, eps)) - sceneSDF(p - float3(0, 0, eps))
    ));
}

[shader("compute")]
[numthreads(8, 8, 1)]
void Raymarching(uint3 id : SV_DispatchThreadID)
{
    if (id.x >= dimensions.x || id.y >= dimensions.y)
        return;

    float2 uv = (float2(id.xy) - float2(dimensions) * 0.5) / float(dimensions.y);
    
    // Camera setup
    float3 ro = float3(0, 0, -5); // Ray origin
    float3 rd = normalize(float3(uv, 1)); // Ray direction
    
    float t = 0.0;
    float3 color = float3(0.1, 0.1, 0.2); // Background color
    
    // Raymarching loop
    for (int i = 0; i < 64; i++)
    {
        float3 p = ro + rd * t;
        float d = sceneSDF(p);
        
        if (d < 0.001)
        {
            // Hit! Calculate lighting
            float3 normal = calculateNormal(p);
            float3 lightDir = normalize(float3(1, 1, -1));
            float diffuse = max(0.0, dot(normal, lightDir));
            
            color = float3(0.8, 0.4, 0.2) * diffuse + float3(0.1, 0.1, 0.1);
            break;
        }
        
        t += d;
        if (t > 20.0) break;
    }
    
    outputTexture[id.xy] = float4(color, 1.0);
}