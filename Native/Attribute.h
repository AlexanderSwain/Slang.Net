#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "TypeReflection.h"
#include "TypeDef.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API Attribute
	{

	public:
		Attribute(void* native);

		char const* getName();
		uint32_t getArgumentCount();
		TypeReflection* getArgumentType(uint32_t index);
		SlangResult getArgumentValueInt(uint32_t index, int* value);
		SlangResult getArgumentValueFloat(uint32_t index, float* value);
		const char* getArgumentValueString(uint32_t index, size_t* outSize);

	private:
		slang::Attribute* m_native;

		TypeReflection* m_argumentType;
	};
}

