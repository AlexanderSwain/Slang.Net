using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Net.Extensions
{
    public static class EnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Select((item, index) => (item, index))
                   .FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.item, value))
                   .index;
        }
    }
}
