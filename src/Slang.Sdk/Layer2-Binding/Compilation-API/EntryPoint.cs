using Slang.Sdk.Interop;
using System;
using static Slang.Sdk.Interop.Utilities;
using static Slang.Sdk.Interop.StringMarshaling;

namespace Slang.Sdk.Binding
{
    internal unsafe class EntryPoint : CompilationBinding
    {
        internal Module Parent { get; }
        internal override Interop.EntryPointHandle Handle { get; }

        public EntryPoint(Module parent, uint index)
        {
            Parent = parent;

            // Using the strongly-typed interop that returns EntryPointHandle directly
            Handle = StrongTypeInterop.EntryPoint_Create(Parent.Handle, index);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang entry point: {GetLastError() ?? "<No error was returned from Slang>"}");
        }

        public EntryPoint(Module parent, string name)
        {
            Parent = parent;
            // Convert managed strings to UTF-8 before passing to native API
            byte* pName = ToUtf8(name);
            try
            {
                // Using the strongly-typed interop that returns EntryPointHandle directly
                Handle = StrongTypeInterop.EntryPoint_CreateByName(Parent.Handle, (char*)pName);
                if (Handle.IsInvalid)
                    throw new SlangException(SlangResult.Fail, $"Failed to create Slang entry point: {GetLastError() ?? "<No error was returned from Slang>"}");
            }
            finally
            {
                // Clean up allocated UTF-8 strings
                FreeUtf8(pName);
            }
        }

        public EntryPoint(Module parent, Interop.EntryPointHandle handle)
        {
            Parent = parent;
            Handle = handle;
        }

        public int Index
        {
            get
            {
                // Use call, for consistency with other properties
                ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

                // Using the strongly-typed interop to get the index of the entry point
                return SlangNativeInterop.EntryPoint_GetIndex(Handle);
            }
        }

        public string Name
        {
            get
            {
                // Use call, for consistency with other properties
                ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

                // Using the strongly-typed interop to get the name of the entry point
                char* pName = SlangNativeInterop.EntryPoint_GetName(Handle);
                if (pName == null)
                    throw new SlangException(SlangResult.Fail, "Failed to retrieve entry point name: <No name was returned from Slang>");

                return FromUtf8((byte*)pName) ?? throw new SlangException(SlangResult.Fail, "Failed to convert entry point name from UTF-8");
            }
        }

        public CompilationResult Compile(int targetIndex)
        {
            // Use call, for consistency with other properties
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
            var target = Parent.Parent.Targets.ElementAt((int)targetIndex);
            char* compiledSource = null;
            SlangResult compileResult = SlangNativeInterop.EntryPoint_Compile(Handle, targetIndex, &compiledSource);
            string? diagnostics = GetLastError();
            return new CompilationResult(StringMarshaling.FromUtf8((byte*)compiledSource) ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), target, this, compileResult, diagnostics);
        }

        ~EntryPoint()
        {
            Handle?.Dispose();
        }
    }
}
