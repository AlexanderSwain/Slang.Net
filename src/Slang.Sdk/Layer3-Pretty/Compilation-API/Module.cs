using Slang.Sdk.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class Module :
        IComposition<EntryPoint>,
        INamedComposition<EntryPoint>
    {
        #region Definition
        public Session Parent { get; }
        internal Binding.Module Binding { get; }

        internal Module(Session parent, string moduleName, string modulePath, string moduleSource)
        {
            Parent = parent;
            Binding = new Binding.Module(Parent.Binding, moduleName, modulePath, moduleSource);
        }

        internal Module(Session parent, string moduleName)
        {
            Parent = parent;
            Binding = new Binding.Module(Parent.Binding, moduleName);
        }

        internal Module(Session parent, Binding.Module binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region Composition<EntryPoint>
        public uint Count => Binding.GetEntryPointCount();

        public EntryPoint GetByIndex(uint index)
        {
            return new EntryPoint(this, Binding.GetEntryPointByIndex(index));
        }

        public EntryPoint? FindByName(string name)
        {
            return new EntryPoint(this, Binding.GetEntryPointByName(name));
        }
        #endregion

        #region Pretty
        Program? _Program;
        public Program Program => _Program ??= new Program(this);
        public string Name => Binding.Name;

        SlangNamedCollection<EntryPoint>? _EntryPoints;
        public SlangNamedCollection<EntryPoint> EntryPoints => _EntryPoints ??= new SlangNamedCollection<EntryPoint>(this, this);
        #endregion
    }
}
