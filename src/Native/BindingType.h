#pragma once
#include <slang.h>

namespace Native
{
    enum class BindingType
    {
        Unknown = SLANG_BINDING_TYPE_UNKNOWN,

        Sampler = SLANG_BINDING_TYPE_SAMPLER,
        Texture = SLANG_BINDING_TYPE_TEXTURE,
        ConstantBuffer = SLANG_BINDING_TYPE_CONSTANT_BUFFER,
        ParameterBlock = SLANG_BINDING_TYPE_PARAMETER_BLOCK,
        TypedBuffer = SLANG_BINDING_TYPE_TYPED_BUFFER,
        RawBuffer = SLANG_BINDING_TYPE_RAW_BUFFER,
        CombinedTextureSampler = SLANG_BINDING_TYPE_COMBINED_TEXTURE_SAMPLER,
        InputRenderTarget = SLANG_BINDING_TYPE_INPUT_RENDER_TARGET,
        InlineUniformData = SLANG_BINDING_TYPE_INLINE_UNIFORM_DATA,
        RayTracingAccelerationStructure = SLANG_BINDING_TYPE_RAY_TRACING_ACCELERATION_STRUCTURE,
        VaryingInput = SLANG_BINDING_TYPE_VARYING_INPUT,
        VaryingOutput = SLANG_BINDING_TYPE_VARYING_OUTPUT,
        ExistentialValue = SLANG_BINDING_TYPE_EXISTENTIAL_VALUE,
        PushConstant = SLANG_BINDING_TYPE_PUSH_CONSTANT,

        MutableFlag = SLANG_BINDING_TYPE_MUTABLE_FLAG,

        MutableTexture = SLANG_BINDING_TYPE_MUTABLE_TETURE,
        MutableTypedBuffer = SLANG_BINDING_TYPE_MUTABLE_TYPED_BUFFER,
        MutableRawBuffer = SLANG_BINDING_TYPE_MUTABLE_RAW_BUFFER,

        BaseMask = SLANG_BINDING_TYPE_BASE_MASK,
        ExtMask = SLANG_BINDING_TYPE_EXT_MASK,
    };
}
