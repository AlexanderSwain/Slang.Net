#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "SessionCLI.h"
#include <string>

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API CompileRequestCLI
	{
	public:
		// Constructor
		CompileRequestCLI(SessionCLI* parent);

		// Destructor
		~CompileRequestCLI();

		// Native getter
		SlangCompileRequest* getNative();
		slang::ISession* getParent();

		// Add methods from SlangCompileRequest
		void addCodeGenTarget(SlangCompileTarget target);
		SlangResult addEntryPoint(int translationUnitIndex, char const* name, SlangStage stage);
		SlangResult addEntryPointEx(int translationUnitIndex, char const* name, SlangStage stage, int genericArgCount, char const** genericArgs);
		void addLibraryReference(SlangCompileRequest* baseRequest, char const* libName);
		void addPreprocessorDefine(char const* key, char const* value);
		void addRef();
		void addSearchPath(char const* searchDir);
		void addTargetCapability(int targetIndex, SlangCapabilityID capability);
		int addTranslationUnit(SlangSourceLanguage language, char const* name);
		void addTranslationUnitPreprocessorDefine(int translationUnitIndex, char const* key, char const* value);
		void addTranslationUnitSourceBlob(int translationUnitIndex, char const* path, ISlangBlob* sourceBlob);
		void addTranslationUnitSourceFile(int translationUnitIndex, char const* path);
		void addTranslationUnitSourceString(int translationUnitIndex, char const* path, char const* source);
		void addTranslationUnitSourceStringSpan(int translationUnitIndex, char const* path, char const* sourceBegin, char const* sourceEnd);

	private:
		Slang::ComPtr<slang::ISession> m_parent;
		Slang::ComPtr<SlangCompileRequest> m_compileRequest;
	};
}
