using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    internal interface ITypedComposition<Key, Value>
    {
        Value? Find(Key key);
    }
}
