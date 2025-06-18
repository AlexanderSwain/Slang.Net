# Slang.Net Class Library

A .NET class library wrapper for the Slang Shader Language compiler, providing multi-platform support and comprehensive APIs for shader compilation and reflection.

## Overview

This project provides a complete .NET class library for integrating Slang shader compilation into .NET applications. The project consists of multiple layers:

- **Slang.Net** - Main C# class library (multi-platform: x86, x64, ARM64)
- **Slang.Net.CPP** - C++/CLI wrapper providing managed interoperability
- **SlangNative** - Native C++ wrapper for direct Slang API access
- **Sample Projects** - Demonstration applications showing library usage

## Features

- ✅ **Class Library**: Ready for integration into other .NET applications
- ✅ **Multi-Platform**: Supports AnyCPU, x86, x64, and ARM64 architectures
- ✅ **NuGet Ready**: Package metadata included for distribution
- ✅ **Automatic Dependencies**: Post-build targets copy all required DLLs
- ✅ **Comprehensive API**: Full access to Slang compilation and reflection APIs

## Prerequisites

### Required Software
- **Visual Studio 2022** (Community, Professional, or Enterprise)
- **OR Visual Studio Build Tools 2022**
- **.NET 9.0 SDK**
- **Windows SDK** (latest version)

### Included Dependencies
- **Slang SDK** - Embedded LLVM version included
  - Headers: `Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\include\`
  - Libraries: `Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\lib\`
  - Runtime DLLs: `Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\bin\`

### Platform Support
- ✅ **AnyCPU**: Supports any CPU architecture
- ✅ **x86 (32-bit)**: Full support with proper dependencies
- ✅ **x64 (64-bit)**: Full support (primary development platform)
- ✅ **ARM64**: Full support for ARM64 systems

## Project Structure

```
Slang.Net/
├── README.md                 # This file
├── BUILD_STATUS.md          # Detailed build status and history
├── build.ps1               # PowerShell build script
├── verify-build.ps1        # Build verification script
├── Slang.Net.sln           # Main Visual Studio solution
├── Native/                 # Native C++ wrapper
│   ├── SlangNative.h
│   ├── SlangNative.cpp
│   ├── SlangNative.vcxproj
│   ├── README.md
│   └── EmbeddedLLVM/       # Embedded Slang SDK
│       └── slang-2025.10.3-windows-x86_64/
│           ├── bin/        # Slang DLLs
│           ├── include/    # Slang headers
│           └── lib/        # Slang libraries
├── Slang.Net/             # Managed C++/CLI wrapper
├── Slang.Net/        # C# test project
└── Shaders/               # Sample shader files
    └── test.slang
