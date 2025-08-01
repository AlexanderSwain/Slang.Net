using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public class TypeLayout : Reflection, IEquatable<TypeLayout>,
        IComposition<VariableLayout>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.TypeLayoutReflection Binding { get; }

        internal TypeLayout(Reflection parent, Binding.TypeLayoutReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<VariableLayout>

        uint IComposition<VariableLayout>.Count => Binding.GetFieldCount();
        VariableLayout IComposition<VariableLayout>.GetByIndex(uint index) =>
            new VariableLayout(this, Binding.GetFieldByIndex(index));

        #endregion

        #region Collections

        SlangCollection<VariableLayout>? _Fields;
        public SlangCollection<VariableLayout> Fields => 
            _Fields ??= new SlangCollection<VariableLayout>(this);

        #endregion

        #region Pretty
        public TypeKind Kind => Binding.GetKind();
        public bool IsArray => Binding.IsArray();
        public nuint ElementCount => Binding.GetElementCount();
        public nuint TotalArrayElementCount => Binding.GetTotalArrayElementCount();

        public Type Type => new Type(this, Binding.GetType());

        public TypeLayout? UnwrapArray() =>
            Binding.UnwrapArray() is { } unwrapped ? new TypeLayout(this, unwrapped) : null;

        public TypeLayout? ElementTypeLayout =>
            Binding.GetElementTypeLayout() is { } element ? new TypeLayout(this, element) : null;

        public VariableLayout? ElementVarLayout =>
            Binding.GetElementVarLayout() is { } element ? new VariableLayout(this, element) : null;

        public VariableLayout? ContainerVarLayout =>
            Binding.GetContainerVarLayout() is { } container ? new VariableLayout(this, container) : null;

        public VariableLayout? ExplicitCounter =>
            Binding.GetExplicitCounter() is { } counter ? new VariableLayout(this, counter) : null;

        public nuint GetSize(ParameterCategory category) => Binding.GetSize(category);
        public nuint GetStride(ParameterCategory category) => Binding.GetStride(category);
        public int GetAlignment(ParameterCategory category) => Binding.GetAlignment(category);
        public nuint GetElementStride(ParameterCategory category) => Binding.GetElementStride(category);

        public int FindFieldIndexByName(string name) => Binding.FindFieldIndexByName(name);

        #endregion

        #region Equality
        public bool Equals(TypeLayout? other)
        {
            return this == other;
        }
        #endregion

    }
}
