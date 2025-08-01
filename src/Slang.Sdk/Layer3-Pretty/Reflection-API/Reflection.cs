using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public abstract class Reflection : IEquatable<Reflection>
    {
        public abstract Reflection? Parent { get; }
        internal abstract Binding.Reflection Binding { get; }

        #region Equality
        public static bool operator ==(Reflection? left, Reflection? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Binding == right.Binding;
        }

        public static bool operator !=(Reflection? left, Reflection? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return left.Binding != right.Binding;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Reflection reflection) return Equals(reflection);
            return false;
        }

        public bool Equals(Reflection? other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return Binding.GetHashCode();
        }
        #endregion

        #region Equality
        public bool Equals(Attribute? other)
        {
            return this == other;
        }
        #endregion
    }
}
