# Slang.Net Class Library

Slang.Net has been successfully converted from an executable to a class library that supports multiple architectures (x86, x64, and ARM64).

## Features

- **Multi-Architecture Support**: Supports x86, x64, and ARM64 platforms
- **Class Library**: Can be referenced by other .NET applications
- **NuGet Package**: Automatically generates NuGet packages during build
- **.NET 9.0 Target**: Uses the latest .NET framework with preview features enabled

## Building the Library

### Prerequisites

- Visual Studio 2022 (or MSBuild tools)
- .NET 9.0 SDK

### Build Instructions

To build the class library for all supported architectures:

```powershell
# Build for Any CPU (default)
msbuild Slang.Net.sln /p:Configuration=Release /p:Platform="Any CPU"

# Build for specific architectures
msbuild Slang.Net.sln /p:Configuration=Release /p:Platform=x64
msbuild Slang.Net.sln /p:Configuration=Release /p:Platform=x86
msbuild Slang.Net.sln /p:Configuration=Release /p:Platform=ARM64
```

### Creating NuGet Package

To create a NuGet package:

```powershell
msbuild Slang.Net/Slang.Net.csproj /p:Configuration=Release /t:Pack
```

The NuGet package will be created at: `Slang.Net/bin/Release/Slang.Net.1.0.0.nupkg`

## Using the Library

### Installation

1. **From Local Package**: Install the generated `.nupkg` file
2. **Project Reference**: Add a project reference to `Slang.Net.csproj`

### Example Usage

```csharp
using Slang;

// Create a Slang session
var session = new Session();

// Load and compile shaders
// ... (use existing Slang.Net API)
```

## Architecture Support

| Architecture | Status | Notes |
|-------------|--------|-------|
| x64 | ✅ Fully Supported | Native Slang binaries available |
| x86 | ⚠️ Limited | May fall back to x64 native binaries |
| ARM64 | ⚠️ Limited | May fall back to x64 native binaries |

**Note**: While the .NET assembly supports all three architectures, the underlying Slang native libraries are currently only distributed for x64 Windows. The native library dependencies may limit runtime support on x86 and ARM64 platforms.

## Project Structure

- `Slang.Net/` - Main .NET class library project
- `Slang.Net.CPP/` - C++/CLI wrapper project  
- `Native/` - Native C++ Slang bindings
- `Native/EmbeddedLLVM/` - Slang compiler binaries

## Build Requirements

The project requires MSBuild (not just dotnet CLI) because of the C++/CLI dependencies. The C++/CLI project (`Slang.Net.CPP`) bridges the native Slang library with the managed .NET code.

## Package Metadata

- **Package ID**: Slang.Net
- **Version**: 1.0.0  
- **Target Framework**: .NET 9.0
- **License**: MIT
- **Tags**: slang, shader, graphics, hlsl, glsl, cross-platform

## Changes Made

1. **Removed executable output**: Changed from `OutputType=Exe` to class library
2. **Added multi-architecture support**: Added x86, x64, ARM64 platform configurations
3. **Updated project references**: All projects now support multiple architectures  
4. **Added NuGet packaging**: Configured automatic package generation
5. **Updated solution configuration**: Added platform mappings for all architectures

The conversion maintains backward compatibility while enabling the library to be consumed by other applications.
