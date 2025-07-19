namespace Slang
{
    public unsafe class ShaderProgram
    {
        internal Slang.Cpp.Program CppObj { get; }

        internal ShaderProgram(Module module)
        {
            CppObj = new Slang.Cpp.Program(module.cppObj);
        }

        internal ShaderProgram(Slang.Cpp.Program cppObj)
        {
            CppObj = cppObj;
        }

        /* [Fix]
         * If this constructor is used: internal ShaderReflection(ShaderProgram parent, Slang.Cpp.ShaderReflection cppObj)
         * And then GetReflection(uint targetIndex) is called continuously
         * More memory will be allocated
         * Not a common thing so this fix doesn't have priority.
         */
        public ShaderReflection GetReflection(uint targetIndex)
        {
            return new ShaderReflection(this, targetIndex);
        }
    }
}