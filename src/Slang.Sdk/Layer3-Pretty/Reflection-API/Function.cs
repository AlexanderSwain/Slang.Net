using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public unsafe class Function : Reflection,
        IComposition<Variable>,
        IComposition<Attribute>,
        INamedComposition<Attribute>,
        IComposition<Function>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.FunctionReflection Binding { get; }

        internal Function(Reflection parent, Binding.FunctionReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<Variable> (Parameters)

        uint IComposition<Variable>.Count => Binding.GetParameterCount();
        Variable IComposition<Variable>.GetByIndex(uint index) =>
            new Variable(this, Binding.GetParameterByIndex(index));

        #endregion

        #region IComposition<Attribute>

        uint IComposition<Attribute>.Count => Binding.GetUserAttributeCount();
        Attribute IComposition<Attribute>.GetByIndex(uint index) =>
            new Attribute(this, Binding.GetUserAttributeByIndex(index));

        #endregion

        #region INamedComposition<Attribute>

        Attribute? INamedComposition<Attribute>.FindByName(string name) =>
            Binding.FindAttributeByName(name) is { } attr ? new Attribute(this, attr) : null;

        #endregion

        #region IComposition<Function> (Overloads)

        uint IComposition<Function>.Count => Binding.GetOverloadCount();
        Function IComposition<Function>.GetByIndex(uint index) =>
            new Function(this, Binding.GetOverload(index));

        #endregion

        #region Collections

        SlangCollection<Variable>? _Parameters;
        public SlangCollection<Variable> Parameters => 
            _Parameters ??= new SlangCollection<Variable>(this);

        SlangNamedCollection<Attribute>? _UserAttributes;
        public SlangNamedCollection<Attribute> UserAttributes => 
            _UserAttributes ??= new SlangNamedCollection<Attribute>(this, this);

        SlangCollection<Function>? _Overloads;
        public SlangCollection<Function> Overloads => 
            _Overloads ??= new SlangCollection<Function>(this);

        #endregion

        #region Pretty

        public string? Name => Binding.GetName();
        public bool IsOverloaded => Binding.IsOverloaded();

        public Type ReturnType => new Type(this, Binding.GetReturnType());

        public Generic? GenericContainer =>
            Binding.GetGenericContainer() is { } container ? new Generic(this, container) : null;

        public Modifier? FindModifier(int id) =>
            Binding.FindModifier(id) is { } modifier ? new Modifier(this, modifier) : null;

        public Function? ApplySpecializations(Generic? genRef) =>
            genRef?.Binding is { } genBinding && Binding.ApplySpecializations(genBinding) is { } specialized ?
                new Function(this, specialized) : null;

        public Function? SpecializeWithArgTypes(Type[] types)
        {
            // Convert to handles and then to void**
            var handles = types.Select(t => t.Binding.Handle.DangerousGetHandle()).ToArray();
            fixed (nint* handlePtr = handles)
            {
                return Binding.SpecializeWithArgTypes((uint)handles.Length, (void**)handlePtr) is { } specialized ?
                    new Function(this, specialized) : null;
            }
        }

        #endregion
    }
}
