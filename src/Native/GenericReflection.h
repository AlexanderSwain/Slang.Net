#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <map>
#include <list>
#include "DeclKind.h"

// Forward declarations
namespace Native
{
    struct VariableReflection;
    struct TypeReflection;
}


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
		~GenericReflection();
        
        slang::GenericReflection* getNative();

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
        GenericReflection* applySpecializations(GenericReflection* genRef);


	private:
		slang::GenericReflection* m_native;
	};
}

