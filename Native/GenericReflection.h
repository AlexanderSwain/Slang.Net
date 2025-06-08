#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <map>
#include "VariableReflection.h"
#include "TypeReflection.h"
#include "DeclKind.h"
#include "GenericReflection.h"


#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{

	struct SLANGNATIVE_API GenericReflection
	{

	public:
		GenericReflection(void* native);
        void* getNative();

        //DeclReflection* asDecl();
        char const* getName();
        unsigned int getTypeParameterCount();
        VariableReflection* getTypeParameter(unsigned index);
        unsigned int getValueParameterCount();
        VariableReflection* getValueParameter(unsigned index);
        unsigned int getTypeParameterConstraintCount(VariableReflection* typeParam);
        TypeReflection* getTypeParameterConstraintType(VariableReflection* typeParam, unsigned index);
        //DeclReflection* getInnerDecl();
        DeclKind getInnerKind();
        GenericReflection* getOuterGenericContainer();
        TypeReflection* getConcreteType(VariableReflection* typeParam);
        int64_t getConcreteIntVal(VariableReflection* valueParam);
        GenericReflection* applySpecializations(GenericReflection* generic);


	private:
		slang::GenericReflection* m_native;
	};
}

