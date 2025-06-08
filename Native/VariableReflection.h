#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "Modifier.h"
#include "TypeReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "VariableReflection.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	struct SLANGNATIVE_API VariableReflection
	{

	public:
		VariableReflection(void* native);
        void* getNative();

        char const* getName();
        TypeReflection* getType();
        Modifier* findModifier(Modifier::ID id);
        unsigned int getUserAttributeCount();
        Attribute* getUserAttributeByIndex(unsigned int index);
        Attribute* findAttributeByName(SlangSession* globalSession, char const* name);
        Attribute* findUserAttributeByName(SlangSession* globalSession, char const* name);
        bool hasDefaultValue();
        SlangResult getDefaultValueInt(int64_t* value);
        GenericReflection* getGenericContainer();
        VariableReflection* applySpecializations(GenericReflection* generic);

	private:
		slang::VariableReflection* m_native;
	};
}

