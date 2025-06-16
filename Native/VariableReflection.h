#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "SessionCLI.h"
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
        
        slang::VariableReflection* getNative();

        char const* getName();
        TypeReflection* getType();
        Modifier* findModifier(Modifier::ID id);
        unsigned int getUserAttributeCount();
        Attribute* getUserAttributeByIndex(unsigned int index);
        Attribute* findAttributeByName(char const* name);
        Attribute* findUserAttributeByName(char const* name);
        bool hasDefaultValue();
        SlangResult getDefaultValueInt(int64_t* value);
        GenericReflection* getGenericContainer();
        VariableReflection* applySpecializations(GenericReflection* genRef);

	private:
		slang::VariableReflection* m_native;
	};
}

