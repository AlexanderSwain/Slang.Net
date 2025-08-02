using Slang.Sdk.Collections;
using Slang.Sdk.Interop;
using System.Diagnostics;

namespace Slang.Sdk
{
    public partial class ProgramTarget : INamedComposition<ProgramEntryPoint>, IComposition<ProgramEntryPoint>, IEquatable<ProgramTarget>
    {
        #region Definition
        public Program Parent { get; }
        public Interop.Target Value { get; }

        public ProgramTarget(Program parent, Interop.Target target, uint index)
        {
            Parent = parent;
            Value = target;
            Index = index;
        }
        #endregion

        #region Pretty
        public uint Index { get; }

        public CompilationResult Compile()
        {
            var cResultBinding = Parent.Binding.CompileTarget(Index);
            return new CompilationResult(cResultBinding.Compiled, this, null, cResultBinding.Result, cResultBinding.Diagnostics);
        }

        ShaderReflection? _Reflection;
        public ShaderReflection GetReflection()
        {
            return _Reflection ??= new ShaderReflection(Parent, Parent.Binding.GetReflection(Index));
        }

        SlangNamedCollectionary<ProgramEntryPoint>? _EntryPoints;
        public SlangNamedCollectionary<ProgramEntryPoint> EntryPoints => _EntryPoints ??= _EntryPoints = new(this, this);
        #endregion

        #region IComposition
        public uint Count => GetReflection().EntryPoints.Count;
        public ProgramEntryPoint GetByIndex(uint index)
        {
            var reflection = GetReflection();
            var name = reflection.EntryPoints[index].Name;
            var stage = reflection.EntryPoints[index].Stage;
            return new ProgramEntryPoint(this, index, name, stage);
        }

        public ProgramEntryPoint? FindByName(string name)
        {
            var reflection = GetReflection();
            var epr = reflection.EntryPoints[name];
            var index = reflection.EntryPoints.IndexOf(epr);

            if (index is null)
                return null;

            return new ProgramEntryPoint(this, index.Value, name, epr.Stage);
        }
        #endregion

        #region Equality
        public static bool operator ==(ProgramTarget? left, ProgramTarget? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            if (left.Parent == right.Parent && left.Value == right.Value) return true;
            return false;
        }

        public static bool operator !=(ProgramTarget? left, ProgramTarget? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            if (left.Parent == right.Parent && left.Value == right.Value) return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is ProgramTarget entryPoint) return Equals(entryPoint);
            return false;
        }

        public bool Equals(ProgramTarget? other)
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