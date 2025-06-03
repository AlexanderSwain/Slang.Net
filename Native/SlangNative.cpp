#include "SlangNative.h"
#include <string>
#include "SessionCLI.h"
#include "ModuleCLI.h"
#include "EntryPointCLI.h"
#include "ProgramCLI.h"


namespace SlangNative
{
    static thread_local std::string g_lastError;

    extern "C" SLANGNATIVE_API void* CreateSession(CompilerOptionCLI* options, int optionsLength,
        PreprocessorMacroDescCLI* macros, int macrosLength,
        ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength,
        const char** error)
    {
        try
        {
            SessionCLI* result = new SessionCLI(options, optionsLength, macros, macrosLength, models, modelsLength, searchPaths, searchPathsLength);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
			return nullptr;
        }
    }

    extern "C" SLANGNATIVE_API void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource, const char** error)
    {
        try
        {
            ModuleCLI* result = new ModuleCLI((SessionCLI*)parentSession, moduleName, modulePath, shaderSource);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
            return nullptr;
        }
    }

    extern "C" SLANGNATIVE_API void* FindEntryPoint(void* parentModule, const char* entryPointName, const char** error)
    {
        try 
        {
            EntryPointCLI* result = new EntryPointCLI((ModuleCLI*)parentModule, entryPointName);
            *error = nullptr;
            return result;
        }
        catch (const std::exception& e)
        {
            g_lastError = e.what();
            *error = g_lastError.c_str();
			return nullptr;
        }
    }

    extern "C" SLANGNATIVE_API void GetParameterInfo(void* parentEntryPoint, Native::ParameterInfoCLI** outParameterInfoArray, int* outParameterCount)
    {
        EntryPointCLI* entryPoint = (EntryPointCLI*)parentEntryPoint;
		*outParameterInfoArray = entryPoint->getParameterInfoArray();
        *outParameterCount = entryPoint->getParameterCount();
    }

    extern "C" SLANGNATIVE_API void* CreateProgram(void* parentModule)
    {
        ProgramCLI* result = new ProgramCLI((ModuleCLI*)parentModule);
        return result;
    }

    extern "C" SLANGNATIVE_API int32_t Compile(void* program, unsigned int entryPointIndex, unsigned int targetIndex, const char** output)
    {
        int32_t result = ((ProgramCLI*)program)->GetCompiled(entryPointIndex, targetIndex, output);
        return result;
    }
}