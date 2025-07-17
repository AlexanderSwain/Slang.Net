#include <iostream>
#include <string>
#include <vector>
#include <memory>
#include <fstream>
#include <sstream>
#include <cassert>
#include <iomanip>
#include <set>
#include <map>
#include "MemoryLeakDetector.h"
#include "../../src/Native/SlangNative.h"

using namespace std;
using namespace SlangNative;

// Test result structure
struct TestResult {
    string testName;
    bool passed;
    string message;
    
    TestResult(const string& name, bool success, const string& msg = "") 
        : testName(name), passed(success), message(msg) {}
};

class SlangReflectionTester {
private:
    vector<TestResult> testResults;
    void* session = nullptr;
    void* module = nullptr;
    void* program = nullptr;
    void* shaderReflection = nullptr;

    // Helper to add test results
    void addTestResult(const string& testName, bool passed, const string& message = "") {
        testResults.push_back(TestResult(testName, passed, message));
        cout << "[" << (passed ? "PASS" : "FAIL") << "] " << testName;
        if (!message.empty()) {
            cout << " - " << message;
        }
        cout << endl;
    }

    // Helper to safely get string from char*
    string safeString(const char* str) {
        return str ? string(str) : "<null>";
    }

    // Helper to read file content
    string readFile(const string& filename) {
        ifstream file(filename);
        if (!file.is_open()) {
            return "";
        }
        stringstream buffer;
        buffer << file.rdbuf();
        return buffer.str();
    }

    // Print section header
    void printHeader(const string& section) {
        cout << "\n" << string(60, '=') << endl;
        cout << "  " << section << endl;
        cout << string(60, '=') << endl;
    }

public:
    SlangReflectionTester() = default;
    
    ~SlangReflectionTester() {
        cleanup();
    }

    void cleanup() {
        if (shaderReflection) {
            ShaderReflection_Release(shaderReflection);
            shaderReflection = nullptr;
        }
        // Note: We can't safely delete these without knowing their actual types
        // In a real implementation, we would need proper cleanup functions
        program = nullptr;
        module = nullptr;
        session = nullptr;
    }

