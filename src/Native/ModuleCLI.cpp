#include "ModuleCLI.h"
#include "SessionCLI.h"
#include "CompileRequest.h"
#include "EntryPointCLI.h"
#include "ProgramCLI.h"
#include <iostream>
#include <memory>
#include <vector>

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, CompileRequestCLI* compileRequest)
{
	if (!parent)
		throw std::invalid_argument("Parent session cannot be null");
	if (!compileRequest)
		throw std::invalid_argument("Compile request cannot be null");

	m_parent = parent->getNative();
	unsigned int moduleIndex = parent->getModuleCount();
	
	// Take ownership of the compile request
	auto ownedCompileRequest = std::unique_ptr<CompileRequestCLI>(compileRequest);
	initializeFromCompileRequest(parent, std::move(ownedCompileRequest), moduleIndex);
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
{
	if (!parent)
		throw std::invalid_argument("Parent session cannot be null");
	if (!moduleName)
		throw std::invalid_argument("Module name cannot be null");
	if (!modulePath)
		throw std::invalid_argument("Module path cannot be null");
	if (!shaderSource)
		throw std::invalid_argument("Shader source cannot be null");

	m_parent = parent->getNative();
	unsigned int moduleIndex = parent->getModuleCount();

	// Create compile request
	auto compileRequest = std::make_unique<CompileRequestCLI>(parent);
	
	// Add the shader file
	compileRequest->addTranslationUnit(SLANG_SOURCE_LANGUAGE_SLANG, moduleName);
	compileRequest->addTranslationUnitSourceFile(moduleIndex, modulePath);

	initializeFromCompileRequest(parent, std::move(compileRequest), moduleIndex);
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, const char* moduleName)
{
	if (!parent)
		throw std::invalid_argument("Parent session cannot be null");
	if (!moduleName)
		throw std::invalid_argument("Module name cannot be null");

	m_parent = parent->getNative();
	
	Slang::ComPtr<slang::IBlob> diagnosticsBlob;
	m_slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());

	// Handle diagnostics
	if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
	{
		std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
		std::string errorMessage = "Issues loading module '" + std::string(moduleName) + "': " + diagnosticsText;

		if (!m_slangModule)
			throw std::runtime_error(errorMessage);
		else
			std::cout << diagnosticsText << std::endl;
	}
	else if (!m_slangModule)
	{
		throw std::runtime_error("Failed to load module '" + std::string(moduleName) + "'. No diagnostics available.");
	}
}

Native::ModuleCLI::ModuleCLI(SessionCLI* parent, slang::IModule* nativeModule)
{
	if (!parent)
		throw std::invalid_argument("Parent session cannot be null");
	if (!nativeModule)
		throw std::invalid_argument("Native module cannot be null");

	m_parent = parent->getNative();
	m_slangModule = nativeModule;
}

Native::ModuleCLI::ModuleCLI(const ModuleCLI& other)
{
	m_parent = other.m_parent;
	m_slangModule = other.m_slangModule;
	
	// Don't copy the compile request - it's not needed for copied modules
	// Don't copy cached objects - they'll be recreated on demand
}

Native::ModuleCLI::~ModuleCLI()
{
	// Clear all cached objects first
	m_entryPoints.clear();
	m_tempEntryPointForSearch.reset();
	m_program.reset();
	m_compileRequest.reset();
	
	// ComPtr will automatically release Slang interfaces
}

void Native::ModuleCLI::initializeFromCompileRequest(SessionCLI* parent, std::unique_ptr<CompileRequestCLI> compileRequest, unsigned int moduleIndex)
{
	m_compileRequest = std::move(compileRequest);

	// Compile the module
	if (m_compileRequest->getNative()->compile() != SLANG_OK)
	{
		auto diagnostics = m_compileRequest->getNative()->getDiagnosticOutput();
		throw std::runtime_error(std::string("Slang compile error:\n") + (diagnostics ? diagnostics : "Unknown error"));
	}

	// Get the compiled module
	Slang::ComPtr<slang::IModule> slangModule;
	SlangResult result = m_compileRequest->getNative()->getModule(moduleIndex, slangModule.writeRef());
	
	if (SLANG_FAILED(result) || !slangModule)
	{
		auto diagnostics = m_compileRequest->getNative()->getDiagnosticOutput();
		throw std::runtime_error("Failed to retrieve compiled module: " + std::string(diagnostics ? diagnostics : "Unknown error"));
	}

	// Handle any diagnostics
	auto diagnostics = m_compileRequest->getNative()->getDiagnosticOutput();
	if (diagnostics && strlen(diagnostics) > 0)
	{
		std::cout << "Module compilation diagnostics:\n" << diagnostics << std::endl;
	}

	m_slangModule = slangModule;
}

const char* Native::ModuleCLI::getName()
{
	if (!m_slangModule)
		return nullptr;
	return m_slangModule->getName();
}

slang::IComponentType* Native::ModuleCLI::getProgramComponent()
{
	if (!m_compileRequest)
		throw std::runtime_error("Cannot get program component: module was not created from a compile request");

	slang::IComponentType* result;
	SlangResult getResult = m_compileRequest->getNative()->getProgramWithEntryPoints(&result);

	if (SLANG_FAILED(getResult))
	{
		throw std::runtime_error("Failed to retrieve program component. Error code: " + std::to_string(getResult));
	}

	return result;
}

unsigned int Native::ModuleCLI::getEntryPointCount()
{
	if (!m_slangModule)
		return 0;
	return m_slangModule->getDefinedEntryPointCount();
}

Native::EntryPointCLI* Native::ModuleCLI::getEntryPointByIndex(unsigned index)
{
	unsigned int entryPointCount = getEntryPointCount();
	if (index >= entryPointCount)
	{
		throw std::out_of_range("Entry point index " + std::to_string(index) + " is out of range. Count: " + std::to_string(entryPointCount));
	}

	// Check if already cached
	auto it = m_entryPoints.find(index);
	if (it != m_entryPoints.end())
	{
		return it->second.get();
	}

	// Create and cache new entry point
	auto entryPoint = std::make_unique<EntryPointCLI>(this, index);
	auto* result = entryPoint.get();
	m_entryPoints[index] = std::move(entryPoint);
	
	return result;
}

Native::EntryPointCLI* Native::ModuleCLI::findEntryPointByName(const char* name)
{
	if (!name)
		throw std::invalid_argument("Entry point name cannot be null");

	// Create a temporary entry point for the search
	// Note: This maintains API compatibility but creates a temporary object
	m_tempEntryPointForSearch = std::make_unique<EntryPointCLI>(this, name);
	return m_tempEntryPointForSearch.get();
}

Slang::ComPtr<slang::ISession> Native::ModuleCLI::getParent()
{
	return m_parent;
}

slang::IModule* Native::ModuleCLI::getNative()
{
	return m_slangModule;
}

std::unique_ptr<Native::ProgramCLI> Native::ModuleCLI::getProgram()
{
	if (!m_program)
	{
		m_program = std::make_unique<ProgramCLI>(this);
	}
	return std::make_unique<ProgramCLI>(*m_program); // Return a copy
}