namespace Slang
{
    public enum TypeKind
    {
        None = 0,
        Struct = 1,
        Array = 2,
        Matrix = 3,
        Vector = 4,
        Scalar = 5,
        ConstantBuffer = 6,
        Resource = 7,
        SamplerState = 8,
        TextureBuffer = 9,
        ShaderStorageBuffer = 10,
        ParameterBlock = 11,
        GenericTypeParameter = 12,
        Interface = 13,
        OutputStream = 14,
        Specialized = 15,
        Feedback = 16,
        Pointer = 17,
        DynamicResource = 18,
    }
}