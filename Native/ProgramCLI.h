#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ModuleCLI.h"
#include "EntryPointCLI.h"
#include <array>
#include <iostream>
#include <string>

// Reflection Includes
#include "TypeParameterReflection.h"
#include "TypeReflection.h"
#include "TypeLayoutReflection.h"
#include "FunctionReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "EntryPointReflection.h"
#include "GenericReflection.h"
#include "LayoutRules.h"
#include "TypeDef.h"
#include "GenericArgType.h"
#include "GenericArgReflection.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	class SLANGNATIVE_API ProgramCLI
	{
	public:
		// Constructor with parameters (example)
		ProgramCLI(ModuleCLI* parent);

		// Destructor
		~ProgramCLI();

		//Properties
		slang::IComponentType* getNative();
		slang::IComponentType* getLinked();
		ModuleCLI* getModule();

		SlangResult GetCompiled(unsigned int entryPointIndex, unsigned int targetIndex, const char** output);

		// Reflection API
        unsigned int getParameterCount();
        unsigned int getTypeParameterCount();
        TypeParameterReflection* getTypeParameterByIndex(unsigned int index);
        TypeParameterReflection* findTypeParameter(char const* name);
        VariableLayoutReflection* getParameterByIndex(unsigned int index);
        //static ProgramLayout* get(SlangCompileRequest* request);
        SlangUInt getEntryPointCount();
        EntryPointReflection* getEntryPointByIndex(SlangUInt index);
        SlangUInt getGlobalConstantBufferBinding();
        size_t getGlobalConstantBufferSize();   
        TypeReflection* findTypeByName(const char* name);
        FunctionReflection* findFunctionByName(const char* name);
        FunctionReflection* findFunctionByNameInType(TypeReflection* type, const char* name);
        VariableReflection* findVarByNameInType(TypeReflection* type, const char* name);
        TypeLayoutReflection* getTypeLayout(TypeReflection* type, LayoutRules rules = LayoutRules::Default);
        EntryPointReflection* findEntryPointByName(const char* name);
        TypeReflection* specializeType(TypeReflection* type, SlangInt specializationArgCount, TypeReflection* const* specializationArgs, ISlangBlob** outDiagnostics);
        GenericReflection* specializeGeneric(GenericReflection* generic, SlangInt specializationArgCount, GenericArgType const* specializationArgTypes, GenericArgReflection const* specializationArgVals, ISlangBlob** outDiagnostics);
        bool isSubType(TypeReflection* subType, TypeReflection* superType);
        SlangUInt getHashedStringCount() const;
        const char* getHashedString(SlangUInt index, size_t* outCount) const;
        TypeLayoutReflection* getGlobalParamsTypeLayout();
        VariableLayoutReflection* getGlobalParamsVarLayout();
        SlangResult toJson(ISlangBlob** outBlob);

	private:
		ModuleCLI* m_module = nullptr;
		slang::IComponentType* m_program = nullptr;
		slang::IComponentType* m_linkedProgram = nullptr;
        slang::ProgramLayout* m_layout = nullptr;
		std::string m_errorBuffer; // Buffer to store error messages
		slang::IComponentType** getProgramComponents();
	};
}