```

## Building the Project

### Method 1: Using Visual Studio (Recommended)

1. **Open the Solution**
   ```
   Open Slang.Net.sln in Visual Studio 2022
   ```

2. **Select Platform**
   - Set the solution platform to **x64** (required)
   - Choose either **Debug** or **Release** configuration

## Quick Start

### Building the Class Library

1. **Open Solution**
   ```powershell
   # Navigate to project directory
   cd "C:\Users\[YourUsername]\Code\Playground\Slang.Net"
   
   # Open in Visual Studio
   devenv Slang.Net.sln
   ```

2. **Build All Projects**
   ```powershell
   # Build Debug configuration
   msbuild Slang.Net.sln -p:Configuration=Debug -p:Platform="Any CPU"
   
   # Build Release configuration  
   msbuild Slang.Net.sln -p:Configuration=Release -p:Platform="Any CPU"
   ```

3. **Run Sample Application**
   ```powershell
   # Build and run the sample (dependencies copied automatically)
   msbuild "Samples\Slang.Net.Samples.SimpleCompileTest\Slang.Net.Samples.SimpleCompileTest.csproj" -p:Configuration=Debug
   
   # Execute the sample
   cd "Samples\Slang.Net.Samples.SimpleCompileTest\bin\Debug\net9.0"
   .\Slang.Net.Samples.SimpleCompileTest.exe
   ```

### Using in Your Project

1. **Add Project Reference**
   ```xml
   <ItemGroup>
     <ProjectReference Include="path\to\Slang.Net\Slang.Net.csproj" />
   </ItemGroup>
   ```

2. **Basic Usage**
   ```csharp
   using System;
   
   // Create a session builder
   var sessionBuilder = new SessionBuilder();
   sessionBuilder.AddSearchPath(@"C:\MyShaders");
   
   // Build session and load module
   var session = sessionBuilder.Build();
   var module = session.LoadModule("MyShader.slang");
   ```

## Build Methods

### Method 1: Visual Studio IDE

1. **Open Solution**
   - Launch Visual Studio 2022
   - File → Open → Project/Solution
   - Select `Slang.Net.sln`

2. **Select Configuration**
   - Choose Debug or Release from the toolbar
   - Choose platform (Any CPU recommended)

3. **Build**
   - Right-click the solution in Solution Explorer
   - Select "Build Solution" or press `Ctrl+Shift+B`

### Method 2: Using MSBuild Command Line

1. **Open Developer Command Prompt**
   - Start "Developer Command Prompt for VS 2022"
   - Navigate to the project directory:
   ```cmd
   cd "C:\Users\[YourUsername]\Code\Playground\Slang.Net"
   ```

2. **Build Debug Configuration**
   ```cmd
   MSBuild.exe "Slang.Net.sln" /p:Configuration=Debug /p:Platform="Any CPU"
   ```

3. **Build Release Configuration**
   ```cmd
   MSBuild.exe "Slang.Net.sln" /p:Configuration=Release /p:Platform="Any CPU"
   ```

### Method 3: Using PowerShell Build Script (Easiest)

A PowerShell build script is provided for convenience:

```powershell
# Build Debug configuration
.\build.ps1

# Build Release configuration
.\build.ps1 -Configuration Release

# Clean and build with verbose output
.\build.ps1 -Clean -Verbose
```

### Method 4: Using PowerShell MSBuild Directly

Open PowerShell in the project directory and run:

```powershell
# For Debug build
& "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Slang.Net.sln" /p:Configuration=Debug /p:Platform=x64

# For Release build
& "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Slang.Net.sln" /p:Configuration=Release /p:Platform=x64
```

## Build Output

After a successful build, the following files will be generated:

### Debug Configuration
```
x64/Debug/
├── SlangNative.dll         # Native C++ wrapper library
├── SlangNative.lib         # Import library
├── SlangNative.pdb         # Debug symbols
├── Slang.Net.dll          # Managed C++/CLI wrapper
├── Slang.Net.pdb          # Debug symbols
└── Slang.Net.dll     # Test project assembly
```

### Release Configuration
```
x64/Release/
├── SlangNative.dll         # Native C++ wrapper library (optimized)
├── SlangNative.lib         # Import library
├── Slang.Net.dll          # Managed C++/CLI wrapper (optimized)
└── Slang.Net.dll     # Test project assembly (optimized)
```

## Testing the Build

After building, you can verify the build using the provided verification script:

```powershell
# Verify Debug build
.\verify-build.ps1

