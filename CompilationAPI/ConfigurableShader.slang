// Configurable shader that demonstrates preprocessor macros and compiler options
// Shows conditional compilation and parameterized shader features

// Configurable constants (set via preprocessor macros)
#ifndef MAX_LIGHTS
#define MAX_LIGHTS 4
#endif

#ifndef LIGHTING_MODEL
#define LIGHTING_MODEL BLINN_PHONG
#endif

#ifndef ENABLE_LIGHTING
#define ENABLE_LIGHTING 1
#endif

#ifndef DEBUG_MODE
#define DEBUG_MODE 0
#endif

// Lighting model definitions
#define LAMBERT 0
#define BLINN_PHONG 1
#define PHONG 2
#define PBR 3

// Scene constants
cbuffer SceneData : register(b0)
{
    float4x4 worldViewProj;
    float4x4 worldMatrix;
    float3 cameraPosition;
    float time;
    float4 lightPositions[MAX_LIGHTS];
    float4 lightColors[MAX_LIGHTS];
    int activeLightCount;
}

// Material properties
cbuffer MaterialData : register(b1)
{
    float4 albedo;
    float metallic;
    float roughness;
    float3 emission;
    float normalScale;
}

// Textures
Texture2D<float4> albedoTexture : register(t0);
Texture2D<float4> normalTexture : register(t1);
Texture2D<float4> materialTexture : register(t2); // R=metallic, G=roughness, B=AO
SamplerState materialSampler : register(s0);

// Vertex input structure
struct VertexInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float3 tangent : TANGENT;
    float2 uv : TEXCOORD0;
};

// Vertex output / Pixel input structure
struct VertexOutput
{
    float4 position : SV_Position;
    float3 worldPos : WORLD_POS;
    float3 normal : NORMAL;
    float3 tangent : TANGENT;
    float2 uv : TEXCOORD0;
    
#if DEBUG_MODE
    float4 debugColor : DEBUG_COLOR;
#endif
}

// Lighting calculation functions
float3 CalculateLambert(float3 normal, float3 lightDir, float3 lightColor)
{
    float ndotl = max(0.0, dot(normal, lightDir));
    return lightColor * ndotl;
}

float3 CalculateBlinnPhong(float3 normal, float3 lightDir, float3 viewDir, float3 lightColor, float shininess)
{
    float3 halfDir = normalize(lightDir + viewDir);
    float ndotl = max(0.0, dot(normal, lightDir));
    float ndoth = max(0.0, dot(normal, halfDir));
    float specular = pow(ndoth, shininess);
    
    return lightColor * (ndotl + specular * 0.5);
}

float3 CalculatePhong(float3 normal, float3 lightDir, float3 viewDir, float3 lightColor, float shininess)
{
    float3 reflectDir = reflect(-lightDir, normal);
    float ndotl = max(0.0, dot(normal, lightDir));
    float rdotv = max(0.0, dot(reflectDir, viewDir));
    float specular = pow(rdotv, shininess);
    
    return lightColor * (ndotl + specular * 0.3);
}

// Simplified PBR calculation
float3 CalculatePBR(float3 normal, float3 lightDir, float3 viewDir, float3 lightColor, 
                   float3 albedoColor, float metallicValue, float roughnessValue)
{
    float ndotl = max(0.0, dot(normal, lightDir));
    float3 halfDir = normalize(lightDir + viewDir);
    float ndoth = max(0.0, dot(normal, halfDir));
    
    // Simplified PBR approximation
    float3 F0 = lerp(float3(0.04, 0.04, 0.04), albedoColor, metallicValue);
    float3 fresnel = F0 + (1.0 - F0) * pow(1.0 - dot(halfDir, lightDir), 5.0);
    
    float roughness2 = roughnessValue * roughnessValue;
    float denom = ndoth * ndoth * (roughness2 - 1.0) + 1.0;
    float distribution = roughness2 / (3.14159 * denom * denom);
    
    return lightColor * ndotl * fresnel * distribution;
}

// Vertex shader
[shader("vertex")]
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    
    // Transform to world space
    float4 worldPos = mul(float4(input.position, 1.0), worldMatrix);
    output.worldPos = worldPos.xyz;
    output.position = mul(worldPos, worldViewProj);
    
    // Transform normals and tangents
    output.normal = normalize(mul(input.normal, (float3x3)worldMatrix));
    output.tangent = normalize(mul(input.tangent, (float3x3)worldMatrix));
    
    output.uv = input.uv;
    
#if DEBUG_MODE
    // Color-code vertices based on position for debugging
    output.debugColor = float4(abs(input.position), 1.0);
#endif
    
    return output;
}

// Pixel shader
[shader("fragment")]
float4 PS(VertexOutput input) : SV_Target
{
#if DEBUG_MODE
    return input.debugColor;
#endif

    // Sample material textures
    float4 albedoSample = albedoTexture.Sample(materialSampler, input.uv);
    float4 materialSample = materialTexture.Sample(materialSampler, input.uv);
    
    float3 finalAlbedo = albedo.rgb * albedoSample.rgb;
    float finalMetallic = metallic * materialSample.r;
    float finalRoughness = roughness * materialSample.g;
    float ambientOcclusion = materialSample.b;
    
    // Normal mapping
    float3 normalMap = normalTexture.Sample(materialSampler, input.uv).xyz * 2.0 - 1.0;
    normalMap.xy *= normalScale;
    
    float3 normal = normalize(input.normal);
    float3 tangent = normalize(input.tangent);
    float3 bitangent = cross(normal, tangent);
    float3x3 TBN = float3x3(tangent, bitangent, normal);
    float3 finalNormal = normalize(mul(normalMap, TBN));
    
    float3 viewDir = normalize(cameraPosition - input.worldPos);
    
#if ENABLE_LIGHTING
    float3 lighting = float3(0.0, 0.0, 0.0);
    
    // Calculate lighting for each active light
    for (int i = 0; i < min(activeLightCount, MAX_LIGHTS); ++i)
    {
        float3 lightPos = lightPositions[i].xyz;
        float3 lightColor = lightColors[i].rgb;
        float lightIntensity = lightColors[i].a;
        
        float3 lightDir = normalize(lightPos - input.worldPos);
        
        // Distance attenuation
        float distance = length(lightPos - input.worldPos);
        float attenuation = 1.0 / (1.0 + 0.1 * distance + 0.01 * distance * distance);
        lightColor *= lightIntensity * attenuation;
        
        // Apply selected lighting model
#if LIGHTING_MODEL == LAMBERT
        lighting += CalculateLambert(finalNormal, lightDir, lightColor);
#elif LIGHTING_MODEL == BLINN_PHONG
        lighting += CalculateBlinnPhong(finalNormal, lightDir, viewDir, lightColor, 32.0);
#elif LIGHTING_MODEL == PHONG
        lighting += CalculatePhong(finalNormal, lightDir, viewDir, lightColor, 32.0);
#elif LIGHTING_MODEL == PBR
        lighting += CalculatePBR(finalNormal, lightDir, viewDir, lightColor, finalAlbedo, finalMetallic, finalRoughness);
#endif
    }
    
    // Apply ambient lighting
    float3 ambient = finalAlbedo * 0.1 * ambientOcclusion;
    lighting += ambient;
    
    // Apply lighting to albedo
    finalAlbedo *= lighting;
#endif

    // Add emission
    finalAlbedo += emission;
    
    return float4(finalAlbedo, albedoSample.a);
}
