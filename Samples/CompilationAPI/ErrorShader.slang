// ErrorShader.slang - A shader with intentional syntax errors for testing error handling

// Missing struct definition - this will cause an error
float4 main(UndefinedStruct input) : SV_Target
{
    // Undefined function call
    float4 color = UndefinedFunction(input.position);
    
    // Invalid syntax
    return color +++ float4(1.0, 0.0, 0.0, 1.0);
}

// Compute shader with errors
[numthreads(8, 8, 1)]
void CS(uint3 id : SV_DispatchThreadID)
{
    // Missing resource declarations
    OutputTexture[id.xy] = float4(1.0, 0.0, 0.0, 1.0); // OutputTexture not defined
    
    // Invalid operation
    int x = "string" + 5; // Type mismatch
}
