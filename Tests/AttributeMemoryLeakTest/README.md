# Native::Attribute Memory Leak Test

This test project is designed to detect memory leaks in the `Native::Attribute` class from the SlangNative library.

## Project Structure

```
Tests/AttributeMemoryLeakTest/
??? AttributeMemoryLeakTest.vcxproj    # Visual Studio project file
??? main.cpp                           # Main test application
??? MemoryLeakDetector.h               # Memory leak detection utilities
??? MemoryLeakDetector.cpp             # Implementation of memory leak detector
??? README.md                          # This file
```

## How to Run

1. **Prerequisites:**
   - Visual Studio 2022 with C++ development tools
   - Windows 10 SDK
   - The SlangNative project must be built first

2. **Building:**
   - Open the main Slang.Net solution in Visual Studio
   - Build the SlangNative project first (in Debug mode for best leak detection)
   - Then build this test project

3. **Running:**
   - Run the executable from Visual Studio (F5) or command line
   - The test will output detailed information about memory usage and any detected leaks

## What It Tests

The test suite includes several test scenarios:

### 1. Basic Attribute Creation
- Creates and destroys `Native::Attribute` objects repeatedly
- Verifies basic functionality (getName, getArgumentCount)

### 2. Attribute Argument Access
- Tests all argument accessor methods
- Verifies correct values are returned

### 3. Attribute Type Reflection
- **This is the main test for the memory leak issue**
- Calls `getArgumentType()` multiple times with different indices
- Demonstrates the bug where only one TypeReflection object is cached

### 4. Multiple Attribute Instances
- Creates many attribute instances simultaneously
- Tests interaction between multiple objects

### 5. Stress Test
- Runs intensive operations to amplify any memory leaks
- Monitors memory usage throughout execution

### 6. Real World Scenario
- Simulates realistic usage patterns
- Tests with various attribute configurations

## Expected Results

### With Current Implementation (Memory Leak)
The current implementation has a potential memory leak because:
- `m_argumentType` is allocated with `new` but never freed
- Only one TypeReflection object is cached regardless of index
- No destructor exists to clean up the allocated memory

### After Fix
Once the memory leak is fixed, all tests should pass with no memory leaks detected.

## Memory Leak Detection Features

The test uses several memory leak detection mechanisms:

1. **CRT Debug Heap**: Uses `_CrtSetDbgFlag` to enable memory leak detection
2. **Memory Checkpoints**: Takes snapshots before and after operations
3. **Scoped Detection**: Uses RAII pattern for automatic leak detection
4. **Process Memory Monitoring**: Tracks overall memory usage

## Output Interpretation

- **"NO MEMORY LEAKS DETECTED"** = Good! The code is clean
- **"MEMORY LEAK DETECTED"** = Problem found, review the dump output
- Memory usage should remain relatively stable during stress tests

## Debugging Memory Leaks

If leaks are detected:

1. Look at the memory dump output for allocation details
2. Use the allocation numbers to set breakpoints: `_CrtSetBreakAlloc(number)`
3. Run in debugger to see exactly where the leak occurs
4. Fix the issue by adding proper cleanup code

## Known Issues with Current Implementation

The `Native::Attribute::getArgumentType()` method has these problems:

```cpp
Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
    if (!m_argumentType)
        m_argumentType = new TypeReflection(m_native->getArgumentType(index));
    return m_argumentType;
}
```

Issues:
1. **Memory Leak**: No destructor to free `m_argumentType`
2. **Wrong Caching**: Only caches the first call, ignoring the index parameter
3. **Logic Error**: Subsequent calls with different indices return wrong object

## Recommended Fix

Remove the caching entirely and let the caller manage the lifetime:

```cpp
Native::TypeReflection* Native::Attribute::getArgumentType(uint32_t index)
{
    return new TypeReflection(m_native->getArgumentType(index));
}
```

This follows the same pattern used elsewhere in the codebase and lets the COM reference counting handle memory management.