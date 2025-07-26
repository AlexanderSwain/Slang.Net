using System.Collections;

namespace Slang.Sdk.Collections
{
    public class SlangCollection<T> : IEnumerable<T>
    {
        internal IComposition<T> Parent { get; set; }

        public T this[uint index]
        {
            get => Parent.GetByIndex(index);
        }

        public uint? IndexOf(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return Parent.IndexOf(item);
        }

        internal SlangCollection(IComposition<T> parent)
        {
            Parent = parent;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SlangEnumerator<T>(Parent);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
