namespace Slang
{
    public unsafe class Module
    {
        internal Slang.Cpp.Module cppObj { get; }

        // Use the field keyword when it becomes generally available, to make this cleaner
        private ShaderProgram? _Program;
        public ShaderProgram Program => _Program ??= new ShaderProgram(this);

        internal Module(Session session, string moduleName)
        {
            cppObj = new Slang.Cpp.Module(session.cppObj, moduleName, null, null);
        }
    }
}