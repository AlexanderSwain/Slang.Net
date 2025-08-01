using System.Collections;

namespace Slang.Sdk.Collections
{
    public class SlangCollection<T> : IEnumerable<T> where T : IEquatable<T>
    {
        internal IComposition<T> Owner { get; set; }

        public T this[uint index]
        {
            get => Owner.GetByIndex(index);
        }

        public uint Count => Owner.Count;

        public uint? IndexOf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return Owner.IndexOf(item);
        }

        internal SlangCollection(IComposition<T> owner)
        {
            Owner = owner;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SlangEnumerator<T>(Owner);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
