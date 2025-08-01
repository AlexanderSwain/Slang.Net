using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public class Attribute : Reflection, IEquatable<Attribute>,
        IComposition<Type>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.AttributeReflection Binding { get; }

        internal Attribute(Reflection parent, Binding.AttributeReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<Type> (Argument Types)
        uint IComposition<Type>.Count => Binding.GetArgumentCount();
        Type IComposition<Type>.GetByIndex(uint index) =>
            new Type(this, Binding.GetArgumentType(index));

        #endregion

        #region Collections

        SlangCollection<Type>? _ArgumentTypes;
        public SlangCollection<Type> ArgumentTypes => _ArgumentTypes ??= new SlangCollection<Type>(this);

        #endregion

        #region Pretty
        public string? Name => Binding.GetName();

        public int GetArgumentValueInt(uint index) => Binding.GetArgumentValueInt(index);

        public float GetArgumentValueFloat(uint index) => Binding.GetArgumentValueFloat(index);

        public string? GetArgumentValueString(uint index) => Binding.GetArgumentValueString(index);

        #endregion

        #region Equality
        public bool Equals(Attribute? other)
        {
            return this == other;
        } 
        #endregion
    }
}
