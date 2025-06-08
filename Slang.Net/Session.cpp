// Define this before including any Windows headers to avoid conflicts
#define NOMINMAX
#ifndef WIN32_LEAN_AND_MEAN
#define WIN32_LEAN_AND_MEAN
#endif

#include "Session.h"

namespace Slang
{
    static const char* FromString(System::String^ str)
    {
        if (str == nullptr)
            return nullptr;
        System::IntPtr strPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(str);
        const char* nativeStr = static_cast<const char*>(strPtr.ToPointer());
        return nativeStr;
    }

    static void ThrowErrorMessage(const char* errorMessage)
    {
        // If an error message is provided, throw an exception with that message
        if (errorMessage != nullptr)
        {
            System::String^ errorStr = gcnew System::String(errorMessage);
            throw gcnew System::ArgumentException(errorStr);
        }
        else
        {
            throw gcnew System::Exception("There was a problem generating an error message.");
        }
    }

    // Constructor with parameters implementation
    Slang::Session::Session(array<Slang::CompilerOption^>^ options,
        array<Slang::PreprocessorMacroDesc^>^ macros,
        array<Slang::ShaderModel^>^ models,
        array<System::String^>^ searchPaths)
    {
        // Marshal managed arrays to native arrays
        int optionsLength = options->Length;
        Native::CompilerOptionCLI* nativeOptions = new Native::CompilerOptionCLI[optionsLength];
        for (int i = 0; i < optionsLength; ++i)
        {
            Native::CompilerOptionNameCLI name = (Native::CompilerOptionNameCLI)options[i]->getName();
			const char* sv0 = FromString(options[i]->getValue()->m_stringValue0);
			const char* sv1 = FromString(options[i]->getValue()->m_stringValue1);
            Native::CompilerOptionValueCLI value = { (Native::CompilerOptionValueKindCLI)options[i]->getValue()->m_kind, options[i]->getValue()->m_intValue0, options[i]->getValue()->m_intValue1, sv0, sv1 };
            nativeOptions = new Native::CompilerOptionCLI(name, value);
        }
        
        int macrosLength = macros->Length;
        Native::PreprocessorMacroDescCLI* nativeMacros = new Native::PreprocessorMacroDescCLI[macrosLength];
        for (int i = 0; i < macrosLength; ++i)
        {
			const char* name = FromString(macros[i]->GetName());
			const char* value = FromString(macros[i]->GetValue());
            nativeMacros[i] = Native::PreprocessorMacroDescCLI(name, value); // or marshal as needed
        }
            
        int modelsLength = models->Length;
        Native::ShaderModelCLI* nativeModels = new Native::ShaderModelCLI[modelsLength];
        for (int i = 0; i < modelsLength; ++i)        
        {
			const char* profile = FromString(models[i]->getProfile());
            nativeModels[i] = Native::ShaderModelCLI((Native::CompileTargetCLI)models[i]->getTarget(), profile); // or marshal as needed
        }
            
        int searchPathsLength = searchPaths->Length;
        char** nativeSearchPaths = new char* [searchPathsLength];
        for (int i = 0; i < searchPathsLength; ++i)
        {
            System::IntPtr strPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(searchPaths[i]);
            nativeSearchPaths[i] = static_cast<char*>(strPtr.ToPointer());
        }

		const char* errorMessage = nullptr;
        
        // Call the native function
        m_NativeSession = SlangNative::CreateSession(
            nativeOptions, optionsLength,
            nativeMacros, macrosLength,
            nativeModels, modelsLength,
            nativeSearchPaths, searchPathsLength,
            &errorMessage);

        if (!m_NativeSession)
            ThrowErrorMessage(errorMessage);
        
        // Clean up native arrays if needed (except m_NativeSession, which you own)
        delete[] nativeOptions;
        delete[] nativeMacros;
        delete[] nativeModels;
        for (int i = 0; i < searchPathsLength; ++i)
            System::Runtime::InteropServices::Marshal::FreeHGlobal(System::IntPtr(nativeSearchPaths[i]));
        delete[] nativeSearchPaths;
    }

    // Destructor implementation
    Slang::Session::~Session()
    {
        // Clean up resources if needed
        if (m_NativeSession != nullptr)
        {
            // Assuming m_NativeSession is a pointer to some native resource that needs to be freed
            // Replace with actual cleanup code as necessary
            delete m_NativeSession;
            m_NativeSession = nullptr;
        }
    }

    void* Slang::Session::getNative()
    {
        return m_NativeSession;
    }
}
