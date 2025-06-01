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
        //slangModule = m_parent->loadModuleFromSource(moduleName, modulePath, sourceBlob, diagnosticsBlob.writeRef());
        //slangModule = m_session->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());

        // Improved diagnostics output
        if (diagnosticsBlob != nullptr && diagnosticsBlob->getBufferSize() > 0)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
			std::string errorMessage = "There are errors present in the shader source: " + diagnosticsText;
            throw std::runtime_error(errorMessage);
        }
        else if (!slangModule)
        {
            throw std::runtime_error("Unknown failure: Failed to create the Slang module.");
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