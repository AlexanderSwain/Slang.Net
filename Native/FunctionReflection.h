#pragma once
#include "TypeReflection.h"
#include "VariableReflection.h"
#include "Attribute.h"
#include "GenericReflection.h"
#include "Modifier.h"
#include <map>

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API FunctionReflection
	{

	public:
		FunctionReflection(void* native);

        char const* getName();
        TypeReflection* getReturnType();
        unsigned int getParameterCount();
        VariableReflection* getParameterByIndex(unsigned int index);
        unsigned int getUserAttributeCount();
        Attribute* getUserAttributeByIndex(unsigned int index);
        Attribute* findAttributeByName(SlangSession* globalSession, char const* name);
        Attribute* findUserAttributeByName(SlangSession* globalSession, char const* name);
        Modifier* findModifier(Modifier::ID id);
        GenericReflection* getGenericContainer();
        FunctionReflection* applySpecializations(GenericReflection* generic);
        FunctionReflection* specializeWithArgTypes(unsigned int argCount, TypeReflection* const* types);
        bool isOverloaded();
        unsigned int getOverloadCount();
        FunctionReflection* getOverload(unsigned int index);

	private:
		slang::FunctionReflection* m_native;
	};
}

