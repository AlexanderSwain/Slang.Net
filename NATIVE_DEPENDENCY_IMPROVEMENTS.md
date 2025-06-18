# Native Dependency Copy and Packaging Improvements

## Overview
This document outlines the improvements made to ensure the correct native DLLs are copied to the right platform-specific directories and properly packaged into the NuGet package.

## Changes Made

### 1. Enhanced .csproj Configuration (`Slang.Net\Slang.Net.csproj`)

**Platform-Specific Robustness:**
- Added `Condition="Exists(...)"` to all Content entries in x64 and ARM64 ItemGroups
- This prevents build errors when expected files are missing
- Ensures only actually present files are included in the package

**ARM64 Corrections:**
- Removed `slang-llvm.dll` reference for ARM64 (this file doesn't exist for ARM64 platform)
- Added consistent file existence checks

**Validation Target:**
- Added `ValidateNativeDependencies` target that runs before build
- Checks for required core DLLs: `slang.dll`, `slang-rt.dll`, `SlangNative.dll`, `Slang.Net.CPP.dll`
- Includes platform-specific checks (e.g., `slang-llvm.dll` only for x64)
- Provides clear error messages if required dependencies are missing

### 2. Improved Copy Logic in C++ Projects

**SlangNative.vcxproj:**
- Enhanced post-build event with better error handling
- Added directory creation logic: `if not exist ... mkdir ...`
- Added informative echo messages for debugging
- Properly escaped XML characters in MSBuild commands

**Slang.Net.CPP.vcxproj:**
- Fixed library dependency paths to reference correct SlangNative.lib location
- Enhanced post-build event with conditional file existence checks
- Added better error reporting for failed copy operations
- Added informative echo messages

### 3. Build Process Improvements

**Error Handling:**
- Copy operations now include error handling and warnings
- Missing files are reported but don't fail the build
- Verbose logging helps identify copy issues

**Directory Management:**
- Automatic creation of target directories if they don't exist
- Consistent use of `$(Platform)` variable for platform-specific paths

## File Organization

The enhanced system ensures files are organized as follows:

```
Slang.Net/
├── lib/
│   ├── x64/
│   │   ├── gfx.dll
│   │   ├── slang-glsl-module.dll
│   │   ├── slang-glslang.dll
│   │   ├── slang-llvm.dll        (x64 only)
│   │   ├── slang-rt.dll
│   │   ├── slang.dll
│   │   ├── Slang.Net.CPP.dll
│   │   ├── Slang.Net.CPP.pdb
│   │   └── SlangNative.dll
│   └── ARM64/
│       ├── gfx.dll
│       ├── slang-glsl-module.dll
│       ├── slang-glslang.dll
│       ├── slang-rt.dll
│       ├── slang.dll
│       ├── Slang.Net.CPP.dll
│       ├── Slang.Net.CPP.pdb
│       └── SlangNative.dll
└── bin/
    ├── x64/Debug/net9.0/
    │   ├── [main assemblies]
    │   └── lib/x64/              (copies of native DLLs)
    └── ARM64/Debug/net9.0/
        ├── [main assemblies]
        └── lib/ARM64/            (copies of native DLLs)
```

## NuGet Package Structure

The NuGet package correctly includes runtime-specific native dependencies:

```
Slang.Net.0.0.1.nupkg
├── lib/net9.0/
│   └── Slang.Net.dll
├── runtimes/
│   ├── win-x64/native/
│   │   ├── gfx.dll
│   │   ├── slang-glsl-module.dll
│   │   ├── slang-glslang.dll
│   │   ├── slang-llvm.dll
│   │   ├── slang-rt.dll
│   │   ├── slang.dll
│   │   ├── Slang.Net.CPP.dll
│   │   ├── Slang.Net.CPP.pdb
│   │   └── SlangNative.dll
│   └── win-arm64/native/
│       ├── gfx.dll
│       ├── slang-glsl-module.dll
│       ├── slang-glslang.dll
│       ├── slang-rt.dll
│       ├── slang.dll
│       ├── Slang.Net.CPP.dll
│       ├── Slang.Net.CPP.pdb
│       └── SlangNative.dll
├── README.md
└── slang.net icon.png
```

## Validation and Testing

### Build Validation
- `ValidateNativeDependencies` target ensures required files are present before build
- Platform-specific validation (different requirements for x64 vs ARM64)
- Clear error messages help identify missing dependencies

### Manual Testing
Both x64 and ARM64 builds have been tested and verified to:
1. Build successfully with all dependencies
2. Copy files to correct directories
3. Package files correctly in NuGet package
4. Include only existing files (no broken references)

## Benefits

1. **Robustness**: Build won't fail due to missing optional files
2. **Correctness**: Each platform gets only its appropriate native dependencies
3. **Debugging**: Clear logging helps identify copy/dependency issues
4. **Validation**: Early detection of missing required dependencies
5. **Packaging**: Proper runtime-specific native dependency inclusion in NuGet packages

## Usage

To build for a specific platform:
```bash
# x64 build
msbuild Slang.Net\Slang.Net.csproj /p:Configuration=Debug /p:Platform=x64

# ARM64 build
msbuild Slang.Net\Slang.Net.csproj /p:Configuration=Debug /p:Platform=ARM64
```

The system automatically:
- Validates required dependencies
- Copies platform-specific native files
- Creates properly structured NuGet packages
- Handles missing optional files gracefully
