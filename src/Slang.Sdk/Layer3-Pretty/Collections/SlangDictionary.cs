using Slang.Sdk.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public class SlangDictionary<Key, Value>
    {
        internal ITypedComposition<Key, Value> Owner { get; set; }

        public Value this[Key key]
        {
            get => Owner.Find(key) ?? throw new($"(Key: {key}) doesn't exist in collection: {this}. Use TryGetValue() instead if you need to check if the key exists.");
        }

        public bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value? value)
        {
            value = Owner.Find(key);
            return value != null;
        }

        internal SlangDictionary(ITypedComposition<Key, Value> owner)
        {
            Owner = owner;
        }
    }
}
