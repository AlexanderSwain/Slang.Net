#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "SessionCLI.h"
#include "CompileRequest.h"
#include "EntryPointCLI.h"
#include <string>

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API ModuleCLI
	{
	public:
		// Compile Request constructor
		ModuleCLI(SessionCLI* parent, CompileRequestCLI* compileRequest);

		// Constructor module from source
		ModuleCLI(SessionCLI* parent, const char* moduleName, const char* modulePath, const char* shaderSource);

		// Module import constructor
		ModuleCLI(SessionCLI* parent, const char* moduleName);

		// Module import constructor
		ModuleCLI(SessionCLI* parent, slang::IModule* nativeModule);

		// Destructor
		~ModuleCLI();

		slang::ISession* getParent();
		slang::IModule* getNative();
		const char* getName();
		slang::IComponentType* getProgramComponent();
		unsigned int getEntryPointCount();
		Native::EntryPointCLI* getEntryPointByIndex(unsigned index);
		Native::EntryPointCLI* findEntryPointByName(const char* name);

	private:

		slang::ISession* m_parent;
		slang::IModule* m_slangModule;
		Native::CompileRequestCLI* m_compileRequest;
		Native::EntryPointCLI** m_entryPoints;
	};
}

