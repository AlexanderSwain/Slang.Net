#include "MemoryLeakDetector.h"
#include <psapi.h>
#include <iomanip>

// Static member definitions
_CrtMemState MemoryLeakDetector::s_memStateStart = {};
_CrtMemState MemoryLeakDetector::s_memStateEnd = {};
bool MemoryLeakDetector::s_isInitialized = false;
bool MemoryLeakDetector::s_isMonitoring = false;

void MemoryLeakDetector::Initialize() {
    if (s_isInitialized) return;
    
    // Enable run-time memory check for debug builds
    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
    
    // Set break on memory leak
    _CrtSetBreakAlloc(-1);
    
    s_isInitialized = true;
    std::cout << "Memory leak detector initialized." << std::endl;
}

void MemoryLeakDetector::StartMonitoring() {
    if (!s_isInitialized) {
        Initialize();
    }
    
    _CrtMemCheckpoint(&s_memStateStart);
    s_isMonitoring = true;
    std::cout << "Memory monitoring started." << std::endl;
}

void MemoryLeakDetector::StopMonitoring() {
    if (!s_isMonitoring) return;
    
    _CrtMemCheckpoint(&s_memStateEnd);
    s_isMonitoring = false;
    std::cout << "Memory monitoring stopped." << std::endl;
}

void MemoryLeakDetector::ReportLeaks() {
    if (!s_isInitialized) return;
    
    _CrtMemState memStateDiff;
    if (_CrtMemDifference(&memStateDiff, &s_memStateStart, &s_memStateEnd)) {
        std::cout << "\n=== MEMORY LEAK DETECTED ===" << std::endl;
        _CrtMemDumpStatistics(&memStateDiff);
        std::cout << "===========================\n" << std::endl;
    } else {
        std::cout << "\n=== NO MEMORY LEAKS DETECTED ===" << std::endl;
    }
}

void MemoryLeakDetector::DumpMemoryLeaks() {
    if (!s_isInitialized) return;
    
    std::cout << "\n=== DUMPING ALL MEMORY LEAKS ===" << std::endl;
    _CrtDumpMemoryLeaks();
    std::cout << "===============================\n" << std::endl;
}

// ScopedMemoryLeakDetector implementation
ScopedMemoryLeakDetector::ScopedMemoryLeakDetector() {
    _CrtMemCheckpoint(&m_memStateStart);
}

ScopedMemoryLeakDetector::~ScopedMemoryLeakDetector() {
    _CrtMemState memStateEnd;
    _CrtMemCheckpoint(&memStateEnd);
    
    _CrtMemState memStateDiff;
    if (_CrtMemDifference(&memStateDiff, &m_memStateStart, &memStateEnd)) {
        std::cout << "\n=== SCOPED MEMORY LEAK DETECTED ===" << std::endl;
        _CrtMemDumpStatistics(&memStateDiff);
        std::cout << "==================================\n" << std::endl;
    }
}

// MemoryUtils namespace implementation
namespace MemoryUtils {
    void PrintMemoryUsage(const std::string& label) {
        PROCESS_MEMORY_COUNTERS_EX pmc;
        if (GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc))) {
            std::cout << label << " - Memory Usage: " 
                      << std::fixed << std::setprecision(2)
                      << (pmc.WorkingSetSize / 1024.0 / 1024.0) << " MB" << std::endl;
        }
    }
    
    size_t GetCurrentMemoryUsage() {
        PROCESS_MEMORY_COUNTERS_EX pmc;
        if (GetProcessMemoryInfo(GetCurrentProcess(), (PROCESS_MEMORY_COUNTERS*)&pmc, sizeof(pmc))) {
            return pmc.WorkingSetSize;
        }
        return 0;
    }
    
    void ForceGarbageCollection() {
        // Force a garbage collection cycle (mainly for testing)
        _CrtCheckMemory();
    }
}