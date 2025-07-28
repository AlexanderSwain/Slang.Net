using Slang.Sdk.Binding;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class EntryPoint
    {
        #region Definition
        public Module Parent { get; }
        internal Binding.EntryPoint Binding { get; }

        internal EntryPoint(Module parent, uint index)
        {
            Parent = parent;
            Binding = new Binding.EntryPoint(Parent.Binding, index);
        }

        internal EntryPoint(Module parent, string name)
        {
            Parent = parent;
            Binding = new Binding.EntryPoint(Parent.Binding, name);
        }

        internal EntryPoint(Module parent, Binding.EntryPoint binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region Pretty

        public int Index
        {
            get
            {
                return Binding.Index;
            }
        }

        public string Name
        {
            get
            {
                var existingName = Binding.Name;
                if (existingName is null)
                    return FindName();
                else
                    return existingName;
            }
        }

        // Delete this
        //public Stage GetStage(Target target)
        //{
        //    return Reflections.Where(r => r.target == target);
        //}

        public CompilationResult Compile(Interop.Target target)
        {
            var targetIndex = Parent.Parent.Targets.IndexOf(target);

            if (targetIndex is null)
                throw new ArgumentException($"Target {target} was not included in parent session {Parent.Parent}. Use Session.Builder.AddTarget(target) to include it.", nameof(target));

            var result = Binding.Compile((int)targetIndex);
            return new CompilationResult(result.Source, target, this, result.Result, result.Diagnostics);
        }
        #endregion

        #region Equality
        public static bool operator ==(EntryPoint? left, EntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Binding == right.Binding;
        }

        public static bool operator !=(EntryPoint? left, EntryPoint? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return left.Binding != right.Binding;
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
            return Binding.GetHashCode();
        }
        #endregion

        #region Helpers
        private string FindName()
        {
            var existingName = Binding.Name;
            if (existingName is not null)
                return existingName;

            List<(Target target, EntryPointReflection reflection)> listResult = new();
            var sessionTargets = Parent.Parent.Targets;
            var program = Parent.Program;

            foreach (var target in sessionTargets)
            {
                var reflection = program.GetReflection(target);
                var entryPointReflections = reflection.EntryPoints;

                // Name based approach, this one will always work
                // Choosing the safer option until we get time to research the alternative
                foreach (var entryPointReflection in entryPointReflections)
                {
                    if (this == Parent.EntryPoints[entryPointReflection.Name])
                    {
                        return entryPointReflection.Name;
                    }
                }

                // Index based approach, whether this works depends on the native implementations
                // If EntryPointReflection is in the same order as EntryPoints
                //return entryPointReflections[(uint)Index];
            }

            throw new Exception("Failed to find name for entry point. This is likely a bug in Slang.Net, please report it.");
        }
        #endregion
    }
}
