#pragma once

namespace Slang::Cpp
{
	public ref class ParameterInfo
    {
    private:
        System::String^ m_name;
        unsigned int m_category;
        unsigned int m_bindingIndex;
        unsigned int m_bindingSpace;

    public:
        ParameterInfo(System::String^ name, unsigned int category, unsigned int bindingIndex, unsigned int bindingSpace);

        property System::String^ Name
        {
            System::String^ get() { return m_name; }
		}
        property unsigned int Category
        {
            unsigned int get() { return m_category; }
        }
        property unsigned int BindingIndex
		{
            unsigned int get() { return m_bindingIndex; }
        }
        property unsigned int BindingSpace
        {
            unsigned int get() { return m_bindingSpace; }
        }

        virtual System::String^ ToString() override
        {
            return System::String::Format("ParameterInfo(Name: {0}, Category: {1}, BindingIndex: {2}, BindingSpace: {3})",
                m_name, m_category, m_bindingIndex, m_bindingSpace);
        }
    };
}