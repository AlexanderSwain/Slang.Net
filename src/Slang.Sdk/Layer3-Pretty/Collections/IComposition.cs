using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    internal interface IComposition<T>
    {
        uint Count { get; }
        T GetByIndex(uint index);
    }
}
