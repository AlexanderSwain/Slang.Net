#include <iostream>
#include <memory>
#include <vector>
#include <chrono>
#include <thread>
#include <cassert>

// Include our memory leak detector
#include "MemoryLeakDetector.h"

// Include the Native Attribute classes
#include "Attribute.h"
#include "TypeReflection.h"
#include "SlangNative.h"

// Test configurations
const int TEST_ITERATIONS = 1000;
const int STRESS_TEST_ITERATIONS = 10000;

// Test functions
void TestBasicAttributeCreation() {
    std::cout << "\n=== Testing Basic Attribute Creation ===" << std::endl;
    
    ScopedMemoryLeakDetector detector;
    
    Native::Attribute mockAttr("TestAttribute", 2);
    
    // Test creating and destroying attributes
    for (int i = 0; i < TEST_ITERATIONS; ++i) {
        Native::Attribute* attr = new Native::Attribute(&mockAttr);
        
        // Use the attribute
        const char* name = attr->getName();
        uint32_t argCount = attr->getArgumentCount();
        
        assert(name != nullptr);
        assert(argCount == 2);
        
        delete attr;
    }
    
    std::cout << "Basic attribute creation test completed." << std::endl;
}

void TestAttributeArgumentAccess() {
    std::cout << "\n=== Testing Attribute Argument Access ===" << std::endl;
    
    ScopedMemoryLeakDetector detector;
    
    MockSlangAttribute mockAttr("TestAttribute", 3);
    
    for (int i = 0; i < TEST_ITERATIONS; ++i) {
        Native::Attribute* attr = new Native::Attribute(&mockAttr);
        
        // Test all accessor methods
        int intValue;
        float floatValue;
        size_t stringSize;
        
        attr->getArgumentValueInt(0, &intValue);
        attr->getArgumentValueFloat(1, &floatValue);
        const char* stringValue = attr->getArgumentValueString(2, &stringSize);
        
        assert(intValue == 42);
        assert(floatValue == 3.14f);
        assert(stringValue != nullptr);
        assert(stringSize == 5);
        
        delete attr;
    }
    
    std::cout << "Attribute argument access test completed." << std::endl;
}

void TestAttributeTypeReflection() {
    std::cout << "\n=== Testing Attribute Type Reflection ===" << std::endl;
    
    ScopedMemoryLeakDetector detector;
    
    MockSlangAttribute mockAttr("TestAttribute", 2);
    
    for (int i = 0; i < TEST_ITERATIONS; ++i) {
        Native::Attribute* attr = new Native::Attribute(&mockAttr);
        
        // This is the method that was causing the potential memory leak
        // Test calling it multiple times with different indices
        Native::TypeReflection* type0 = attr->getArgumentType(0);
        Native::TypeReflection* type1 = attr->getArgumentType(1);
        Native::TypeReflection* type0_again = attr->getArgumentType(0);
        
        // Note: The current implementation has a bug - it will return the same
        // TypeReflection object regardless of index, and only creates one
        assert(type0 != nullptr);
        assert(type1 != nullptr);
        assert(type0_again == type0); // This demonstrates the bug
        
        delete attr;
    }
    
    std::cout << "Attribute type reflection test completed." << std::endl;
}

void TestMultipleAttributeInstances() {
    std::cout << "\n=== Testing Multiple Attribute Instances ===" << std::endl;
    
    ScopedMemoryLeakDetector detector;
    
    std::vector<std::unique_ptr<Native::Attribute>> attributes;
    
    // Create multiple attributes
    for (int i = 0; i < TEST_ITERATIONS; ++i) {
        MockSlangAttribute* mockAttr = new MockSlangAttribute("TestAttribute", i % 5);
        attributes.push_back(std::make_unique<Native::Attribute>(mockAttr));
    }
    
    // Use all attributes
    for (auto& attr : attributes) {
        const char* name = attr->getName();
        uint32_t argCount = attr->getArgumentCount();
        
        if (argCount > 0) {
            Native::TypeReflection* type = attr->getArgumentType(0);
            assert(type != nullptr);
        }
    }
    
    // Clean up (unique_ptr will handle this automatically)
    attributes.clear();
    
    std::cout << "Multiple attribute instances test completed." << std::endl;
}

