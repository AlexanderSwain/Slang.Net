#include "SlangNative.h"
#include "SessionCLI.h"
#include "ModuleCLI.h"
#include "EntryPointCLI.h"
#include "ProgramCLI.h"


namespace SlangNative
{
    extern "C" SLANGNATIVE_API void* CreateSession(CompilerOptionCLI* options, int optionsLength,
        PreprocessorMacroDescCLI* macros, int macrosLength,
        ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength)
    {
        SessionCLI* result = new SessionCLI(options, optionsLength, macros, macrosLength, models, modelsLength, searchPaths, searchPathsLength);
        return result;
    }

    extern "C" SLANGNATIVE_API void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource)
    {
        ModuleCLI* result = new ModuleCLI((SessionCLI*)parentSession, moduleName, modulePath, shaderSource);
        return result;
    }

    extern "C" SLANGNATIVE_API void* FindEntryPoint(void* parentModule, const char* entryPointName)
    {
        EntryPointCLI* result = new EntryPointCLI((ModuleCLI*)parentModule, entryPointName);
        return result;
    }

    extern "C" SLANGNATIVE_API void* CreateProgram(void* parentEntryPoint)
    {
        ProgramCLI* result = new ProgramCLI((EntryPointCLI*)parentEntryPoint);
        return result;
    }

    extern "C" SLANGNATIVE_API int32_t Compile(void* program, const char** output)
    {
        int32_t result = ((ProgramCLI*)program)->GetCompiled(output);
        return result;
    }
}