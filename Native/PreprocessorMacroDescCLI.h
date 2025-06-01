#pragma once

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
    struct SLANGNATIVE_API PreprocessorMacroDescCLI
    {
    private:
        const char* m_Name;
        const char* m_Value;

    public:
		PreprocessorMacroDescCLI();
        PreprocessorMacroDescCLI(const char* name, const char* value);

        const char* getName();
        const char* getValue();
    };
}
