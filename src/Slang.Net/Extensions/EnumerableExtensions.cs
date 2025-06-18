using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Net.Extensions
{
    public static class EnumerableExtensions
    {
        extension<T>(IEnumerable<T> enumerable)
        {
            public int IndexOf(T value)
            {
                return enumerable.Select((item, index) => (item, index))
                       .FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.item, value))
                       .index;
            }
        }
    }
}
