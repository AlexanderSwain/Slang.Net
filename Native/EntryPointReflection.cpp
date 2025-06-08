#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "EntryPointReflection.h"
#include <stdexcept>

Native::EntryPointReflection::EntryPointReflection(void* native)
{
	m_native = (slang::EntryPointReflection*)native;
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
    return new Native::FunctionReflection(m_native->getFunction());
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getParameterByIndex(unsigned index)
{
    return new Native::VariableLayoutReflection(m_native->getParameterByIndex(index));
}

SlangStage Native::EntryPointReflection::getStage()
{
    return m_native->getStage();
}

void Native::EntryPointReflection::getComputeThreadGroupSize(SlangUInt axisCount, SlangUInt* outSizeAlongAxis)
{
    return m_native->getComputeThreadGroupSize(axisCount, outSizeAlongAxis);
}

void Native::EntryPointReflection::getComputeWaveSize(SlangUInt* outWaveSize)
{
    return m_native->getComputeWaveSize(outWaveSize);
}

bool Native::EntryPointReflection::usesAnySampleRateInput()
{
    return m_native->usesAnySampleRateInput();
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getVarLayout()
{
    return new Native::VariableLayoutReflection(m_native->getVarLayout());
}

Native::TypeLayoutReflection* Native::EntryPointReflection::getTypeLayout() 
{ 
    return new Native::TypeLayoutReflection(m_native->getTypeLayout());
}

Native::VariableLayoutReflection* Native::EntryPointReflection::getResultVarLayout()
{
    return new Native::VariableLayoutReflection(m_native->getResultVarLayout());
}

bool Native::EntryPointReflection::hasDefaultConstantBuffer()
{
    return m_native->hasDefaultConstantBuffer();
}