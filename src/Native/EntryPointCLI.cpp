#include "EntryPointCLI.h"
#include <map>
#include <string>
#include <iostream>

Native::EntryPointCLI::EntryPointCLI(ModuleCLI* parent, unsigned index)
{
    if (!parent) {
        throw std::invalid_argument("Parent module is null!");
    }
    if (index < 0 || index >= parent->getEntryPointCount()) {
        throw std::invalid_argument("Index is out of range!");
    }

    m_name = nullptr;
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
    if (SLANG_FAILED(parent->getNative()->findEntryPointByName(m_name, &m_native)))
    {
		throw std::runtime_error("Failed to find entry point named: " + std::string(entryPointName));
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
	// Idea to use reflection to get the name if m_name is null
    return m_name;
}

int Native::EntryPointCLI::getIndex()
{
	// Idea to use reflection to get the index if m_index is -1
    return m_index;
}