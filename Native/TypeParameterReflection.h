#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "TypeReflection.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API TypeParameterReflection
	{

	public:
		TypeParameterReflection(void* native);

        char const* getName();
        unsigned getIndex();
        unsigned getConstraintCount();
		TypeReflection* getConstraintByIndex(int index);

	private:
		slang::TypeParameterReflection* m_native;
	};
}

