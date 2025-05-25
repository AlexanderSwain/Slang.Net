#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "Session.h"
#include "EntryPoint.h"

#ifdef SESSION_EXPORTS
#define SESSION_API __declspec(dllexport)
#else
#define SESSION_API __declspec(dllimport)
#endif

namespace Slang
{
    public ref class Module
    {
    public:
        // Constructor with parameters (example)
        Module(slang::ISession* parent, const char* moduleName, const char* modulePath, const char* shaderSource);

        // Destructor
        ~Module();

        slang::IModule* getNative();

    private:
        slang::ISession* m_parent;
        slang::IModule* m_slangModule;
    };
}