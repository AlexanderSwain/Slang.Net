#pragma once
#include "TypeDef.h"
#include "TypeReflection.h"

namespace Native
{
    union GenericArgReflection
    {
        TypeReflection* typeVal;
        int64_t intVal;
        bool boolVal;
    };
}