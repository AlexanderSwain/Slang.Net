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
}

Native::EntryPointReflection::~EntryPointReflection()
{
    // No cached state to clean up
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
    slang::FunctionReflection* functionPtr = m_native->getFunction();
    return functionPtr ? new Native::FunctionReflection(functionPtr) : nullptr;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getParameterByIndex(unsigned index)
{
    slang::VariableLayoutReflection* nativeParameter = m_native->getParameterByIndex(index);
    return nativeParameter ? new Native::VariableLayoutReflection(nativeParameter) : nullptr;
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
    slang::VariableLayoutReflection* varLayoutPtr = m_native->getVarLayout();
    return varLayoutPtr ? new Native::VariableLayoutReflection(varLayoutPtr) : nullptr;
}

Native::TypeLayoutReflection* Native::EntryPointReflection::getTypeLayout() 
{ 
    slang::TypeLayoutReflection* typeLayoutPtr = m_native->getTypeLayout();
    return typeLayoutPtr ? new Native::TypeLayoutReflection(typeLayoutPtr) : nullptr;
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getResultVarLayout()
{
    slang::VariableLayoutReflection* resultVarLayoutPtr = m_native->getResultVarLayout();
    return resultVarLayoutPtr ? new Native::VariableLayoutReflection(resultVarLayoutPtr) : nullptr;
}

bool Native::EntryPointReflection::hasDefaultConstantBuffer()
{
    return m_native->hasDefaultConstantBuffer();
}