//#pragma once
//#include "slang.h"
//#include "slang-com-ptr.h"
//#include "slang-com-helper.h"
//
//#ifdef SLANGNATIVE_EXPORTS
//#define SLANGNATIVE_API __declspec(dllexport)
//#else
//#define SLANGNATIVE_API __declspec(dllimport)
//#endif
//
//namespace Native
//{
//	// This type is empty in slang.h for some reason
//	struct SLANGNATIVE_API DeclReflection
//	{
//		enum class Kind
//		{
//			Unsupported = SLANG_DECL_KIND_UNSUPPORTED_FOR_REFLECTION,
//			Struct = SLANG_DECL_KIND_STRUCT,
//			Func = SLANG_DECL_KIND_FUNC,
//			Module = SLANG_DECL_KIND_MODULE,
//			Generic = SLANG_DECL_KIND_GENERIC,
//			Variable = SLANG_DECL_KIND_VARIABLE,
//			Namespace = SLANG_DECL_KIND_NAMESPACE,
//		};
//        template<Kind K> struct FilteredList
//        {
//            unsigned int count;
//            DeclReflection* parent;
//
//            struct FilteredIterator
//            {
//                DeclReflection* parent;
//                unsigned int count;
//                unsigned int index;
//
//                DeclReflection* operator*() { return parent->getChild(index); }
//                void operator++()
//                {
//                    index++;
//                    while (index < count && !(parent->getChild(index)->getKind() == K))
//                    {
//                        index++;
//                    }
//                }
//                bool operator!=(FilteredIterator const& other) { return index != other.index; }
//            };
//
//            // begin/end for range-based for that checks the kind
//            FilteredIterator begin()
//            {
//                // Find the first child of the right kind
//                unsigned int index = 0;
//                while (index < count && !(parent->getChild(index)->getKind() == K))
//                {
//                    index++;
//                }
//                return FilteredIterator{ parent, count, index };
//            }
//
//            FilteredIterator end() { return FilteredIterator{ parent, count, count }; }
//        };
//
//        struct IteratedList;
//
//
//	public:
//		DeclReflection(void* native);
//
//	private:
//		slang::DeclReflection* m_native;
//
//        char const* getName();
//
//        Kind getKind();
//
//        unsigned int getChildrenCount();
//
//        DeclReflection* getChild(unsigned int index);
//
//        TypeReflection* getType();
//
//        VariableReflection* asVariable();
//
//        FunctionReflection* asFunction();
//
//        GenericReflection* asGeneric();
//
//        DeclReflection* getParent();
//
//        template<Kind K> FilteredList<K> getChildrenOfKind();
//
//        IteratedList getChildren();
//	};
//}
//
