#include "EntryPoint.h"
#include <map>
#include <string>
#include <iostream>

Slang::EntryPoint::EntryPoint(slang::IModule* parent, const char* entryPointName)
{
    m_parent = parent;
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

    slang::ProgramLayout* typeLayout = m_entryPoint->getLayout();
    if (!typeLayout) {
        throw std::runtime_error("m_entryPoint->getLayout() returned null!");
    }

    // Get the index of the entry point in the module
    int entryPointCount = typeLayout->getEntryPointCount();
    for (int i = 0; i < entryPointCount; i++)
    {
        slang::EntryPointLayout* epLayout = typeLayout->getEntryPointByIndex(i);
        if (!epLayout) {
            throw std::runtime_error("typeLayout->getEntryPointByIndex(" + std::to_string(i) + ") returned null!");
        }
        if (strcmp(epLayout->getName(), entryPointName) == 0) 
        {
            m_index = i;
            m_stage = epLayout->getStage();
            break;
        }
    }

    // Get binding information for each parameter
    for (unsigned int i = 0; i < typeLayout->getParameterCount(); i++)
    {
        slang::VariableLayoutReflection* param = typeLayout->getParameterByIndex(i);
        if (!param) {
            throw std::runtime_error("typeLayout->getParameterByIndex(" + std::to_string(i) + ") returned null!");
        }
        const char* name = param->getName();
        slang::ParameterCategory category = param->getCategory();
        unsigned int bindingIndex = param->getBindingIndex();
        unsigned int bindingSpace = param->getBindingSpace();

        m_parameterInfoMap[name] = { category, bindingIndex, bindingSpace };
    }
}

bool Slang::EntryPoint::getParameterInfo(const char* name, ParameterInfo& outInfo)
{
    auto it = m_parameterInfoMap.find(name);
    if (it != m_parameterInfoMap.end())
    {
        outInfo = it->second;
        return true;
    }
    else
    {
        return false;
    }
}

slang::IModule* Slang::EntryPoint::getParent() const
{
    return m_parent;    
}

const char* Slang::EntryPoint::getName()
{
    return m_name.c_str();
}

int Slang::EntryPoint::getIndex() const
{
    return m_index;
}

SlangStage Slang::EntryPoint::getStage() const
{
    return m_stage;
}

slang::IEntryPoint* Slang::EntryPoint::getNative()
{
    return m_entryPoint;
}

Slang::EntryPoint::~EntryPoint()
{
    if (m_entryPoint)
    {
        m_entryPoint->Release();
        m_entryPoint = nullptr;
    }
}

//extern "C" {
//    __declspec(dllexport) void* CreateEntryPoint(void* module, char* name)
//    {
//        if (module)
//        {
//            Module* modulePtr = static_cast<Module*>(module);
//            EntryPoint* entryPoint = new EntryPoint(modulePtr->getNative(), name);
//            return static_cast<void*>(entryPoint);
//        }
//        return nullptr;
//    }
//
//    __declspec(dllexport) bool TryGetParameter(void* entryPoint, char* name, ParameterInfo& outInfo)
//    {
//        if (entryPoint)
//        {
//            EntryPoint* entryPointPtr = static_cast<EntryPoint*>(entryPoint);
//            return entryPointPtr->getParameterInfo(name, outInfo);
//        }
//        return false;
//    }
//
//    __declspec(dllexport) void* DestroyEntryPoint(void* entryPoint)
//    {
//        if (entryPoint)
//        {
//            EntryPoint* entryPointPtr = static_cast<EntryPoint*>(entryPoint);
//            delete entryPointPtr;
//            return nullptr;
//        }
//        return nullptr;
//    }
//}