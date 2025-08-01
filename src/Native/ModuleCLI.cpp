#include "ModuleCLI.h"
#include <iostream>

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
{
	m_parent = parent->getNative();
	unsigned int index = parent->getModuleCount();

	Slang::ComPtr<ISlangBlob> diagnosticsBlob;
	m_parent->createCompileRequest(&m_compileRequest);

	// Add the shader file
	m_compileRequest->addTranslationUnit(SLANG_SOURCE_LANGUAGE_SLANG, moduleName);
	m_compileRequest->addTranslationUnitSourceFile(index, modulePath);

	// Compile it
	if (m_compileRequest->compile() != SLANG_OK)
	{
		auto diagnostics = m_compileRequest->getDiagnosticOutput();
		throw std::runtime_error(std::string("Slang compile error:\n") + diagnostics);
	}

	Slang::ComPtr<slang::IModule> slangModule;
	{
		Slang::ComPtr<slang::IBlob> sourceBlob;
		Slang::ComPtr<slang::IBlob> diagnosticsBlob;

		//slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());
		//slangModule = m_parent->loadModuleFromSource(moduleName, modulePath, sourceBlob, diagnosticsBlob.writeRef());
		//slangModule = m_parent->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());
		m_compileRequest->getModule(index, slangModule.writeRef());

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

slang::IComponentType* Native::ModuleCLI::getProgramComponent()
{
	slang::IComponentType* result;
	{
		SlangResult getResult = m_compileRequest->getProgramWithEntryPoints(&result);

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

	if (!m_entryPoints)
	{
		// Allocate and initialize the entry points array on first access
		m_entryPoints = new Native::EntryPointCLI * [entryPointCount];

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