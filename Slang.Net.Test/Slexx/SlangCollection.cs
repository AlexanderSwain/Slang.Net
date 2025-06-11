using System.Collections;
using System.Diagnostics.CodeAnalysis;

// Delete this
namespace Slang.Net.Test.Slexx
{
    public interface IComposedOf<T>
    {
        uint Count { get; }
        T GetByIndex(uint index);
    }

    public class SlangCollection<T> : IEnumerable<T>
    {
        public IComposedOf<T> Parent { get; set; }

        public SlangCollection(IComposedOf<T> parent)
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

    public class SlangEnumerator<T> : IEnumerator<T>
    {
        public IComposedOf<T> Composition { get; }
        public uint? Current_Index { get; set; }

        public SlangEnumerator(IComposedOf<T> composition)
        {
            Composition = composition;
        }

        #region Enumerable
        public T Current => Composition.GetByIndex(Current_Index ?? throw new NullReferenceException());
        object? IEnumerator.Current => Composition.GetByIndex(Current_Index ?? throw new NullReferenceException());

        public bool MoveNext()
        {
            if (Current_Index == null)
                Current_Index = 0;
            else
                Current_Index++;

            return Current_Index < Composition.Count;
        }

        public void Reset()
        {
            Current_Index = null;
        }

        public void Dispose()
        {
            // Nothing to dispose here
        }
        #endregion
    }
}
