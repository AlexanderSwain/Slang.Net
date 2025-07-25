using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding
{
    internal abstract class CompilationBinding
    {
        internal abstract SlangHandle Handle { get; }

        internal T Call<T>(Func<T> function)
        {
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

            T resultHandle = function();

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to unwrap array: {GetLastError() ?? "<No error was returned from Slang>"}");

            return resultHandle;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: <Handle>{Handle}</Handle>";
        }
    }
}
