# Slang.Net Testing Guide

This guide provides comprehensive instructions for building and running the Slang.Net test suite, ensuring all native dependencies are properly configured.

## Test Projects Overview

### AttributeMemoryLeakTest
A comprehensive C++ test suite that validates the Slang reflection API and checks for memory leaks. This test directly exercises the native Slang API through the SlangNative.dll wrapper.

**Key Features:**
- Comprehensive reflection API testing
- Memory leak detection using Windows CRT debug heap
- Extensive shader compilation and introspection validation
- Cross-platform support (x64, ARM64)

## Prerequisites

Before running tests, ensure you have:

- **Windows 10/11** (x64 or ARM64)
- **Visual Studio 2022** with C++ development tools
- **.NET 9.0 SDK** or later
- **PowerShell 5.1** or later (for build scripts)

## Building and Running Tests

### Method 1: Visual Studio IDE (Recommended)

1. **Open Solution**:
   ```
   Open Slang.Net.sln in Visual Studio 2022
   ```

2. **Build Dependencies First**:
   - Right-click on `SlangNative` project ? Build
   - Wait for build to complete successfully

3. **Build Test Project**:
   - Right-click on `AttributeMemoryLeakTest` project ? Build
   - The post-build script will automatically copy required DLLs

4. **Run Tests**:
   - Right-click on `AttributeMemoryLeakTest` project ? Debug ? Start New Instance
   - Or use Ctrl+F5 to run without debugging

### Method 2: Command Line Build

1. **Build SlangNative First**:
   ```powershell
   # From the solution root directory
   msbuild src\Native\SlangNative.vcxproj /p:Configuration=Debug /p:Platform=x64
   ```

2. **Build and Run Test**:
   ```powershell
   # Build the test project
   msbuild Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj /p:Configuration=Debug /p:Platform=x64
   
   # Run the test
   .\bin\Debug\x64\AttributeMemoryLeakTest.exe
   ```

3. **Alternative: Use the build script directly**:
   ```powershell
   # Navigate to test directory
   cd Tests\AttributeMemoryLeakTest
   
   # Run build script to copy DLLs
   .\build.ps1 -Configuration Debug -Platform x64
   
   # Run the test
   .\..\..\bin\Debug\x64\AttributeMemoryLeakTest.exe
   ```

### Method 3: Automated Testing Script

Create a batch file for automated testing:

```batch
@echo off
echo ===== Slang.Net Test Suite =====

echo Building SlangNative...
msbuild src\Native\SlangNative.vcxproj /p:Configuration=Debug /p:Platform=x64 /nologo
if %errorlevel% neq 0 (
    echo SlangNative build failed!
    exit /b 1
)

echo Building AttributeMemoryLeakTest...
msbuild Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj /p:Configuration=Debug /p:Platform=x64 /nologo
if %errorlevel% neq 0 (
    echo AttributeMemoryLeakTest build failed!
    exit /b 1
)

echo Running tests...
bin\Debug\x64\AttributeMemoryLeakTest.exe
echo Test completed with exit code: %errorlevel%
```

## Platform-Specific Instructions

### x64 Platform
```powershell
# Build for x64
msbuild /p:Configuration=Debug /p:Platform=x64

# Required DLLs (automatically copied by build script):
# - SlangNative.dll
# - slang.dll
# - slang-llvm.dll (x64 only)
# - gfx.dll
# - slang-glslang.dll
# - slang-glsl-module.dll
# - slang-rt.dll
```

### ARM64 Platform
```powershell
# Build for ARM64
msbuild /p:Configuration=Debug /p:Platform=ARM64

# Required DLLs (automatically copied by build script):
# - SlangNative.dll
# - slang.dll
# - gfx.dll
# - slang-glslang.dll
# - slang-glsl-module.dll
# - slang-rt.dll
# Note: slang-llvm.dll is not available on ARM64
```

## Test Configuration

### Required Files Structure
```
bin\Debug\x64\                          # Output directory
??? AttributeMemoryLeakTest.exe          # Test executable
??? SlangNative.dll                      # Native wrapper DLL
??? slang.dll                            # Slang compiler DLL
??? slang-llvm.dll                       # LLVM backend (x64 only)
??? gfx.dll                              # Graphics abstraction DLL
??? slang-glslang.dll                    # GLSL integration DLL
??? slang-glsl-module.dll                # GLSL module support DLL
??? slang-rt.dll                         # Slang runtime DLL
??? ComprehensiveSlangTest.slang         # Test shader file
```

### Environment Variables
No special environment variables are required. The test uses relative paths to locate the test shader files.

## Test Data Requirements

### ComprehensiveSlangTest.slang
The test requires a comprehensive Slang shader file that includes:

