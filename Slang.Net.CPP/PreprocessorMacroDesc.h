namespace Slang::Cpp
{
    public ref class PreprocessorMacroDesc
    {
        System::String^ name;
        System::String^ value;

    public:
        PreprocessorMacroDesc(System::String^ name, System::String^ value)
        {
            this->name = name;
            this->value = value;
        }

        System::String^ GetName()
        {
            return name;
        }

        System::String^ GetValue()
        {
            return value;
        }
    };
}