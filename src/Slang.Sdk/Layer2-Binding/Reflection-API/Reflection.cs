using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal abstract class Reflection
    {
        internal abstract Reflection? Parent { get; }
        internal abstract SlangHandle Handle { get; }

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
    }
}
