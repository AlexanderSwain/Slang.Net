#pragma once
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "ShaderModelCLI.h"
#include "ParameterInfoCLI.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

using namespace Native;

namespace SlangNative
{
    extern "C" SLANGNATIVE_API void* CreateSession(Native::CompilerOptionCLI* options, int optionsLength,
        Native::PreprocessorMacroDescCLI* macros, int macrosLength,
        Native::ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength,
        const char** error);

    extern "C" SLANGNATIVE_API void* CreateModule(void* parentSession, const char* moduleName, const char* modulePath, const char* shaderSource, const char** error);

    extern "C" SLANGNATIVE_API void* FindEntryPoint(void* parentModule, const char* entryPointName, const char** error);

    extern "C" SLANGNATIVE_API void GetParameterInfo(void* parentEntryPoint, Native::ParameterInfoCLI** outParameterInfo, int* outParameterCount);

    extern "C" SLANGNATIVE_API void* CreateProgram(void* parentEntryPoint);

    extern "C" SLANGNATIVE_API int32_t Compile(void* program, const char** output);
}
