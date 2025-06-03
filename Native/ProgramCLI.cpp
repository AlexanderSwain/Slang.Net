#include "ProgramCLI.h"

Native::ProgramCLI::ProgramCLI(ModuleCLI* parent)
{
    m_module = parent;

	slang::IComponentType** programComponents = getProgramComponents();

    slang::IComponentType* composedProgram;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = m_module->getParent()->createCompositeComponentType(
            programComponents,
            m_module->getEntryPointCount() + 1,
            &composedProgram,
            diagnosticsBlob.writeRef());

        if (diagnosticsBlob != nullptr)
        {
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }

        if (SLANG_FAILED(result))
        {
            std::cerr << "Failed to create composite component type." << std::endl;
            return;
        }
    }

    slang::ProgramLayout* pl = composedProgram->getLayout();
	ISlangBlob* layoutBlob = nullptr;
    pl->toJson(&layoutBlob);

    if (layoutBlob != nullptr)
    {
        std::cout << (const char*)layoutBlob->getBufferPointer() << std::endl;
    }

    if (composedProgram == nullptr)
    {
        std::cerr << "Failed to create composed program." << std::endl;
        return;
    }

    m_program = composedProgram;

    slang::IComponentType* linkedProgram;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = composedProgram->link(
            &linkedProgram,
            diagnosticsBlob.writeRef());

        if (diagnosticsBlob != nullptr)
        {
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }
    }

    m_linkedProgram = linkedProgram;
}

SlangResult Native::ProgramCLI::GetCompiled(unsigned int entryPointIndex, unsigned int targetIndex, const char** output)
{
    Slang::ComPtr<slang::IBlob> bytecode;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        bytecode.writeRef();
        diagnosticsBlob.writeRef();
        SlangResult result = m_linkedProgram->getEntryPointCode(
            entryPointIndex,
            targetIndex,
            bytecode.writeRef(),
            diagnosticsBlob.writeRef());        
        if (result < 0)
        {
            m_errorBuffer = "Failed to get entry point code.";
            if (diagnosticsBlob != nullptr)
            {
                m_errorBuffer += " Diagnostics: ";
                m_errorBuffer += static_cast<const char*>(diagnosticsBlob->getBufferPointer());
            }
            *output = m_errorBuffer.c_str();
			return result;
        }
        else
        {
            *output = (const char*)bytecode->getBufferPointer();
            return result;
        }
    }
}

slang::IComponentType* Native::ProgramCLI::getNative()
{
    return m_program;
}

slang::IComponentType* Native::ProgramCLI::getLinked()
{
    return m_linkedProgram;
}

Native::ModuleCLI* Native::ProgramCLI::getModule()
{
    return m_module;
}

Native::ProgramCLI::~ProgramCLI()
{
    if (m_program)
    {
        m_program->Release();
        m_program = nullptr;
    }
}

// Helpers
slang::IComponentType** Native::ProgramCLI::getProgramComponents()
{
    unsigned int entryPointCount = m_module->getEntryPointCount();
    unsigned int componentCount = entryPointCount + 1;

    slang::IComponentType** programComponents = new slang::IComponentType * [componentCount];

	// Fill the program components with entry points
    slang::IEntryPoint** entryPoints = m_module->getEntryPoints();
    for (int i = 0; i < entryPointCount; i++)
    {
        programComponents[i] = entryPoints[i];
    }

	// Add the native module as the last component
    programComponents[m_module->getEntryPointCount()] = m_module->getNative();

	return programComponents;
}