namespace Slang
{
    public unsafe class GenericReflection
    {
        internal Slang.Cpp.GenericReflection cppObj { get; }

        public GenericReflection(Slang.Cpp.GenericReflection comObj)
        {
            cppObj = comObj;
        }

        // Add methods and properties as needed here
        public string Name => cppObj.Name;

        // Type parameters
        public uint TypeParameterCount => cppObj.TypeParameterCount;

        public VariableReflection GetTypeParameter(uint index)
        {
            var cppVar = cppObj.GetTypeParameter(index);
            return new VariableReflection(cppVar);
        }

        // Value parameters
        public uint ValueParameterCount => cppObj.ValueParameterCount;

        public VariableReflection GetValueParameter(uint index)
        {
            var cppVar = cppObj.GetValueParameter(index);
            return new VariableReflection(cppVar);
        }

        // Type parameter constraints
        public uint GetTypeParameterConstraintCount(VariableReflection typeParam)
        {
            return cppObj.GetTypeParameterConstraintCount(typeParam.cppObj);
        }

        public TypeReflection? GetTypeParameterConstraintType(VariableReflection typeParam, uint index)
        {
            var cppType = cppObj.GetTypeParameterConstraintType(typeParam.cppObj, index);
            return cppType != null ? new TypeReflection(cppType) : null;
        }

        // Container operations
        public DeclKind InnerKind => (DeclKind)cppObj.InnerKind;

        public GenericReflection? OuterGenericContainer
        {
            get
            {
                var container = cppObj.OuterGenericContainer;
                return container != null ? new GenericReflection(container) : null;
            }
        }

        // Specialization operations
        public TypeReflection? GetConcreteType(VariableReflection typeParam)
        {
            var cppType = cppObj.GetConcreteType(typeParam.cppObj);
            return cppType != null ? new TypeReflection(cppType) : null;
        }

        public long? GetConcreteIntVal(VariableReflection valueParam)
        {
            return cppObj.GetConcreteIntVal(valueParam.cppObj);
        }

        public GenericReflection? ApplySpecializations(GenericReflection? genRef)
        {
            var cppGeneric = cppObj.ApplySpecializations(genRef?.cppObj);
            return cppGeneric != null ? new GenericReflection(cppGeneric) : null;
        }
    }
}