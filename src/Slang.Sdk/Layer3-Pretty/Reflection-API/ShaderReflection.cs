using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slang.Sdk.Collections;

namespace Slang.Sdk
{
    public unsafe class ShaderReflection : Reflection,
        IComposition<TypeParameter>,
        INamedComposition<TypeParameter>,
        IComposition<VariableLayout>,
        IComposition<EntryPointReflection>,
        INamedComposition<EntryPointReflection>,
        IComposition<string>,
        INamedComposition<Type>
    {
        #region Definition
        public Program Program { get; }
        public override Reflection? Parent => null;
        internal override Binding.ShaderReflection Binding { get; }

        internal ShaderReflection(Program program, Binding.ShaderReflection binding)
        {
            Program = program;
            Binding = binding;
        }
        #endregion

        #region IComposition<TypeParameter>

        uint IComposition<TypeParameter>.Count => Binding.GetTypeParameterCount();
        TypeParameter IComposition<TypeParameter>.GetByIndex(uint index) =>
            new TypeParameter(this, Binding.GetTypeParameterByIndex(index));

        #endregion

        #region INamedComposition<TypeParameter>

        TypeParameter? INamedComposition<TypeParameter>.FindByName(string name) =>
            new TypeParameter(this, Binding.FindTypeParameter(name));

        #endregion

        #region IComposition<VariableLayout>

        uint IComposition<VariableLayout>.Count => Binding.GetParameterCount();
        VariableLayout IComposition<VariableLayout>.GetByIndex(uint index) =>
            new VariableLayout(this, Binding.GetParameterByIndex(index));

        #endregion

        #region IComposition<EntryPoint>

        uint IComposition<EntryPointReflection>.Count => Binding.GetEntryPointCount();
        EntryPointReflection IComposition<EntryPointReflection>.GetByIndex(uint index) =>
            new EntryPointReflection(this, Binding.GetEntryPointByIndex(index));

        #endregion

        #region INamedComposition<EntryPoint>

        EntryPointReflection? INamedComposition<EntryPointReflection>.FindByName(string name) =>
            new EntryPointReflection(this, Binding.FindEntryPointByName(name));

        #endregion

        #region IComposition<string>

        uint IComposition<string>.Count => Binding.GetHashedStringCount();
        string IComposition<string>.GetByIndex(uint index) =>
            Binding.GetHashedString(index) ?? string.Empty;

        #endregion

        #region INamedComposition<Type>

        Type? INamedComposition<Type>.FindByName(string name) =>
            new Type(this, Binding.FindTypeByName(name));

        #endregion

        #region Collections

        SlangNamedCollection<TypeParameter>? _TypeParameters;
        public SlangNamedCollection<TypeParameter> TypeParameters => 
            _TypeParameters ??= new SlangNamedCollection<TypeParameter>(this, this);

        SlangCollection<VariableLayout>? _Parameters;
        public SlangCollection<VariableLayout> Parameters => 
            _Parameters ??= new SlangCollection<VariableLayout>(this);

        SlangNamedCollection<EntryPointReflection>? _EntryPoints;
        public SlangNamedCollection<EntryPointReflection> EntryPoints => 
            _EntryPoints ??= new SlangNamedCollection<EntryPointReflection>(this, this);

        SlangCollection<string>? _HashedStrings;
        public SlangCollection<string> HashedStrings => 
            _HashedStrings ??= new SlangCollection<string>(this);

        SlangDictionary<Type>? _Types;
        public SlangDictionary<Type> Types => 
            _Types ??= new SlangDictionary<Type>(this);

        #endregion

        #region Pretty
        public uint GlobalConstantBufferBinding => Binding.GetGlobalConstantBufferBinding();
        public nuint GlobalConstantBufferSize => Binding.GetGlobalConstantBufferSize();

        public TypeLayout GlobalParamsTypeLayout => new TypeLayout(this, Binding.GetGlobalParamsTypeLayout());
        public VariableLayout GlobalParamsVarLayout => new VariableLayout(this, Binding.GetGlobalParamsVarLayout());

        public TypeLayout GetTypeLayout(Type type, int layoutRules) =>
            new TypeLayout(this, Binding.GetTypeLayout(type.Binding, layoutRules));

        public Type SpecializeType(Type type, int argCount, nint args) =>
            new Type(this, Binding.SpecializeType(type.Binding, argCount, (void**)args));

        public bool IsSubType(Type subType, Type superType) =>
            Binding.IsSubType(subType.Binding, superType.Binding);

        public Function FindFunctionByNameInType(Type type, string name) =>
            new Function(this, Binding.FindFunctionByNameInType(type.Binding, name));

        public Variable FindVarByNameInType(Type type, string name) =>
            new Variable(this, Binding.FindVarByNameInType(type.Binding, name));

        public string? ToJson() => Binding.ToJson();

        #endregion
    }
}
