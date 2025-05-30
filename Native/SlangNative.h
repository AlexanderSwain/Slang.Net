#pragma once
#include "CompilerOptionCLI.h"
#include "PreprocessorMacroDescCLI.h"
#include "ShaderModelCLI.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

using namespace Native;

namespace SlangNative
{

    //// Export a simple function to test the DLL
    //extern "C" SLANGNATIVE_API const char* GetVersion();
    //
    //// Export function to create a global session
    //extern "C" SLANGNATIVE_API slang::IGlobalSession* CreateGlobalSession();
    //
    //// Export function to release a global session
    //extern "C" SLANGNATIVE_API void ReleaseGlobalSession(slang::IGlobalSession* session);

    extern "C" SLANGNATIVE_API void* CreateSession(Native::CompilerOptionCLI* options, int optionsLength,
        Native::PreprocessorMacroDescCLI* macros, int macrosLength,
        Native::ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength);
}
