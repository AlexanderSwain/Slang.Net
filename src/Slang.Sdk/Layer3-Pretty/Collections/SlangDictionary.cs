using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    public class SlangDictionary<T>
    {
        internal INamedComposition<T> Parent { get; set; }

        public T? this[string name]
        {
            get => Parent.FindByName(name);
        }

        public bool TryGetValue(string name, [MaybeNullWhen(false)] out T? value)
        {
            value = Parent.FindByName(name);
            return value != null;
        }

        internal SlangDictionary(INamedComposition<T> parent)
        {
            Parent = parent;
        }
    }
}
