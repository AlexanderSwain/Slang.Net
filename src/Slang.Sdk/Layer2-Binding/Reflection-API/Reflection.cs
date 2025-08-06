using Slang.Sdk.Interop;
using System.Diagnostics;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal abstract class Reflection : IEquatable<Reflection>
    {
        internal abstract Reflection? Parent { get; }
        internal abstract SlangHandle Handle { get; }
        internal abstract SlangHandle NativeHandle { get; }

        ~Reflection()
        {
            Handle?.Dispose();
        }

        internal T Call<T>(Func<T> function, Func<string?> error)
        {
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

            T resultHandle = function();

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to unwrap array: {error() ?? "<No error was returned from Slang>"}");

            return resultHandle;
        }

        #region Equality
        public static bool operator ==(Reflection? left, Reflection? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.NativeHandle == right.NativeHandle;
        }

        public static bool operator !=(Reflection? left, Reflection? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return left.NativeHandle != right.NativeHandle;
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
            return (int)NativeHandle.Handle;
        }
        #endregion
    }
}
