using Slang.CLI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Layer3_Pretty.CLI
{
    public partial class ComplierOptions
    {
        public class Builder
        {
            CompilerOptionsCLI _options = new();

            public Builder SetTarget(string? target)
            {
                _options.Target = target;
                return this;
            }
            public Builder SetProfile(string? profile)
            {
                _options.Profile = profile;
                return this;
            }

            public Builder SetEntry(string? entry)
            {
                _options.Entry = entry;
                return this;
            }

            public Builder SetStage(string? stage)
            {
                _options.Stage = stage;
                return this;
            }

            public Builder SetOutputPath(string? outputPath)
            {
                _options.OutputPath = outputPath;
                return this;
            }

            public Builder SetModuleName(string? moduleName)
            {
                _options.ModuleName = moduleName;
                return this;
            }

            public Builder AddIncludePaths(string includePath)
            {
                _options.IncludePaths.Add(includePath);
                return this;
            }

            public Builder Define(string key, string value)
            {
                if (_options.Defines == null)
                    _options.Defines = new Dictionary<string, string>();
                _options.Defines[key] = value;
                return this;
            }
            public Builder SetStd(string? std)
            {
                _options.Std = std;
                return this;
            }
            public Builder SetWarningsAsErrors(bool warningsAsErrors)
            {
                _options.WarningsAsErrors = warningsAsErrors;
                return this;
            }
            public Builder DisableWarning(string warning)
            {
                _options.DisableWarnings.Add(warning);
                return this;
            }

            public Builder SetOptimizationLevel(string? optimizationLevel)
            {
                _options.OptimizationLevel = optimizationLevel;
                return this;
            }

            public Builder SetEmitIR(bool emitIR)
            {
                _options.EmitIR = emitIR;
                return this;
            }

            public Builder SetDebugInfo(bool debugInfo)
            {
                _options.DebugInfo = debugInfo;
                return this;
            }

            public Builder SetReflectionJsonPath(string? reflectionJsonPath)
            {
                _options.ReflectionJsonPath = reflectionJsonPath;
                return this;
            }

            public Builder AddInputFile(string inputFile)
            {
                _options.InputFiles.Add(inputFile);
                return this;
            }

            public CompilerOptionsCLI Build()
            {
                return _options;
            }
        }
    }
}
