using System.Collections;

namespace Slang.Sdk.Collections
{
    public class SlangNamedCollectionary<T> : IEnumerable<T> where T : IEquatable<T>
    {
        internal IComposition<T> List { get; }
        internal INamedComposition<T> Dict { get; }

        public T this[uint index]
        {
            get
            {
                return List.GetByIndex(index);
            }
        }

        public T this[string name]
        {
            get
            {
                return Dict.FindByName(name) ?? throw new ($"(Key: {name}) doesn't exist in collection: {this}. Use TryGetValue() instead if you need to check if the name exists.");
            }
        }

        public uint Count => List.Count;

        public uint? IndexOf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return List.IndexOf(item);
        }

        public bool TryGetValue(string name, out T? value)
        {
            value = Dict.FindByName(name);
            return value != null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SlangEnumerator<T>(List);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal SlangNamedCollectionary(IComposition<T> list, INamedComposition<T> dict)
        {
            List = list;
            Dict = dict;
        }
    }
}
