#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "EntryPoint.h"
#include <array>
#include <iostream>

#ifdef SESSION_EXPORTS
#define SESSION_API __declspec(dllexport)
#else
#define SESSION_API __declspec(dllimport)
#endif

namespace Slang
{
    public ref class Program
    {
    public:
        // Constructor with parameters (example)
        Program(EntryPoint* entryPoint);

        // Destructor
        ~Program();

        //Properties
        slang::IComponentType* getNative();
        slang::IComponentType* getLinked();
        EntryPoint* getEntryPoint();

        SlangResult GetCompiled(const char** output);

    private:
        EntryPoint* m_entryPoint = nullptr;
        slang::IComponentType* m_program = nullptr;
        slang::IComponentType* m_linkedProgram = nullptr;
    };
}