#include "slang.h"

// Example: wrap a function you want to call from C++/CLI
extern "C" __declspec(dllexport)
slang::IGlobalSession* CreateGlobalSession()
{
    slang::IGlobalSession* session = nullptr;
    slang_createGlobalSession(0, &session);
    return session;
}