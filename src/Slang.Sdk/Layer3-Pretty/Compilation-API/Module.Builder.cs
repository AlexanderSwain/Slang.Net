

using Slang.Sdk.Interop;

namespace Slang.Sdk
{
    public partial class Module
    {
        public class Builder
        {
            #region Definition
            public Session Parent { get; }
            internal Binding.CompileRequest Binding { get; }

            public Builder(Session parent)
            {
                Parent = parent;
                Binding = new Binding.CompileRequest(Parent.Binding);
            }
            #endregion

            #region Pretty
            #region Code Generation and Targets
            public Builder AddCodeGenTarget(Target.CompileTarget target)
            {
                Binding.AddCodeGenTarget(target);
                return this;
            }

            public Builder AddTargetCapability(int targetIndex, CapabilityID capability)
            {
                Binding.AddTargetCapability(targetIndex, capability);
                return this;
            }
            #endregion

            #region Entry Points
            public Builder AddEntryPoint(int translationUnitIndex, string name, Stage stage)
            {
                Binding.AddEntryPoint(translationUnitIndex, name, stage);
                return this;
            }

            public Builder AddEntryPointEx(int translationUnitIndex, string name, Stage stage, params string[] genericArgs)
            {
                Binding.AddEntryPointEx(translationUnitIndex, name, stage, genericArgs);
                return this;
            }
            #endregion

            #region Translation Units and Source
            public int AddTranslationUnit(SourceLanguage language, string name)
            {
                return Binding.AddTranslationUnit(language, name);
            }

            public Builder AddTranslationUnitSourceString(int translationUnitIndex, string path, string source)
            {
                Binding.AddTranslationUnitSourceString(translationUnitIndex, path, source);
                return this;
            }

            public Builder AddTranslationUnitSourceFile(int translationUnitIndex, string path)
            {
                Binding.AddTranslationUnitSourceFile(translationUnitIndex, path);
                return this;
            }

            public Builder AddTranslationUnitSourceBlob(int translationUnitIndex, string path, nint sourceBlob)
            {
                Binding.AddTranslationUnitSourceBlob(translationUnitIndex, path, sourceBlob);
                return this;
            }

            public Builder AddTranslationUnitSourceStringSpan(int translationUnitIndex, string path, string sourceBegin, string sourceEnd)
            {
                Binding.AddTranslationUnitSourceStringSpan(translationUnitIndex, path, sourceBegin, sourceEnd);
                return this;
            }
            #endregion

            #region Preprocessor and Configuration
            public Builder AddPreprocessorDefine(string key, string value)
            {
                Binding.AddPreprocessorDefine(key, value);
                return this;
            }

            public Builder AddTranslationUnitPreprocessorDefine(int translationUnitIndex, string key, string value)
            {
                Binding.AddTranslationUnitPreprocessorDefine(translationUnitIndex, key, value);
                return this;
            }

            public Builder AddSearchPath(string searchDir)
            {
                Binding.AddSearchPath(searchDir);
                return this;
            }
            #endregion

            #region Library References
            public Builder AddLibraryReference(Builder baseRequest, string libName)
            {
                Binding.AddLibraryReference(baseRequest.Binding, libName);
                return this;
            }

            public Builder AddRef()
            {
                Binding.AddRef();
                return this;
            }
            #endregion
            #endregion

            #region Build
            public Module Create()
            {
                return new Module(Parent, this);
            }
            #endregion
        }
    }
}