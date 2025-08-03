// Simple compute shader for Vulkan
struct ComputeInput
{
    float4 data;
};

struct ComputeOutput
{
    float4 result;
};

RWStructuredBuffer<ComputeInput> inputBuffer;
RWStructuredBuffer<ComputeOutput> outputBuffer;

[shader("compute")]
[numthreads(16, 16, 1)]
void CS(uint3 threadID : SV_DispatchThreadID)
{
    uint index = threadID.x + threadID.y * 16;
    
    // Simple computation: multiply by 2
    outputBuffer[index].result = inputBuffer[index].data * 2.0;
}
