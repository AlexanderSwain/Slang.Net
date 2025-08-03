// ReflectionSamples.slang - Comprehensive shader for demonstrating reflection API features
// This shader includes various features to showcase different aspects of the reflection system

// ========================================
// Constant Buffers and Parameters
// ========================================

// Camera and view parameters
cbuffer ViewParams : register(b0)
{
    float4x4 viewMatrix;
    float4x4 projectionMatrix;
    float4x4 viewProjectionMatrix;
    float3 cameraPosition;
    float padding0;
    float3 cameraForward;
    float padding1;
}

// Material properties
cbuffer MaterialParams : register(b1)
{
    float4 baseColor;
    float metallic;
    float roughness;
    float emission;
    float padding2;
    float3 emissionColor;
    float padding3;
}

// Animation and time parameters
cbuffer AnimationParams : register(b2)
{
    float time;
    float deltaTime;
    float animationSpeed;
    float waveAmplitude;
    float4 windDirection;
    int frameCount;
    bool enableAnimation;
    bool enableWireframe;
    float lodBias;
}

// ========================================
// Textures and Samplers
// ========================================

// Main textures
Texture2D<float4> albedoTexture : register(t0);
Texture2D<float4> normalTexture : register(t1);
Texture2D<float4> metallicRoughnessTexture : register(t2);
Texture2D<float4> emissionTexture : register(t3);
TextureCube<float4> environmentTexture : register(t4);
Texture2D<float> depthTexture : register(t5);

// Compute shader textures
Texture2D<float4> inputImageTexture : register(t6);
RWTexture2D<float4> outputImageTexture : register(u0);

// Structured buffers
StructuredBuffer<float4> vertexBuffer : register(t7);
StructuredBuffer<uint> indexBuffer : register(t8);
RWStructuredBuffer<float4> particleBuffer : register(u1);
RWStructuredBuffer<uint> counterBuffer : register(u2);

// Samplers
SamplerState linearSampler : register(s0);
SamplerState pointSampler : register(s1);
SamplerState anisotropicSampler : register(s2);
SamplerComparisonState shadowSampler : register(s3);

// ========================================
// Data Structures
// ========================================

struct VertexInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float3 tangent : TANGENT;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR;
    uint instanceId : SV_InstanceID;
};

struct VertexOutput
{
    float4 position : SV_Position;
    float3 worldPosition : POSITION1;
    float3 normal : NORMAL;
    float3 tangent : TANGENT;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR;
    float3 viewDirection : TEXCOORD1;
    float4 shadowCoord : TEXCOORD2;
};

struct PixelOutput
{
    float4 color : SV_Target0;
    float4 normal : SV_Target1;
    float4 motion : SV_Target2;
    float depth : SV_Depth;
};

struct ComputeData
{
    float4 position;
    float4 velocity;
    float4 color;
    float life;
    float size;
    uint type;
    uint flags;
};

// ========================================
// Vertex Shader
// ========================================

[shader("vertex")]
VertexOutput VS_Main(VertexInput input)
{
    VertexOutput output;
    
    // Transform position
    float3 worldPos = input.position;
    
    // Apply animation if enabled
    if (enableAnimation)
    {
        float wave = sin(time * animationSpeed + worldPos.x * 0.1) * waveAmplitude;
        worldPos.y += wave;
    }
    
    // Apply view-projection transform
    output.position = mul(float4(worldPos, 1.0), viewProjectionMatrix);
    output.worldPosition = worldPos;
    
    // Transform normal and tangent
    output.normal = normalize(input.normal);
    output.tangent = normalize(input.tangent);
    
    // Pass through texture coordinates and color
    output.texCoord = input.texCoord;
    output.color = input.color;
    
    // Calculate view direction
    output.viewDirection = normalize(cameraPosition - worldPos);
    
    // Shadow mapping (placeholder)
    output.shadowCoord = float4(0, 0, 0, 1);
    
    return output;
}

// ========================================
// Pixel Shader
// ========================================

