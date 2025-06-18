#pragma once

namespace Slang::Cpp
{
    public enum class ResourceShape
    {
        None = 0x00,
        Texture1D = 0x01,
        Texture2D = 0x02,
        Texture3D = 0x03,
        TextureCube = 0x04,
        TextureBuffer = 0x05,
        StructuredBuffer = 0x06,
        ByteAddressBuffer = 0x07,
        Unknown = 0x08,
        AccelerationStructure = 0x09,
        TextureSubpass = 0x0A,
        Texture1DArray = 0x41,
        Texture2DArray = 0x42,
        TextureCubeArray = 0x44,
        Texture2DMultisample = 0x82,
        Texture2DMultisampleArray = 0xC2,
        TextureSubpassMultisample = 0x8A,
    };
}
