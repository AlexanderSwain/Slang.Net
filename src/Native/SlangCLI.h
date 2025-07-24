#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API SlangCLI
	{
	public:
		// Constructor module from source
		SlangCLI();

		// Destructor
		~SlangCLI();

		slang::IModule* getNative();slang::ICompileRequest

		void slangc();

	private:
		slang::IModule* m_native;
	};
}

