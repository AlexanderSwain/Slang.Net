#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "TypeReflection.h"
#include "TypeLayoutReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "FunctionReflection.h"
#include "EntryPointReflection.h"
#include "GenericReflection.h"
#include "TypeParameterReflection.h"
#include "LayoutRules.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	struct SLANGNATIVE_API ShaderReflection
	{
	public:
		ShaderReflection(void* native);

        unsigned getParameterCount();
        unsigned getTypeParameterCount();
        
        TypeParameterReflection* getTypeParameterByIndex(unsigned index);
        TypeParameterReflection* findTypeParameter(char const* name);
        VariableLayoutReflection* getParameterByIndex(unsigned index);
        
        SlangUInt getEntryPointCount();
        EntryPointReflection* getEntryPointByIndex(SlangUInt index);
        EntryPointReflection* findEntryPointByName(const char* name);
        
        SlangUInt getGlobalConstantBufferBinding();
        size_t getGlobalConstantBufferSize();
        
        TypeReflection* findTypeByName(const char* name);
        FunctionReflection* findFunctionByName(const char* name);
        FunctionReflection* findFunctionByNameInType(TypeReflection* type, const char* name);
        VariableReflection* findVarByNameInType(TypeReflection* type, const char* name);
        
        TypeLayoutReflection* getTypeLayout(TypeReflection* type, LayoutRules rules);
        
        TypeReflection* specializeType(
            TypeReflection* type,
            SlangInt specializationArgCount,
            TypeReflection* const* specializationArgs,
            ISlangBlob** outDiagnostics);
            
        bool isSubType(TypeReflection* subType, TypeReflection* superType);
        
        SlangUInt getHashedStringCount();
        const char* getHashedString(SlangUInt index, size_t* outCount);
        
        TypeLayoutReflection* getGlobalParamsTypeLayout();
        VariableLayoutReflection* getGlobalParamsVarLayout();
        
        SlangResult toJson(ISlangBlob** outBlob);

	private:
		slang::ShaderReflection* m_native;
	};
}
