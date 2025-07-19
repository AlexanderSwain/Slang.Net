#include "EntryPointReflection.h"
#include "ShaderReflection.h"
#include "FunctionReflection.h"
#include "VariableLayoutReflection.h"
#include "TypeLayoutReflection.h"


Native::EntryPointReflection::EntryPointReflection(Native::ShaderReflection* parent, void* native)
{
    if (!native) throw std::invalid_argument("Native pointer cannot be null");
    
    m_parent = parent;
	m_native = (slang::EntryPointReflection*)native;

    // Use lazy initialization - only initialize when accessed
    m_function = nullptr;
    m_parameters = nullptr;
	m_varLayout = nullptr;
    m_typeLayout = nullptr;
	m_resultVarLayout = nullptr;
}

Native::EntryPointReflection::~EntryPointReflection()
{
    // Clean up the function reflection
    delete m_function;
    m_function = nullptr;

    // Clean up the parameters array
    if (m_parameters)
    {
        for (uint32_t index = 0; index < m_native->getParameterCount(); index++)
        {
            delete m_parameters[index];
        }
        delete[] m_parameters;
        m_parameters = nullptr;
    }

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
    if (!m_function)
    {
        slang::FunctionReflection* functionPtr = m_native->getFunction();
        if (functionPtr) 
            m_function = new Native::FunctionReflection(functionPtr);
        else
            m_function = nullptr;
    }
    return m_function;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getParameterByIndex(unsigned index)
{
    if (!m_parameters)
    {
        uint32_t parameterCount = m_native->getParameterCount();
        m_parameters = new Native::VariableLayoutReflection*[parameterCount];
        for (uint32_t i = 0; i < parameterCount; i++)
        {
            slang::VariableLayoutReflection* nativeParameter = m_native->getParameterByIndex(i);
            if (nativeParameter)
                m_parameters[i] = new Native::VariableLayoutReflection(nativeParameter);
            else
                m_parameters[i] = nullptr;
        }
    }
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
    if (!m_varLayout)
    {
        slang::VariableLayoutReflection* varLayoutPtr = m_native->getVarLayout();
        if (varLayoutPtr) 
            m_varLayout = new Native::VariableLayoutReflection(varLayoutPtr);
        else
            m_varLayout = nullptr;
    }
    return m_varLayout;
}

Native::TypeLayoutReflection* Native::EntryPointReflection::getTypeLayout() 
{ 
    if (!m_typeLayout)
    {
        slang::TypeLayoutReflection* typeLayoutPtr = m_native->getTypeLayout();
        if (typeLayoutPtr) 
            m_typeLayout = new Native::TypeLayoutReflection(typeLayoutPtr);
        else
            m_typeLayout = nullptr;
    }
    return m_typeLayout;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getResultVarLayout()
{
    if (!m_resultVarLayout)
    {
        slang::VariableLayoutReflection* resultVarLayoutPtr = m_native->getResultVarLayout();
        if (resultVarLayoutPtr) 
            m_resultVarLayout = new Native::VariableLayoutReflection(resultVarLayoutPtr);
        else
            m_resultVarLayout = nullptr;
    }
    return m_resultVarLayout;
}

bool Native::EntryPointReflection::hasDefaultConstantBuffer()
{
    return m_native->hasDefaultConstantBuffer();
}