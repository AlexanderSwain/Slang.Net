namespace Slang
{
    public unsafe class TypeParameterReflection
    {
        internal Slang.Cpp.TypeParameterReflection cppObj { get; }

        public TypeParameterReflection(Slang.Cpp.TypeParameterReflection comObj)
        {
            cppObj = comObj;
        }

        public string Name => cppObj.Name;
        public uint Index => cppObj.Index;
        public uint ConstraintCount => cppObj.ConstraintCount;

        public TypeReflection GetConstraintByIndex(int index)
        {
            return new TypeReflection(cppObj.GetConstraintByIndex(index));
        }
    }
}