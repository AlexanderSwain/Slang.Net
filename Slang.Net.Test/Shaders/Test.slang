RWStructuredBuffer<float> result;
[shader("compute")]
[numthreads(1,1,1)]
void computeMain(uint3  threadId :  SV_DispatchThreadID)
{
    result[threadId.x] = threadId.x;
};