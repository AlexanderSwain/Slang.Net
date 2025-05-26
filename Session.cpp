//#include "Session.h"
//#include <vector>
//#include <iostream>
//#include <sys/stat.h>
//
//slang::IGlobalSession* Slang::Session::s_context = nullptr;
//
//// Constructor with parameters implementation
//Slang::Session::Session(array<CompilerOption^>^ options,
//    array<slang::PreprocessorMacroDesc*>^ macros,
//    array<ShaderModel^>^ models,
//    array<String^>^ searchPaths)
//{
//    slang::SessionDesc sessionDesc = {};
//
//    // Set targets
//    std::vector<slang::TargetDesc> targetDescs;
//    targetDescs.reserve(models->Length);
//
//    for (int i = 0; i < models->Length; i++)
//    {
//        ShaderModel^ model = models[i];
//
//                // Convert System::String^ to const char* before passing to findProfile
//        System::String^ managedProfile = model->getProfile();
//        IntPtr profilePointer = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(managedProfile);
//        const char* nativeProfile = static_cast<const char*>(profilePointer.ToPointer());
//
//        slang::TargetDesc targetDesc = {};
//        targetDesc.format = model->getTarget();
//        targetDesc.profile = GetGlobalSession()->findProfile(nativeProfile);
//
//        // Free the allocated memory after use
//        System::Runtime::InteropServices::Marshal::FreeHGlobal(profilePointer);
//        
//        // Add to our collection
//        targetDescs.push_back(targetDesc);
//    }
//
//    // Use the vector data as the targets array if we have any targets
//    if (!targetDescs.empty())
//    {
//        sessionDesc.targets = targetDescs.data();
//        sessionDesc.targetCount = static_cast<int>(targetDescs.size());
//    }
//
//	// Set compiler options
//    std::vector<slang::CompilerOptionEntry> compilerOptions;
//    compilerOptions.reserve(options->Length);
//    
//    for (int i = 0; i < options->Length; i++)
//    {
//        CompilerOption^ option = options[i];
//    
//        slang::CompilerOptionEntry entry = 
//        {
//                option->getName(),
//                option->getValue()
//        };
//
//        std::cout << (int)option->getName() << std::endl;
//        std::cout << option->getValue().intValue1 << std::endl;
//    
//        // Add to our collection
//        compilerOptions.push_back(entry);
//    }
//    
//    // Use the vector data as the targets array if we have any targets
//    if (!targetDescs.empty())
//    {
//        sessionDesc.compilerOptionEntries = compilerOptions.data();
//        sessionDesc.compilerOptionEntryCount = compilerOptions.size();
//    }
//    
//    // Set PreprocessorMacroDesc
//        // Set compiler options
//    std::vector<slang::PreprocessorMacroDesc> preprocessorMacroDesc;
//    preprocessorMacroDesc.reserve(macros->Length);
//    
//    for (int i = 0; i < macros->Length; i++)
//    {
//        slang::PreprocessorMacroDesc* macroDesc = macros[i];
//
//        std::cout << macroDesc->name << std::endl;
//        std::cout << macroDesc->value << std::endl;
//    
//        // Add to our collection
//        preprocessorMacroDesc.push_back(*macroDesc);
//    }
//    
//    // Use the vector data as the targets array if we have any targets
//    if (!preprocessorMacroDesc.empty())
//    {
//        sessionDesc.preprocessorMacros = preprocessorMacroDesc.data();
//        sessionDesc.preprocessorMacroCount = static_cast<int>(preprocessorMacroDesc.size());
//    }
//
//    // Convert the CLI array of System::String^ to a native array of const char*  
//    std::vector<const char*> nativeSearchPaths;  
//    nativeSearchPaths.reserve(searchPaths->Length);  
//
//    for (int i = 0; i < searchPaths->Length; i++)  
//    {  
//        System::String^ managedString = searchPaths[i];  
//        IntPtr stringPointer = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(managedString);  
//        nativeSearchPaths.push_back(static_cast<const char*>(stringPointer.ToPointer()));  
//    }  
//
//    // Assign the native array to sessionDesc.searchPaths  
//    sessionDesc.searchPaths = nativeSearchPaths.data();  
//    sessionDesc.searchPathCount = static_cast<int>(nativeSearchPaths.size());  
//
//    // Clean up the allocated memory after the session is created  
//    for (const char* path : nativeSearchPaths)  
//    {  
//        System::Runtime::InteropServices::Marshal::FreeHGlobal(IntPtr(const_cast<char*>(path)));  
//    }
//    sessionDesc.searchPathCount = static_cast<int>(searchPaths->Length); // Update the count to reflect the number of paths
//
//    Slang::ComPtr<slang::ISession> session;
//    s_context->createSession(sessionDesc, session.writeRef());
//    m_session = session;
//
//    std::cout << sessionDesc.searchPaths[0] << std::endl;
//}
//
//// Destructor implementation
//Slang::Session::~Session()
//{
//    // Clean up resources if needed
//}
//
//slang::IGlobalSession* Slang::Session::GetGlobalSession()
//{
//    if (!s_context)
//    {
//        // Call the C API entry point directly
//        slang::IGlobalSession* nativeContext = nullptr;
//        slang_createGlobalSession(0, &nativeContext);
//        s_context = nativeContext;
//    }
//
//    return s_context;
//}
//
//slang::ISession* Slang::Session::getNative()
//{
//    return m_session;
//}