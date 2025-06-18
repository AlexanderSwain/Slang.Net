#pragma once
#include <slang.h>

namespace Native
{
    struct Modifier
    {
        enum ID
        {
            Shared = SLANG_MODIFIER_SHARED,
            NoDiff = SLANG_MODIFIER_NO_DIFF,
            Static = SLANG_MODIFIER_STATIC,
            Const = SLANG_MODIFIER_CONST,
            Export = SLANG_MODIFIER_EXPORT,
            Extern = SLANG_MODIFIER_EXTERN,
            Differentiable = SLANG_MODIFIER_DIFFERENTIABLE,
            Mutating = SLANG_MODIFIER_MUTATING,
            In = SLANG_MODIFIER_IN,
            Out = SLANG_MODIFIER_OUT,
            InOut = SLANG_MODIFIER_INOUT
        };

    private:
        slang::Modifier* m_native;

    public:
        Modifier(slang::Modifier* modifier) : m_native(modifier) { }
        
        void* getNative() { return m_native; }
        
        // Get the modifier ID
        ID getID();
        
        // Get the modifier name as string
        const char* getName();
    };
}