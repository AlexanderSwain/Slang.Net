# SlangNative

This is a native C++ wrapper library for the Slang shader compiler. Unlike the parent Slang.Net project which uses C++/CLI for .NET interoperability, this project provides a pure native C++ interface.

## Project Structure

- `SlangNative.h` - Main header file with API declarations
- `SlangNative.cpp` - Implementation of the native C++ wrapper functions
- `SlangNative.vcxproj` - Visual Studio project file
- `SlangNative.sln` - Visual Studio solution file

## Building

1. Ensure you have the Slang SDK installed at `C:\Slang\`
2. Open `SlangNative.sln` in Visual Studio
3. Build the solution for x64 platform

## API

The library provides a simple C-style API for creating and managing Slang global sessions:

- `GetVersion()` - Returns the library version
- `CreateGlobalSession()` - Creates a new Slang global session
- `ReleaseGlobalSession()` - Releases a Slang global session

## Usage

This native library can be used directly from C++ applications or as a foundation for other language bindings.
