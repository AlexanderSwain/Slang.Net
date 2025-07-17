# Slang.Net Test Suite

This directory contains the comprehensive test suite for Slang.Net, ensuring all functionality works correctly and safely.

## Quick Start

### Prerequisites
- Windows 10/11 (x64 or ARM64)
- Visual Studio 2022 with C++ tools
- .NET 9.0 SDK

### Run Tests (Recommended)
```powershell
# From the Tests directory
.\run_tests.ps1

# Or with specific configuration
.\run_tests.ps1 -Configuration Release -Platform x64
```

### Alternative: Batch File
```batch
# From the Tests directory  
run_tests.bat

# Or with parameters
run_tests.bat --config Release --platform x64
```

## Test Projects

### [AttributeMemoryLeakTest](AttributeMemoryLeakTest/)
**Purpose**: Comprehensive validation of the Slang reflection API with memory leak detection.

**What it tests**:
- ? Session creation and configuration
- ? Module loading and compilation
- ? Entry point reflection (VS, PS, CS, GS, HS, DS)
- ? Type system reflection (structs, interfaces, generics)
- ? Function reflection (parameters, attributes, overloads)
- ? Variable and layout reflection (bindings, offsets, sizes)
- ? Attribute system (discovery, argument extraction)
- ? Memory leak detection using Windows CRT debug heap

**Key features**:
- Direct native API testing through SlangNative.dll
- Comprehensive error reporting with detailed diagnostics
- Cross-platform support (x64, ARM64)
- Automated dependency management (DLL copying)

## Build System

The test suite uses an automated build system that:

1. **Builds Dependencies**: Automatically builds SlangNative.vcxproj first
2. **Copies Native DLLs**: Ensures all required DLLs are in the test output directory
3. **Copies Test Data**: Moves .slang shader files to the test directory
4. **Validates Setup**: Verifies all dependencies are present before running tests

### Required DLLs (automatically copied)
- `SlangNative.dll` - Native wrapper library
- `slang.dll` - Core Slang compiler
- `slang-llvm.dll` - LLVM backend (x64 only)
- `gfx.dll` - Graphics abstraction layer
- `slang-glslang.dll` - GLSL integration
- `slang-glsl-module.dll` - GLSL module support  
- `slang-rt.dll` - Slang runtime

## Test Results

### Expected Output
```
===== Slang Reflection API Tests =====

============================================================
  INITIALIZING SLANG SESSION
============================================================
[PASS] Session Creation - 
[PASS] Read Shader File - Successfully read 2048 bytes
[PASS] Module Creation - 
[PASS] Program Creation - 
[PASS] Program Reflection - 

============================================================
  TESTING SHADER REFLECTION BASICS
============================================================
[PASS] Parameter Count - Found 3 parameters
[PASS] Entry Point Count - Found 7 entry points
[PASS] JSON Export - JSON exported successfully

... (additional test output) ...

============================================================
  TEST SUMMARY
============================================================
Total Tests: 127
Passed: 127
Failed: 0
Success Rate: 100.0%

? All tests passed successfully!
```

### Memory Leak Report
```
Memory Leak Detection Results:
- Start heap state: 2,048 allocations
- End heap state: 2,048 allocations  
- Net change: 0 allocations
? No memory leaks detected
```

## Troubleshooting

### Common Issues

**? "SlangNative.dll not found"**
```powershell
# Build SlangNative first
msbuild src\Native\SlangNative.vcxproj /p:Configuration=Debug /p:Platform=x64
```

**? "PowerShell execution policy"**
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

**? "Test executable crashes"**
- Ensure all DLLs are in bin\Debug\x64\ directory
- Check that executable and DLLs match the same platform (x64/ARM64)
- Verify Visual C++ Redistributable is installed

### Manual Build Steps
If automated build fails:
```powershell
# 1. Build native library
msbuild src\Native\SlangNative.vcxproj /p:Configuration=Debug /p:Platform=x64

# 2. Build test
msbuild Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj /p:Configuration=Debug /p:Platform=x64

# 3. Copy DLLs manually
Tests\AttributeMemoryLeakTest\build.ps1 -Configuration Debug -Platform x64

# 4. Run test
bin\Debug\x64\AttributeMemoryLeakTest.exe
```

## CI/CD Integration

### GitHub Actions
```yaml
- name: Run Slang.Net Tests
  run: Tests\run_tests.ps1 -Configuration Release -Platform x64
  shell: pwsh
```

### Command Line
```batch
rem For CI environments
Tests\run_tests.bat --config Release --platform x64
if %errorlevel% neq 0 exit /b %errorlevel%
```

## Contributing

To add new tests:

1. **Extend test shader**: Modify `ComprehensiveSlangTest.slang`
2. **Add test methods**: Create new test functions in `main.cpp`
3. **Update expectations**: Add new types/functions to expected lists
4. **Test thoroughly**: Ensure no memory leaks and all assertions pass

## Documentation

- **[Detailed Testing Guide](TESTING.md)** - Comprehensive build and testing instructions
- **[AttributeMemoryLeakTest README](AttributeMemoryLeakTest/README.md)** - Specific test documentation  
- **[Main Project README](../README.md)** - Slang.Net overview and samples

---

**Quick Links**:
- [????? Run Tests](#quick-start) 
- [?? Troubleshooting](#troubleshooting)
- [?? Detailed Guide](TESTING.md)
- [?? Test Details](AttributeMemoryLeakTest/README.md)