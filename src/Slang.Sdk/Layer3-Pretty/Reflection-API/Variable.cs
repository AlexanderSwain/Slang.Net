using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public unsafe class Variable : Reflection,
        IComposition<Attribute>,
        INamedComposition<Attribute>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.VariableReflection Binding { get; }

        internal Variable(Reflection parent, Binding.VariableReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<Attribute>

        uint IComposition<Attribute>.Count => Binding.GetUserAttributeCount();
        Attribute IComposition<Attribute>.GetByIndex(uint index) =>
            new Attribute(this, Binding.GetUserAttributeByIndex(index));

        #endregion

        #region INamedComposition<Attribute>

        Attribute? INamedComposition<Attribute>.FindByName(string name) =>
            Binding.FindUserAttributeByName(name) is { } attr ? new Attribute(this, attr) : null;

        #endregion

        #region Collections

        SlangNamedCollection<Attribute>? _UserAttributes;
        public SlangNamedCollection<Attribute> UserAttributes => 
            _UserAttributes ??= new SlangNamedCollection<Attribute>(this, this);

        #endregion

        #region Pretty
        public string? Name => Binding.GetName();
        public bool HasDefaultValue => Binding.HasDefaultValue();

        public Type Type => new Type(this, Binding.GetType());

        public Generic? GenericContainer =>
            Binding.GetGenericContainer() is { } container ? new Generic(this, container) : null;

        public Modifier? FindModifier(int id) =>
            Binding.FindModifier(id) is { } modifier ? new Modifier(this, modifier) : null;

        public long GetDefaultValueInt() => Binding.GetDefaultValueInt();

        public Variable? ApplySpecializations(Generic[] specializations)
        {
            if (specializations == null || specializations.Length == 0)
                return null;

            var handles = specializations.Select(s => s.Binding.Handle.DangerousGetHandle()).ToArray();
            fixed (nint* handlePtr = handles)
            {
                return Binding.ApplySpecializations((void**)handlePtr, specializations.Length) is { } specialized ?
                    new Variable(this, specialized) : null;
            }
        }

        #endregion
    }
}
