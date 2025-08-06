#include "ModuleCLI.h"
#include <iostream>
#include <memory>
#include <vector>

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, CompileRequestCLI* compileRequest)
{
	m_parent = parent->getNative();
	unsigned int index = parent->getModuleCount();

	m_compileRequest = std::unique_ptr<CompileRequestCLI>(compileRequest);

	// Compile it
	if (m_compileRequest->getNative()->compile() != SLANG_OK)
	{
		auto diagnostics = m_compileRequest->getNative()->getDiagnosticOutput();
		throw std::runtime_error(std::string("Slang compile error:\n") + diagnostics);
	}

	auto c = m_compileRequest->getNative()->getTranslationUnitCount();

	Slang::ComPtr<slang::IModule> slangModule;
	{
		Slang::ComPtr<slang::IBlob> sourceBlob;
		std::string diagnosticsBlob = std::string(m_compileRequest->getNative()->getDiagnosticOutput());

		m_compileRequest->getNative()->getModule(index, slangModule.writeRef());

		// Improved diagnostics output
		if (!diagnosticsBlob.empty())
		{
			std::string errorMessage = "There are issues in the shader source: " + diagnosticsBlob;

			if (!slangModule)
				throw std::runtime_error(errorMessage);
			else
				std::cout << diagnosticsBlob << std::endl;
		}
		else if (!slangModule)
		{
			throw std::runtime_error("Unknown failure: Failed to create the Slang module.");
		}
	}

	m_slangModule = slangModule;
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
{
	m_parent = parent->getNative();
	unsigned int index = parent->getModuleCount();

	Slang::ComPtr<ISlangBlob> diagnosticsBlob;
	m_compileRequest = std::make_unique<CompileRequestCLI>(parent);

	// Add the shader file
	m_compileRequest->addTranslationUnit(SLANG_SOURCE_LANGUAGE_SLANG, moduleName);
	m_compileRequest->addTranslationUnitSourceFile(index, modulePath);

	// Compile it
	if (m_compileRequest->getNative()->compile() != SLANG_OK)
	{
		auto diagnostics = m_compileRequest->getNative()->getDiagnosticOutput();
		throw std::runtime_error(std::string("Slang compile error:\n") + diagnostics);
	}

	Slang::ComPtr<slang::IModule> slangModule;
	{
		Slang::ComPtr<slang::IBlob> sourceBlob;
		Slang::ComPtr<slang::IBlob> diagnosticsBlob;

		m_compileRequest->getNative()->getModule(index, slangModule.writeRef());

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
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, slang::IModule* nativeModule)
{
	m_parent = parent->getNative();
	m_slangModule = nativeModule;
}

Native::ModuleCLI::~ModuleCLI()
{
	// Smart pointers will automatically clean up the objects
	// ComPtr will automatically release Slang interfaces
	// unique_ptr will automatically delete owned objects
	// vector will automatically destroy all elements
}

const char* Native::ModuleCLI::getName()
{
	return m_slangModule->getName();
}

slang::IComponentType* Native::ModuleCLI::getProgramComponent()
{
	slang::IComponentType* result;
	{
		SlangResult getResult = m_compileRequest->getNative()->getProgramWithEntryPoints(&result);

		if (SLANG_FAILED(getResult))
		{
			std::string errorMessage = "[Failure Retrieving ProgramComponent] ErrorCode:" + std::to_string(getResult);
			throw std::runtime_error(errorMessage);
		}
	}

	return result;
}

unsigned int Native::ModuleCLI::getEntryPointCount()
{
	return m_slangModule->getDefinedEntryPointCount();
	// Delete
 //   SlangResult result = SLANG_OK;
 //   slang::IEntryPoint* _ = nullptr;
 //   Slang::ComPtr<slang::IBlob> diagnosticsBlob;
 //   slang::ISession* session = m_slangModule->getSession();

	//int i = -1;
 //   while (SLANG_SUCCEEDED(result))
 //   {
 //       m_slangModule->findAndCheckEntryPoint("CS", SlangStage::SLANG_STAGE_COMPUTE, &_, diagnosticsBlob.writeRef());
 //       result = m_slangModule->getDefinedEntryPoint(i + 1, &_);
 //       i++;
 //   }
		
 //   return i;
}

Native::EntryPointCLI* Native::ModuleCLI::getEntryPointByIndex(unsigned index)
{
	unsigned int entryPointCount = getEntryPointCount();
	if (index >= entryPointCount)
	{
		throw std::out_of_range("Entry point index is out of range");
	}

	// Resize vector if needed
	if (m_entryPoints.size() < entryPointCount)
	{
		m_entryPoints.resize(entryPointCount);
		for (unsigned int i = 0; i < entryPointCount; i++)
		{
			if (!m_entryPoints[i])
			{
				m_entryPoints[i] = std::make_unique<Native::EntryPointCLI>(this, i);
			}
		}
	}

	return m_entryPoints[index].get();
}

Native::EntryPointCLI* Native::ModuleCLI::findEntryPointByName(const char* name)
{
	// Create a temporary entry point for the search
	// Note: This approach maintains API compatibility but should be redesigned
	// to avoid temporary object creation in a production environment
	m_tempEntryPointForSearch = std::make_unique<Native::EntryPointCLI>(this, name);
	return m_tempEntryPointForSearch.get();
}

slang::ISession* Native::ModuleCLI::getParent()
{
	return m_parent;
}

slang::IModule* Native::ModuleCLI::getNative()
{
	return m_slangModule;
}