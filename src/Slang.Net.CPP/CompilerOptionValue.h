#include "CompileOptionValueKind.h"
#include <cstdint>

using namespace System;

namespace Slang::Cpp
{
    public ref class CompilerOptionValue
    {
    public:
        CompilerOptionValueKind m_kind;
        int32_t m_intValue0;
        int32_t m_intValue1;
        String^ m_stringValue0;
        String^ m_stringValue1;

        CompilerOptionValue(CompilerOptionValueKind kind, int32_t m_intValue0, int32_t m_intValue1, String^ m_stringValue0, String^ m_stringValue1)
            : m_kind(kind),
              m_intValue0(m_intValue0),
              m_intValue1(m_intValue1),
              m_stringValue0(m_stringValue0),
              m_stringValue1(m_stringValue1)
        {
        }
    };
}