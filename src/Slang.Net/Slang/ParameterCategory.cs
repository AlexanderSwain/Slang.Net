namespace Slang
{
    public enum ParameterCategory
    {
        None,
        Mixed,
        ConstantBuffer,
        ShaderResource,
        UnorderedAccess,
        VaryingInput,
        VaryingOutput,
        SamplerState,
        Uniform,
        DescriptorTableSlot,
        SpecializationConstant,
        PushConstantBuffer,
        RegisterSpace,
        GenericResource,
        RayPayload,
        HitAttributes,
        CallablePayload,
        ShaderRecord,
        ExistentialTypeParam,
        ExistentialObjectParam,
        SubElementRegisterSpace,
        InputAttachmentIndex,
        MetalBuffer,
        MetalTexture,
        MetalArgumentBufferElement,
        MetalAttribute,
        MetalPayload,

        // DEPRECATED:
        VertexInput,
        FragmentOutput,
    }
}