//#pragma once
//#include "slang.h"
//#include "slang-com-ptr.h"
//#include "slang-com-helper.h"
//#include "Module.h"
//#include "ParameterInfo.h"
//#include <string>
//
//using namespace System;
//using namespace System::Collections::Generic;
//
//namespace Slang
//{
//    public ref class EntryPoint
//    {
//    public:
//        // Constructor with parameters (example)
//        EntryPoint(slang::IModule* parent, String^ entryPointName);
//
//        // Destructor
//        ~EntryPoint();
//
//        bool getParameterInfo(String^ name, ParameterInfo% outInfo);
//
//        // Properties
//        slang::IModule* getParent();
//        String^ getName();
//        int getIndex();
//        SlangStage getStage();
//        slang::IEntryPoint* getNative();
//
//    private:
//        slang::IEntryPoint* m_entryPoint = nullptr;
//        slang::IModule* m_parent = nullptr;
//        String^ m_name;
//        int m_index = -1;
//        SlangStage m_stage = SLANG_STAGE_NONE;
//        Dictionary<String^, ParameterInfo>^ m_parameterInfoMap;
//    };
//}