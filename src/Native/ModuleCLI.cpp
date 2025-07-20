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

    setEntryPoints();
}

Native::ModuleCLI::~ModuleCLI()
{
    // Does nothing, kept for consistency and in case a future update requires something to be disposed (such as children like EntryPoints).
}

void Native::ModuleCLI::setEntryPoints()
{
    unsigned int entryPointCount = m_slangModule->getDefinedEntryPointCount();
	m_entryPointCount = entryPointCount;

	m_entryPoints = new slang::IEntryPoint*[m_entryPointCount];

    for (unsigned int i = 0; i < entryPointCount; i++)
    {
        m_slangModule->getDefinedEntryPoint(i, &m_entryPoints[i]);
	}
}

slang::ISession* Native::ModuleCLI::getParent()
{
    return m_parent;
}

slang::IModule* Native::ModuleCLI::getNative()
{
    return m_slangModule;
}

slang::IEntryPoint** Native::ModuleCLI::getEntryPoints()
{
    return m_entryPoints;
}

unsigned int Native::ModuleCLI::getEntryPointCount()
{
    return m_entryPointCount;
}