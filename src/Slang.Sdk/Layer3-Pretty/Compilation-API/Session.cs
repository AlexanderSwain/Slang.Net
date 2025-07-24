using Slang.Sdk.Collections;
using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public partial class Session
        : IComposition<Target>
    {
        #region Definition
        internal Binding.Session Binding { get; }

        internal Session(CompilerOption[] options, PreprocessorMacro[] macros, Target[] models, string[] searchPaths)
        {
            Binding = new Binding.Session(options, macros, models, searchPaths);
        }
        #endregion

        #region Pretty
        public Module ImportModule(string moduleName)
        {
            // Allows the users to either include or exclude the .slang extension
            moduleName = Path.GetFileNameWithoutExtension(moduleName);

            return new Module(this, moduleName);
        }
        public Module FindModule(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentException("Module name cannot be null or empty.", nameof(moduleName));

            if (!Binding.SearchPaths.Any())
                throw new InvalidOperationException("");

            // Allows the users to either include or exclude the .slang extension
            moduleName = Path.GetFileNameWithoutExtension(moduleName);

            string? modulePath = null;
            string? moduleSource = null;
            foreach (var searchPath in Binding.SearchPaths)
            {
                var filePath = Path.Combine(searchPath, $"{moduleName}.slang");
                if (File.Exists(filePath))
                {
                    modulePath = filePath;
                    moduleSource = File.ReadAllText(filePath);
                    break;
                }
            }
            if (modulePath is null || moduleSource is null)
                throw new FileNotFoundException($"{moduleName} could not be found in any of the parent session's search paths.");

            return new Module(this, moduleName, modulePath, moduleSource);
        }

        public Module LoadModule(string modulePath)
        {
            if (string.IsNullOrWhiteSpace(modulePath))
                throw new ArgumentException("Module name cannot be null or empty.", nameof(modulePath));

            string? moduleName = Path.GetFileName(modulePath);
            string? moduleSource = File.Exists(modulePath) ? File.ReadAllText(modulePath) : null;

            if (moduleName is null)
                throw new FormatException($"The specified path is not correctly formatted: {modulePath}");
            if (moduleSource is null)
                throw new FileNotFoundException($"The specified slang file was not found: {modulePath}", modulePath);

            return new Module(this, moduleName, modulePath, moduleSource);
        }

        public Module LoadModule(string moduleName, string modulePath)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentException("Module name cannot be null or empty.", nameof(moduleName));

            if (string.IsNullOrWhiteSpace(modulePath))
                throw new ArgumentException("Module path cannot be null or empty.", nameof(modulePath));

            if (!Binding.SearchPaths.Any())
                throw new InvalidOperationException("");

            // Allows the users to either include or exclude the .slang extension
            moduleName = Path.GetFileNameWithoutExtension(moduleName);

            string? moduleSource = null;
            if (File.Exists(modulePath))
                moduleSource = File.ReadAllText(modulePath);

            if (moduleSource is null)
                throw new FileNotFoundException($"The specified slang file was not found: { modulePath}", modulePath);

            return new Module(this, moduleName, modulePath, moduleSource);
        }

        SlangCollection<Target>? _Targets;
        public SlangCollection<Target> Targets => _Targets ??= new SlangCollection<Target>(this);
        #endregion

        #region Composition

        // Targets
        Target IComposition<Target>.GetByIndex(uint index)
        {
            return Binding.Targets.ElementAt((int)index);
        }
        uint IComposition<Target>.Count => (uint)Binding.Targets.Count;

        #endregion
    }
}
