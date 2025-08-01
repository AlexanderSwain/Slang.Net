using System.Diagnostics.CodeAnalysis;

namespace Slang.Sdk.Collections
{
    public class SlangNameDictionary<T>
    {
        internal INamedComposition<T> Owner { get; set; }

        public T? this[string name]
        {
            get => Owner.FindByName(name);
        }

        public bool TryGetValue(string name, [MaybeNullWhen(false)] out T? value)
        {
            value = Owner.FindByName(name);
            return value != null;
        }

        internal SlangNameDictionary(INamedComposition<T> owner)
        {
            Owner = owner;
        }
    }
}