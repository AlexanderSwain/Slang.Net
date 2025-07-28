#include "ModuleCLI.h"
#include <iostream>

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
{
    m_parent = parent->getNative();
    Slang::ComPtr<slang::IModule> slangModule;
    {
        Slang::ComPtr<slang::IBlob> sourceBlob;
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;

        //slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());
        //slangModule = m_parent->loadModuleFromSource(moduleName, modulePath, sourceBlob, diagnosticsBlob.writeRef());
        slangModule = m_parent->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());

        // Improved diagnostics output
        if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
			std::string errorMessage = "There are issues in the shader source: " + diagnosticsText;

            if (!slangModule)
                throw std::runtime_error(errorMessage);
            else
                std::cout << diagnosticsText << std::endl;
        }
        else if (!slangModule)
        {
            throw std::runtime_error("Unknown failure: Failed to create the Slang module.");
        }
    }

    m_slangModule = slangModule;
    m_entryPoints = nullptr; // Initialize to nullptr, will be allocated on demand
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName)
{
    m_parent = parent->getNative();
    Slang::ComPtr<slang::IModule> slangModule;
    {
        Slang::ComPtr<slang::IBlob> sourceBlob;
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;

        // import module
        slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());

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
    m_entryPoints = nullptr; // Initialize to nullptr, will be allocated on demand
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, slang::IModule* nativeModule)
{
    m_parent = parent->getNative();
    m_slangModule = nativeModule;
    m_entryPoints = nullptr; // Initialize to nullptr, will be allocated on demand
}

Native::ModuleCLI::~ModuleCLI()
{
    // Clean up entry points array if allocated
    if (m_entryPoints)
    {
        unsigned int entryPointCount = getEntryPointCount();
        for (unsigned int i = 0; i < entryPointCount; i++)
        {
            delete m_entryPoints[i];
        }
        delete[] m_entryPoints;
    }
}

const char* Native::ModuleCLI::getName()
{
    return m_slangModule->getName();
}

unsigned int Native::ModuleCLI::getEntryPointCount()
{
    return m_slangModule->getDefinedEntryPointCount();
}

Native::EntryPointCLI* Native::ModuleCLI::getEntryPointByIndex(unsigned index)
{
    unsigned int entryPointCount = getEntryPointCount();
    if (index >= entryPointCount)
    {
        throw std::out_of_range("Entry point index is out of range");
    }

    if (!m_entryPoints)
    {
        // Allocate and initialize the entry points array on first access
        m_entryPoints = new Native::EntryPointCLI*[entryPointCount];
        
        for (unsigned int i = 0; i < entryPointCount; i++)
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