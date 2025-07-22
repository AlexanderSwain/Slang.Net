using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public abstract class Reflection
    {
        public abstract Reflection? Parent { get; }
        internal abstract Binding.Reflection Binding { get; }
    }
}
