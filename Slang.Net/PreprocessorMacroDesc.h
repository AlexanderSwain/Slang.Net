namespace Slang
{
    public ref class PreprocessorMacroDesc
    {
        String^ name;
        String^ value;

    public:
        PreprocessorMacroDesc(String^ name, String^ value)
        {
            this->name = name;
            this->value = value;
        }

        String^ GetName()
        {
            return name;
        }

        String^ GetValue()
        {
            return value;
        }
    };
}