    bool initializeSlangSession() {
        printHeader("INITIALIZING SLANG SESSION");
        
        // For this test, we'll use a simplified session creation
        // In a real implementation, this would need proper options setup
        char* searchPaths[] = { 
            const_cast<char*>("./"),
            const_cast<char*>("../")
        };

        Native::ShaderModelCLI models[] = {
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "vs_5_0"),
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "gs_5_0"),
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "hs_5_0"),
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "ds_5_0"),
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "ps_5_0"),
            Native::ShaderModelCLI(Native::CompileTargetCLI::SLANG_HLSL, "cs_5_0"),
		};
        
        session = CreateSession(
            nullptr, 0,  // No options for now
            nullptr, 0,  // No macros for now  
            models, 6,  // Models for session
            searchPaths, 2);
        const char* error = SlangNative_GetLastError();
        
        bool success = (session != nullptr);
        addTestResult("Session Creation", success, error ? error : "");
        return success;
    }

    bool loadShaderModule() {
        printHeader("LOADING SHADER MODULE");
        
        if (!session) {
            addTestResult("Load Module", false, "No session available");
            return false;
        }
        
        // Load the comprehensive test shader
        string shaderPath = "ComprehensiveSlangTest.slang";
        string shaderSource = readFile(shaderPath);
        
        if (shaderSource.empty()) {
            addTestResult("Read Shader File", false, "Could not read " + shaderPath);
            return false;
        }
        addTestResult("Read Shader File", true, "Successfully read " + to_string(shaderSource.size()) + " bytes");
        
        module = CreateModule(
            session, 
            "ComprehensiveSlangTest", 
            shaderPath.c_str(), 
            shaderSource.c_str());
        const char* error = SlangNative_GetLastError();
        
        bool success = (module != nullptr);
        addTestResult("Module Creation", success, error ? error : "");
        return success;
    }

    bool createProgram() {
        printHeader("CREATING PROGRAM");
        
        if (!module) {
            addTestResult("Create Program", false, "No module available");
            return false;
        }
        
        program = CreateProgram(module);
        const char* error = SlangNative_GetLastError();
        bool success = (program != nullptr);
        addTestResult("Program Creation", success, error ? error : "");
        return success;
    }

    bool getProgramReflection() {
        printHeader("GETTING PROGRAM REFLECTION");
        
        if (!program) {
            addTestResult("Get Program Reflection", false, "No program available");
            return false;
        }
        
        shaderReflection = GetProgramReflection(program, 0);
        const char* error = SlangNative_GetLastError();
        bool success = (shaderReflection != nullptr);
        addTestResult("Program Reflection", success, error ? error : "");
        return success;
    }

    void testShaderReflectionBasics() {
        printHeader("TESTING SHADER REFLECTION BASICS");
        
        if (!shaderReflection) {
            addTestResult("Shader Reflection Basics", false, "No shader reflection available");
            return;
        }
        
        // Test basic counts
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection);
        addTestResult("Parameter Count", paramCount >= 0, "Found " + to_string(paramCount) + " parameters");
        
        unsigned int typeParamCount = ShaderReflection_GetTypeParameterCount(shaderReflection);
        addTestResult("Type Parameter Count", typeParamCount >= 0, "Found " + to_string(typeParamCount) + " type parameters");
        
        unsigned int entryPointCount = ShaderReflection_GetEntryPointCount(shaderReflection);
        addTestResult("Entry Point Count", entryPointCount > 0, "Found " + to_string(entryPointCount) + " entry points");
        
        // Test global constant buffer info
        unsigned int globalBinding = ShaderReflection_GetGlobalConstantBufferBinding(shaderReflection);
        size_t globalSize = ShaderReflection_GetGlobalConstantBufferSize(shaderReflection);
        addTestResult("Global Constant Buffer", true, "Binding: " + to_string(globalBinding) + ", Size: " + to_string(globalSize));
        
        // Test hashed strings
        unsigned int hashedStringCount = ShaderReflection_GetHashedStringCount(shaderReflection);
        addTestResult("Hashed String Count", hashedStringCount >= 0, "Found " + to_string(hashedStringCount) + " hashed strings");
        
        // Test JSON export
        const char* jsonOutput = nullptr;
        int jsonResult = ShaderReflection_ToJson(shaderReflection, &jsonOutput);
        bool jsonSuccess = (jsonResult >= 0 && jsonOutput != nullptr);
        addTestResult("JSON Export", jsonSuccess, jsonSuccess ? "JSON exported successfully" : "JSON export failed");
    }

    void testEntryPoints() {
        printHeader("TESTING ENTRY POINTS");
        
        if (!shaderReflection) {
            addTestResult("Entry Points Test", false, "No shader reflection available");
            return;
        }
        
        unsigned int entryPointCount = ShaderReflection_GetEntryPointCount(shaderReflection);
        
        // Expected entry points from our comprehensive shader
        vector<string> expectedEntryPoints = { "VS", "PS", "GS", "CS", "HS", "DS", "TestAttributeFunction" };
        set<string> foundEntryPoints;
        
        for (unsigned int i = 0; i < entryPointCount; i++) {
            void* entryPoint = ShaderReflection_GetEntryPointByIndex(shaderReflection, i);
            if (!entryPoint) {
                addTestResult("Entry Point " + to_string(i), false, "Could not get entry point");
                continue;
            }
            
            const char* name = EntryPointReflection_GetName(entryPoint);
            const char* nameOverride = EntryPointReflection_GetNameOverride(entryPoint);
            int stage = EntryPointReflection_GetStage(entryPoint);
            unsigned int paramCount = EntryPointReflection_GetParameterCount(entryPoint);
            
            string entryPointName = safeString(name);
            foundEntryPoints.insert(entryPointName);
            
            addTestResult("Entry Point " + entryPointName, true, 
                "Stage: " + to_string(stage) + ", Parameters: " + to_string(paramCount) + 
                ", Override: " + safeString(nameOverride));
            
            // Test compute-specific properties for compute shaders
            if (entryPointName == "CS") {
                SlangUInt threadGroupSize[3] = {0, 0, 0};
                EntryPointReflection_GetComputeThreadGroupSize(entryPoint, 3, threadGroupSize);
                addTestResult("Compute Thread Group Size", true, 
                    "(" + to_string(threadGroupSize[0]) + ", " + 
                    to_string(threadGroupSize[1]) + ", " + 
                    to_string(threadGroupSize[2]) + ")");
                
                SlangUInt waveSize = 0;
                SlangResult waveResult = EntryPointReflection_GetComputeWaveSize(entryPoint, &waveSize);
                addTestResult("Compute Wave Size", waveResult >= 0, 
                    waveResult >= 0 ? "Wave size: " + to_string(waveSize) : "Not available");
            }
            
            // Test entry point reflection properties
            void* function = EntryPointReflection_GetFunction(entryPoint);
            addTestResult("Entry Point Function", function != nullptr);
            
            void* varLayout = EntryPointReflection_GetVarLayout(entryPoint);
            addTestResult("Entry Point VarLayout", varLayout != nullptr);
            
            void* typeLayout = EntryPointReflection_GetTypeLayout(entryPoint);
            addTestResult("Entry Point TypeLayout", typeLayout != nullptr);
            
            bool usesAnySampleRate = EntryPointReflection_UsesAnySampleRateInput(entryPoint);
            bool hasDefaultCB = EntryPointReflection_HasDefaultConstantBuffer(entryPoint);
            addTestResult("Entry Point Properties", true, 
                "UsesSampleRate: " + string(usesAnySampleRate ? "true" : "false") + 
                ", HasDefaultCB: " + string(hasDefaultCB ? "true" : "false"));
            
            EntryPointReflection_Release(entryPoint);
        }
        
        // Test finding entry points by name
        for (const string& expectedName : expectedEntryPoints) {
            void* foundEntryPoint = ShaderReflection_FindEntryPointByName(shaderReflection, expectedName.c_str());
            bool found = (foundEntryPoint != nullptr);
            addTestResult("Find Entry Point: " + expectedName, found);
            if (foundEntryPoint) {
                EntryPointReflection_Release(foundEntryPoint);
            }
        }
        
        addTestResult("Entry Points Coverage", foundEntryPoints.size() >= 6, 
            "Found " + to_string(foundEntryPoints.size()) + " entry points");
    }

    void testTypeReflection() {
        printHeader("TESTING TYPE REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Type Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding types by name
        vector<string> expectedTypes = { 
            "ComplexStruct", "NestedStruct", "MyAttributeStruct", 
            "GenericBuffer", "LambertianModel", "ILightModel",
            "float", "float2", "float3", "float4", "float3x3", "float4x4"
        };
        
        for (const string& typeName : expectedTypes) {
            void* type = ShaderReflection_FindTypeByName(shaderReflection, typeName.c_str());
            bool found = (type != nullptr);
            addTestResult("Find Type: " + typeName, found);
            
            if (type) {
                // Test type properties
                const char* name = TypeReflection_GetName(type);
                int kind = TypeReflection_GetKind(type);
                unsigned int fieldCount = TypeReflection_GetFieldCount(type);
                bool isArray = TypeReflection_IsArray(type);
                
                addTestResult("Type " + typeName + " Properties", true,
                    "Name: " + safeString(name) + ", Kind: " + to_string(kind) + 
                    ", Fields: " + to_string(fieldCount) + ", IsArray: " + (isArray ? "true" : "false"));
                
                // Test array properties if it's an array
                if (isArray) {
                    void* elementType = TypeReflection_GetElementType(type);
                    size_t elementCount = TypeReflection_GetElementCount(type);
                    addTestResult("Array Type Properties", elementType != nullptr,
                        "Element count: " + to_string(elementCount));
                    if (elementType) {
                        TypeReflection_Release(elementType);
                    }
                }
                
                // Test matrix properties
                unsigned int rowCount = TypeReflection_GetRowCount(type);
                unsigned int colCount = TypeReflection_GetColumnCount(type);
                if (rowCount > 1 || colCount > 1) {
                    addTestResult("Matrix Properties", true,
                        "Rows: " + to_string(rowCount) + ", Cols: " + to_string(colCount));
                }
                
                // Test struct fields
                if (fieldCount > 0) {
                    for (unsigned int i = 0; i < min(fieldCount, 10u); i++) { // Limit to first 10 fields
                        void* field = TypeReflection_GetFieldByIndex(type, i);
                        if (field) {
                            const char* fieldName = VariableReflection_GetName(field);
                            addTestResult("Field " + to_string(i), true, 
                                "Name: " + safeString(fieldName));
                            VariableReflection_Release(field);
                        }
                    }
                }
                
                // Test user attributes
                unsigned int attrCount = TypeReflection_GetUserAttributeCount(type);
                addTestResult("Type Attributes", attrCount >= 0, 
                    "Found " + to_string(attrCount) + " attributes");
                
                TypeReflection_Release(type);
            }
        }
    }

    void testFunctionReflection() {
        printHeader("TESTING FUNCTION REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Function Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding functions by name
        vector<string> expectedFunctions = {
            "VS", "PS", "CS", "GS", "HS", "DS",
            "genericMax", "utilityFunction", "overloadedFunction",
            "functionWithDefaults", "calculateLighting"
        };
        
        for (const string& functionName : expectedFunctions) {
            void* function = ShaderReflection_FindFunctionByName(shaderReflection, functionName.c_str());
            bool found = (function != nullptr);
            addTestResult("Find Function: " + functionName, found);
            
            if (function) {
                // Test function properties
                const char* name = FunctionReflection_GetName(function);
                unsigned int paramCount = FunctionReflection_GetParameterCount(function);
                unsigned int attrCount = FunctionReflection_GetUserAttributeCount(function);
                bool isOverloaded = FunctionReflection_IsOverloaded(function);
                unsigned int overloadCount = FunctionReflection_GetOverloadCount(function);
                
                addTestResult("Function " + functionName + " Properties", true,
                    "Name: " + safeString(name) + ", Params: " + to_string(paramCount) + 
                    ", Attrs: " + to_string(attrCount) + ", Overloaded: " + (isOverloaded ? "true" : "false") +
                    ", Overloads: " + to_string(overloadCount));
                
                // Test return type
                void* returnType = FunctionReflection_GetReturnType(function);
                if (returnType) {
                    const char* returnTypeName = TypeReflection_GetName(returnType);
                    addTestResult("Function Return Type", true, 
                        "Return type: " + safeString(returnTypeName));
                    TypeReflection_Release(returnType);
                }
                
                // Test parameters
                for (unsigned int i = 0; i < min(paramCount, 5u); i++) { // Limit to first 5 parameters
                    void* param = FunctionReflection_GetParameterByIndex(function, i);
                    if (param) {
                        const char* paramName = VariableReflection_GetName(param);
                        addTestResult("Function Parameter " + to_string(i), true,
                            "Name: " + safeString(paramName));
                        VariableReflection_Release(param);
                    }
                }
                
                // Test function attributes
                for (unsigned int i = 0; i < attrCount; i++) {
                    void* attr = FunctionReflection_GetUserAttributeByIndex(function, i);
                    if (attr) {
                        const char* attrName = Attribute_GetName(attr);
                        unsigned int argCount = Attribute_GetArgumentCount(attr);
                        addTestResult("Function Attribute " + to_string(i), true,
                            "Name: " + safeString(attrName) + ", Args: " + to_string(argCount));
                        Attribute_Release(attr);
                    }
                }
                
                FunctionReflection_Release(function);
            }
        }
    }

    void testVariableReflection() {
        printHeader("TESTING VARIABLE REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Variable Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test global parameters
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection);
        addTestResult("Global Parameters", paramCount >= 0, 
            "Found " + to_string(paramCount) + " global parameters");
        
        for (unsigned int i = 0; i < min(paramCount, 10u); i++) { // Limit to first 10 parameters
            void* param = ShaderReflection_GetParameterByIndex(shaderReflection, i);
            if (param) {
                const char* paramName = VariableLayoutReflection_GetName(param);
                unsigned int bindingIndex = VariableLayoutReflection_GetBindingIndex(param);
                unsigned int bindingSpace = VariableLayoutReflection_GetBindingSpace(param);
                
                addTestResult("Global Parameter " + to_string(i), true,
                    "Name: " + safeString(paramName) + ", Binding: " + to_string(bindingIndex) + 
                    ", Space: " + to_string(bindingSpace));
                
                // Test variable layout properties
                void* typeLayout = VariableLayoutReflection_GetTypeLayout(param);
                if (typeLayout) {
                    size_t size = TypeLayoutReflection_GetSize(typeLayout, 0); // Uniform category
                    addTestResult("Parameter Type Layout", true,
                        "Size: " + to_string(size) + " bytes");
                    TypeLayoutReflection_Release(typeLayout);
                }
                
                VariableLayoutReflection_Release(param);
            }
        }
        
        // Test global params layouts
        void* globalParamsTypeLayout = ShaderReflection_GetGlobalParamsTypeLayout(shaderReflection);
        void* globalParamsVarLayout = ShaderReflection_GetGlobalParamsVarLayout(shaderReflection);
        
        addTestResult("Global Params Type Layout", globalParamsTypeLayout != nullptr);
        addTestResult("Global Params Var Layout", globalParamsVarLayout != nullptr);
        
        if (globalParamsTypeLayout) {
            TypeLayoutReflection_Release(globalParamsTypeLayout);
        }
        if (globalParamsVarLayout) {
            VariableLayoutReflection_Release(globalParamsVarLayout);
        }
    }

    void testAttributeReflection() {
        printHeader("TESTING ATTRIBUTE REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Attribute Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding types with attributes
        void* attributeStruct = ShaderReflection_FindTypeByName(shaderReflection, "MyAttributeStruct");
        if (attributeStruct) {
            unsigned int attrCount = TypeReflection_GetUserAttributeCount(attributeStruct);
            addTestResult("MyAttributeStruct Attributes", attrCount > 0,
                "Found " + to_string(attrCount) + " attributes");
            
            for (unsigned int i = 0; i < attrCount; i++) {
                void* attr = TypeReflection_GetUserAttributeByIndex(attributeStruct, i);
                if (attr) {
                    const char* attrName = Attribute_GetName(attr);
                    unsigned int argCount = Attribute_GetArgumentCount(attr);
                    
                    addTestResult("Attribute " + to_string(i), true,
                        "Name: " + safeString(attrName) + ", Args: " + to_string(argCount));
                    
                    // Test attribute arguments
                    for (unsigned int j = 0; j < argCount; j++) {
                        // Test string arguments
                        const char* stringArg = Attribute_GetArgumentValueString(attr, j);
                        if (stringArg) {
                            addTestResult("Attribute String Arg " + to_string(j), true,
                                "Value: " + safeString(stringArg));
                        }
                        
                        // Test int arguments
                        int intArg = 0;
                        SlangResult intResult = Attribute_GetArgumentValueInt(attr, j, &intArg);
                        if (intResult >= 0) {
                            addTestResult("Attribute Int Arg " + to_string(j), true,
                                "Value: " + to_string(intArg));
                        }
                        
                        // Test float arguments
                        float floatArg = 0.0f;
                        SlangResult floatResult = Attribute_GetArgumentValueFloat(attr, j, &floatArg);
                        if (floatResult >= 0) {
                            addTestResult("Attribute Float Arg " + to_string(j), true,
                                "Value: " + to_string(floatArg));
                        }
                    }
                    
                    Attribute_Release(attr);
                }
            }
            
            TypeReflection_Release(attributeStruct);
        }
        
        // Test finding functions with attributes
        void* utilityFunction = ShaderReflection_FindFunctionByName(shaderReflection, "utilityFunction");
        if (utilityFunction) {
            unsigned int attrCount = FunctionReflection_GetUserAttributeCount(utilityFunction);
            addTestResult("utilityFunction Attributes", attrCount > 0,
                "Found " + to_string(attrCount) + " attributes");
            
            FunctionReflection_Release(utilityFunction);
        }
    }

    void testGenericReflection() {
        printHeader("TESTING GENERIC REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Generic Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding generic types
        void* genericBuffer = ShaderReflection_FindTypeByName(shaderReflection, "GenericBuffer");
        if (genericBuffer) {
            void* genericContainer = TypeReflection_GetGenericContainer(genericBuffer);
            if (genericContainer) {
                const char* name = GenericReflection_GetName(genericContainer);
                unsigned int typeParamCount = GenericReflection_GetTypeParameterCount(genericContainer);
                unsigned int valueParamCount = GenericReflection_GetValueParameterCount(genericContainer);
                
                addTestResult("Generic Container", true,
                    "Name: " + safeString(name) + ", TypeParams: " + to_string(typeParamCount) + 
                    ", ValueParams: " + to_string(valueParamCount));
                
                // Test type parameters
                for (unsigned int i = 0; i < typeParamCount; i++) {
                    void* typeParam = GenericReflection_GetTypeParameter(genericContainer, i);
                    if (typeParam) {
                        const char* paramName = VariableReflection_GetName(typeParam);
                        addTestResult("Generic Type Parameter " + to_string(i), true,
                            "Name: " + safeString(paramName));
                        VariableReflection_Release(typeParam);
                    }
                }
                
                // Test value parameters
                for (unsigned int i = 0; i < valueParamCount; i++) {
                    void* valueParam = GenericReflection_GetValueParameter(genericContainer, i);
                    if (valueParam) {
                        const char* paramName = VariableReflection_GetName(valueParam);
                        addTestResult("Generic Value Parameter " + to_string(i), true,
                            "Name: " + safeString(paramName));
                        VariableReflection_Release(valueParam);
                    }
                }
                
                GenericReflection_Release(genericContainer);
            }
            
            TypeReflection_Release(genericBuffer);
        }
        
        // Test finding generic functions
        void* genericMax = ShaderReflection_FindFunctionByName(shaderReflection, "genericMax");
        if (genericMax) {
            void* genericContainer = FunctionReflection_GetGenericContainer(genericMax);
            if (genericContainer) {
                addTestResult("Generic Function Container", true);
                GenericReflection_Release(genericContainer);
            }
            
            FunctionReflection_Release(genericMax);
        }
    }

    void testLayoutReflection() {
        printHeader("TESTING LAYOUT REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Layout Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test type layouts by examining global parameters that should have layout information
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection);
        
        for (unsigned int i = 0; i < min(paramCount, 5u); i++) { // Test first 5 parameters
            void* param = ShaderReflection_GetParameterByIndex(shaderReflection, i);
            if (param) {
                const char* paramName = VariableLayoutReflection_GetName(param);
                void* typeLayout = VariableLayoutReflection_GetTypeLayout(param);
                
                if (typeLayout) {
                    size_t size = TypeLayoutReflection_GetSize(typeLayout, 0); // Uniform category
                    size_t stride = TypeLayoutReflection_GetStride(typeLayout, 0);
                    int alignment = TypeLayoutReflection_GetAlignment(typeLayout, 0);
                    unsigned int fieldCount = TypeLayoutReflection_GetFieldCount(typeLayout);
                    
                    addTestResult("Parameter Layout " + to_string(i), true,
                        "Name: " + safeString(paramName) + ", Size: " + to_string(size) + 
                        ", Stride: " + to_string(stride) + ", Alignment: " + to_string(alignment) + 
                        ", Fields: " + to_string(fieldCount));
                    
                    // Test field layouts
                    for (unsigned int j = 0; j < min(fieldCount, 3u); j++) { // Limit to first 3 fields
                        void* fieldLayout = TypeLayoutReflection_GetFieldByIndex(typeLayout, j);
                        if (fieldLayout) {
                            const char* fieldName = VariableLayoutReflection_GetName(fieldLayout);
                            size_t fieldOffset = VariableLayoutReflection_GetOffset(fieldLayout, 0);
                            
                            addTestResult("Field Layout " + to_string(j), true,
                                "Name: " + safeString(fieldName) + ", Offset: " + to_string(fieldOffset));
                            
                            VariableLayoutReflection_Release(fieldLayout);
                        }
                    }
                    
                    TypeLayoutReflection_Release(typeLayout);
                }
                
                VariableLayoutReflection_Release(param);
            }
        }
    }

    void testCompilation() {
        printHeader("TESTING SHADER COMPILATION");
        
        if (!program) {
            addTestResult("Shader Compilation Test", false, "No program available");
            return;
        }
        
        // Test compilation of different entry points
        vector<pair<string, unsigned int>> entryPointsToCompile = {
            {"Vertex Shader", 0}, {"Pixel Shader", 1}, {"Compute Shader", 2}
        };
        
        for (const auto& entry : entryPointsToCompile) {
            const char* output = nullptr;
            int32_t result = Compile(program, entry.second, 0, &output);
            const char* error = SlangNative_GetLastError();
            
            bool success = (result >= 0 && output != nullptr);
            string message = success ? "Compiled successfully" : nullptr;
            if (success && output) {
                size_t outputLength = strlen(output);
                message += " (" + to_string(outputLength) + " chars)";
            }
            else if (error) {
                message = "Compilation failed: " + string(error);
			}
            
            addTestResult("Compile " + entry.first, success, success ? message : error);
        }
    }

    void runAllTests() {
        printHeader("STARTING COMPREHENSIVE SLANG REFLECTION API TESTS");
        
        // Initialize memory leak detection
        MemoryLeakDetector::Initialize();
        MemoryLeakDetector::StartMonitoring();
        
        // Initialize and setup
        if (!initializeSlangSession()) return;
        if (!loadShaderModule()) return;
        if (!createProgram()) return;
        if (!getProgramReflection()) return;
        
        // Run all reflection tests
        testShaderReflectionBasics();
        testEntryPoints();
        testTypeReflection();
        testFunctionReflection();
        testVariableReflection();
        testAttributeReflection();
        testGenericReflection();
        testLayoutReflection();
        testCompilation();
        
        // Print summary
        printSummary();
        
        // Clean up
        cleanup();
        
        // Check for memory leaks
        MemoryLeakDetector::StopMonitoring();
        MemoryLeakDetector::ReportLeaks();
    }

    void printSummary() {
        printHeader("TEST SUMMARY");
        
        int passed = 0;
        int failed = 0;
        
        for (const auto& result : testResults) {
            if (result.passed) {
                passed++;
            } else {
                failed++;
            }
        }
        
        cout << "Total Tests: " << testResults.size() << endl;
        cout << "Passed: " << passed << endl;
        cout << "Failed: " << failed << endl;
        cout << "Success Rate: " << fixed << setprecision(1) 
             << (100.0 * passed / testResults.size()) << "%" << endl;
        
        if (failed > 0) {
            cout << "\nFailed Tests:" << endl;
            for (const auto& result : testResults) {
                if (!result.passed) {
                    cout << "  - " << result.testName;
                    if (!result.message.empty()) {
                        cout << " (" << result.message << ")";
                    }
                    cout << endl;
                }
            }
        }
    }
};

int main() {
    cout << "Comprehensive Slang Reflection API Test Suite" << endl;
    cout << "=============================================" << endl;
    
    SlangReflectionTester tester;
    tester.runAllTests();
    
    return 0;
}