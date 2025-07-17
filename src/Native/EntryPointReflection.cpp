#include "EntryPointReflection.h"
#include "ShaderReflection.h"
#include "FunctionReflection.h"
#include "VariableLayoutReflection.h"
#include "TypeLayoutReflection.h"


Native::EntryPointReflection::EntryPointReflection(Native::ShaderReflection* parent, void* native)
{
    m_parent = parent;
	m_native = (slang::EntryPointReflection*)native;

    m_function = new Native::FunctionReflection(m_native->getFunction());

    // Initialize the argument types array
    uint32_t parameterCount = m_native->getParameterCount();
    m_parameters = new Native::VariableLayoutReflection*[parameterCount];
    for (uint32_t index = 0; index < parameterCount; index++)
    {
        m_parameters[index] = new Native::VariableLayoutReflection(m_native->getParameterByIndex(index));
    }

	m_varLayout = new Native::VariableLayoutReflection(m_native->getVarLayout());
    m_typeLayout = new Native::TypeLayoutReflection(m_native->getTypeLayout());
	m_resultVarLayout = new Native::VariableLayoutReflection(m_native->getResultVarLayout());
}

Native::EntryPointReflection::~EntryPointReflection()
{
    // Clean up the function reflection
    delete m_function;
    m_function = nullptr;

    // Clean up the parameters array
    for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
    {
        delete m_parameters[index];
    }
    delete[] m_parameters;
    m_parameters = nullptr;

    // Clean up variable layout
    delete m_varLayout;
    m_varLayout = nullptr;

	// Clean up type layout
    delete m_typeLayout;
    m_typeLayout = nullptr;

	// Clean up result variable layout
    delete m_resultVarLayout;
    m_resultVarLayout = nullptr;

	// No need to delete m_native here, as it is managed by Slang
    //if (m_native)
    //{
    //    delete m_native;
    //    m_native = nullptr;
    //}
}

Native::ShaderReflection*  Native::EntryPointReflection::getParent()
{
    return m_parent;
}

slang::EntryPointReflection* Native::EntryPointReflection::getNative()
{
    return m_native;
}

char const* Native::EntryPointReflection::getName()
{
    return m_native->getName();
}

char const* Native::EntryPointReflection::getNameOverride()
{
    return m_native->getNameOverride();
}

unsigned Native::EntryPointReflection::getParameterCount()
{
    return m_native->getParameterCount();
}

Native::FunctionReflection* Native::EntryPointReflection::getFunction()
{
    return m_function;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getParameterByIndex(unsigned index)
{
    return m_parameters[index];
}

SlangStage Native::EntryPointReflection::getStage()
{
    return m_native->getStage();
}

void Native::EntryPointReflection::getComputeThreadGroupSize(SlangUInt axisCount, SlangUInt* outSizeAlongAxis)
{
    m_native->getComputeThreadGroupSize(axisCount, outSizeAlongAxis);
}

void Native::EntryPointReflection::getComputeWaveSize(SlangUInt* outWaveSize)
{
    m_native->getComputeWaveSize(outWaveSize);
}

bool Native::EntryPointReflection::usesAnySampleRateInput()
{
    return m_native->usesAnySampleRateInput();
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getVarLayout()
{
    return m_varLayout;
}

Native::TypeLayoutReflection* Native::EntryPointReflection::getTypeLayout() 
{ 
    return m_typeLayout;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getResultVarLayout()
{
    return m_resultVarLayout;
}

bool Native::EntryPointReflection::hasDefaultConstantBuffer()
{
    return m_native->hasDefaultConstantBuffer();
}