//#include "DeclReflection.h"
//
//struct IteratedList
//{
//    slang::DeclReflection::IteratedList m_native;
//    unsigned int getCount()
//    {
//        return m_native.count;
//    }
//
//    Native::DeclReflection getParent()
//    {
//        return Native::DeclReflection(m_native.parent);
//    }
//
//    struct Iterator
//    {
//        slang::DeclReflection::IteratedList::Iterator m_native;
//
//        DeclReflection* operator*() { return m_native.parent->getChild(m_native.index); }
//        void operator++() { index++; }
//        bool operator!=(Iterator const& other) { return index != other.index; }
//    };
//
//    // begin/end for range-based for that checks the kind
//    IteratedList::Iterator begin() { return IteratedList::Iterator{ parent, count, 0 }; }
//    IteratedList::Iterator end() { return IteratedList::Iterator{ parent, count, count }; }
//};
//
//Native::DeclReflection::DeclReflection(void* native)
//{
//	m_native = (slang::DeclReflection*)native;
//}
//
//char const* Native::DeclReflection::getName()
//{
//	return m_native->getName();
//}
//
//Native::DeclReflection::Kind Native::DeclReflection::getKind()
//{
//	return (Native::DeclReflection::Kind)m_native->getKind();
//}
//
//unsigned int Native::DeclReflection::getChildrenCount()
//{
//	return m_native->getChildrenCount();
//}
//
//DeclReflection* Native::DeclReflection::getChild(unsigned int index)
//{
//	return new DeclReflection(m_native->getChild());
//}
//
//TypeReflection* Native::DeclReflection::getType()
//{
//	return new TypeReflection(m_native->getType());
//}
//
//VariableReflection* Native::DeclReflection::asVariable()
//{
//	return new VariableReflection(m_native->asVariable());
//}
//
//FunctionReflection* Native::DeclReflection::asFunction()
//{
//	return new FunctionReflection(m_native->asFunction());
//}
//
//GenericReflection* Native::DeclReflection::asGeneric()
//{
//	return new GenericReflection(m_native->asGeneric());
//}
//
//DeclReflection* Native::DeclReflection::getParent()
//{
//	return new DeclReflection(m_native->getParent());
//}
//
//template<Kind K> FilteredList<K> Native::DeclReflection::getChildrenOfKind()
//{
//	return m_native->getChildrenOfKind();
//}
//
//IteratedList Native::DeclReflection::getChildren()
//{
//	return m_native->getChildren();
//}
//
