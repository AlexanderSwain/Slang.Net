#pragma once

namespace Native
{
    struct PreprocessorMacroDescCLI
    {
    private:
        const char* m_Name;
        const char* m_Value;

    public:
		PreprocessorMacroDescCLI();
        PreprocessorMacroDescCLI(const char* name, const char* value);

        const char* getName2();
        const char* getValue2();
    };
}
