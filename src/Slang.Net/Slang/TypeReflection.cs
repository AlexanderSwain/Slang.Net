
public unsafe class TypeReflection : IComposedOf<Attribute>
{
    internal Slang.Cpp.TypeReflection cppObj { get; }
    
    #region Composite
    uint IComposedOf<Attribute>.Count => cppObj.UserAttributeCount;
    Attribute IComposedOf<Attribute>.GetByIndex(uint index) => new Attribute(cppObj.GetUserAttributeByIndex(index));
    #endregion

    public SlangCollection<Attribute> UserAttributes { get; }

    public TypeReflection(Slang.Cpp.TypeReflection comObj)
    {
        cppObj = comObj;
        UserAttributes = new SlangCollection<Attribute>(this);
    }

    public enum Kind
    {
        None = 0,
        Struct,
        Array,
        Matrix,
        Vector,
        Scalar,
        ConstantBuffer,
        Resource,
        SamplerState,
        TextureBuffer,
        ShaderStorageBuffer,
        ParameterBlock,
        GenericTypeParameter,
        Interface,
        OutputStream,
        Specialized,
        Feedback,
        Pointer,
        DynamicResource,
    }

    public enum ScalarType
    {
        None = 0,
        Void,
        Bool,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Float16,
        Float32,
        Float64,
        Int8,
        UInt8,
        Int16,
        UInt16,
    }

    public enum ResourceShape
    {
        ResourceBaseshapeMask = 0x0F,
        ResourceNone = 0x00,
        Texture1D = 0x01,
        Texture2D = 0x02,
        Texture3D = 0x03,
        TextureCube = 0x04,
        TextureBuffer = 0x05,
        StructuredBuffer = 0x06,
        ByteAddressBuffer = 0x07,
        ResourceUnknown = 0x08,
        AccelerationStructure = 0x09,
        TextureSubpass = 0x0A,
        ResourceExtShapeMask = 0xF0,
        TextureFeedbackFlag = 0x10,
        TextureShadowFlag = 0x20,
        TextureArrayFlag = 0x40,
        TextureMultisampleFlag = 0x80,
        Texture1DArray = Texture1D | TextureArrayFlag,
        Texture2DArray = Texture2D | TextureArrayFlag,
        TextureCubeArray = TextureCube | TextureArrayFlag,
        Texture2DMultisample = Texture2D | TextureMultisampleFlag,
        Texture2DMultisampleArray = Texture2D | TextureMultisampleFlag | TextureArrayFlag,
        TextureSubpassMultisample = TextureSubpass | TextureMultisampleFlag,
    }

    public enum ResourceAccess
    {
        None,
        Read,
        ReadWrite,
        RasterOrdered,
        Append,
        Consume,
        Write,
        Feedback,
        Unknown = 0x7FFFFFFF,
    }

    public Kind GetKind()
    {
        return (Kind)cppObj.Kind;
    }

    public uint GetFieldCount()
    {
        return cppObj.FieldCount;
    }

    public VariableReflection GetFieldByIndex(uint index)
    {
        var cppField = cppObj.GetFieldByIndex(index);
        return new VariableReflection(cppField);
    }

    public bool IsArray()
    {
        return cppObj.IsArray;
    }

    public TypeReflection? UnwrapArray()
    {
        var cppType = cppObj.UnwrapArray();
        return cppType != null ? new TypeReflection(cppType) : null;
    }

    public UIntPtr GetElementCount()
    {
        return cppObj.ElementCount;
    }

    public UIntPtr GetTotalArrayElementCount()
    {
        return cppObj.TotalArrayElementCount;
    }

    public TypeReflection? GetElementType()
    {
        var cppType = cppObj.ElementType;
        return cppType != null ? new TypeReflection(cppType) : null;
    }

    public uint GetRowCount()
    {
        return cppObj.RowCount;
    }

    public uint GetColumnCount()
    {
        return cppObj.ColumnCount;
    }

    public ScalarType GetScalarType()
    {
        return (ScalarType)cppObj.ScalarType;
    }

    public TypeReflection? GetResourceResultType()
    {
        var cppType = cppObj.ResourceResultType;
        return cppType != null ? new TypeReflection(cppType) : null;
    }

    public ResourceShape GetResourceShape()
    {
        return (ResourceShape)cppObj.ResourceShape;
    }

    public ResourceAccess GetResourceAccess()
    {
        return (ResourceAccess)cppObj.ResourceAccess;
    }

    public string GetName()
    {
        return cppObj.Name;
    }

    public string GetFullName()
    {
        return cppObj.FullName;
    }

    public uint GetUserAttributeCount()
    {
        return cppObj.UserAttributeCount;
    }

    public Attribute? GetUserAttributeByIndex(uint index)
    {
        var cppAttr = cppObj.GetUserAttributeByIndex(index);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }

    public Attribute? FindAttributeByName(string name)
    {
        var cppAttr = cppObj.FindAttributeByName(name);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }

    public Attribute? FindUserAttributeByName(string name)
    {
        var cppAttr = cppObj.FindUserAttributeByName(name);
        return cppAttr != null ? new Attribute(cppAttr) : null;
    }

    public TypeReflection? ApplySpecializations(GenericReflection? genRef)
    {
        var cppGenericObj = genRef != null ? genRef.cppObj : null;
        var cppType = cppObj.ApplySpecializations(cppGenericObj);
        return cppType != null ? new TypeReflection(cppType) : null;
    }

    public GenericReflection? GetGenericContainer()
    {
        var cppGeneric = cppObj.GenericContainer;
        return cppGeneric != null ? new GenericReflection(cppGeneric) : null;
    }
}