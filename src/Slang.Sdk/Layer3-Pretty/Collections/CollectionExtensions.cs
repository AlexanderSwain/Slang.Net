using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    internal static class CollectionExtensions
    {
        internal static uint? IndexOf<T>(this IComposition<T> source, T value)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (uint i = 0; i < source.Count; i++)
            {
                if (comparer.Equals(source.GetByIndex(i), value))
                    return i;
            }

            return null;
        }
    }
}
