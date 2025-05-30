//#include "Module.h"
//#include "EntryPoint.h"
//#include <iostream>
//
//Slang::Module::Module(slang::ISession* parent, const char* moduleName, const char* modulePath, const char* shaderSource)
//{
//    m_parent = parent;
//    Slang::ComPtr<slang::IModule> slangModule;
//    {
//        Slang::ComPtr<slang::IBlob> sourceBlob;
//        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
//
//        // Use moduleName instead of modulePath for loadModule
//        slangModule = m_parent->loadModule(moduleName, diagnosticsBlob.writeRef());
//        //slangModule = m_session->loadModuleFromSource(moduleName, modulePath, sourceBlob, diagnosticsBlob.writeRef());
//        //slangModule = m_session->loadModuleFromSourceString(moduleName, modulePath, shaderSource, diagnosticsBlob.writeRef());
//
//        // Improved diagnostics output
//        if (diagnosticsBlob != nullptr && diagnosticsBlob->getBufferSize() > 0)
//        {
//            std::cout << "Slang diagnostics: " << std::endl;
//            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
//        }
//        else {
//            std::cout << "No diagnostics output from Slang." << std::endl;
//        }
//
//        if (!slangModule)
//        {
//            std::cout << "Slang failed to create module from source string." << std::endl;
//        }
//    }
//    
//    m_slangModule = slangModule;
//}
//
//Slang::Module::~Module()
//{
//    if (m_slangModule)
//    {
//        m_slangModule->Release();
//        m_slangModule = nullptr;
//	}
//}
//
//slang::IModule* Slang::Module::getNative()
//{
//    return m_slangModule;
//}
//
////extern "C" {
////    __declspec(dllexport) void* CreateModule(void* session, char* moduleName, char* modulePath, char* shaderSource)
////    {
////        if (session)
////        {
////            Session* sessionPtr = static_cast<Session*>(session);
////            Module* module = new Module(sessionPtr->getNative(), moduleName, modulePath, shaderSource);
////            return static_cast<void*>(module);
////        }
////        return nullptr;
////    }
////
////    __declspec(dllexport) void DestroyModule(void* modulePtr)
////    {
////        if (modulePtr)
////        {
////            Module* module = static_cast<Module*>(modulePtr);
////            delete module;
////        }
////    }
////}