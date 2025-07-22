using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Collections
{
    internal interface INamedComposition<T>
    {
        T? FindByName(string name);
    }
}
