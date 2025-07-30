using Slang.Sdk.Binding;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public class Program
    {
        #region Definition
        public Module Parent { get; }
        internal Binding.Program Binding { get; }

        public Program(Module parent)
        {
            Parent = parent;
            Binding = new Binding.Program(Parent.Binding);
        }
        #endregion

        #region Pretty
        ShaderReflection? _Reflection;
        public ShaderReflection GetReflection(Target target)
        {
            var session = Parent.Parent;
            uint? targetIndex = session.Targets.IndexOf(target);

            if (targetIndex is null)
                throw new ArgumentException($"Target {target} was not included in Session {session}. Use Session.Builder.AddTarget(target) to include it.", nameof(target));

            return _Reflection ??= new ShaderReflection(this, Binding.GetReflection(targetIndex.Value));
        }

        public CompilationResult Compile(EntryPoint entryPoint, Target target)
        {
            var session = Parent.Parent;
            uint? targetIndex = session.Targets.IndexOf(target);

            if (targetIndex is null)
                throw new ArgumentException($"Target {target} was not included in Session {session}. Use Session.Builder.AddTarget({target}) to include it.", nameof(target));

            var result = Binding.Compile((uint)entryPoint.Index, targetIndex.Value);
            return new CompilationResult(result.Source, target, entryPoint, result.Result, result.Diagnostics);
        }
        #endregion
    }
}
