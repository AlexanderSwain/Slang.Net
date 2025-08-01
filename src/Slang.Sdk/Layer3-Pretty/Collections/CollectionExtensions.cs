using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    internal static class CollectionExtensions
    {
        internal static uint? IndexOf<T>(this IComposition<T> source, T value) where T : IEquatable<T>
        {
            for (uint i = 0; i < source.Count; i++)
            {
                if (source.GetByIndex(i).Equals(value))
                    return i;
            }

            return null;
        }
    }
}
