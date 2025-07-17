# Comprehensive Slang Reflection API Test

This test suite provides comprehensive testing of the Slang reflection API, designed to test all major reflection features including:

## Features Tested

### Core Reflection API
- **Session Management**: Creating and configuring Slang sessions
- **Module Loading**: Loading Slang shader modules from source
- **Program Creation**: Creating shader programs from modules
- **Reflection Extraction**: Getting reflection data from compiled programs

### Shader Reflection Tests
- Parameter enumeration and inspection
- Type parameter analysis
- Entry point discovery and properties
- Global constant buffer information
- Hashed string management
- JSON export functionality

### Entry Point Reflection Tests
- Entry point enumeration by index and name
- Stage identification (vertex, pixel, compute, etc.)
- Parameter counting and inspection
- Compute shader specific properties:
  - Thread group size analysis
  - Wave size detection
- Function reflection access
- Layout information retrieval
- Default constant buffer detection

### Type Reflection Tests
- Type discovery by name
- Type kind identification
- Field enumeration and inspection
- Array type analysis:
  - Element type extraction
  - Element count determination
- Matrix properties (rows/columns)
- User attribute analysis
- Generic container inspection

### Function Reflection Tests
- Function discovery by name
- Parameter enumeration
- Return type analysis
- User attribute inspection
- Overload detection and counting
- Generic container access

### Variable Reflection Tests
- Global parameter enumeration
- Variable layout information:
  - Binding indices and spaces
  - Memory offsets
  - Type layout details
- Size and alignment analysis

### Attribute Reflection Tests
- Attribute discovery on types and functions
- Argument counting and value extraction:
  - String arguments
  - Integer arguments
  - Float arguments
- Named attribute lookup

### Generic Reflection Tests
- Generic type container analysis
- Type parameter enumeration
- Value parameter inspection
- Constraint analysis

### Layout Reflection Tests
- Type layout analysis:
  - Size calculation
  - Stride determination
  - Alignment requirements
- Field layout inspection:
  - Field offsets
  - Field naming
- Memory layout validation

### Compilation Tests
- Multi-target compilation testing
- Entry point specific compilation
- Error detection and reporting

## Test Data Requirements

The test expects a comprehensive Slang shader file named `ComprehensiveSlangTest.slang` that should include:

### Expected Entry Points
- `VS` - Vertex shader
- `PS` - Pixel shader  
- `GS` - Geometry shader
- `CS` - Compute shader
- `HS` - Hull shader
- `DS` - Domain shader
- `TestAttributeFunction` - Function with attributes

### Expected Types
- `ComplexStruct` - Complex structure type
- `NestedStruct` - Nested structure
- `MyAttributeStruct` - Structure with attributes
- `GenericBuffer` - Generic buffer type
- `LambertianModel` - Interface implementation
- `ILightModel` - Interface definition
- Standard types: `float`, `float2`, `float3`, `float4`, `float3x3`, `float4x4`

### Expected Functions
- `genericMax` - Generic function
- `utilityFunction` - Function with attributes
- `overloadedFunction` - Overloaded function
- `functionWithDefaults` - Function with default parameters
- `calculateLighting` - Lighting calculation function

## Memory Leak Detection

The test includes comprehensive memory leak detection using the Windows CRT debug heap:

- **Automatic Tracking**: Memory allocations are tracked throughout test execution
- **Leak Reporting**: Any memory leaks are reported at the end of testing
- **Resource Cleanup**: All Slang objects are properly released

## Usage

1. **Prerequisites**:
   - Windows with Visual Studio 2022
   - C++14 or later compiler
   - Slang shader file: `ComprehensiveSlangTest.slang`

2. **Build**:# From Visual Studio
Build -> Build Solution

# Or from command line
msbuild AttributeMemoryLeakTest.vcxproj
3. **Run**:# From Visual Studio
Debug -> Start Without Debugging

# Or from command line
.\Debug\AttributeMemoryLeakTest.exe
## Test Output

The test provides detailed output including:

### Test Categories
Each test category is clearly separated with headers showing:
- Test category name
- Individual test results with PASS/FAIL status
- Detailed information about discovered reflection data

### Summary Report
- Total test count
- Pass/fail statistics
- Success percentage
- Detailed failure list (if any)

### Memory Leak Report
- Memory allocation tracking results
- Any detected leaks with details
- Memory usage statistics

## Expected Results

A successful test run should show:
- ? Session creation and configuration
- ? Module loading from shader source
- ? Program compilation
- ? Reflection data extraction
- ? Discovery of expected types, functions, and entry points
- ? Proper attribute and layout analysis
- ? No memory leaks detected

## Troubleshooting

### Common Issues

1. **Shader File Not Found**:
   - Ensure `ComprehensiveSlangTest.slang` is in the working directory
   - Check file path and permissions

2. **API Function Not Found**:
   - Verify Slang library is properly linked
   - Check that all native functions are correctly declared

3. **Memory Leaks Detected**:
   - Review resource cleanup code
   - Ensure all reflection objects are properly released
   - Check for exception handling gaps

4. **Test Failures**:
   - Verify shader content matches expected structures
   - Check that entry points and types are correctly named
   - Ensure attributes are properly defined

## Integration with CI/CD

This test can be integrated into continuous integration pipelines:
# Example GitHub Actions step
- name: Run Reflection Tests
  run: |
    .\Tests\AttributeMemoryLeakTest\Debug\AttributeMemoryLeakTest.exe
  working-directory: ${{ github.workspace }}
The test returns:
- **Exit code 0**: All tests passed
- **Exit code 1**: One or more tests failed or exceptions occurred

## Extending the Tests

To add new reflection tests:

1. **Add Test Method**: Create new test method in `SlangReflectionTester` class
2. **Call in Main**: Add call to new test in `runAllTests()` method  
3. **Add Expected Data**: Update expected types/functions/entry points lists
4. **Update Shader**: Modify `ComprehensiveSlangTest.slang` to include new test cases

## Performance Considerations

The test suite is designed for correctness over performance:
- Comprehensive reflection API coverage
- Detailed validation of all returned data
- Memory leak detection overhead
- Extensive logging and reporting

For performance-critical scenarios, consider:
- Selective test execution
- Reduced logging verbosity
- Parallel test execution where possible