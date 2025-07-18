#include "SessionCLI.h"
#include <vector>
#include <iostream>
#include <sys/stat.h>

slang::IGlobalSession* Native::SessionCLI::s_context = nullptr;

// Constructor with parameters implementation
Native::SessionCLI::SessionCLI(CompilerOptionCLI* options, int optionsLength,
    PreprocessorMacroDescCLI* macros, int macrosLength, 
    ShaderModelCLI* models, int modelsLength,
    char** searchPaths, int searchPathsLength)
{
    slang::SessionDesc sessionDesc = {};

    // Set targets
    std::vector<slang::TargetDesc> targetDescs;
    targetDescs.reserve(modelsLength);

    for (int i = 0; i < modelsLength; i++)
    {
        ShaderModelCLI model = models[i];

        slang::TargetDesc targetDesc = {};
        targetDesc.format = (SlangCompileTarget)model.getTarget();
        targetDesc.profile = GetGlobalSession()->findProfile(model.getProfile());
        
        // Add to our collection
        targetDescs.push_back(targetDesc);
    }

    // Use the vector data as the targets array if we have any targets
    if (!targetDescs.empty())
    {
        sessionDesc.targets = targetDescs.data();
        sessionDesc.targetCount = static_cast<int>(targetDescs.size());
    }

	// Set compiler options
    std::vector<slang::CompilerOptionEntry> compilerOptions;
    compilerOptions.reserve(optionsLength);
    
    for (int i = 0; i < optionsLength; i++)
    {
        CompilerOptionCLI option = options[i];
		CompilerOptionValueCLI value = option.getValue();
    
        slang::CompilerOptionEntry entry = 
        {
                (slang::CompilerOptionName)option.getName(),
                {(slang::CompilerOptionValueKind)value.kind, value.intValue0, value.intValue1, value.stringValue0, value.stringValue1 }
        };
    
        // Add to our collection
        compilerOptions.push_back(entry);
    }
    
    // Use the vector data as the targets array if we have any targets
    if (!targetDescs.empty())
    {
        sessionDesc.compilerOptionEntries = compilerOptions.data();
        sessionDesc.compilerOptionEntryCount = compilerOptions.size();
    }
    
    // Set PreprocessorMacroDesc
        // Set compiler options
    std::vector<slang::PreprocessorMacroDesc> preprocessorMacroDesc;
    preprocessorMacroDesc.reserve(macrosLength);
    
    for (int i = 0; i < macrosLength; i++)
    {
        slang::PreprocessorMacroDesc macroDesc = { macros[i].getName(), macros[i].getValue() };
    
        // Add to our collection
        preprocessorMacroDesc.push_back(macroDesc);
    }
    
    // Use the vector data as the targets array if we have any targets
    if (!preprocessorMacroDesc.empty())
    {
        sessionDesc.preprocessorMacros = preprocessorMacroDesc.data();
        sessionDesc.preprocessorMacroCount = preprocessorMacroDesc.size();
    }

    sessionDesc.searchPaths = searchPaths;
    sessionDesc.searchPathCount = searchPathsLength; // Update the count to reflect the number of paths

    Slang::ComPtr<slang::ISession> session;
    SlangResult createResult = s_context->createSession(sessionDesc, session.writeRef());
    m_session = session;
    
    if (SLANG_FAILED(createResult))
        throw std::runtime_error("Failed to create session: ErrorCode = " + std::to_string(createResult));
}

// Destructor implementation
Native::SessionCLI::~SessionCLI()
{
    // Clean up resources if needed
}

slang::IGlobalSession* Native::SessionCLI::GetGlobalSession()
{
    if (!s_context)
    {
        // Call the C API entry point directly
        slang_createGlobalSession(0, &s_context);
    }

    return s_context;
}

slang::ISession* Native::SessionCLI::getNative()
{
    return m_session;
}