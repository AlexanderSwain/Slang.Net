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

        internal T Call<T>(Func<T> function)
            where T : SlangHandle, new()
        {
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

            T resultHandle = function();

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to unwrap array: {GetLastError() ?? "<No error was returned from Slang>"}");

            return resultHandle;

        }
    }
}
