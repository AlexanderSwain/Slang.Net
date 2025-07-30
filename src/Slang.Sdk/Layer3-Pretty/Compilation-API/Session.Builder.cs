using Slang.Sdk.Collections;
using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Slang.Sdk.Interop.Target;

namespace Slang.Sdk
{
    public partial class Session
    {
        public class Builder
        {
            #region Check and Prepare Spirv
            private static object _InitializeSpirvLock = new object();
            private static bool _IsSpirvInitialized = false;
            static void CheckAndPrepareSpirv(Target target)
            {
                Debug.Assert(target.target == Target.CompileTarget.SpirV, "This method should only be called for SpirV targets.");

                if (!SpirvTools.IsInstalled)
                    throw new Exception("The SpirV tools is not installed. It must be installed in order to add a SpirV target. You can get it from the Vulkan Sdk. Download it here: https://www.lunarg.com/vulkan-sdk/");

                lock (_InitializeSpirvLock)
                {
                    if (!_IsSpirvInitialized)
                    {
                        SpirvTools.CopyTools();
                        _IsSpirvInitialized = true;
                    }
                }
            } 
            #endregion

            public List<CompilerOption> CompilerOptions { get; } = new();
            public List<PreprocessorMacro> PreprocessorMacros { get; } = new();
            public List<Target> Targets { get; } = new();
            public List<string> SearchPaths { get; } = new();

            public Builder AddCompilerOption(CompilerOption.Name name, CompilerOption.Value value)
            {
                CompilerOptions.Add(new CompilerOption(name, value));
                return this;
            }

            public Builder AddPreprocessorMacro(string name, string value)
            {
                PreprocessorMacros.Add(new PreprocessorMacro(name, value));
                return this;
            }

            public Builder AddTarget(Target target)
            {
                if (target.target == Target.CompileTarget.SpirV)
                    CheckAndPrepareSpirv(target);

                Targets.Add(target);
                return this;
            }

            public Builder AddSearchPath(string absolutePath)
            {
                SearchPaths.Add(absolutePath);
                return this;
            }

            public Session Create()
            {
                return new Session(CompilerOptions.ToArray(), PreprocessorMacros.ToArray(), Targets.ToArray(), SearchPaths.ToArray());
            }
        }
    }
}