void TestStressTest() {
    std::cout << "\n=== Running Stress Test ===" << std::endl;
    
    MemoryUtils::PrintMemoryUsage("Before stress test");
    
    ScopedMemoryLeakDetector detector;
    
    for (int i = 0; i < STRESS_TEST_ITERATIONS; ++i) {
        MockSlangAttribute mockAttr("StressTestAttribute", 3);
        Native::Attribute* attr = new Native::Attribute(&mockAttr);
        
        // Exercise all methods
        attr->getName();
        attr->getArgumentCount();
        attr->getArgumentType(0);
        attr->getArgumentType(1);
        attr->getArgumentType(2);
        
        int intVal;
        float floatVal;
        size_t strSize;
        
        attr->getArgumentValueInt(0, &intVal);
        attr->getArgumentValueFloat(1, &floatVal);
        attr->getArgumentValueString(2, &strSize);
        
        delete attr;
        
        // Periodic memory check
        if (i % 1000 == 0) {
            MemoryUtils::ForceGarbageCollection();
            if (i % 5000 == 0) {
                MemoryUtils::PrintMemoryUsage("Stress test iteration " + std::to_string(i));
            }
        }
    }
    
    MemoryUtils::PrintMemoryUsage("After stress test");
    std::cout << "Stress test completed." << std::endl;
}

void TestRealWorldScenario() {
    std::cout << "\n=== Testing Real World Scenario ===" << std::endl;
    
    ScopedMemoryLeakDetector detector;
    
    // Simulate a real-world scenario where attributes are created, used, and destroyed
    // in a more realistic pattern
    
    std::vector<MockSlangAttribute*> mockAttributes;
    std::vector<Native::Attribute*> attributes;
    
    // Create a bunch of mock attributes
    for (int i = 0; i < 100; ++i) {
        mockAttributes.push_back(new MockSlangAttribute("Attr" + std::to_string(i), i % 4));
    }
    
    // Create Native::Attribute wrappers
    for (auto* mockAttr : mockAttributes) {
        attributes.push_back(new Native::Attribute(mockAttr));
    }
    
    // Use them in various ways
    for (int iteration = 0; iteration < 10; ++iteration) {
        for (auto* attr : attributes) {
            const char* name = attr->getName();
            uint32_t argCount = attr->getArgumentCount();
            
            for (uint32_t i = 0; i < argCount; ++i) {
                Native::TypeReflection* type = attr->getArgumentType(i);
                assert(type != nullptr);
                
                int intVal;
                float floatVal;
                size_t strSize;
                
                attr->getArgumentValueInt(i, &intVal);
                attr->getArgumentValueFloat(i, &floatVal);
                attr->getArgumentValueString(i, &strSize);
            }
        }
    }
    
    // Clean up
    for (auto* attr : attributes) {
        delete attr;
    }
    attributes.clear();
    
    for (auto* mockAttr : mockAttributes) {
        delete mockAttr;
    }
    mockAttributes.clear();
    
    std::cout << "Real world scenario test completed." << std::endl;
}

int main() {
    std::cout << "=== Native::Attribute Memory Leak Test ===" << std::endl;
    std::cout << "This test will check for memory leaks in the Native::Attribute class." << std::endl;
    std::cout << "Test iterations: " << TEST_ITERATIONS << std::endl;
    std::cout << "Stress test iterations: " << STRESS_TEST_ITERATIONS << std::endl;
    
    // Initialize memory leak detection
    MemoryLeakDetector::Initialize();
    MemoryUtils::PrintMemoryUsage("Initial");
    
    try {
        // Start global memory monitoring
        MemoryLeakDetector::StartMonitoring();
        
        // Run all tests
        TestBasicAttributeCreation();
        TestAttributeArgumentAccess();
        TestAttributeTypeReflection();
        TestMultipleAttributeInstances();
        TestStressTest();
        TestRealWorldScenario();
        
        // Stop monitoring and report results
        MemoryLeakDetector::StopMonitoring();
        MemoryUtils::PrintMemoryUsage("Final");
        
        std::cout << "\n=== All tests completed ===" << std::endl;
        
    } catch (const std::exception& e) {
        std::cerr << "Test failed with exception: " << e.what() << std::endl;
        return 1;
    } catch (...) {
        std::cerr << "Test failed with unknown exception" << std::endl;
        return 1;
    }
    
    // Final memory leak report
    MemoryLeakDetector::ReportLeaks();
    MemoryLeakDetector::DumpMemoryLeaks();
    
    std::cout << "\nPress any key to exit..." << std::endl;
    std::cin.get();
    
    return 0;
}