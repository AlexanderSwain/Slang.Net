using Slang.Sdk.Interop;
using System.Diagnostics;

namespace Slang.Sdk
{
    public class ProgramEntryPoint : IEquatable<ProgramEntryPoint>
    {
        #region Definition
        public ProgramTarget Parent { get; }

        internal ProgramEntryPoint(ProgramTarget parent, uint index, string name, Stage stage)
        {
            Parent = parent;
            Index = index;
            Name = name;
            Stage = stage;
        }
        #endregion

        #region Pretty

        public uint Index { get; }

        public string Name { get; }

        public Stage Stage { get; }

        public CompilationResult Compile()
        {
            var programBinding = Parent.Parent.Binding;
            var targetIndex = Parent.Index;

            var result = programBinding.CompileEntryPoint(Index, targetIndex);
            return new CompilationResult(result.Source, Parent, this, result.Result, result.Diagnostics);
        }

        public EntryPointReflection GetReflection()
        {
            var shaderReflection = Parent.GetReflection();
            return shaderReflection.EntryPoints[Index];
        }
        #endregion

        #region Equality
        public static bool operator ==(ProgramEntryPoint? left, ProgramEntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            if (left.Parent == right.Parent && left.Index == right.Index)
            {
                // Names and Stages Should be equal
                Debug.Assert(left.Name == right.Name);
                Debug.Assert(left.Stage == right.Stage);
                return true;
            }
            return false;
        }

        public static bool operator !=(ProgramEntryPoint? left, ProgramEntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            if (left.Parent == right.Parent && left.Index == right.Index)
            {
                // Names and Stages Should be equal
                Debug.Assert(left.Name == right.Name);
                Debug.Assert(left.Stage == right.Stage);
                return false;
            }
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ProgramEntryPoint entryPoint) return Equals(entryPoint);
            return false;
        }

        public bool Equals(ProgramEntryPoint? other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Parent.GetHashCode(), Index.GetHashCode());
        }
        #endregion
    }
}