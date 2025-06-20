namespace Slang
{
    public unsafe class Modifier
    {
        internal Slang.Cpp.Modifier cppObj { get; }

        public Modifier(Slang.Cpp.Modifier comObj)
        {
            cppObj = comObj;
        }

        public enum ID
        {
            Shared,
            NoDiff,
            Static,
            Const,
            Export,
            Extern,
            Differentiable,
            Mutating,
            In,
            Out,
            InOut
        }
    }
}