# Verify Release build  
.\verify-build.ps1 -Configuration Release
```

You can also run the test project manually to verify everything works:

```cmd
cd x64\Debug
.\Slang.Net.exe
```

Or run it through Visual Studio by setting `Slang.Net` as the startup project and pressing F5.

## Troubleshooting

### Common Issues

1. **"Slang SDK not found"**
   - The project uses an embedded Slang SDK in `Native\EmbeddedLLVM\slang-2025.6.3-windows-x86_64\`
   - Verify the embedded headers exist at `Native\EmbeddedLLVM\slang-2025.6.3-windows-x86_64\include\slang.h`

2. **"Cannot find slang.lib"**
   - Check that libraries exist in `Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\lib\`
   - Verify the library is for x64 architecture

3. **"Platform 'Win32' not supported"**
   - This is expected - only x64 is supported
   - Ensure you're building with Platform=x64

4. **Linker errors about missing symbols**
   - ✅ **Resolved** - All linker errors (LNK2019) have been fixed in the current version
   - Verify all `.cpp` files are included in their respective projects
   - Check that `Modifier.cpp` is properly included in the SlangNative project
   - If you encounter new linker errors, ensure any new `.cpp` files are added to the project

5. **"ThrowNotImplemented" exceptions at runtime**
   - ✅ **Resolved** - All unimplemented methods have been completed
   - The current version has full API coverage with no remaining stubs

## Updating the Slang Compiler Version

This section provides comprehensive step-by-step instructions for updating the project to use a newer version of the Slang compiler. The process involves updating file paths, building, and testing. These instructions assume you want to update from the current version **2025.10.3** to a newer version.

### Overview

The project currently uses Slang compiler version **2025.10.3**. To update to a newer version, you need to:
1. Download and extract the new Slang SDK (✅ you've already done this!)
2. Update project configuration files 
3. Update documentation and scripts
4. Build and test the project

### Step-by-Step Update Process

#### Step 1: Download and Extract New Slang SDK ✅ (Already Completed)

You've already completed this step by adding `slang-2025.10.3-windows-x86_64` to the `EmbeddedLLVM` directory. When updating to a future version, you would:

1. **Download a newer Slang release**:
   - Go to the [Slang releases page](https://github.com/shader-slang/slang/releases)
   - Download the Windows x64 version (e.g., `slang-2025.11.0-windows-x86_64.zip`)

2. **Extract to the EmbeddedLLVM directory**:
   ```powershell
   # Navigate to the project directory
   cd "c:\Users\[YourUsername]\Code\Playground\Slang.Net"
   
   # Extract the downloaded ZIP file to Native\EmbeddedLLVM\
   # This should create a folder like: Native\EmbeddedLLVM\slang-2025.11.0-windows-x86_64\
   ```

3. **Verify the extracted folder structure**:
   ```
   Native\EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\
   ├── bin\            # Contains DLL files (slang.dll, gfx.dll, etc.)
   ├── include\        # Contains header files (slang.h, etc.)
   ├── lib\            # Contains library files (slang.lib, gfx.lib, etc.)
   └── ...
   ```

#### Step 2: Update Project Configuration Files

**Update the Native C++ Project** (`Native\SlangNative.vcxproj`):

⚠️ **Important**: You need to update this file in **3 specific locations**. Use Find & Replace in your text editor to make this easier.

1. **Find and replace the include paths** (appears **twice** - once for Debug, once for Release):
   
   **Find this:**
   ```xml
   <AdditionalIncludeDirectories>$(ProjectDir)\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\include;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
   ```
   
   **Replace with:** (using your new version number)
   ```xml
   <AdditionalIncludeDirectories>$(ProjectDir)\EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\include;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
   ```

2. **Find and replace the library paths** (appears **twice** - once for Debug, once for Release):
   
   **Find this:**
   ```xml
   <AdditionalLibraryDirectories>$(ProjectDir)EmbeddedLLVM\slang-2025.10.3-windows-x86_64\lib;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
   ```
   
   **Replace with:**
   ```xml
   <AdditionalLibraryDirectories>$(ProjectDir)EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\lib;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
   ```

3. **Find and replace the post-build copy command** (appears **once**):
   
   **Find this:**
   ```xml
   <Command>copy "$(ProjectDir)EmbeddedLLVM\slang-2025.10.3-windows-x86_64\bin\*.dll" "$(TargetDir)"</Command>
   ```
   
   **Replace with:**
   ```xml
   <Command>copy "$(ProjectDir)EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\bin\*.dll" "$(TargetDir)"</Command>
   ```

#### Step 3: Update Scripts and Documentation

**Update the verification script** (`verify-build.ps1`):

1. **Find and replace the library and binary paths** (2 lines to update):
   
   **Find these lines:**
   ```powershell
   $libDir = "Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\lib"
   $binDir = "Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64\bin"
   ```
   
   **Replace with:**
   ```powershell
   $libDir = "Native\EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\lib"
   $binDir = "Native\EmbeddedLLVM\slang-YYYY.MM.V-windows-x86_64\bin"
   ```

**Update documentation files**:

1. **Update `README.md`** (this file):
   - Update the version number in the "Required Dependencies" section
   - Update the "Overview" section in "Updating the Slang Compiler Version"
   - Update any example paths that reference the old version

2. **Update `BUILD_STATUS.md`**:
   - Update the Slang SDK path reference (search for the old version number)

#### Step 4: Build and Test

1. **Clean and build the project**:
   ```powershell
   # Using the build script (easiest method)
   .\build.ps1 -Configuration Debug -Clean
   
   # OR using MSBuild directly (replace path with your VS version)
   & "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Slang.Net.sln" /p:Configuration=Debug /p:Platform=x64 /t:Clean
   & "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Slang.Net.sln" /p:Configuration=Debug /p:Platform=x64
   ```

2. **Verify the build completed successfully**:
   ```powershell
   # Check that all required files are generated
   Get-ChildItem "Slang.Net\bin\Debug\net9.0\*.dll"
   
   # Should show files like:
   # - SlangNative.dll
   # - Slang.Net.dll  
   # - slang.dll, gfx.dll, slang-rt.dll (from the new Slang version)
   ```

3. **Test the application runs correctly**:   ```powershell
   # Run the test application
   Set-Location "Slang.Net\bin\Debug\net9.0"
   .\Slang.Net.exe
   
   # Should run without errors and show test output
   ```

4. **Run the verification script**:
   ```powershell
   # Go back to project root
   Set-Location "c:\Users\[YourUsername]\Code\Playground\Slang.Net"
   
   # Run verification
   .\verify-build.ps1
   
   # Should show "All checks passed!"
   ```

#### Step 5: Optional Cleanup

**Remove the old Slang version** (only after confirming everything works perfectly):
```powershell
# Delete the old directory
Remove-Item "Native\EmbeddedLLVM\slang-2025.10.3-windows-x86_64" -Recurse -Force
```

### Update Checklist

Use this checklist to ensure you don't miss any steps:

**Preparation:**
- [ ] ✅ **Step 1**: Downloaded and extracted new Slang SDK to `Native\EmbeddedLLVM\` (already done)

**File Updates:**
- [ ] **Step 2**: Updated `Native\SlangNative.vcxproj`:
  - [ ] Updated include paths (2 locations: Debug and Release configurations)
  - [ ] Updated library paths (2 locations: Debug and Release configurations) 
  - [ ] Updated post-build copy command (1 location)
- [ ] **Step 3**: Updated `verify-build.ps1`:
  - [ ] Updated `$libDir` path
  - [ ] Updated `$binDir` path
- [ ] **Step 3**: Updated documentation:
  - [ ] Updated version number in `README.md` overview and dependencies section
  - [ ] Updated `BUILD_STATUS.md` if needed

**Testing:**
- [ ] **Step 4**: Successfully built project:
  - [ ] Debug build completed without errors
  - [ ] All required DLLs generated in output directory
  - [ ] Test application runs without errors
  - [ ] Verification script passes
- [ ] **Step 5**: Cleaned up old version (optional)

### Troubleshooting

**Common issues and solutions**:

1. **Build fails with "file not found" errors**:
   - ❌ **Problem**: New Slang SDK not extracted correctly or file paths not updated
   - ✅ **Solution**: 
     - Verify the new Slang SDK folder exists in `Native\EmbeddedLLVM\`
     - Double-check that all file paths in `SlangNative.vcxproj` were updated correctly
     - Ensure you updated all 5 path references (not just some of them)

2. **Linker errors about missing libraries (LNK1104)**:
   - ❌ **Problem**: Library paths incorrect or new Slang version missing required files
   - ✅ **Solution**:
     - Verify the new Slang SDK contains all required `.lib` files in the `lib\` folder
     - Check that library paths in `SlangNative.vcxproj` point to the correct new directory
     - Ensure the new Slang version is compatible with your project

3. **Runtime errors when running the test (DLL not found)**:
   - ❌ **Problem**: Slang DLLs not copied to output directory or wrong versions
   - ✅ **Solution**:
     - Check that the post-build copy command was updated in `SlangNative.vcxproj`
     - Verify all Slang DLLs exist in `Slang.Net\bin\Debug\net9.0\`
     - Ensure DLL versions match the library versions you're linking against

4. **Application crashes on startup**:
   - ❌ **Problem**: Incompatible Slang version or API breaking changes
   - ✅ **Solution**:
     - Check the [Slang release notes](https://github.com/shader-slang/slang/releases) for breaking changes
     - Try building in Release mode instead of Debug
     - Consider reverting to the previous version if issues persist
     - Check that your project code is compatible with the new Slang API version

5. **Verification script fails**:
   - ❌ **Problem**: Path references in script not updated
   - ✅ **Solution**:
     - Ensure you updated both `$libDir` and `$binDir` variables in `verify-build.ps1`
     - Check that the paths point to existing directories
     - Run the script with `-Verbose` flag for more detailed output

## Usage Examples

### C++ Native Usage
```cpp
#include "SlangNative.h"

