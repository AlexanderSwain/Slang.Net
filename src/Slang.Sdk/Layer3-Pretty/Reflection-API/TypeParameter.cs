using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public class TypeParameter : Reflection, IEquatable<TypeParameter>,
        IComposition<Type>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.TypeParameterReflection Binding { get; }

        internal TypeParameter(Reflection parent, Binding.TypeParameterReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<Type> (Constraints)

        uint IComposition<Type>.Count => Binding.GetConstraintCount();
        Type IComposition<Type>.GetByIndex(uint index) =>
            new Type(this, Binding.GetConstraintByIndex((int)index));

        #endregion

        #region Collections

        SlangCollection<Type>? _Constraints;
        public SlangCollection<Type> Constraints => 
            _Constraints ??= new SlangCollection<Type>(this);

        #endregion

        #region Pretty

        public string? Name => Binding.GetName();
        public uint Index => Binding.GetIndex();

        #endregion

        #region Equality
        public bool Equals(TypeParameter? other)
        {
            return this == other;
        }
        #endregion
    }
}
