#pragma once

namespace Slang::Cpp
{
    public enum class ParameterCategory
    {
        None = 0,
        Mixed = 1,
        ConstantBuffer = 2,
        ShaderResource = 3,
        UnorderedAccess = 4,
        VaryingInput = 5,
        VaryingOutput = 6,
        SamplerState = 7,
        Uniform = 8,
        DescriptorTableSlot = 9,
        SpecializationConstant = 10,
        PushConstantBuffer = 11,
        RegisterSpace = 12,
        GenericResource = 13,
        RayPayload = 14,
        HitAttributes = 15,
        CallablePayload = 16,
        ShaderRecord = 17,
        ExistentialTypeParam = 18,
        ExistentialObjectParam = 19,
    };
}
