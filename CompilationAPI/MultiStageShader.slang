// MultiStageShader.slang - Multiple shader stages for entry point discovery demo

// Vertex shader input/output structures
struct VertexInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
};

struct VertexOutput
{
    float4 position : SV_Position;
    float3 worldPos : POSITION;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR;
};

// Constant buffer
cbuffer TransformBuffer
{
    float4x4 worldMatrix;
    float4x4 viewMatrix;
    float4x4 projMatrix;
    float3 cameraPos;
    float time;
};

// Vertex Shader
[shader("vertex")]
VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    
    float4 worldPos = mul(float4(input.position, 1.0), worldMatrix);
    float4 viewPos = mul(worldPos, viewMatrix);
    output.position = mul(viewPos, projMatrix);
    
    output.worldPos = worldPos.xyz;
    output.normal = normalize(mul(input.normal, (float3x3)worldMatrix));
    output.texCoord = input.texCoord;
    
    // Add some animation based on time
    output.color = float4(
        0.5 + 0.5 * sin(time + input.position.x),
        0.5 + 0.5 * cos(time + input.position.y),
        0.5 + 0.5 * sin(time + input.position.z),
        1.0
    );
    
    return output;
}

// Pixel Shader
[shader("fragment")]
float4 PS(VertexOutput input) : SV_Target
{
    // Simple lighting calculation
    float3 lightDir = normalize(float3(1.0, 1.0, 1.0));
    float3 viewDir = normalize(cameraPos - input.worldPos);
    
    float diffuse = max(0.0, dot(input.normal, lightDir));
    float3 reflectDir = reflect(-lightDir, input.normal);
    float specular = pow(max(0.0, dot(viewDir, reflectDir)), 32.0);
    
    float3 finalColor = input.color.rgb * diffuse + float3(1.0, 1.0, 1.0) * specular;
    
    return float4(finalColor, 1.0);
}

// Compute Shader for post-processing
RWTexture2D<float4> outputTexture;
Texture2D<float4> inputTexture;

[shader("compute")]
[numthreads(8, 8, 1)]
void CS(uint3 id : SV_DispatchThreadID)
{
    uint2 dimensions;
    outputTexture.GetDimensions(dimensions.x, dimensions.y);
    
    if (id.x >= dimensions.x || id.y >= dimensions.y)
        return;
    
    float2 uv = float2(id.xy) / float2(dimensions);
    float4 color = inputTexture[id.xy];
    
    // Apply a simple blur effect
    float4 blurred = color;
    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            int2 samplePos = clamp(int2(id.xy) + int2(x, y), int2(0, 0), int2(dimensions) - 1);
            blurred += inputTexture[samplePos];
        }
    }
    blurred /= 9.0;
    
    // Mix original and blurred based on time
    float blendFactor = 0.5 + 0.5 * sin(time * 2.0);
    outputTexture[id.xy] = lerp(color, blurred, blendFactor);
}

// Geometry Shader (optional - demonstrates more stages)
struct GeometryOutput
{
    float4 position : SV_Position;
    float3 normal : NORMAL;
    float2 texCoord : TEXCOORD0;
    float4 color : COLOR;
};

[shader("geometry")]
[maxvertexcount(3)]
void GS(triangle VertexOutput input[3], inout TriangleStream<GeometryOutput> outputStream)
{
    // Calculate face normal
    float3 edge1 = input[1].worldPos - input[0].worldPos;
    float3 edge2 = input[2].worldPos - input[0].worldPos;
    float3 faceNormal = normalize(cross(edge1, edge2));
    
    // Output the triangle with enhanced normal
    for (int i = 0; i < 3; i++)
    {
        GeometryOutput output;
        output.position = input[i].position;
        output.normal = faceNormal; // Use face normal instead of vertex normal
        output.texCoord = input[i].texCoord;
        output.color = input[i].color;
        
        outputStream.Append(output);
    }
    
    outputStream.RestartStrip();
}
