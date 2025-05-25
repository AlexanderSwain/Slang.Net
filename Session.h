#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "CompilerOption.h"
#include "ShaderModel.h"
#include "Module.h"

#ifdef SESSION_EXPORTS
#define SESSION_API __declspec(dllexport)
#else
#define SESSION_API __declspec(dllimport)
#endif

namespace Slang
{
    public ref class Session
    {
    public:
        // Constructor with parameters (example)
        Session(CompilerOption* options, int optionsLength,
            slang::PreprocessorMacroDesc* macros, int macrosLength,
            ShaderModel* models, int modelsLength,
            char** searchPaths, int searchPathsLength);

        // Destructor
        ~Session();

        slang::ISession* getNative();
        static slang::IGlobalSession* GetGlobalSession();


    private:
        Slang::ComPtr<slang::ISession> m_session;
        static slang::IGlobalSession* s_context;
    };
}
