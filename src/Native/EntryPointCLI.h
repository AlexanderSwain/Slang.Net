#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ParameterInfoCLI.h"
#include <map>
#include <string>

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

// Forward declaration to avoid circular dependency
namespace Native
{
    class ModuleCLI;
}

namespace Native
{
	class SLANGNATIVE_API EntryPointCLI
	{
	public:
		// Constructor with parameters (example)
		EntryPointCLI(ModuleCLI* parent, unsigned index);
		EntryPointCLI(ModuleCLI* parent, const char* entryPointName);

		// Destructor
		~EntryPointCLI();

		// Properties
		ModuleCLI* getParent();
		slang::IEntryPoint* getNative();
		int getIndex();
		const char* getName();

		SlangResult Compile(int targetIndex, const char** outCode);

	private:
		ModuleCLI* m_parent = nullptr;
		slang::IEntryPoint* m_native = nullptr;
		int m_index = -1;
		std::string m_name = "";
	};
}

