//#include "slang.h"
//#include "slang-com-ptr.h"
//#include "slang-com-helper.h"
//#include "CompilerOption.h"
//
//
//Slang::CompilerOption::CompilerOption(slang::CompilerOptionName name,
//    slang::CompilerOptionValueKind kind,
//    int32_t intValue0,
//    int32_t intValue1,
//    String^ stringValue0,
//    String^ stringValue1)
//	: _name(name)
//    , _kind(kind)
//    , _intValue0(intValue0)
//    , _intValue1(intValue1)
//    , _stringValue0(stringValue0)
//    , _stringValue1(stringValue1)
//{
//}
//
//slang::CompilerOptionName Slang::CompilerOption::getName()
//{
//	return _name;
//}
//
//slang::CompilerOptionValue Slang::CompilerOption::getValue()
//{
//    IntPtr stringPointer0 = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(_stringValue0);
//    const char* strVal0 = static_cast<const char*>(stringPointer0.ToPointer());
//
//    IntPtr stringPointer1 = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(_stringValue1);
//    const char* strVal1 = static_cast<const char*>(stringPointer1.ToPointer());
//
//    slang::CompilerOptionValue result = { _kind, _intValue0, _intValue1, strVal0, strVal1 };
//
//    System::Runtime::InteropServices::Marshal::FreeHGlobal(stringPointer0);
//    System::Runtime::InteropServices::Marshal::FreeHGlobal(stringPointer1);
//
//	return result;
//}