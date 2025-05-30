//#include "EntryPoint.h"
//#include <map>
//#include <string>
//#include <iostream>
//#include <msclr/marshal.h>
//#include <msclr/marshal_cppstd.h>
//
//using namespace System;
//using namespace msclr::interop;
//
//Slang::EntryPoint::EntryPoint(slang::IModule* parent, String^ entryPointName)
//{
//    m_parent = parent;
//    m_name = entryPointName;
//
//    if (!m_parent) {
//        throw std::invalid_argument("Parent module is null!");
//    }
//
//    Slang::ComPtr<slang::IEntryPoint> entryPoint;
//    std::string stdString = marshal_as<std::string>(entryPointName);
//    const char* cString2 = stdString.c_str();
//    m_parent->findEntryPointByName(cString2, entryPoint.writeRef());
//
//    if (!entryPoint) {
//		String^ errorMessage = String::Format("Failed to find entry point named: {0}", entryPointName);
//        throw std::invalid_argument(marshal_as<std::string>(errorMessage));
//    }
//    m_entryPoint = entryPoint;
//
//    slang::ProgramLayout* typeLayout = m_entryPoint->getLayout();
//    if (!typeLayout) {
//        throw std::runtime_error("m_entryPoint->getLayout() returned null!");
//    }
//
//    // Get the index of the entry point in the module
//    int entryPointCount = typeLayout->getEntryPointCount();
//    for (int i = 0; i < entryPointCount; i++)
//    {
//        slang::EntryPointLayout* epLayout = typeLayout->getEntryPointByIndex(i);
//        if (!epLayout) {
//            throw std::runtime_error("typeLayout->getEntryPointByIndex(" + std::to_string(i) + ") returned null!");
//        }
//        if (strcmp(epLayout->getName(), marshal_as<std::string>(entryPointName).c_str()) == 0)
//        {
//            m_index = i;
//            m_stage = epLayout->getStage();
//            break;
//        }
//    }
//
//    // Get binding information for each parameter
//    for (unsigned int i = 0; i < typeLayout->getParameterCount(); i++)
//    {
//        slang::VariableLayoutReflection* param = typeLayout->getParameterByIndex(i);
//        if (!param) {
//            throw std::runtime_error("typeLayout->getParameterByIndex(" + std::to_string(i) + ") returned null!");
//        }
//        String^ name = gcnew String(param->getName());
//        slang::ParameterCategory category = param->getCategory();
//        unsigned int bindingIndex = param->getBindingIndex();
//        unsigned int bindingSpace = param->getBindingSpace();
//
//        m_parameterInfoMap[name] = { category, bindingIndex, bindingSpace };
//    }
//}
//
//bool Slang::EntryPoint::getParameterInfo(String^ name, ParameterInfo% outInfo)
//{
//    return m_parameterInfoMap->TryGetValue(name, outInfo);
//}
//
//slang::IModule* Slang::EntryPoint::getParent()
//{
//    return m_parent;    
//}
//
//String^ Slang::EntryPoint::getName()
//{
//    return m_name;
//}
//
//int Slang::EntryPoint::getIndex()
//{
//    return m_index;
//}
//
//SlangStage Slang::EntryPoint::getStage()
//{
//    return m_stage;
//}
//
//slang::IEntryPoint* Slang::EntryPoint::getNative()
//{
//    return m_entryPoint;
//}
//
//Slang::EntryPoint::~EntryPoint()
//{
//    if (m_entryPoint)
//    {
//        m_entryPoint->Release();
//        m_entryPoint = nullptr;
//    }
//}
//
////extern "C" {
////    __declspec(dllexport) void* CreateEntryPoint(void* module, char* name)
////    {
////        if (module)
////        {
////            Module* modulePtr = static_cast<Module*>(module);
////            EntryPoint* entryPoint = new EntryPoint(modulePtr->getNative(), name);
////            return static_cast<void*>(entryPoint);
////        }
////        return nullptr;
////    }
////
////    __declspec(dllexport) bool TryGetParameter(void* entryPoint, char* name, ParameterInfo& outInfo)
////    {
////        if (entryPoint)
////        {
////            EntryPoint* entryPointPtr = static_cast<EntryPoint*>(entryPoint);
////            return entryPointPtr->getParameterInfo(name, outInfo);
////        }
////        return false;
////    }
////
////    __declspec(dllexport) void* DestroyEntryPoint(void* entryPoint)
////    {
////        if (entryPoint)
////        {
////            EntryPoint* entryPointPtr = static_cast<EntryPoint*>(entryPoint);
////            delete entryPointPtr;
////            return nullptr;
////        }
////        return nullptr;
////    }
////}