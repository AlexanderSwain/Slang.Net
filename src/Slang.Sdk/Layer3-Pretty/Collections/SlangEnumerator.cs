using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    internal class SlangEnumerator<T> : IEnumerator<T>
    {
        internal IComposition<T> Composition { get; }
        internal uint? Current_Index { get; set; }

        internal SlangEnumerator(IComposition<T> composition)
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
