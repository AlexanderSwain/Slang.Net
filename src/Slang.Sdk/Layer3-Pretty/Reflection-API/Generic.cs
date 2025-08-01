using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public class Generic : Reflection, IEquatable<Generic>,
        IComposition<Variable>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.GenericReflection Binding { get; }

        internal Generic(Reflection parent, Binding.GenericReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<Variable> - Type Parameters

        uint IComposition<Variable>.Count => Binding.GetTypeParameterCount();
        Variable IComposition<Variable>.GetByIndex(uint index) =>
            new Variable(this, Binding.GetTypeParameter(index));

        #endregion

        #region Value Parameters (accessed via property)

        public uint ValueParameterCount => Binding.GetValueParameterCount();
        public Variable GetValueParameter(uint index) =>
            new Variable(this, Binding.GetValueParameter(index));

        #endregion

        #region Collections

        SlangCollection<Variable>? _TypeParameters;
        public SlangCollection<Variable> TypeParameters => 
            _TypeParameters ??= new SlangCollection<Variable>(this);

        SlangCollection<Variable>? _ValueParameters;
        public SlangCollection<Variable> ValueParameters => 
            _ValueParameters ??= new SlangCollection<Variable>(this);

        #endregion

        #region Pretty

        public string? Name => Binding.GetName();
        public int InnerKind => Binding.GetInnerKind();

        public Generic? OuterGenericContainer =>
            Binding.GetOuterGenericContainer() is { } outer ? new Generic(this, outer) : null;

        public uint GetTypeParameterConstraintCount(TypeParameter typeParam) =>
            Binding.GetTypeParameterConstraintCount(typeParam.Binding);

        public Type GetTypeParameterConstraintType(TypeParameter typeParam, uint index) =>
            new Type(this, Binding.GetTypeParameterConstraintType(typeParam.Binding, index));

        public Type? GetConcreteType(TypeParameter typeParam) =>
            Binding.GetConcreteType(typeParam.Binding) is { } concrete ? 
                new Type(this, concrete) : null;

        public long GetConcreteIntVal(Variable valueParam) =>
            Binding.GetConcreteIntVal(valueParam.Binding);

        public Generic? ApplySpecializations(Generic genRef) =>
            Binding.ApplySpecializations(genRef.Binding) is { } specialized ?
                new Generic(this, specialized) : null;

        #endregion

        #region Equality
        public bool Equals(Generic? other)
        {
            return this == other;
        }
        #endregion
    }
}
