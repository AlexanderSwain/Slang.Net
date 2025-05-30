#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "ModuleCLI.h"
#include "ParameterInfoCLI.h"
#include <map>
#include <string>

namespace Native
{
	class EntryPointCLI
	{
	public:
		// Constructor with parameters (example)
		EntryPointCLI(slang::IModule* parent, const char* entryPointName);

		// Destructor
		~EntryPointCLI();

		bool getParameterInfo(const char* name, ParameterInfoCLI& outInfo);

		// Properties
		slang::IModule* getParent() const;
		const char* getName();
		int getIndex() const;
		SlangStage getStage() const;
		slang::IEntryPoint* getNative();

	private:
		slang::IEntryPoint* m_entryPoint = nullptr;
		slang::IModule* m_parent = nullptr;
		std::string m_name;
		int m_index = -1;
		SlangStage m_stage = SLANG_STAGE_NONE;
		std::map<std::string, ParameterInfoCLI> m_parameterInfoMap;
	};
}

