#include "ProgramCLI.h"

Native::ProgramCLI::ProgramCLI(EntryPointCLI* entryPoint)
{
    m_entryPoint = entryPoint;

    std::array<slang::IComponentType*, 2> componentTypes =
    {
        entryPoint->getParent(),
        entryPoint->getNative()
    };

    slang::IComponentType* composedProgram;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = entryPoint->getParent()->getSession()->createCompositeComponentType(
            componentTypes.data(),
            componentTypes.size(),
            &composedProgram,
            diagnosticsBlob.writeRef());

        if (diagnosticsBlob != nullptr)
        {
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }

        if (SLANG_FAILED(result))
        {
            std::cerr << "Failed to create composite component type." << std::endl;
            return;
        }
    }

    if (composedProgram == nullptr)
    {
        std::cerr << "Failed to create composed program." << std::endl;
        return;
    }

    m_program = composedProgram;

    slang::IComponentType* linkedProgram;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        SlangResult result = composedProgram->link(
            &linkedProgram,
            diagnosticsBlob.writeRef());

        if (diagnosticsBlob != nullptr)
        {
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }
    }

    m_linkedProgram = linkedProgram;
}

SlangResult Native::ProgramCLI::GetCompiled(const char** output)
{
    SlangInt index = m_entryPoint->getIndex();
	SlangStage stage = m_entryPoint->getStage();

    SlangInt stageIndex = -1;
    switch (stage)
    {
        case SLANG_STAGE_VERTEX:
            stageIndex = 0;
            break;
        case SLANG_STAGE_GEOMETRY:
            stageIndex = 1;
            break;
        case SLANG_STAGE_PIXEL:
            stageIndex = 2;
            break;
        case SLANG_STAGE_DOMAIN:
            stageIndex = 3;
            break;
        case SLANG_STAGE_HULL:
            stageIndex = 4;
			break;
        case SLANG_STAGE_COMPUTE:
            stageIndex = 5;
            break;
        default:
            std::cerr << "Unsupported stage." << std::endl;
			return SLANG_FAIL;
    }

    Slang::ComPtr<slang::IBlob> bytecode;
    {
        Slang::ComPtr<slang::IBlob> diagnosticsBlob;
        bytecode.writeRef();
        diagnosticsBlob.writeRef();
        SlangResult result = m_linkedProgram->getEntryPointCode(
            index,
            stageIndex, // update this when implementing other graphics APIs
            bytecode.writeRef(),
            diagnosticsBlob.writeRef());
        if (diagnosticsBlob != nullptr)
        {
            std::cout << (const char*)diagnosticsBlob->getBufferPointer() << std::endl;
        }

        if (result < 0)
            SLANG_RETURN_ON_FAIL(result);

        *output = (const char*)bytecode->getBufferPointer();
    }
}

slang::IComponentType* Native::ProgramCLI::getNative()
{
    return m_program;
}

slang::IComponentType* Native::ProgramCLI::getLinked()
{
    return m_linkedProgram;
}

Native::EntryPointCLI* Native::ProgramCLI::getEntryPoint()
{
    return m_entryPoint;
}

Native::ProgramCLI::~ProgramCLI()
{
    if (m_program)
    {
        m_program->Release();
        m_program = nullptr;
    }
}

//extern "C" {
//    __declspec(dllexport) void* CreateProgram(void* entryPoint, char* name)
//    {
//        if (entryPoint)
//        {
//            EntryPoint* entryPoint = static_cast<EntryPoint*>(entryPoint);
//            Program* program = new Program(entryPoint);
//            return static_cast<void*>(program);
//        }
//        return nullptr;
//    }
//
//    __declspec(dllexport) bool GetCompiled(void* program, const char** output)
//    {
//        if (program)
//        {
//            Program* programPtr = static_cast<Program*>(program);
//            return programPtr->GetCompiled(output) == SLANG_OK;
//        }
//
//        return false;
//    }
//
//    __declspec(dllexport) void* DestroyProgram(void* program)
//    {
//        if (program)
//        {
//            EntryPoint* programPtr = static_cast<EntryPoint*>(program);
//            delete programPtr;
//            return nullptr;
//        }
//        return nullptr;
//    }
//}