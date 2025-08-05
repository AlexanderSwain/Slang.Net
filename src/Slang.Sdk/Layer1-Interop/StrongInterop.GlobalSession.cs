using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Interop
{
    internal partial class StrongInterop
    {
        internal unsafe class GlobalSession
        {
            /// <summary>
            /// Creates a module with strongly-typed handle.
            /// </summary>
            internal static void EnableGlsl(out string? error)
            {
                char* pError = null;

                SlangNativeInterop.GlobalSession_SetEnableGlsl(true, &pError);

                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);

                SlangNativeInterop.FreeChar(&pError);
            }
        }
    }
}
