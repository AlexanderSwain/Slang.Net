#include "ModuleCLI.h"
#include <iostream>

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
{
    m_parent = parent->getNative();
    Slang::ComPtr<slang::IModule> slangModule;
    {
        Slang::ComPtr<slang::IBlob> sourceBlob;
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;

        // Use moduleName instead of modulePath for loadModule
        slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());
        //slangModule = m_session->loadModuleFromSource(moduleName, modulePath, sourceBlob, diagnosticsBlob.writeRef());
        //slangModule = m_session->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());

        // Improved diagnostics output
        if (diagnosticsBlob != nullptr && diagnosticsBlob->getBufferSize() > 0)
        {
            std::cout << "Slang diagnostics: " << std::endl;
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }
        else {
            std::cout << "No diagnostics output from Slang." << std::endl;
        }

        if (!slangModule)
        {
            std::cout << "Slang failed to create module from source string." << std::endl;
        }
    }
    
    m_slangModule = slangModule;
}

Native::ModuleCLI::~ModuleCLI()
{
    if (m_slangModule)
    {
        m_slangModule->Release();
        m_slangModule = nullptr;
	}
}

slang::IModule* Native::ModuleCLI::getNative()
{
    return m_slangModule;
}