# Slang.Sdk

A comprehensive .NET wrapper for the Slang Shader Language compiler, providing seamless integration of shader compilation and reflection capabilities into .NET applications.

## Project Structure

This project follows a layered architecture approach:

### Layer 1 - Interop (`Layer1-Interop/`)

The foundational layer providing direct P/Invoke declarations and low-level interop with SlangNative.dll.

**Key Components:**

- **`SlangNativeInterop.cs`**: Complete P/Invoke declarations for all SlangNative.dll functions using LibraryImport
- **`SlangHandles.cs`**: Safe handles for managing native Slang object lifetimes with automatic cleanup
- **`SlangTypes.cs`**: Enums and constants for Slang types (SlangResult, CompileTarget, ShaderStage, etc.)
- **`StringMarshaling.cs`**: Utilities for marshaling strings between managed and native code
- **`SlangExceptions.cs`**: Exception types and error handling utilities
- **`InteropExample.cs`**: Example code demonstrating basic interop usage

**Features:**

- ? **Memory Safe**: Uses SafeHandle-derived classes for automatic resource cleanup
- ? **Modern P/Invoke**: Uses LibraryImport instead of legacy DllImport for better performance
- ? **String Handling**: Proper UTF-8 string marshaling with automatic memory management
- ? **Error Handling**: Structured exception handling with SlangResult integration
- ? **Type Safety**: Strongly typed enums and handles prevent common interop errors

### Layer 2 - Binding (`Layer2-Binding/`)

*Coming Soon* - This layer will provide object-oriented wrappers around the interop layer.

### Layer 3 - Pretty (`Layer3-Pretty/`)

*Coming Soon* - This layer will provide high-level, idiomatic .NET APIs for shader compilation.

## Native Dependencies

The project automatically includes the required native dependencies:

- **SlangNative.dll**: The main interop library
- **slang.dll**: Core Slang compiler
- **gfx.dll**: Graphics abstraction layer
- **slang-glslang.dll**: GLSL compiler support
- **slang-llvm.dll**: LLVM backend support
- **slang-rt.dll**: Runtime support

These are automatically copied to the output directory and included in NuGet packages.

## Usage Examples

### Basic Session Creation

```csharp
using Slang.Sdk.Interop;

// Create a Slang session
var sessionHandle = SlangNativeInterop.CreateSession(
    null, 0,    // No compiler options
    null, 0,    // No preprocessor macros  
    null, 0,    // No shader models
    null, 0     // No search paths
);

if (sessionHandle == IntPtr.Zero)
{
    var errorPtr = SlangNativeInterop.SlangNative_GetLastError();
    var errorMessage = StringMarshaling.FromUtf8(errorPtr);
    throw new InvalidOperationException($"Session creation failed: {errorMessage}");
}
```

### Safe Resource Management

```csharp
using var session = new SlangSessionHandle(sessionPtr);
using var reflection = SlangReflectionHandle.ForShaderReflection(reflectionPtr);

// Resources are automatically disposed when using blocks exit
```

### Error Handling

```csharp
try
{
    var result = someSlangOperation();
    SlangResultHelper.ThrowOnFailure(result, "Operation failed");
}
catch (SlangException ex)
{
    Console.WriteLine($"Slang error: {ex.Message} (Code: {ex.Result})");
}
```

### String Marshaling

```csharp
// Convert managed string to native UTF-8
var nativeString = StringMarshaling.ToUtf8("Hello, Slang!");

// Use with native function
var result = SlangNativeInterop.SomeFunction(nativeString);

// Clean up
StringMarshaling.FreeUtf8(nativeString);
```

## Platform Support

- **Windows x64** ? Fully supported
- **Windows ARM64** ? Fully supported  
- **Windows x86** ? Not supported (Slang limitation)
- **Linux** ?? Planned
- **macOS** ?? Planned

## Building

```bash
dotnet build src/Slang.Sdk/Slang.Sdk.csproj
```

The project requires:
- .NET 9.0 or later
- Unsafe code support (already configured)
- SlangNative.dll and dependencies (automatically included)

## API Coverage

The interop layer provides complete coverage of SlangNative.h exports:

### Session Management
- ? CreateSession
- ? SlangNative_GetLastError

### Module Operations  
- ? CreateModule
- ? FindEntryPoint
- ? GetParameterInfo

### Program Compilation
- ? CreateProgram  
- ? Compile
- ? GetProgramReflection

### Reflection APIs
- ? ShaderReflection (20+ functions)
- ? TypeReflection (20+ functions)
- ? TypeLayoutReflection (20+ functions) 
- ? VariableReflection (15+ functions)
- ? VariableLayoutReflection (15+ functions)
- ? FunctionReflection (15+ functions)
- ? EntryPointReflection (15+ functions)
- ? GenericReflection (15+ functions)
- ? TypeParameterReflection (5+ functions)
- ? Attribute (8+ functions)
- ? Modifier (4+ functions)

**Total: 150+ native functions wrapped**

## Future Roadmap

### Layer 2 - Binding (Next)
- Object-oriented wrappers for reflection types
- Automatic handle management
- LINQ-style query APIs for reflection
- Type-safe parameter binding

### Layer 3 - Pretty (Future)
- High-level SessionBuilder pattern
- Fluent configuration APIs
- Automatic target detection
- Built-in error diagnostics
- Integration helpers for common graphics APIs

## Contributing

This interop layer provides the foundation for all higher-level functionality. When contributing:

1. **Maintain API Coverage**: All SlangNative.h exports should be wrapped
2. **Memory Safety**: Always use SafeHandle classes for native resources  
3. **Error Handling**: Use SlangResult and SlangException consistently
4. **Documentation**: Document all public APIs with XML comments
5. **Testing**: Include examples demonstrating new functionality

## License

Apache-2.0 WITH LLVM-exception (same as Slang)