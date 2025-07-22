using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;
using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public class VariableLayout : Reflection,
        IComposition<ParameterCategory>
    {
        #region Definition
        public override Reflection? Parent { get; }
        internal override Binding.VariableLayoutReflection Binding { get; }

        internal VariableLayout(Reflection parent, Binding.VariableLayoutReflection binding)
        {
            Parent = parent;
            Binding = binding;
        }
        #endregion

        #region IComposition<ParameterCategory> (Categories)

        uint IComposition<ParameterCategory>.Count => Binding.GetCategoryCount();
        ParameterCategory IComposition<ParameterCategory>.GetByIndex(uint index) =>
            Binding.GetCategoryByIndex(index);

        #endregion

        #region Collections

        SlangCollection<ParameterCategory>? _Categories;
        public SlangCollection<ParameterCategory> Categories => 
            _Categories ??= new SlangCollection<ParameterCategory>(this);

        #endregion

        #region Pretty
        public string? Name => Binding.GetName();
        public ParameterCategory Category => Binding.GetCategory();
        public uint BindingIndex => Binding.GetBindingIndex();
        public uint BindingSpace => Binding.GetBindingSpace();
        public int ImageFormat => Binding.GetImageFormat();
        public string? SemanticName => Binding.GetSemanticName();
        public nuint SemanticIndex => Binding.GetSemanticIndex();
        public uint Stage => Binding.GetStage();

        public Variable? Variable => 
            Binding.GetVariable() is { } variable ? new Variable(this, variable) : null;

        public TypeLayout TypeLayout => new TypeLayout(this, Binding.GetTypeLayout());

        public Type Type => new Type(this, Binding.GetType());

        public VariableLayout? PendingDataLayout =>
            Binding.GetPendingDataLayout() is { } pending ? new VariableLayout(this, pending) : null;

        public Modifier? FindModifier(int id) =>
            Binding.FindModifier(id) is { } modifier ? new Modifier(this, modifier) : null;

        public nuint GetOffset(ParameterCategory category) => Binding.GetOffset(category);

        public VariableLayout? GetSpace(ParameterCategory category) =>
            Binding.GetSpace(category) is { } space ? new VariableLayout(this, space) : null;

        #endregion
    }
}
