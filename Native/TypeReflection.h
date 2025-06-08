#pragma once
#include "slang.h"
#include "slang-com-ptr.h"
#include "slang-com-helper.h"
#include "VariableReflection.h"
#include "TypeReflection.h"
#include "ResourceShape.h"
#include "ResourceAccess.h"
#include "Attribute.h"
#include "GenericReflection.h"

#ifdef SLANGNATIVE_EXPORTS
#define SLANGNATIVE_API __declspec(dllexport)
#else
#define SLANGNATIVE_API __declspec(dllimport)
#endif

namespace Native
{
	// This type is empty in slang.h for some reason
	struct SLANGNATIVE_API TypeReflection
	{
    public:
        enum class Kind
        {
            None = SLANG_TYPE_KIND_NONE,
            Struct = SLANG_TYPE_KIND_STRUCT,
            Array = SLANG_TYPE_KIND_ARRAY,
            Matrix = SLANG_TYPE_KIND_MATRIX,
            Vector = SLANG_TYPE_KIND_VECTOR,
            Scalar = SLANG_TYPE_KIND_SCALAR,
            ConstantBuffer = SLANG_TYPE_KIND_CONSTANT_BUFFER,
            Resource = SLANG_TYPE_KIND_RESOURCE,
            SamplerState = SLANG_TYPE_KIND_SAMPLER_STATE,
            TextureBuffer = SLANG_TYPE_KIND_TEXTURE_BUFFER,
            ShaderStorageBuffer = SLANG_TYPE_KIND_SHADER_STORAGE_BUFFER,
            ParameterBlock = SLANG_TYPE_KIND_PARAMETER_BLOCK,
            GenericTypeParameter = SLANG_TYPE_KIND_GENERIC_TYPE_PARAMETER,
            Interface = SLANG_TYPE_KIND_INTERFACE,
            OutputStream = SLANG_TYPE_KIND_OUTPUT_STREAM,
            Specialized = SLANG_TYPE_KIND_SPECIALIZED,
            Feedback = SLANG_TYPE_KIND_FEEDBACK,
            Pointer = SLANG_TYPE_KIND_POINTER,
            DynamicResource = SLANG_TYPE_KIND_DYNAMIC_RESOURCE,
        };

        enum ScalarType
        {
            None = SLANG_SCALAR_TYPE_NONE,
            Void = SLANG_SCALAR_TYPE_VOID,
            Bool = SLANG_SCALAR_TYPE_BOOL,
            Int32 = SLANG_SCALAR_TYPE_INT32,
            UInt32 = SLANG_SCALAR_TYPE_UINT32,
            Int64 = SLANG_SCALAR_TYPE_INT64,
            UInt64 = SLANG_SCALAR_TYPE_UINT64,
            Float16 = SLANG_SCALAR_TYPE_FLOAT16,
            Float32 = SLANG_SCALAR_TYPE_FLOAT32,
            Float64 = SLANG_SCALAR_TYPE_FLOAT64,
            Int8 = SLANG_SCALAR_TYPE_INT8,
            UInt8 = SLANG_SCALAR_TYPE_UINT8,
            Int16 = SLANG_SCALAR_TYPE_INT16,
            UInt16 = SLANG_SCALAR_TYPE_UINT16,
        };

		TypeReflection(void* native);
        void* getNative();

        Kind getKind();

        // only useful if `getKind() == Kind::Struct`
        unsigned int getFieldCount();

        VariableReflection* getFieldByIndex(unsigned int index);

        bool isArray();

        TypeReflection* unwrapArray();

        // only useful if `getKind() == Kind::Array`
        size_t getElementCount();

        size_t getTotalArrayElementCount();

        TypeReflection* getElementType();

        unsigned getRowCount() { return spReflectionType_GetRowCount((SlangReflectionType*)this); }

        unsigned getColumnCount();

        ScalarType getScalarType();

        TypeReflection* getResourceResultType();

        ResourceShape getResourceShape();

        ResourceAccess getResourceAccess();

        char const* getName();

        SlangResult getFullName(ISlangBlob** outNameBlob);

        unsigned int getUserAttributeCount();

        Attribute* getUserAttributeByIndex(unsigned int index);

        Attribute* findAttributeByName(char const* name);

        Attribute* findUserAttributeByName(char const* name) { return findAttributeByName(name); }

        TypeReflection* applySpecializations(GenericReflection* generic);

        GenericReflection* getGenericContainer();

	private:
		slang::TypeReflection* m_native;
	};
}