#pragma once
#include <Windows.h>
#include <crtdbg.h>
#include <iostream>
#include <vector>
#include <string>

class MemoryLeakDetector {
public:
    static void Initialize();
    static void StartMonitoring();
    static void StopMonitoring();
    static void ReportLeaks();
    static void DumpMemoryLeaks();
    
private:
    static _CrtMemState s_memStateStart;
    static _CrtMemState s_memStateEnd;
    static bool s_isInitialized;
    static bool s_isMonitoring;
};

// Helper class for RAII-style memory leak detection
class ScopedMemoryLeakDetector {
public:
    ScopedMemoryLeakDetector();
    ~ScopedMemoryLeakDetector();
    
private:
    _CrtMemState m_memStateStart;
};

// Utility functions for memory debugging
namespace MemoryUtils {
    void PrintMemoryUsage(const std::string& label);
    size_t GetCurrentMemoryUsage();
    void ForceGarbageCollection();
}