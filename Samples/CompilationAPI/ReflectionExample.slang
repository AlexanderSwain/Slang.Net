// Comprehensive shader for reflection demonstration
// Shows various parameter types, structures, and reflection features

// Constant buffer with various types
cbuffer SceneConstants : register(b0)
{
    float4x4 viewMatrix;
    float4x4 projectionMatrix;
    float3 cameraPosition;
    float time;
    int frameCount;
    bool enableLighting;
    float2 screenResolution;
}

// Structured buffer
struct ParticleData
{
    float3 position;
    float3 velocity;
    float4 color;
    float lifetime;
    float size;
};

StructuredBuffer<ParticleData> particles : register(t0);
RWStructuredBuffer<ParticleData> outputParticles : register(u0);

// Textures
Texture2D<float4> diffuseTexture : register(t1);
Texture2D<float4> normalTexture : register(t2);
TextureCube<float4> environmentTexture : register(t3);

// Sampler states
SamplerState linearSampler : register(s0);
SamplerState pointSampler : register(s1);

// Interface for material evaluation
interface IMaterial
{
    float4 evaluate(float2 uv);
}

// Implementation of material interface
struct SimpleMaterial : IMaterial
{
    float4 baseColor;
    
    float4 evaluate(float2 uv)
    {
        return baseColor * diffuseTexture.Sample(linearSampler, uv);
    }
}

// Vertex shader output structure
struct VSOutput
{
    float4 position : SV_Position;
    float3 worldPos : WORLD_POS;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
}

// Vertex shader entry point
[shader("vertex")]
VSOutput VS(
    float3 position : POSITION,
    float3 normal : NORMAL,
    float2 uv : TEXCOORD0,
    uint vertexID : SV_VertexID
)
{
    VSOutput output;
    
    float4 worldPos = float4(position, 1.0);
    output.worldPos = worldPos.xyz;
    output.position = mul(mul(worldPos, viewMatrix), projectionMatrix);
    output.normal = normal;
    output.uv = uv;
    output.color = float4(1.0, 1.0, 1.0, 1.0);
    
    return output;
}

// Pixel shader entry point
[shader("fragment")]
float4 PS(VSOutput input) : SV_Target
{
    // Material evaluation
    SimpleMaterial material = { float4(1.0, 1.0, 1.0, 1.0) };
    float4 baseColor = material.evaluate(input.uv);
    
    // Normal mapping
    float3 normalMap = normalTexture.Sample(linearSampler, input.uv).xyz * 2.0 - 1.0;
    float3 finalNormal = normalize(input.normal + normalMap);
    
    // Environment reflection
    float3 viewDir = normalize(cameraPosition - input.worldPos);
    float3 reflectDir = reflect(-viewDir, finalNormal);
    float4 envColor = environmentTexture.Sample(linearSampler, reflectDir);
    
    // Simple lighting calculation
    float3 lightDir = normalize(float3(1.0, 1.0, 1.0));
    float ndotl = max(0.0, dot(finalNormal, lightDir));
    
    float4 finalColor = baseColor;
    if (enableLighting)
    {
        finalColor.rgb *= ndotl;
        finalColor.rgb += envColor.rgb * 0.1; // Weak environment reflection
    }
    
    return finalColor;
}

// Compute shader for particle simulation
[shader("compute")]
[numthreads(64, 1, 1)]
void CS(uint3 dispatchThreadID : SV_DispatchThreadID)
{
    uint particleIndex = dispatchThreadID.x;
    uint numParticles;
    uint stride;
    particles.GetDimensions(numParticles, stride);
    
    if (particleIndex >= numParticles)
        return;
    
    ParticleData particle = particles[particleIndex];
    
    // Update particle position
    particle.position += particle.velocity * 0.016; // Assume 60 FPS
    
    // Update lifetime
    particle.lifetime -= 0.016;
    
    // Apply gravity
    particle.velocity.y -= 9.81 * 0.016;
    
    // Fade out over time
    float normalizedLifetime = particle.lifetime / 5.0; // Assume 5 second max lifetime
    particle.color.a = normalizedLifetime;
    
    // Write back
    outputParticles[particleIndex] = particle;
}
