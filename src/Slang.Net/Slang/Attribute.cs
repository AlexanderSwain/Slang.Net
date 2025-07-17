using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Slang
{
    public unsafe class Attribute
    {
        internal Slang.Cpp.Attribute cppObj { get; }

        public Attribute(Slang.Cpp.Attribute comObj)
        {
            cppObj = comObj;
        }

        public string Name => cppObj.Name;

        public uint ArgumentCount => cppObj.ArgumentCount;

        public string GetArgumentValueString(uint index)
        {
            return cppObj.GetArgumentValueString(index);
        }

        public int? GetArgumentValueInt(uint index)
        {
            return cppObj.GetArgumentValueInt(index);
        }

        public float? GetArgumentValueFloat(uint index)
        {
            return cppObj.GetArgumentValueFloat(index);
        }
    }
}