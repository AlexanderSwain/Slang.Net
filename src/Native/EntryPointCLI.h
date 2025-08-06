#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <string>
#include <stdexcept>

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
		// Constructor with index
		EntryPointCLI(ModuleCLI* parent, unsigned index);
		
		// Constructor with name
		EntryPointCLI(ModuleCLI* parent, const char* entryPointName);

		// Copy constructor for caching
		EntryPointCLI(const EntryPointCLI& other);

		// Destructor
		~EntryPointCLI();

		// Properties
		ModuleCLI* getParent();
		slang::IEntryPoint* getNative();
		int getIndex();
		const char* getName();

		// Compilation
		SlangResult Compile(int targetIndex, const char** outCode);

	private:
		ModuleCLI* m_parent;
		Slang::ComPtr<slang::IEntryPoint> m_native;
		int m_index;
		std::string m_name;
	};
}

