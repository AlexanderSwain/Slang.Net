using System;
using System.Linq;
using Slang;

namespace Slang.Net.Examples
{
    /// <summary>
    /// Example program demonstrating the new IEnumerable functionality in Slang.Net
    /// This shows how the "get by index" methods are now hidden behind convenient IEnumerable abstractions
    /// </summary>
    class IEnumerableExamples
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Slang.Net IEnumerable Examples");
            Console.WriteLine("==============================");
            
            // Note: This is example code showing the API usage
            // Actual execution would require a proper Slang reflection setup
            
            ExampleTypeReflectionUsage();
            ExampleShaderReflectionUsage();
            ExampleFunctionReflectionUsage();
        }

        static void ExampleTypeReflectionUsage()
        {
            Console.WriteLine("\n1. TypeReflection Examples:");
            Console.WriteLine("---------------------------");
            
            // Before: Manual index iteration
            Console.WriteLine("// OLD WAY (still supported):");
            Console.WriteLine("for (uint i = 0; i < typeReflection.FieldCount; i++)");
            Console.WriteLine("{");
            Console.WriteLine("    var field = typeReflection.GetFieldByIndex(i);");
            Console.WriteLine("    Console.WriteLine($\"Field: {field.Name}\");");
            Console.WriteLine("}");
            
            // After: IEnumerable with foreach
            Console.WriteLine("\n// NEW WAY - foreach:");
            Console.WriteLine("foreach (var field in typeReflection.Fields)");
            Console.WriteLine("{");
            Console.WriteLine("    Console.WriteLine($\"Field: {field.Name}\");");
            Console.WriteLine("}");
            
            // After: IEnumerable with LINQ
            Console.WriteLine("\n// NEW WAY - LINQ:");
            Console.WriteLine("var publicFields = typeReflection.Fields");
            Console.WriteLine("    .Where(f => f.Name.StartsWith(\"m_\"))");
            Console.WriteLine("    .Select(f => f.Name)");
            Console.WriteLine("    .ToList();");
            
            Console.WriteLine("\n// Count fields easily:");
            Console.WriteLine("int fieldCount = typeReflection.Fields.Count();");
            
            Console.WriteLine("\n// Find specific field:");
            Console.WriteLine("var positionField = typeReflection.Fields");
            Console.WriteLine("    .FirstOrDefault(f => f.Name == \"position\");");
        }

        static void ExampleShaderReflectionUsage()
        {
            Console.WriteLine("\n\n2. ShaderReflection Examples:");
            Console.WriteLine("-----------------------------");
            
            Console.WriteLine("// Iterate through all entry points:");
            Console.WriteLine("foreach (var entryPoint in shaderReflection.EntryPoints)");
            Console.WriteLine("{");
            Console.WriteLine("    Console.WriteLine($\"Entry Point: {entryPoint.Name} (Stage: {entryPoint.Stage})\");");
            Console.WriteLine("    ");
            Console.WriteLine("    // Iterate through parameters of each entry point");
            Console.WriteLine("    foreach (var param in entryPoint.Parameters)");
            Console.WriteLine("    {");
            Console.WriteLine("        Console.WriteLine($\"  Parameter: {param.Name}\");");
            Console.WriteLine("    }");
            Console.WriteLine("}");
            
            Console.WriteLine("\n// Find vertex shader entry points:");
            Console.WriteLine("var vertexShaders = shaderReflection.EntryPoints");
            Console.WriteLine("    .Where(ep => ep.Stage == SlangStage.Vertex)");
            Console.WriteLine("    .ToList();");
            
            Console.WriteLine("\n// Get all parameter names:");
            Console.WriteLine("var allParamNames = shaderReflection.Parameters");
            Console.WriteLine("    .Select(p => p.Name)");
            Console.WriteLine("    .ToArray();");
        }

        static void ExampleFunctionReflectionUsage()
        {
            Console.WriteLine("\n\n3. FunctionReflection Examples:");
            Console.WriteLine("-------------------------------");
            
            Console.WriteLine("// Check function parameters:");
            Console.WriteLine("foreach (var parameter in functionReflection.Parameters)");
            Console.WriteLine("{");
            Console.WriteLine("    Console.WriteLine($\"Parameter: {parameter.Name} of type {parameter.Type.Name}\");");
            Console.WriteLine("}");
            
            Console.WriteLine("\n// Find functions with specific attributes:");
            Console.WriteLine("var deprecatedFunctions = functionReflection.UserAttributes");
            Console.WriteLine("    .Where(attr => attr.Name == \"deprecated\")");
            Console.WriteLine("    .Any();");
            
            Console.WriteLine("\n// Work with function overloads:");
            Console.WriteLine("if (functionReflection.IsOverloaded)");
            Console.WriteLine("{");
            Console.WriteLine("    Console.WriteLine($\"Function has {functionReflection.Overloads.Count()} overloads:\");");
            Console.WriteLine("    foreach (var overload in functionReflection.Overloads)");
            Console.WriteLine("    {");
            Console.WriteLine("        var paramTypes = overload.Parameters");
            Console.WriteLine("            .Select(p => p.Type.Name)");
            Console.WriteLine("            .ToArray();");
            Console.WriteLine("        Console.WriteLine($\"  Overload({string.Join(\", \", paramTypes)})\");");
            Console.WriteLine("    }");
            Console.WriteLine("}");
        }
    }
    
    /// <summary>
    /// Additional examples showing advanced LINQ usage with the new IEnumerable properties
    /// </summary>
    class AdvancedLinqExamples
    {
        static void AdvancedTypeAnalysis()
        {
            Console.WriteLine("\n\n4. Advanced LINQ Examples:");
            Console.WriteLine("--------------------------");
            
            // Example: Complex type analysis
            Console.WriteLine("// Find all struct types with more than 5 fields:");
            Console.WriteLine("// (This would be used in actual code with a collection of TypeReflection objects)");
            Console.WriteLine("var complexStructs = allTypes");
            Console.WriteLine("    .Where(t => t.Kind == TypeKind.Struct)");
            Console.WriteLine("    .Where(t => t.Fields.Count() > 5)");
            Console.WriteLine("    .Select(t => new { Name = t.Name, FieldCount = t.Fields.Count() })");
            Console.WriteLine("    .OrderByDescending(t => t.FieldCount);");
            
            // Example: Attribute analysis
            Console.WriteLine("\n// Find all types with specific attributes:");
            Console.WriteLine("var serializedTypes = allTypes");
            Console.WriteLine("    .Where(t => t.UserAttributes.Any(attr => attr.Name == \"Serializable\"))");
            Console.WriteLine("    .ToList();");
            
            // Example: Cross-type analysis
            Console.WriteLine("\n// Find relationships between types:");
            Console.WriteLine("var typeRelationships = allTypes");
            Console.WriteLine("    .SelectMany(t => t.Fields, (type, field) => new { Type = type, Field = field })");
            Console.WriteLine("    .Where(tf => tf.Field.Type.Name.EndsWith(\"Texture\"))");
            Console.WriteLine("    .GroupBy(tf => tf.Field.Type.Name)");
            Console.WriteLine("    .Select(g => new { TextureType = g.Key, UsedByTypes = g.Select(x => x.Type.Name).ToList() });");
        }
    }
}
