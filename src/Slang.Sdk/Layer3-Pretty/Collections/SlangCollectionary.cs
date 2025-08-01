using Slang.Sdk.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Slang.Sdk.Interop.CompilerOption;

namespace Slang.Sdk
{
    public class SlangCollectionary<Key, Value> : IEnumerable<Value> where Value : IEquatable<Value>
    {
        internal IComposition<Value> List { get; }
        internal ITypedComposition<Key, Value> Dict { get; }

        public Value this[uint index]
        {
            get
            {
                return List.GetByIndex(index);
            }
        }

        public Value this[Key key]
        {
            get
            {
                return Dict.Find(key) ?? throw new($"(Key: {key}) doesn't exist in collection: {this}. Use TryGetValue() instead if you need to check if the key exists.");
            }
        }

        public uint Count => List.Count;

        public uint? IndexOf(Value item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return List.IndexOf(item);
        }

        public bool TryGetValue(Key key, out Value? value)
        {
            value = Dict.Find(key);
            return value != null;
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return new SlangEnumerator<Value>(List);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal SlangCollectionary(IComposition<Value> list, ITypedComposition<Key, Value> dict)
        {
            List = list;
            Dict = dict;
        }
    }
}
