using Slang.Sdk.Interop;

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
            Handle = StrongInterop.EntryPoint.Create(Parent.Handle, index, out var error);

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang entry point: {error ?? "<No error was returned from Slang>"}");
        }

        public EntryPoint(Module parent, string name)
        {
            Parent = parent;

            // Using the strongly-typed interop that returns EntryPointHandle directly
            Handle = StrongInterop.EntryPoint.CreateByName(Parent.Handle, name, out var error);
            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang entry point: {error ?? "<No error was returned from Slang>"}");
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
                var result = StrongInterop.EntryPoint.GetIndex(Handle, out var error);
                if (error != null)
                    throw new SlangException(SlangResult.Fail, $"Failed to get an EntryPoint's index: {error ?? "<No error was returned from Slang>"}");

                return result;
            }
        }

        public string? Name
        {
            get
            {
                // Use call, for consistency with other properties
                ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);

                var result = StrongInterop.EntryPoint.GetName(Handle, out var error);
                if (error != null)
                    throw new SlangException(SlangResult.Fail, $"Failed to get an EntryPoint's name: {error ?? "<No error was returned from Slang>"}");

                return result;
            }
        }

        public CompilationResult Compile(int targetIndex)
        {
            // Use call, for consistency with other properties
            ObjectDisposedException.ThrowIf(Handle.IsInvalid, this);
            var target = Parent.Parent.Targets.ElementAt((int)targetIndex);
            SlangResult compileResult = StrongInterop.EntryPoint.Compile(Handle, targetIndex, out string compiledSource, out var error);
            return new CompilationResult(compiledSource ?? throw new SlangException(SlangResult.Fail, "Failed to convert compiled source from UTF-8"), target, this, compileResult, error);
        }

        ~EntryPoint()
        {
            Handle?.Dispose();
        }

        #region Equality
        public static bool operator ==(EntryPoint? left, EntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Handle == right.Handle;
        }

        public static bool operator !=(EntryPoint? left, EntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return left.Handle != right.Handle;
        }

        public override bool Equals(object? obj)
        {
            if (obj is EntryPoint entryPoint) return Equals(entryPoint);
            return false;
        }

        public bool Equals(EntryPoint? other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
        #endregion
    }
}
