#include "SlangNative.h"
#include "SessionCLI.h"

namespace SlangNative
{
    //extern "C" SLANGNATIVE_API const char* GetVersion()
    //{
    //    return "SlangNative 1.0.0";
    //}

    //extern "C" SLANGNATIVE_API slang::IGlobalSession* CreateGlobalSession()
    //{
    //    slang::IGlobalSession* session = nullptr;
    //    SlangResult result = slang_createGlobalSession(SLANG_API_VERSION, &session);
    //
    //    if (SLANG_FAILED(result))
    //    {
    //        return nullptr;
    //    }
    //
    //    return session;
    //}

    //extern "C" SLANGNATIVE_API void ReleaseGlobalSession(slang::IGlobalSession* session)
    //{
    //    if (session)
    //    {
    //        session->release();
    //    }
    //}

    extern "C" SLANGNATIVE_API void* CreateSession(CompilerOptionCLI* options, int optionsLength,
        PreprocessorMacroDescCLI* macros, int macrosLength,
        ShaderModelCLI* models, int modelsLength,
        char* searchPaths[], int searchPathsLength)
    {
        // Create a new Session object on the heap
        SessionCLI* newSession = new SessionCLI(options, optionsLength, macros, macrosLength, models, modelsLength, searchPaths, searchPathsLength);
        return newSession;
    }
}