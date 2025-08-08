#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include <map>
#include <list>
#include <string>
#include <memory>

namespace Native
{
	// Forward declarations
	struct TypeReflection;
	struct TypeLayoutReflection;
	struct VariableLayoutReflection;
	struct VariableReflection;
	struct FunctionReflection;
	struct EntryPointReflection;
	struct GenericReflection;
	struct TypeParameterReflection;
	struct ProgramCLI;
	enum class LayoutRules;
}

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
		ShaderReflection(ProgramCLI* parent, unsigned int targetIndex);
		~ShaderReflection();

		ProgramCLI* getParent();
		slang::ShaderReflection* getNative();

		unsigned getParameterCount();
		unsigned getTypeParameterCount();

		std::unique_ptr<TypeParameterReflection> getTypeParameterByIndex(unsigned index);
		std::unique_ptr<TypeParameterReflection> findTypeParameter(char const* name);
		std::unique_ptr<VariableLayoutReflection> getParameterByIndex(unsigned index);

		SlangUInt getEntryPointCount();
		std::unique_ptr<EntryPointReflection> getEntryPointByIndex(SlangUInt index);
		std::unique_ptr<EntryPointReflection> findEntryPointByName(const char* name);

		SlangUInt getGlobalConstantBufferBinding();
		size_t getGlobalConstantBufferSize();

		std::unique_ptr<TypeReflection> findTypeByName(const char* name);
		std::unique_ptr<FunctionReflection> findFunctionByName(const char* name);
		std::unique_ptr<FunctionReflection> findFunctionByNameInType(TypeReflection* type, const char* name);
		std::unique_ptr<VariableReflection> findVarByNameInType(TypeReflection* type, const char* name);

		std::unique_ptr<TypeLayoutReflection> getTypeLayout(TypeReflection* type, LayoutRules rules);

		// Fix: ISlangBlob**
		std::unique_ptr<TypeReflection> specializeType(
			TypeReflection* type,
			SlangInt specializationArgCount,
			TypeReflection* const* specializationArgs,
			ISlangBlob** outDiagnostics);

		bool isSubType(TypeReflection* subType, TypeReflection* superType);

		SlangUInt getHashedStringCount();
		const char* getHashedString(SlangUInt index, size_t* outCount);

		std::unique_ptr<TypeLayoutReflection> getGlobalParamsTypeLayout();
		std::unique_ptr<VariableLayoutReflection> getGlobalParamsVarLayout();

		SlangResult toJson(const char** outBlob);

	private:
		ProgramCLI* m_parent;
		slang::ShaderReflection* m_native;
	};
}
