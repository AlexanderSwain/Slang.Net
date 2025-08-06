#include "CompileRequest.h"
#include <iostream>

Native::CompileRequestCLI::CompileRequestCLI(SessionCLI* parent)
{
	m_parent = parent->getNative();
	m_parent->createCompileRequest(m_compileRequest.writeRef());
}

Native::CompileRequestCLI::~CompileRequestCLI()
{
	// ComPtr will automatically release the compile request
}

SlangCompileRequest* Native::CompileRequestCLI::getNative()
{
	return m_compileRequest;
}

slang::ISession* Native::CompileRequestCLI::getParent()
{
	return m_parent;
}

void Native::CompileRequestCLI::addCodeGenTarget(SlangCompileTarget target)
{
	m_compileRequest->addCodeGenTarget(target);
}

SlangResult Native::CompileRequestCLI::addEntryPoint(int translationUnitIndex, char const* name, SlangStage stage)
{
	return m_compileRequest->addEntryPoint(translationUnitIndex, name, stage);
}

SlangResult Native::CompileRequestCLI::addEntryPointEx(int translationUnitIndex, char const* name, SlangStage stage, int genericArgCount, char const** genericArgs)
{
	return m_compileRequest->addEntryPointEx(translationUnitIndex, name, stage, genericArgCount, genericArgs);
}

void Native::CompileRequestCLI::addLibraryReference(SlangCompileRequest* baseRequest, char const* libName)
{
	throw std::runtime_error("addLibraryReference not implemented");
}

void Native::CompileRequestCLI::addPreprocessorDefine(char const* key, char const* value)
{
	m_compileRequest->addPreprocessorDefine(key, value);
}

void Native::CompileRequestCLI::addRef()
{
	m_compileRequest->addRef();
}

void Native::CompileRequestCLI::addSearchPath(char const* searchDir)
{
	m_compileRequest->addSearchPath(searchDir);
}

void Native::CompileRequestCLI::addTargetCapability(int targetIndex, SlangCapabilityID capability)
{
	m_compileRequest->addTargetCapability(targetIndex, capability);
}

int Native::CompileRequestCLI::addTranslationUnit(SlangSourceLanguage language, char const* name)
{
	return m_compileRequest->addTranslationUnit(language, name);
}

void Native::CompileRequestCLI::addTranslationUnitPreprocessorDefine(int translationUnitIndex, char const* key, char const* value)
{
	m_compileRequest->addTranslationUnitPreprocessorDefine(translationUnitIndex, key, value);
}

void Native::CompileRequestCLI::addTranslationUnitSourceBlob(int translationUnitIndex, char const* path, ISlangBlob* sourceBlob)
{
	m_compileRequest->addTranslationUnitSourceBlob(translationUnitIndex, path, sourceBlob);
}

void Native::CompileRequestCLI::addTranslationUnitSourceFile(int translationUnitIndex, char const* path)
{
	m_compileRequest->addTranslationUnitSourceFile(translationUnitIndex, path);
}

void Native::CompileRequestCLI::addTranslationUnitSourceString(int translationUnitIndex, char const* path, char const* source)
{
	m_compileRequest->addTranslationUnitSourceString(translationUnitIndex, path, source);
}

void Native::CompileRequestCLI::addTranslationUnitSourceStringSpan(int translationUnitIndex, char const* path, char const* sourceBegin, char const* sourceEnd)
{
	m_compileRequest->addTranslationUnitSourceStringSpan(translationUnitIndex, path, sourceBegin, sourceEnd);
}