[shader("pixel")]
PixelOutput PS_Main(VertexOutput input)
{
    PixelOutput output;
    
    // Sample textures
    float4 albedo = albedoTexture.Sample(linearSampler, input.texCoord) * baseColor;
    float3 normal = normalTexture.Sample(linearSampler, input.texCoord).xyz * 2.0 - 1.0;
    float4 metallicRoughness = metallicRoughnessTexture.Sample(linearSampler, input.texCoord);
    float3 emission = emissionTexture.Sample(linearSampler, input.texCoord).rgb * emissionColor;
    
    // Extract material properties
    float metallic_value = metallicRoughness.b * metallic;
    float roughness_value = metallicRoughness.g * roughness;
    
    // Calculate lighting (simplified)
    float3 N = normalize(input.normal);
    float3 V = normalize(input.viewDirection);
    float3 L = normalize(float3(1, 1, 1)); // Directional light
    float3 H = normalize(L + V);
    
    float NdotL = max(0.0, dot(N, L));
    float NdotV = max(0.0, dot(N, V));
    float NdotH = max(0.0, dot(N, H));
    float VdotH = max(0.0, dot(V, H));
    
    // Simple PBR calculation
    float3 F0 = lerp(float3(0.04, 0.04, 0.04), albedo.rgb, metallic_value);
    float3 F = F0 + (1.0 - F0) * pow(1.0 - VdotH, 5.0);
    
    float alpha = roughness_value * roughness_value;
    float alpha2 = alpha * alpha;
    float denom = NdotH * NdotH * (alpha2 - 1.0) + 1.0;
    float D = alpha2 / (3.14159 * denom * denom);
    
    float k = (roughness_value + 1.0) * (roughness_value + 1.0) / 8.0;
    float G1L = NdotL / (NdotL * (1.0 - k) + k);
    float G1V = NdotV / (NdotV * (1.0 - k) + k);
    float G = G1L * G1V;
    
    float3 specular = D * G * F / (4.0 * NdotL * NdotV + 0.001);
    float3 diffuse = albedo.rgb / 3.14159 * (1.0 - F) * (1.0 - metallic_value);
    
    float3 color = (diffuse + specular) * NdotL + emission;
    
    // Environment reflection
    float3 R = reflect(-V, N);
    float3 envColor = environmentTexture.Sample(linearSampler, R).rgb;
    color += envColor * F * (1.0 - roughness_value) * 0.1;
    
    // Output to multiple render targets
    output.color = float4(color, albedo.a);
    output.normal = float4(N * 0.5 + 0.5, 1.0);
    output.motion = float4(0, 0, 0, 1); // Motion vectors placeholder
    output.depth = input.position.z;
    
    return output;
}

// ========================================
// Compute Shaders
// ========================================

[shader("compute")]
[numthreads(16, 16, 1)]
void CS_ImageProcessing(uint3 dispatchThreadID : SV_DispatchThreadID, uint3 groupThreadID : SV_GroupThreadID)
{
    uint2 texelCoord = dispatchThreadID.xy;
    
    // Get texture dimensions
    uint width, height;
    inputImageTexture.GetDimensions(width, height);
    
    // Bounds check
    if (texelCoord.x >= width || texelCoord.y >= height)
        return;
    
    // Sample input
    float4 color = inputImageTexture[texelCoord];
    
    // Apply various image processing effects based on frame count
    int effect = frameCount % 4;
    
    switch (effect)
    {
        case 0: // Grayscale
        {
            float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
            color.rgb = float3(gray, gray, gray);
            break;
        }
        case 1: // Sepia
        {
            float3 sepia = float3(
                dot(color.rgb, float3(0.393, 0.769, 0.189)),
                dot(color.rgb, float3(0.349, 0.686, 0.168)),
                dot(color.rgb, float3(0.272, 0.534, 0.131))
            );
            color.rgb = sepia;
            break;
        }
        case 2: // Invert
        {
            color.rgb = 1.0 - color.rgb;
            break;
        }
        case 3: // Blur (simple box filter)
        {
            float4 sum = float4(0, 0, 0, 0);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int2 sampleCoord = texelCoord + int2(x, y);
                    sampleCoord = clamp(sampleCoord, int2(0, 0), int2(width - 1, height - 1));
                    sum += inputImageTexture[sampleCoord];
                }
            }
            color = sum / 9.0;
            break;
        }
    }
    
    // Write output
    outputImageTexture[texelCoord] = color;
}

[shader("compute")]
[numthreads(64, 1, 1)]
void CS_ParticleSimulation(uint3 dispatchThreadID : SV_DispatchThreadID)
{
    uint particleIndex = dispatchThreadID.x;
    
    // Get buffer size
    uint numParticles, stride;
    particleBuffer.GetDimensions(numParticles, stride);
    
    if (particleIndex >= numParticles)
        return;
    
    // Load particle data
    ComputeData particle;
    particle.position = particleBuffer[particleIndex * 2 + 0];
    particle.velocity = particleBuffer[particleIndex * 2 + 1];
    
    // Simple physics simulation
    particle.velocity.xyz += windDirection.xyz * deltaTime * 0.1;
    particle.velocity.y -= 9.81 * deltaTime; // Gravity
    particle.position.xyz += particle.velocity.xyz * deltaTime;
    
    // Bounce off ground
    if (particle.position.y < 0.0)
    {
        particle.position.y = 0.0;
        particle.velocity.y = abs(particle.velocity.y) * 0.8;
    }
    
    // Update life
    particle.life -= deltaTime;
    if (particle.life <= 0.0)
    {
        // Reset particle
        particle.position = float4(0, 10, 0, 1);
        particle.velocity = float4((frac(sin(particleIndex * 12.9898)) - 0.5) * 10.0, 0, (frac(sin(particleIndex * 78.233)) - 0.5) * 10.0, 0);
        particle.life = 5.0;
    }
    
    // Store updated data
    particleBuffer[particleIndex * 2 + 0] = particle.position;
    particleBuffer[particleIndex * 2 + 1] = particle.velocity;
}