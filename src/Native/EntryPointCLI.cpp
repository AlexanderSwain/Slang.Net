#include "EntryPointCLI.h"
#include "ModuleCLI.h"
#include <map>
#include <string>
#include <iostream>

Native::EntryPointCLI::EntryPointCLI (ModuleCLI* parent, unsigned index)
{
    if (!parent) {
        throw std::invalid_argument("Parent module is null!");
    }
    if (index < 0 || index >= parent->getEntryPointCount()) {
        throw std::invalid_argument("Index is out of range!");
    }

    m_name = "";
    m_index = index;

    m_parent = parent;
	// set m_native to the native entry point at the given index
    if (SLANG_FAILED(parent->getNative()->getDefinedEntryPoint(m_index, &m_native)))
    {
        throw std::runtime_error("Failed to get native entry point.");
    }
}
Native::EntryPointCLI::EntryPointCLI(ModuleCLI* parent, const char* entryPointName)
{
    if (!parent) {
        throw std::invalid_argument("Parent module is null!");
    }
    if (!entryPointName) {
        throw std::invalid_argument("Name cannot be null!");
	}

    m_name = entryPointName;
    m_index = -1;

    m_parent = parent;
    if (SLANG_FAILED(parent->getNative()->findEntryPointByName(m_name.c_str(), &m_native)))
    {
		throw std::runtime_error("Failed to find entry point named: " + std::string(entryPointName));
    }
    else
    {
        // Finds and sets the index
        unsigned epCount = m_parent->getEntryPointCount();
        for (int i = 0; i < epCount; i++)
        {
            if (m_native == m_parent->getEntryPointByIndex(i)->getNative())
            {
                m_index = i;
                break;
            }
        }
    }
}
Native::EntryPointCLI::~EntryPointCLI()
{
    // Nothing to release for now
}


// Properties
Native::ModuleCLI* Native::EntryPointCLI::getParent()
{
    return m_parent;
}
slang::IEntryPoint* Native::EntryPointCLI::getNative()
{
    return m_native;
}
const char* Native::EntryPointCLI::getName()
{
    if (m_name.empty())
        return nullptr;
    return m_name.c_str();
}

int Native::EntryPointCLI::getIndex()
{
	// Idea to use reflection to get the index if m_index is -1
    return m_index;
}

SlangResult Native::EntryPointCLI::Compile(int targetIndex, const char** outCode)
{
    Slang::ComPtr<slang::IBlob> sourceBlob;
    Slang::ComPtr<slang::IBlob> diagnosticsBlob;

    SlangResult result = m_native->getEntryPointCode(m_index, targetIndex, sourceBlob.writeRef(), diagnosticsBlob.writeRef());

    // Improved diagnostics output
    if (sourceBlob && sourceBlob->getBufferSize() > 0)
        *outCode = (const char*)sourceBlob->getBufferPointer();

    // Improved diagnostics output
    if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
    {
        std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
        std::string errorMessage = "There are issues in the shader source: " + diagnosticsText;

        if (SLANG_FAILED(result))
            throw std::runtime_error(errorMessage);
    }

    return result;
}