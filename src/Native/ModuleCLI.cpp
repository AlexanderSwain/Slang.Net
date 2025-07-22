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
        //slangModule = m_parent->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());

        // Improved diagnostics output
        if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
			std::string errorMessage = "There are issues in the shader source: " + diagnosticsText;

            if (!slangModule)
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
    // Does nothing, kept for consistency and in case a future update requires something to be disposed (such as children like EntryPoints).
}

unsigned int Native::ModuleCLI::getEntryPointCount()
{
    return m_slangModule->getDefinedEntryPointCount();
}

Native::EntryPointCLI* Native::ModuleCLI::getEntryPointByIndex(unsigned index)
{
    if (!m_entryPoints)
    {
        slang::IEntryPoint** entryPoints = new slang::IEntryPoint*[m_entryPointCount];

        for (unsigned int i = 0; i < m_entryPointCount; i++)
        {
            m_entryPoints[i] = new Native::EntryPointCLI(this, i);
        }
    }

    return m_entryPoints[index];
}

Native::EntryPointCLI* Native::ModuleCLI::findEntryPointByName(const char* name)
{
    // Memory leak here, but this method is unused anyways.
    // Decided to keep it for consistency with slang api.
	return new Native::EntryPointCLI(this, name);
}

slang::ISession* Native::ModuleCLI::getParent()
{
    return m_parent;
}

slang::IModule* Native::ModuleCLI::getNative()
{
    return m_slangModule;
}