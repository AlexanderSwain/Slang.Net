#include "ProgramCLI.h"
//#include <cstdlib>  // For malloc and strcpy_s

#include "TypeParameterReflection.h"
#include "TypeReflection.h"
#include "TypeLayoutReflection.h"
#include "FunctionReflection.h"
#include "VariableLayoutReflection.h"
#include "VariableReflection.h"
#include "EntryPointReflection.h"
#include "GenericReflection.h"
#include "GenericArgReflection.h"
#include "ShaderReflection.h"

Native::ProgramCLI::ProgramCLI(ModuleCLI* parent)
{
    m_parent = parent;
    m_composedProgram = m_parent->getProgramComponent();
}

SlangResult Native::ProgramCLI::GetCompiled(unsigned int targetIndex, const void** output, int* outputSize)
{
    Slang::ComPtr<slang::IBlob> targetCode;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = m_composedProgram->getTargetCode(
            targetIndex, 
            targetCode.writeRef(), 
            diagnosticsBlob.writeRef());

        if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
            std::string errorMessage = "There are issues in the shader source: " + diagnosticsText;

            if (SLANG_FAILED(result))
                throw std::runtime_error(errorMessage);
            else
                std::cout << diagnosticsText << std::endl;
        }
        if (!targetCode)
            throw std::runtime_error("Error when compiling a program. No slang diagnostics were returned. Probably target with the specified index does not exist.");

		// TODO: verify that there is no memory leak here, because it looks like there is
        *output = targetCode->getBufferPointer();
		*outputSize = targetCode->getBufferSize();

        return result;
    }
}

SlangResult Native::ProgramCLI::GetCompiled(unsigned int entryPointIndex, unsigned int targetIndex, const void** output, int* outputSize)
{
    if (!m_composedProgram)
    {
        std::string errorMessage = "m_composedProgram is null";
        throw std::runtime_error(errorMessage);
    }

    Slang::ComPtr<slang::IBlob> targetCode;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = m_composedProgram->getEntryPointCode(
            entryPointIndex,
            targetIndex,
            targetCode.writeRef(),
            diagnosticsBlob.writeRef());
        // Improved diagnostics output
        if (diagnosticsBlob && diagnosticsBlob->getBufferSize() > 0)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());
            std::string errorMessage = "There are issues in the shader source: " + diagnosticsText;

            if (SLANG_FAILED(result))
                throw std::runtime_error(errorMessage);
            else
                std::cout << diagnosticsText << std::endl;
        }
        if (!targetCode)
            throw std::runtime_error("Error when compiling a program. No slang diagnostics were returned. Probably target with the specified index does not exist.");

        // TODO: verify that there is no memory leak here, because it looks like there is
        *output = targetCode->getBufferPointer();
        *outputSize = targetCode->getBufferSize();

        return result;
    }
}

void* Native::ProgramCLI::GetLayout(int targetIndex)
{
    slang::ProgramLayout* result;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        result = m_composedProgram->getLayout(targetIndex, diagnosticsBlob.writeRef());

        if (diagnosticsBlob != nullptr)
        {
            std::string diagnosticsText = std::string((const char*)diagnosticsBlob->getBufferPointer());

            if (!m_composedProgram)
            {
                throw std::runtime_error(diagnosticsText);
            }
            else
            {
                std::cout << diagnosticsText << std::endl;
            }
        }
        else if (!result)
        {
			// Why is there not a diagnostic message for this? Update if this ever get fixed in Slang.
            std::string errorMessage = std::string("[targetIndex = ") + std::to_string(targetIndex) + std::string("]: The target for the specified target index doesn't exist.");
            throw std::runtime_error(errorMessage);
		}
    }

    return result;
}

slang::IComponentType* Native::ProgramCLI::getNative()
{
    return m_composedProgram;
}

Native::ModuleCLI* Native::ProgramCLI::getParent()
{
    return m_parent;
}

Native::ProgramCLI::~ProgramCLI()
{
	// No need to release m_parent here, as it is not owned by this class
    //if (m_parent)
    //{
    //    m_parent->Release();
    //    m_parent = nullptr;
	//}

	// No need to delete m_composedProgram here, as it is managed by Slang
    //if (m_composedProgram)
    //{
    //    m_composedProgram->Release();
    //    m_composedProgram = nullptr;
    //}
}

// Helpers: Now obsolete thanks to GetProgramWithEntryPoints()
slang::IComponentType** Native::ProgramCLI::getProgramComponents()
{
    unsigned int entryPointCount = m_parent->getEntryPointCount();
    unsigned int componentCount = entryPointCount + 1;

    slang::IComponentType** programComponents = new slang::IComponentType*[componentCount];

	// Fill the program components with entry points
    slang::IEntryPoint** entryPoints = new slang::IEntryPoint*[entryPointCount];

    for (unsigned int i = 0; i < entryPointCount; i++)
    {
        entryPoints[i] = m_parent->getEntryPointByIndex(i)->getNative();
	}

    for (unsigned int i = 0; i < entryPointCount; i++)
    {
        if (i < componentCount) // Ensure we do not write out of bounds
            programComponents[i] = entryPoints[i];
    }

	// Add the native module as the last component
    programComponents[componentCount - 1] = m_parent->getNative();

	return programComponents;
}