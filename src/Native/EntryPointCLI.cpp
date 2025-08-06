#include "EntryPointCLI.h"
#include "ModuleCLI.h"
#include <iostream>
#include <stdexcept>

Native::EntryPointCLI::EntryPointCLI(ModuleCLI* parent, unsigned index)
	: m_parent(parent), m_index(static_cast<int>(index))
{
	if (!parent)
		throw std::invalid_argument("Parent module cannot be null");
	
	if (index >= parent->getEntryPointCount())
		throw std::out_of_range("Entry point index " + std::to_string(index) + " is out of range. Count: " + std::to_string(parent->getEntryPointCount()));

	// Get the native entry point at the given index
	SlangResult result = parent->getNative()->getDefinedEntryPoint(m_index, m_native.writeRef());
	if (SLANG_FAILED(result) || !m_native)
	{
		throw std::runtime_error("Failed to get native entry point at index " + std::to_string(index) + ". Error code: " + std::to_string(result));
	}
}

Native::EntryPointCLI::EntryPointCLI(ModuleCLI* parent, const char* entryPointName)
	: m_parent(parent), m_index(-1)
{
	if (!parent)
		throw std::invalid_argument("Parent module cannot be null");
	if (!entryPointName)
		throw std::invalid_argument("Entry point name cannot be null");

	m_name = entryPointName;

	// Find the entry point by name
	SlangResult result = parent->getNative()->findEntryPointByName(m_name.c_str(), m_native.writeRef());
	if (SLANG_FAILED(result) || !m_native)
	{
		throw std::runtime_error("Failed to find entry point named '" + std::string(entryPointName) + "'. Error code: " + std::to_string(result));
	}

	// Find the index by comparing native pointers
	unsigned int epCount = parent->getEntryPointCount();
	for (unsigned int i = 0; i < epCount; i++)
	{
		Slang::ComPtr<slang::IEntryPoint> testEntryPoint;
		if (SLANG_SUCCEEDED(parent->getNative()->getDefinedEntryPoint(i, testEntryPoint.writeRef())) && 
			testEntryPoint.get() == m_native.get())
		{
			m_index = static_cast<int>(i);
			break;
		}
	}
}

Native::EntryPointCLI::EntryPointCLI(const EntryPointCLI& other)
	: m_parent(other.m_parent)
	, m_native(other.m_native)
	, m_index(other.m_index)
	, m_name(other.m_name)
{
	// Copy constructor for caching support
}

Native::EntryPointCLI::~EntryPointCLI()
{
	// ComPtr will automatically release the entry point
}

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
	return m_index;
}

SlangResult Native::EntryPointCLI::Compile(int targetIndex, const char** outCode)
{
	if (!outCode)
		throw std::invalid_argument("Output code pointer cannot be null");

	Slang::ComPtr<slang::IBlob> sourceBlob;
	Slang::ComPtr<slang::IBlob> diagnosticsBlob;

	SlangResult result = m_native->getEntryPointCode(m_index, targetIndex, sourceBlob.writeRef(), diagnosticsBlob.writeRef());

	// Set output code if successful
	if (sourceBlob && sourceBlob->getBufferSize() > 0)
	{
		*outCode = (const char*)sourceBlob->getBufferPointer();
	}
	else
	{
		*outCode = nullptr;
	}

	// Handle diagnostics
	if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
	{
		std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
		
		if (SLANG_FAILED(result))
		{
			throw std::runtime_error("Entry point compilation failed: " + diagnosticsText);
		}
		else
		{
			std::cout << "Entry point compilation diagnostics: " << diagnosticsText << std::endl;
		}
	}
	else if (SLANG_FAILED(result))
	{
		throw std::runtime_error("Entry point compilation failed. Error code: " + std::to_string(result));
	}

	return result;
}