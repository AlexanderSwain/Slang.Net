[shader("compute")]
void main(uint3 dispatchThreadID : SV_DispatchThreadID)
{
    float average = float(dispatchThreadID.x + dispatchThreadID.y + dispatchThreadID.z) / 3.0;
}
