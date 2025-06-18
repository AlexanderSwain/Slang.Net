#include "EntryPointCLI.h"
#include <map>
#include <string>
#include <iostream>

Native::EntryPointCLI::EntryPointCLI(ModuleCLI* parent, const char* entryPointName)
{
    m_parent = parent->getNative();
    m_name = entryPointName;

    if (!m_parent) {
        throw std::invalid_argument("Parent module is null!");
    }

    Slang::ComPtr<slang::IEntryPoint> entryPoint;
    m_parent->findEntryPointByName(m_name.c_str(), entryPoint.writeRef());

    if (!entryPoint) {
        throw std::invalid_argument("Failed to find entry point named: " + std::string(m_name));
    }
    m_entryPoint = entryPoint;

    slang::ProgramLayout* programLayout = m_entryPoint->getLayout();
    if (!programLayout) {
        throw std::runtime_error("m_entryPoint->getLayout() returned null!");
    }

    // Get the index of the entry point in the module
    int entryPointCount = programLayout->getEntryPointCount();
    std::cout << "typeLayout->getEntryPointCount(): " << programLayout->getEntryPointCount() << std::endl;
    for (int i = 0; i < entryPointCount; i++)
    {
        slang::EntryPointLayout* epLayout = programLayout->getEntryPointByIndex(i);
        if (!epLayout) {
            throw std::runtime_error("typeLayout->getEntryPointByIndex(" + std::to_string(i) + ") returned null!");
        }
        std::cout << "epLayout[" << i << "]->getParameterCount: " << epLayout->getParameterCount() << std::endl;

        for (int j = 0; j < epLayout->getParameterCount(); j++)
        {
            slang::VariableLayoutReflection* param = epLayout->getParameterByIndex(j);
            if (!param) {
                throw std::runtime_error("epLayout->getParameterByIndex(" + std::to_string(j) + ") returned null!");
            }
            std::cout << "epLayout[" << i << "]->getParameterByIndex(" << j << ")->getName(): " << param->getName() << std::endl;
		}

        if (strcmp(epLayout->getName(), entryPointName) == 0) 
        {
            m_index = i;
            m_stage = epLayout->getStage();
            break;
        }
    }

	// Initialize the parameter info array
	m_parameterCount = programLayout->getParameterCount();
    std::cout << "typeLayout->getParameterCount: " << programLayout->getParameterCount() << std::endl;
	m_parameterInfoArray = new ParameterInfoCLI[m_parameterCount];

    // Get binding information for each parameter
    for (unsigned int i = 0; i < m_parameterCount; i++)
    {
        slang::VariableLayoutReflection* param = programLayout->getParameterByIndex(i);
        if (!param) {
            throw std::runtime_error("typeLayout->getParameterByIndex(" + std::to_string(i) + ") returned null!");
        }
        const char* name = param->getName();
        slang::ParameterCategory category = param->getCategory();
        unsigned int bindingIndex = param->getBindingIndex();
        unsigned int bindingSpace = param->getBindingSpace();

        m_parameterInfoArray[i] = ParameterInfoCLI(name, category, bindingIndex, bindingSpace);
    }
}

Native::ParameterInfoCLI* Native::EntryPointCLI::getParameterInfoArray()
{
    return m_parameterInfoArray;
}

unsigned int Native::EntryPointCLI::getParameterCount()
{
    return m_parameterCount;
}

slang::IModule* Native::EntryPointCLI::getParent() const
{
    return m_parent;    
}

const char* Native::EntryPointCLI::getName()
{
    return m_name.c_str();
}

int Native::EntryPointCLI::getIndex() const
{
    return m_index;
}

SlangStage Native::EntryPointCLI::getStage() const
{
    return m_stage;
}

slang::IEntryPoint* Native::EntryPointCLI::getNative()
{
    return m_entryPoint;
}

Native::EntryPointCLI::~EntryPointCLI()
{
    if (m_entryPoint)
    {
        m_entryPoint->Release();
        m_entryPoint = nullptr;
    }
}