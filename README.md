# Slang.Net

A .NET wrapper for the Slang Shader Language compiler, which includes the Compilation API, and Reflection API. providing both native C++ and managed C++/CLI interfaces for integrating Slang into .NET applications.

## Overview

This project consists of three main components:
- **SlangNative** - Pure native C++ wrapper library
- **Slang.Net** - Managed C++/CLI wrapper for .NET interoperability
- **Slang.Net.Test** - C# test project demonstrating usage

## Prerequisites

### Required Software
- **Visual Studio 2022** (or Visual Studio 2019 with C++/CLI support)
- **Windows SDK** (latest version recommended)
- **.NET Framework 4.7.2 or later** (for the test project)

### Required Dependencies
- **Slang SDK** - Must be installed at `C:\Slang\`
  - The project expects Slang headers at `C:\Slang\include\`
  - Slang libraries should be in the `lib/` directory of this project

### Platform Support
- ✅ **x64 (64-bit)**: Fully supported for Debug and Release configurations
- ❌ **x86 (32-bit)**: Not supported (Slang libraries are x64-only)

## Project Structure

```
Slang.Net/
├── README.md                 # This file
├── BUILD_STATUS.md          # Detailed build status and history
├── build.ps1               # PowerShell build script
├── verify-build.ps1        # Build verification script
├── Slang.Net.sln           # Main Visual Studio solution
├── lib/                    # Slang libraries (x64 only)
│   ├── slang.dll
│   ├── slang.lib
│   ├── gfx.dll
│   ├── gfx.lib
│   ├── slang-rt.dll
│   └── slang-rt.lib
├── Native/                 # Native C++ wrapper
│   ├── SlangNative.h
│   ├── SlangNative.cpp
│   ├── SlangNative.vcxproj
│   └── README.md
├── Slang.Net/             # Managed C++/CLI wrapper
├── Slang.Net.Test/        # C# test project
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
   MSBuild.exe "Slang.Net.sln" /p:Configuration=Debug /p:Platform=x64
   ```

3. **Build Release Configuration**
   ```cmd
   MSBuild.exe "Slang.Net.sln" /p:Configuration=Release /p:Platform=x64
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
└── Slang.Net.Test.dll     # Test project assembly
```

### Release Configuration
```
x64/Release/
├── SlangNative.dll         # Native C++ wrapper library (optimized)
├── SlangNative.lib         # Import library
├── Slang.Net.dll          # Managed C++/CLI wrapper (optimized)
└── Slang.Net.Test.dll     # Test project assembly (optimized)
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
.\Slang.Net.Test.exe
```

Or run it through Visual Studio by setting `Slang.Net.Test` as the startup project and pressing F5.

## Troubleshooting

### Common Issues

1. **"Slang SDK not found"**
   - Ensure Slang SDK is installed at `C:\Slang\`
   - Verify `C:\Slang\include\slang.h` exists

2. **"Cannot find slang.lib"**
   - Check that `lib/slang.lib` exists in the project directory
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

### Build Verification

To verify your build is working correctly:

1. Check that all DLL files are generated
2. Run the test project
3. Verify no missing dependency errors occur

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