// Create a global session
auto session = CreateGlobalSession();
// Use the session...
ReleaseGlobalSession(session);
```

### C# Managed Usage
```csharp
using Slang.Net;

// Create session through managed wrapper
var session = new Session();
// Use the session...
session.Dispose();
```

## Contributing

When contributing to this project:

1. Ensure your changes build on x64 platform
2. Update tests if you modify public APIs
3. Keep native and managed wrappers in sync
4. Follow the existing code style and patterns

## Development History

### Recent Implementation Work
This project recently underwent a major implementation effort where all previously unimplemented methods were completed:

**Files Modified:**
- `Native/SlangNative.cpp` - All `ThrowNotImplemented` stubs replaced with working implementations
- `Native/Modifier.h` - Updated to store native pointer and declare interface methods
- `Native/Modifier.cpp` - Created new file with `getID()` and `getName()` implementations
- `Native/SlangNative.vcxproj` - Updated to include new source files

**Methods Implemented:**
- `GenericReflection_*` - 9 methods for generic type reflection
- `EntryPointReflection_*` - 5 methods for shader entry point reflection  
- `Modifier_*` - 2 methods for modifier information

All implementations properly call into the underlying Slang native API and follow the established error handling patterns.

## Recent Updates

- ✅ **All previously unimplemented methods have been implemented** - All `ThrowNotImplemented` stubs in `SlangNative.cpp` have been replaced with working implementations
- ✅ **Project builds successfully** - All linker errors have been resolved and the project compiles cleanly
- ✅ **Comprehensive API coverage** - Full implementation of GenericReflection, EntryPointReflection, and Modifier APIs

## Implementation Status

### Completed APIs
- **GenericReflection**: All 9 methods implemented (`ApplySpecializations`, `GetConcreteIntVal`, `GetConcreteType`, etc.)
- **EntryPointReflection**: All 5 methods implemented (`HasDefaultConstantBuffer`, `GetResultVarLayout`, etc.)
- **Modifier**: Both methods implemented (`GetID`, `GetName`)

### Notes

- This project was successfully built and tested on Windows with Visual Studio 2022
- All previously unimplemented methods in `SlangNative.cpp` have been implemented as of the latest version
- The Slang SDK path is currently hardcoded to `C:\Slang\` - this may be made configurable in future versions
- Only x64 builds are supported due to Slang library limitations
- The project includes proper error handling and follows the established API patterns

## License

[Add your license information here]
