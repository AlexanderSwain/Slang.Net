#pragma pack_matrix(row_major)
#ifdef SLANG_HLSL_ENABLE_NVAPI
#include "nvHLSLExtns.h"
#endif

#ifndef __DXC_VERSION_MAJOR
// warning X3557: loop doesn't seem to do anything, forcing loop to unroll
#pragma warning(disable : 3557)
#endif


#line 18 "49f6e054-obfuscated"
Texture2D<float4> _ShBB56BDDF05F4E863 : register(t0);



RWStructuredBuffer<uint4> _Sh6BDD8D378330D1A8 : register(u0);


#line 40
[numthreads(32, 32, 1)]
void CS(uint3 _S1 : SV_DispatchThreadID, uint3 _S2 : SV_GroupThreadID, uint3 _S3 : SV_GroupID)
{

#line 24
    uint4 _S4 = uint4(_ShBB56BDDF05F4E863.Load(int3(int2(_S1.xy), int(0))) * 255.0f);

#line 57
    uint _S5;
    InterlockedAdd(_Sh6BDD8D378330D1A8[int(0)][int(0)], _S4.x, _S5);

#line 36
    uint _S6;
    InterlockedAdd(_Sh6BDD8D378330D1A8[int(0)][int(1)], _S4.y, _S6);

#line 31
    uint _S7;
    InterlockedAdd(_Sh6BDD8D378330D1A8[int(0)][int(2)], _S4.z, _S7);

#line 41
    uint _S8;
    InterlockedAdd(_Sh6BDD8D378330D1A8[int(0)][int(3)], _S4.w, _S8);

#line 55
    return;
}

