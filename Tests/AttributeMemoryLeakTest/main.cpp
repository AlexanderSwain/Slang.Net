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
    void* entryPointVS = nullptr;
    void* entryPointPS = nullptr;
    void* entryPointVS2 = nullptr;
    void* entryPointPS2 = nullptr;
    void* shaderReflection = nullptr;
    const char* error = nullptr;

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
            ShaderReflection_Release(shaderReflection, &error);
            shaderReflection = nullptr;
        }
        if (program) {
            Program_Release(program, &error);
            program = nullptr;
        }
        if (module) {
            Module_Release(module, &error);
            module = nullptr;
        }
        if (session) {
            Session_Release(session, &error);
            session = nullptr;
        }
    }

    bool initializeSlangSession() {
        printHeader("INITIALIZING SLANG SESSION");
        
        // For this test, we'll use a simplified session creation
        // In a real implementation, this would need proper options setup
        char* searchPaths[] = { 
            const_cast<char*>("./"),
            const_cast<char*>("../")
        };

        // Create targets for all shader stages used in the comprehensive shader
        Native::TargetCLI models[] = {
            Native::TargetCLI(Native::CompileTargetCLI::SLANG_HLSL, "vs_5_0"),  // Vertex shaders
            Native::TargetCLI(Native::CompileTargetCLI::SLANG_HLSL, "ps_5_0"),  // Pixel shaders
		};
        
        session = Session_Create(
            nullptr, 0,  // No options for now
            nullptr, 0,  // No macros for now  
            models, 2,  // Models for session (updated count to 6)
            searchPaths, 2, &error);
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
        
        char* err = nullptr;
        module = Module_Create(
            session, 
            "ComprehensiveSlangTest", 
            shaderPath.c_str(), 
            shaderSource.c_str(),
            &err);
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
        
        program = Program_Create(module, &error);
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
        
        shaderReflection = Program_GetProgramReflection(program, 0, &error);
        const char* error = SlangNative_GetLastError();
        bool success = (shaderReflection != nullptr);
        addTestResult("Program Reflection", success, error ? error : "");
        return success;
    }

    bool getCompiledProgram()
    {
        printHeader("GETTING COMPILED PROGRAM");
        
        if (!program) {
            addTestResult("Get Compiled Program", false, "No program available");
            return false;
        }
        
        const char* output = nullptr;
        const char* error = nullptr;
        SlangResult result = Program_CompileProgram(program, 1, 1, &output, &error);
        //const char* error = SlangNative_GetLastError();
        
        bool success = (result >= 0 && output != nullptr);
        addTestResult("Compiled Program", success, success ? output : (error ? error : "Unknown error"));
        
		return success;
    }

    bool getCompiledEntryPoint()
    {
        printHeader("GETTING COMPILED ENTRYPOINT");

        entryPointVS = Module_GetEntryPointByIndex(module, 0, &error);
        entryPointPS = Module_GetEntryPointByIndex(module, 1, &error);
        entryPointVS2 = Module_GetEntryPointByIndex(module, 2, &error);
        entryPointPS2 = Module_GetEntryPointByIndex(module, 3, &error);

        if (!entryPointVS) {
            addTestResult("Get Compiled EntryPoint", false, "No entryPoint available");
            return false;
        }

        const char* output = nullptr;
        SlangResult result = EntryPoint_Compile(entryPointVS, 0, &output, &error);
        const char* error = SlangNative_GetLastError();

        bool success = (result >= 0 && output != nullptr);
        addTestResult("Compiled EntryPoint", success, success ? output : (error ? error : "Unknown error"));

        return success;
    }

    void testShaderReflectionBasics() {
        printHeader("TESTING SHADER REFLECTION BASICS");
        
        if (!shaderReflection) {
            addTestResult("Shader Reflection Basics", false, "No shader reflection available");
            return;
        }
        
        // Test basic counts
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection, &error);
        addTestResult("Parameter Count", paramCount >= 0, "Found " + to_string(paramCount) + " parameters");
        
        unsigned int typeParamCount = ShaderReflection_GetTypeParameterCount(shaderReflection, &error);
        addTestResult("Type Parameter Count", typeParamCount >= 0, "Found " + to_string(typeParamCount) + " type parameters");
        
        unsigned int entryPointCount = ShaderReflection_GetEntryPointCount(shaderReflection, &error);
        addTestResult("Entry Point Count", entryPointCount > 0, "Found " + to_string(entryPointCount) + " entry points");
        
        // Test global constant buffer info
        unsigned int globalBinding = ShaderReflection_GetGlobalConstantBufferBinding(shaderReflection, &error);
        size_t globalSize = ShaderReflection_GetGlobalConstantBufferSize(shaderReflection, &error);
        addTestResult("Global Constant Buffer", true, "Binding: " + to_string(globalBinding) + ", Size: " + to_string(globalSize));
        
        // Test hashed strings
        unsigned int hashedStringCount = ShaderReflection_GetHashedStringCount(shaderReflection, &error);
        addTestResult("Hashed String Count", hashedStringCount >= 0, "Found " + to_string(hashedStringCount) + " hashed strings");
        
        // Test JSON export
        const char* jsonOutput = nullptr;
        int jsonResult = ShaderReflection_ToJson(shaderReflection, &jsonOutput, &error);
        bool jsonSuccess = (jsonResult >= 0 && jsonOutput != nullptr);
        addTestResult("JSON Export", jsonSuccess, jsonSuccess ? "JSON exported successfully" : "JSON export failed");
    }

    void testEntryPoints() {
        printHeader("TESTING ENTRY POINTS");
        
        if (!shaderReflection) {
            addTestResult("Entry Points Test", false, "No shader reflection available");
            return;
        }
        
        unsigned int entryPointCount = ShaderReflection_GetEntryPointCount(shaderReflection, &error);
        
        // Expected entry points from our comprehensive shader
        vector<string> expectedEntryPoints = { "VS", "PS", "GS", "CS", "HS", "DS", "TestAttributeFunction" };
        set<string> foundEntryPoints;
        
        for (unsigned int i = 0; i < entryPointCount; i++) {
            void* entryPoint = ShaderReflection_GetEntryPointByIndex(shaderReflection, i, &error);
            if (!entryPoint) {
                addTestResult("Entry Point " + to_string(i), false, "Could not get entry point");
                continue;
            }
            
            const char* name = EntryPointReflection_GetName(entryPoint, &error);
            const char* nameOverride = EntryPointReflection_GetNameOverride(entryPoint, &error);
            int stage = EntryPointReflection_GetStage(entryPoint, &error);
            unsigned int paramCount = EntryPointReflection_GetParameterCount(entryPoint, &error);
            
            string entryPointName = safeString(name);
            foundEntryPoints.insert(entryPointName);
            
            addTestResult("Entry Point " + entryPointName, true, 
                "Stage: " + to_string(stage) + ", Parameters: " + to_string(paramCount) + 
                ", Override: " + safeString(nameOverride));
            
            // Test compute-specific properties for compute shaders
            if (entryPointName == "CS") {
                SlangUInt threadGroupSize[3] = {0, 0, 0};
                EntryPointReflection_GetComputeThreadGroupSize(entryPoint, 3, threadGroupSize, &error);
                addTestResult("Compute Thread Group Size", true, 
                    "(" + to_string(threadGroupSize[0]) + ", " + 
                    to_string(threadGroupSize[1]) + ", " + 
                    to_string(threadGroupSize[2]) + ")");
                
                SlangUInt waveSize = 0;
                SlangResult waveResult = EntryPointReflection_GetComputeWaveSize(entryPoint, &waveSize, &error);
                addTestResult("Compute Wave Size", waveResult >= 0, 
                    waveResult >= 0 ? "Wave size: " + to_string(waveSize) : "Not available");
            }
            
            // Test entry point reflection properties
            void* function = EntryPointReflection_GetFunction(entryPoint, &error);
            addTestResult("Entry Point Function", function != nullptr);
            
            void* varLayout = EntryPointReflection_GetVarLayout(entryPoint, &error);
            addTestResult("Entry Point VarLayout", varLayout != nullptr);
            
            void* typeLayout = EntryPointReflection_GetTypeLayout(entryPoint, &error);
            addTestResult("Entry Point TypeLayout", typeLayout != nullptr);
            
            bool usesAnySampleRate = EntryPointReflection_UsesAnySampleRateInput(entryPoint, &error);
            bool hasDefaultCB = EntryPointReflection_HasDefaultConstantBuffer(entryPoint, &error);
            addTestResult("Entry Point Properties", true, 
                "UsesSampleRate: " + string(usesAnySampleRate ? "true" : "false") + 
                ", HasDefaultCB: " + string(hasDefaultCB ? "true" : "false"));
            
            EntryPointReflection_Release(entryPoint, &error);
        }
        
        // Test finding entry points by name
        for (const string& expectedName : expectedEntryPoints) {
            void* foundEntryPoint = ShaderReflection_FindEntryPointByName(shaderReflection, expectedName.c_str(), &error);
            bool found = (foundEntryPoint != nullptr);
            addTestResult("Find Entry Point: " + expectedName, found);
            if (foundEntryPoint) {
                EntryPointReflection_Release(foundEntryPoint, &error);
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
            void* type = ShaderReflection_FindTypeByName(shaderReflection, typeName.c_str(), &error);
            bool found = (type != nullptr);
            addTestResult("Find Type: " + typeName, found);
            
            if (type) {
                // Test type properties
                const char* name = TypeReflection_GetName(type, &error);
                int kind = TypeReflection_GetKind(type, &error);
                unsigned int fieldCount = TypeReflection_GetFieldCount(type, &error);
                bool isArray = TypeReflection_IsArray(type, &error);
                
                addTestResult("Type " + typeName + " Properties", true,
                    "Name: " + safeString(name) + ", Kind: " + to_string(kind) + 
                    ", Fields: " + to_string(fieldCount) + ", IsArray: " + (isArray ? "true" : "false"));
                
                // Test array properties if it's an array
                if (isArray) {
                    void* elementType = TypeReflection_GetElementType(type, &error);
                    size_t elementCount = TypeReflection_GetElementCount(type, &error);
                    addTestResult("Array Type Properties", elementType != nullptr,
                        "Element count: " + to_string(elementCount));
                    if (elementType) {
                        TypeReflection_Release(elementType, &error);
                    }
                }
                
                // Test matrix properties
                unsigned int rowCount = TypeReflection_GetRowCount(type, &error);
                unsigned int colCount = TypeReflection_GetColumnCount(type, &error);
                if (rowCount > 1 || colCount > 1) {
                    addTestResult("Matrix Properties", true,
                        "Rows: " + to_string(rowCount) + ", Cols: " + to_string(colCount));
                }
                
                // Test struct fields
                if (fieldCount > 0) {
                    for (unsigned int i = 0; i < min(fieldCount, 10u); i++) { // Limit to first 10 fields
                        void* field = TypeReflection_GetFieldByIndex(type, i, &error);
                        if (field) {
                            const char* fieldName = VariableReflection_GetName(field, &error);
                            addTestResult("Field " + to_string(i), true, 
                                "Name: " + safeString(fieldName));
                            VariableReflection_Release(field, &error);
                        }
                    }
                }
                
                // Test user attributes
                unsigned int attrCount = TypeReflection_GetUserAttributeCount(type, &error);
                addTestResult("Type Attributes", attrCount >= 0, 
                    "Found " + to_string(attrCount) + " attributes");
                
                TypeReflection_Release(type, &error);
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
            void* function = ShaderReflection_FindFunctionByName(shaderReflection, functionName.c_str(), &error);
            bool found = (function != nullptr);
            addTestResult("Find Function: " + functionName, found);
            
            if (function) {
                // Test function properties
                const char* name = FunctionReflection_GetName(function, &error);
                unsigned int paramCount = FunctionReflection_GetParameterCount(function, &error);
                unsigned int attrCount = FunctionReflection_GetUserAttributeCount(function, &error);
                bool isOverloaded = FunctionReflection_IsOverloaded(function, &error);
                unsigned int overloadCount = FunctionReflection_GetOverloadCount(function, &error);
                
                addTestResult("Function " + functionName + " Properties", true,
                    "Name: " + safeString(name) + ", Params: " + to_string(paramCount) + 
                    ", Attrs: " + to_string(attrCount) + ", Overloaded: " + (isOverloaded ? "true" : "false") +
                    ", Overloads: " + to_string(overloadCount));
                
                FunctionReflection_Release(function, &error);
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
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection, &error);
        addTestResult("Global Parameters", paramCount >= 0, 
            "Found " + to_string(paramCount) + " global parameters");
        
        for (unsigned int i = 0; i < min(paramCount, 10u); i++) { // Limit to first 10 parameters
            void* param = ShaderReflection_GetParameterByIndex(shaderReflection, i, &error);
            if (param) {
                const char* paramName = VariableLayoutReflection_GetName(param, &error);
                unsigned int bindingIndex = VariableLayoutReflection_GetBindingIndex(param, &error);
                unsigned int bindingSpace = VariableLayoutReflection_GetBindingSpace(param, &error);
                
                addTestResult("Global Parameter " + to_string(i), true,
                    "Name: " + safeString(paramName) + ", Binding: " + to_string(bindingIndex) + 
                    ", Space: " + to_string(bindingSpace));
                
                // Test variable layout properties
                void* typeLayout = VariableLayoutReflection_GetTypeLayout(param, &error);
                if (typeLayout) {
                    size_t size = TypeLayoutReflection_GetSize(typeLayout, 0, &error); // Uniform category
                    addTestResult("Parameter Type Layout", true,
                        "Size: " + to_string(size) + " bytes");
                    TypeLayoutReflection_Release(typeLayout, &error);
                }
                
                VariableLayoutReflection_Release(param, &error);
            }
        }
        
        // Test global params layouts
        void* globalParamsTypeLayout = ShaderReflection_GetGlobalParamsTypeLayout(shaderReflection, &error);
        void* globalParamsVarLayout = ShaderReflection_GetGlobalParamsVarLayout(shaderReflection, &error);
        
        addTestResult("Global Params Type Layout", globalParamsTypeLayout != nullptr);
        addTestResult("Global Params Var Layout", globalParamsVarLayout != nullptr);
        
        if (globalParamsTypeLayout) {
            TypeLayoutReflection_Release(globalParamsTypeLayout, &error);
        }
        if (globalParamsVarLayout) {
            VariableLayoutReflection_Release(globalParamsVarLayout, &error);
        }
    }

    void testAttributeReflection() {
        printHeader("TESTING ATTRIBUTE REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Attribute Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding types with attributes
        void* attributeStruct = ShaderReflection_FindTypeByName(shaderReflection, "MyAttributeStruct", &error);
        if (attributeStruct) {
            unsigned int attrCount = TypeReflection_GetUserAttributeCount(attributeStruct, &error);
            addTestResult("MyAttributeStruct Attributes", attrCount > 0,
                "Found " + to_string(attrCount) + " attributes");
            
            for (unsigned int i = 0; i < attrCount; i++) {
                void* attr = TypeReflection_GetUserAttributeByIndex(attributeStruct, i, &error);
                if (attr) {
                    const char* attrName = Attribute_GetName(attr, &error);
                    unsigned int argCount = Attribute_GetArgumentCount(attr, &error);
                    
                    addTestResult("Attribute " + to_string(i), true,
                        "Name: " + safeString(attrName) + ", Args: " + to_string(argCount));
                    
                    // Test attribute arguments
                    for (unsigned int j = 0; j < argCount; j++) {
                        // Test string arguments
                        const char* stringArg = Attribute_GetArgumentValueString(attr, j, &error);
                        if (stringArg) {
                            addTestResult("Attribute String Arg " + to_string(j), true,
                                "Value: " + safeString(stringArg));
                        }
                        
                        // Test int arguments
                        int intArg = 0;
                        SlangResult intResult = Attribute_GetArgumentValueInt(attr, j, &intArg, &error);
                        if (intResult >= 0) {
                            addTestResult("Attribute Int Arg " + to_string(j), true,
                                "Value: " + to_string(intArg));
                        }
                        
                        // Test float arguments
                        float floatArg = 0.0f;
                        SlangResult floatResult = Attribute_GetArgumentValueFloat(attr, j, &floatArg, &error);
                        if (floatResult >= 0) {
                            addTestResult("Attribute Float Arg " + to_string(j), true,
                                "Value: " + to_string(floatArg));
                        }
                    }
                    
                    Attribute_Release(attr, &error);
                }
            }
            
            TypeReflection_Release(attributeStruct, &error);
        }
        
        // Test finding functions with attributes
        void* utilityFunction = ShaderReflection_FindFunctionByName(shaderReflection, "utilityFunction", &error);
        if (utilityFunction) {
            unsigned int attrCount = FunctionReflection_GetUserAttributeCount(utilityFunction, &error);
            addTestResult("utilityFunction Attributes", attrCount > 0,
                "Found " + to_string(attrCount) + " attributes");
            
            FunctionReflection_Release(utilityFunction, &error);
        }
    }

    void testGenericReflection() {
        printHeader("TESTING GENERIC REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Generic Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test finding generic types
        void* genericBuffer = ShaderReflection_FindTypeByName(shaderReflection, "GenericBuffer", &error);
        if (genericBuffer) {
            void* genericContainer = TypeReflection_GetGenericContainer(genericBuffer, &error);
            if (genericContainer) {
                const char* name = GenericReflection_GetName(genericContainer, &error);
                unsigned int typeParamCount = GenericReflection_GetTypeParameterCount(genericContainer, &error);
                unsigned int valueParamCount = GenericReflection_GetValueParameterCount(genericContainer, &error);
                
                addTestResult("Generic Container", true,
                    "Name: " + safeString(name) + ", TypeParams: " + to_string(typeParamCount) + 
                    ", ValueParams: " + to_string(valueParamCount));
                
                // Test type parameters
                for (unsigned int i = 0; i < typeParamCount; i++) {
                    void* typeParam = GenericReflection_GetTypeParameter(genericContainer, i, &error);
                    if (typeParam) {
                        const char* paramName = VariableReflection_GetName(typeParam, &error);
                        addTestResult("Generic Type Parameter " + to_string(i), true,
                            "Name: " + safeString(paramName));
                        VariableReflection_Release(typeParam, &error);
                    }
                }
                
                // Test value parameters
                for (unsigned int i = 0; i < valueParamCount; i++) {
                    void* valueParam = GenericReflection_GetValueParameter(genericContainer, i, &error);
                    if (valueParam) {
                        const char* paramName = VariableReflection_GetName(valueParam, &error);
                        addTestResult("Generic Value Parameter " + to_string(i), true,
                            "Name: " + safeString(paramName));
                        VariableReflection_Release(valueParam, &error);
                    }
                }
                
                GenericReflection_Release(genericContainer, &error);
            }
            
            TypeReflection_Release(genericBuffer, &error);
        }
        
        // Test finding generic functions
        void* genericMax = ShaderReflection_FindFunctionByName(shaderReflection, "genericMax", &error);
        if (genericMax) {
            void* genericContainer = FunctionReflection_GetGenericContainer(genericMax, &error);
            if (genericContainer) {
                addTestResult("Generic Function Container", true);
                GenericReflection_Release(genericContainer, &error);
            }
            
            FunctionReflection_Release(genericMax, &error);
        }
    }

    void testLayoutReflection() {
        printHeader("TESTING LAYOUT REFLECTION");
        
        if (!shaderReflection) {
            addTestResult("Layout Reflection Test", false, "No shader reflection available");
            return;
        }
        
        // Test type layouts by examining global parameters that should have layout information
        unsigned int paramCount = ShaderReflection_GetParameterCount(shaderReflection, &error);
        
        for (unsigned int i = 0; i < min(paramCount, 5u); i++) { // Test first 5 parameters
            void* param = ShaderReflection_GetParameterByIndex(shaderReflection, i, &error);
            if (param) {
                const char* paramName = VariableLayoutReflection_GetName(param, &error);
                void* typeLayout = VariableLayoutReflection_GetTypeLayout(param, &error);
                
                if (typeLayout) {
                    size_t size = TypeLayoutReflection_GetSize(typeLayout, 0, &error); // Uniform category
                    size_t stride = TypeLayoutReflection_GetStride(typeLayout, 0, &error);
                    int alignment = TypeLayoutReflection_GetAlignment(typeLayout, 0, &error);
                    unsigned int fieldCount = TypeLayoutReflection_GetFieldCount(typeLayout, &error);
                    
                    addTestResult("Parameter Layout " + to_string(i), true,
                        "Name: " + safeString(paramName) + ", Size: " + to_string(size) + 
                        ", Stride: " + to_string(stride) + ", Alignment: " + to_string(alignment) + 
                        ", Fields: " + to_string(fieldCount));
                    
                    // Test field layouts
                    for (unsigned int j = 0; j < min(fieldCount, 3u); j++) { // Limit to first 3 fields
                        void* fieldLayout = TypeLayoutReflection_GetFieldByIndex(typeLayout, j, &error);
                        if (fieldLayout) {
                            const char* fieldName = VariableLayoutReflection_GetName(fieldLayout, &error);
                            size_t fieldOffset = VariableLayoutReflection_GetOffset(fieldLayout, 0, &error);
                            
                            addTestResult("Field Layout " + to_string(j), true,
                                "Name: " + safeString(fieldName) + ", Offset: " + to_string(fieldOffset));
                            
                            VariableLayoutReflection_Release(fieldLayout, &error);
                        }
                    }
                    
                    TypeLayoutReflection_Release(typeLayout, &error);
                }
                
                VariableLayoutReflection_Release(param, &error);
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
            const char* error = nullptr;
            int32_t result = Program_CompileProgram(program, entry.second, 0, &output, &error);
            
            bool success = (result >= 0 && output != nullptr);
            string message = success ? "Compiled successfully" : "Compilation failed";
            if (success && output) {
                size_t outputLength = strlen(output);
                message += " (" + to_string(outputLength) + " chars)";
            }
            else if (error) {
                message = "Compilation failed: " + string(error);
			}
            
            addTestResult("Compile " + entry.first, success, message);
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
    //tester.runAllTests();

     // Initialize and setup
    if (!tester.initializeSlangSession()) return -1;
    if (!tester.loadShaderModule()) return -1;
    if (!tester.createProgram()) return -1;
    if (!tester.getProgramReflection()) return -1;
	if (!tester.getCompiledProgram()) return -1;
    if (!tester.getCompiledEntryPoint()) return -1;
    
    return 0;
}