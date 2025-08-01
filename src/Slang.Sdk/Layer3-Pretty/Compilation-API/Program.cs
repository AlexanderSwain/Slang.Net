using Slang.Sdk.Binding;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public class Program : IComposition<ProgramTarget>, ITypedComposition<Target, ProgramTarget>, IEquatable<Program>
    {
        #region Definition
        public Module Parent { get; }
        internal Binding.Program Binding { get; }

        public Program(Module parent)
        {
            Parent = parent;
            Binding = new Binding.Program(Parent.Binding);
        }
        #endregion

        #region Typed Composision
        ProgramTarget? ITypedComposition<Target, ProgramTarget>.Find(Target key)
        {
            var targetIndex = Parent.Parent.Targets.IndexOf(key);

            if (targetIndex is null)
                return null;

            return new ProgramTarget(this, key, targetIndex.Value);
        }

        public uint Count => Parent.Parent.Targets.Count;
        public ProgramTarget GetByIndex(uint index)
        {
            return ((ITypedComposition<Target, ProgramTarget>)this).Find(Parent.Parent.Targets[index]) ?? 
                throw new IndexOutOfRangeException($"No target found at index {index}.");
        }
        #endregion

        #region Pretty
        SlangCollectionary<Target, ProgramTarget>? _Targets;
        public SlangCollectionary<Target, ProgramTarget> Targets => _Targets ??= _Targets = new(this, this);
        #endregion

        #region Equality
        public static bool operator ==(Program? left, Program? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            if (left.Binding == right.Binding) return true;
            return false;
        }

        public static bool operator !=(Program? left, Program? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            if (left.Binding == right.Binding) return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Program entryPoint) return Equals(entryPoint);
            return false;
        }

        public bool Equals(Program? other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return Binding.GetHashCode();
        }
        #endregion
    }
}