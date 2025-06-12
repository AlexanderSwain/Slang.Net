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

    m_layout = composedProgram->getLayout();

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

void* Native::ProgramCLI::GetReflection()
{
    return m_layout;
}

// Reflection API: Delete these, moved to ShaderReflection
unsigned Native::ProgramCLI::getParameterCount()
{
	return m_layout->getParameterCount();
}

unsigned Native::ProgramCLI::getTypeParameterCount()
{
    return m_layout->getTypeParameterCount();
}

Native::TypeParameterReflection* Native::ProgramCLI::getTypeParameterByIndex(unsigned index)
{
    return new TypeParameterReflection(m_layout->getTypeParameterByIndex(index));
}

Native::TypeParameterReflection* Native::ProgramCLI::findTypeParameter(char const* name)
{
    return new TypeParameterReflection(m_layout->findTypeParameter(name));
}

Native::VariableLayoutReflection* Native::ProgramCLI::getParameterByIndex(unsigned int index)
{
    return new VariableLayoutReflection(m_layout->getParameterByIndex(index));
}

SlangUInt Native::ProgramCLI::getEntryPointCount()
{
    return m_layout->getEntryPointCount();
}

Native::EntryPointReflection* Native::ProgramCLI::getEntryPointByIndex(SlangUInt index)
{
    return new EntryPointReflection(m_layout, m_layout->getEntryPointByIndex(index));
}

SlangUInt Native::ProgramCLI::getGlobalConstantBufferBinding()
{
    return m_layout->getGlobalConstantBufferBinding();
}

size_t Native::ProgramCLI::getGlobalConstantBufferSize()
{
    return m_layout->getGlobalConstantBufferSize();
}

Native::TypeReflection* Native::ProgramCLI::findTypeByName(const char* name)
{
    return new TypeReflection(m_layout->findTypeByName(name));
}

Native::FunctionReflection* Native::ProgramCLI::findFunctionByName(const char* name)
{
    return new FunctionReflection(m_layout->findFunctionByName(name));
}

Native::FunctionReflection* Native::ProgramCLI::findFunctionByNameInType(TypeReflection* type, const char* name)
{
    return new FunctionReflection(m_layout->findFunctionByNameInType((slang::TypeReflection*)type->getNative(), name));
}

Native::VariableReflection* Native::ProgramCLI::findVarByNameInType(TypeReflection* type, const char* name)
{
    return new VariableReflection(m_layout->findVarByNameInType((slang::TypeReflection*)type->getNative(), name));
}

Native::TypeLayoutReflection* Native::ProgramCLI::getTypeLayout(
    TypeReflection* type,
    LayoutRules rules)
{
    return new TypeLayoutReflection(m_layout->getTypeLayout((slang::TypeReflection*)type->getNative(), (slang::LayoutRules)rules));
}

Native::EntryPointReflection* Native::ProgramCLI::findEntryPointByName(const char* name)
{
    return new EntryPointReflection(m_layout, m_layout->findEntryPointByName(name));
}

Native::TypeReflection* Native::ProgramCLI::specializeType(
    TypeReflection* type,
    SlangInt specializationArgCount,
    TypeReflection* const* specializationArgs,
    ISlangBlob** outDiagnostics)
{
	slang::TypeReflection** args = new slang::TypeReflection*[specializationArgCount]();
	for (SlangInt i = 0; i < specializationArgCount; i++)
	{
        args[i] = (slang::TypeReflection*)specializationArgs[i]->getNative();
	}
    slang::TypeReflection* specializeResult = m_layout->specializeType((slang::TypeReflection*)type->getNative(), specializationArgCount, args, outDiagnostics);
	delete[] args;
    return new TypeReflection(specializeResult);
}

Native::GenericReflection* Native::ProgramCLI::specializeGeneric(  
    GenericReflection* genRef,  
    SlangInt specializationArgCount,  
    GenericArgType const* specializationArgTypes,  
    GenericArgReflection const* specializationArgVals,  
    ISlangBlob** outDiagnostics)  
{  
    // Convert Native::GenericArgType to slang::GenericArgType  
    slang::GenericArgType* slangSpecializationArgTypes = new slang::GenericArgType[specializationArgCount];  
    for (SlangInt i = 0; i < specializationArgCount; i++)  
    {  
        slangSpecializationArgTypes[i] = (slang::GenericArgType)specializationArgTypes[i];
    }  

    // Convert Native::GenericArgReflection to slang::GenericArgReflection  
    slang::GenericArgReflection* slangSpecializationArgVals = new slang::GenericArgReflection[specializationArgCount];  
    for (SlangInt i = 0; i < specializationArgCount; i++)  
        // Modify the loop to explicitly handle the conversion from Native::GenericArgReflection to slang::GenericArgReflection
    {  
        union slang::GenericArgReflection val;

        switch (specializationArgTypes[i])
        {
            case GenericArgType::SLANG_GENERIC_ARG_TYPE:
				val.typeVal = (slang::TypeReflection*)specializationArgVals[i].typeVal->getNative();
                break;
            case GenericArgType::SLANG_GENERIC_ARG_INT:
				val.intVal = specializationArgVals[i].intVal;
                break;
            case GenericArgType::SLANG_GENERIC_ARG_BOOL:
            default:
				val.boolVal = specializationArgVals[i].boolVal;
                break;
		}

        slangSpecializationArgVals[i] = val;
    }  

    // Call the underlying function with converted arguments  
    auto result = m_layout->specializeGeneric(  
        static_cast<slang::GenericReflection*>(genRef->getNative()),  
        specializationArgCount,  
        slangSpecializationArgTypes,  
        slangSpecializationArgVals,  
        outDiagnostics);  

    // Clean up allocated memory  
    delete[] slangSpecializationArgTypes;  
    delete[] slangSpecializationArgVals;  

    return new GenericReflection(result);  
}

bool Native::ProgramCLI::isSubType(TypeReflection* subType, TypeReflection* superType)
{
    return m_layout->isSubType((slang::TypeReflection*)subType->getNative(), (slang::TypeReflection*)superType->getNative());
}

SlangUInt Native::ProgramCLI::getHashedStringCount() const
{
    return m_layout->getHashedStringCount();
}

const char* Native::ProgramCLI::getHashedString(SlangUInt index, size_t* outCount) const
{
    return m_layout->getHashedString(index, outCount);
}

Native::TypeLayoutReflection* Native::ProgramCLI::getGlobalParamsTypeLayout()
{
    return new TypeLayoutReflection(m_layout->getGlobalParamsTypeLayout());
}

Native::VariableLayoutReflection* Native::ProgramCLI::getGlobalParamsVarLayout()
{
    return new VariableLayoutReflection(m_layout->getGlobalParamsVarLayout());
}

SlangResult Native::ProgramCLI::toJson(ISlangBlob** outBlob)
{
    return m_layout->toJson(outBlob);
}

// Helpers
slang::IComponentType** Native::ProgramCLI::getProgramComponents()
{
    unsigned int entryPointCount = m_module->getEntryPointCount();
    unsigned int componentCount = entryPointCount + 1;

    slang::IComponentType** programComponents = new slang::IComponentType * [componentCount];

	// Fill the program components with entry points
    slang::IEntryPoint** entryPoints = m_module->getEntryPoints();
    for (unsigned int i = 0; i < entryPointCount; i++)
    {
        programComponents[i] = entryPoints[i];
    }

	// Add the native module as the last component
    programComponents[entryPointCount] = m_module->getNative();

	return programComponents;
}