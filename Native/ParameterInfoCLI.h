#pragma once

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
    struct SLANGNATIVE_API ParameterInfoCLI
    {
    private:
        const char* m_Name;
        unsigned int m_Category;
        unsigned int m_BindingIndex;
        unsigned int m_BindingSpace;
    public:
        ParameterInfoCLI();
        ParameterInfoCLI(const char* name, unsigned int category, unsigned int bindingIndex, unsigned int bindingSpace);
        
        const char* getName();
        unsigned int getCategory();
        unsigned int getBindingIndex();
        unsigned int getBindingSpace();
    };
}