```hlsl
// Example structure (auto-generated during build)
struct ComplexStruct { /* ... */ };
struct MyAttributeStruct { /* ... */ };

interface ILightModel { /* ... */ };
struct LambertianModel : ILightModel { /* ... */ };

[shader("vertex")]
float4 VS(/* ... */) : SV_Position { /* ... */ }

[shader("pixel")]
float4 PS(/* ... */) : SV_Target { /* ... */ }

[shader("compute")]
[numthreads(32, 32, 1)]
void CS(/* ... */) { /* ... */ }

// Additional shaders: GS, HS, DS
// Generic functions, attributes, etc.
```

## Troubleshooting

### Common Build Issues

1. **"SlangNative.dll not found"**:
   ```
   Solution: Ensure SlangNative project is built first
   Check: bin\Debug\x64\ contains SlangNative.dll
   ```

2. **"PowerShell execution policy error"**:
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

3. **"Missing slang.dll or related DLLs"**:
   ```
   Solution: Verify embedded LLVM binaries are present
   Check: src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\bin\
   ```

4. **"ComprehensiveSlangTest.slang not found"**:
   ```
   Solution: Ensure the .slang file is in the test output directory
   The build script should copy it automatically
   ```

### Runtime Issues

1. **Test crashes on startup**:
   - Check that all required DLLs are in the same directory as the executable
   - Verify the executable and DLLs are for the same platform (x64/ARM64)

2. **Memory leak detection false positives**:
   - Ensure you're running a Debug build for accurate leak detection
   - Some Slang internal allocations might appear as leaks - this is normal

3. **Reflection tests fail**:
   - Verify the test shader file contains the expected entry points and types
   - Check console output for specific API failures

### Manual DLL Copy (if build script fails)

```powershell
# Copy from Native output
copy src\Native\bin\Debug\x64\SlangNative.dll bin\Debug\x64\

# Copy from embedded Slang
copy src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\bin\*.dll bin\Debug\x64\

# Copy test data
copy Tests\AttributeMemoryLeakTest\ComprehensiveSlangTest.slang bin\Debug\x64\
```

## Integration with CI/CD

### GitHub Actions Example
```yaml
name: Run Slang.Net Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup MSBuild
      uses: microsoft/setup-msbuild@v1
    
    - name: Build SlangNative
      run: msbuild src\Native\SlangNative.vcxproj /p:Configuration=Release /p:Platform=x64
    
    - name: Build Tests
      run: msbuild Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj /p:Configuration=Release /p:Platform=x64
    
    - name: Run Tests
      run: bin\Release\x64\AttributeMemoryLeakTest.exe
```

### Azure DevOps Example
```yaml
pool:
  vmImage: 'windows-latest'

steps:
- task: MSBuild@1
  displayName: 'Build SlangNative'
  inputs:
    solution: 'src\Native\SlangNative.vcxproj'
    configuration: 'Release'
    platform: 'x64'

- task: MSBuild@1
  displayName: 'Build Tests'
  inputs:
    solution: 'Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj'
    configuration: 'Release'
    platform: 'x64'

- task: CmdLine@2
  displayName: 'Run Tests'
  inputs:
    script: 'bin\Release\x64\AttributeMemoryLeakTest.exe'
```

## Advanced Testing Scenarios

### Memory Leak Testing
The test includes comprehensive memory leak detection:

```cpp
// Automatic tracking in test
MemoryLeakDetector::Initialize();
MemoryLeakDetector::StartMonitoring();

// Run tests...

MemoryLeakDetector::StopMonitoring();
MemoryLeakDetector::ReportLeaks(); // Reports any leaks found
```

### Performance Testing
To run performance benchmarks:

1. Build in Release mode for accurate performance measurements
2. Run multiple iterations for statistical significance
3. Monitor memory usage over extended periods

### Cross-Platform Validation
Test on different architectures:

```powershell
# Test x64
msbuild /p:Platform=x64 && bin\Debug\x64\AttributeMemoryLeakTest.exe

# Test ARM64  
msbuild /p:Platform=ARM64 && bin\Debug\ARM64\AttributeMemoryLeakTest.exe
```

## Best Practices

1. **Always build dependencies first**: SlangNative ? Tests
2. **Use Debug builds for development**: Better debugging and leak detection
3. **Use Release builds for performance testing**: Optimized for speed
4. **Run tests after any API changes**: Ensure no regressions
5. **Check memory leaks regularly**: Part of quality assurance process

## Contributing Test Cases

To add new test cases to the reflection test suite:

1. **Extend the test shader**: Add new types, functions, or entry points to `ComprehensiveSlangTest.slang`
2. **Update expected data**: Modify the expected types/functions lists in `main.cpp`
3. **Add test methods**: Create new test methods in the `SlangReflectionTester` class
4. **Update documentation**: Document new test scenarios in the test README

This testing infrastructure ensures that all Slang.Net functionality is thoroughly validated and that native resources are properly managed throughout the development lifecycle.