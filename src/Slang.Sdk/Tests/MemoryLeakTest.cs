using Slang.Sdk.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk.Tests
{
    internal class MemoryLeakTest
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== Starting Memory Leak Tests ===");
            
            bool allTestsPassed = true;
            
            // Run all memory leak tests
            allTestsPassed &= MemLeak_Session1Module5_100Times();
            allTestsPassed &= MemLeak_SessionModuleDisposeTest();
            allTestsPassed &= MemLeak_ModuleWithProgram_50Times();
            allTestsPassed &= MemLeak_MultipleSessionsTest();
            
            // Force garbage collection to ensure finalizers run
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine("=== Memory Leak Tests Completed ===");
            Console.WriteLine($"Overall Result: {(allTestsPassed ? "PASS" : "FAIL")}");
            
            if (!allTestsPassed)
            {
                Console.WriteLine("⚠️  One or more memory leak tests failed!");
                Environment.ExitCode = 1;
            }
            else
            {
                Console.WriteLine("✅ All memory leak tests passed!");
            }
        }

        public static bool MemLeak_Session1Module5_100Times()
        {
            Console.WriteLine("Running: Session with 5 Modules - 100 iterations");
            
            var memoryBefore = GC.GetTotalMemory(true);
            
            

            for (int i = 0; i < 10000; i++)
            {
                // Create a session with compiler options and search paths
                Session.Builder builder = new Session.Builder()
                    .AddCompilerOption(CompilerOption.Name.WarningsAsErrors, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 0, 0, "all", null))
                    .AddCompilerOption(CompilerOption.Name.Obfuscate, new CompilerOption.Value(CompilerOption.Value.Kind.Int, 1, 0, null, null))
                    .AddPreprocessorMacro($"LIGHTING_SCALER{i}", "i")
                    .AddTarget(Targets.Hlsl.cs_5_0)
                    .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");

                // Create the session
                Session session1 = builder.Create();

                // Create modules list for proper disposal
                List<Module> modules = new List<Module>();

                try
                {
                    // Load the modules from the specified file, different names should prevent caching issues
                    modules.Add(session1.LoadModule($"AverageColor{i * 5 + 0}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang"));
                    modules.Add(session1.LoadModule($"AverageColor{i * 5 + 1}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang"));
                    modules.Add(session1.LoadModule($"AverageColor{i * 5 + 2}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang"));
                    modules.Add(session1.LoadModule($"AverageColor{i * 5 + 3}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang"));
                    modules.Add(session1.LoadModule($"AverageColor{i * 5 + 4}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang"));
                }
                finally
                {
                    // Dispose session (which should handle modules)
                    session1.Dispose();
                }
                
                // Periodic garbage collection to help track memory usage
                if (i % 25 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var memoryDuring = GC.GetTotalMemory(false);
                    Console.WriteLine($"Iteration {i}: Memory usage: {memoryDuring / 1024.0 / 1024.0:F2} MB");
                }
            }
            
            // Final garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = (memoryAfter - memoryBefore) / 1024.0 / 1024.0;
            
            Console.WriteLine($"Memory before: {memoryBefore / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory after: {memoryAfter / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory difference: {memoryDiff:F2} MB");
            
            // Accept up to 5MB of memory growth as normal (GC overhead, JIT, etc.)
            bool passed = memoryDiff < 5.0;
            Console.WriteLine($"✓ Session1Module5_100Times: {(passed ? "PASS" : "FAIL")}");
            
            return passed;
        }

        public static bool MemLeak_SessionModuleDisposeTest()
        {
            Console.WriteLine("Running: Session and Module Dispose Test");
            
            var memoryBefore = GC.GetTotalMemory(true);
            
            for (int i = 0; i < 50; i++)
            {
                Session.Builder builder = new Session.Builder()
                    .AddTarget(Targets.Hlsl.cs_5_0)
                    .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");

                using (Session session = builder.Create())
                {
                    Module module = session.LoadModule("AverageColor", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
                    
                    // Use the module (access some properties)
                    var name = module.Name;
                    var program = module.Program;
                    
                    // Module should be disposed automatically when session is disposed
                }
                // Session automatically disposed by using statement
            }
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = (memoryAfter - memoryBefore) / 1024.0 / 1024.0;
            
            Console.WriteLine($"Memory before: {memoryBefore / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory after: {memoryAfter / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory difference: {memoryDiff:F2} MB");
            
            // Accept up to 3MB of memory growth
            bool passed = memoryDiff < 3.0;
            Console.WriteLine($"✓ SessionModuleDisposeTest: {(passed ? "PASS" : "FAIL")}");
            
            return passed;
        }

        public static bool MemLeak_ModuleWithProgram_50Times()
        {
            Console.WriteLine("Running: Module with Program - 50 iterations");
            
            var memoryBefore = GC.GetTotalMemory(true);
            
            Session.Builder builder = new Session.Builder()
                .AddTarget(Targets.Hlsl.cs_5_0)
                .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");

            using (Session session = builder.Create())
            {
                for (int i = 0; i < 50; i++)
                {
                    Module module = session.LoadModule("AverageColor", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
                    
                    // Access the program to ensure it's created
                    var program = module.Program;
                    
                    // Modules are managed by the session and will be cleaned up when session disposes
                    
                    if (i % 10 == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        var memoryDuring = GC.GetTotalMemory(false);
                        Console.WriteLine($"Iteration {i}: Memory usage: {memoryDuring / 1024.0 / 1024.0:F2} MB");
                    }
                }
            }
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = (memoryAfter - memoryBefore) / 1024.0 / 1024.0;
            
            Console.WriteLine($"Memory before: {memoryBefore / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory after: {memoryAfter / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory difference: {memoryDiff:F2} MB");
            
            // Accept up to 3MB of memory growth
            bool passed = memoryDiff < 3.0;
            Console.WriteLine($"✓ ModuleWithProgram_50Times: {(passed ? "PASS" : "FAIL")}");
            
            return passed;
        }

        public static bool MemLeak_MultipleSessionsTest()
        {
            Console.WriteLine("Running: Multiple Sessions Test - 25 iterations");
            
            var memoryBefore = GC.GetTotalMemory(true);
            
            for (int i = 0; i < 25; i++)
            {
                // Create multiple sessions with different configurations
                Session.Builder builder1 = new Session.Builder()
                    .AddTarget(Targets.Hlsl.cs_5_0)
                    .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");
                
                Session.Builder builder2 = new Session.Builder()
                    .AddTarget(Targets.Hlsl.cs_6_0)
                    .AddPreprocessorMacro("TEST_MACRO", "1")
                    .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");

                using (Session session1 = builder1.Create())
                using (Session session2 = builder2.Create())
                {
                    // Load modules in both sessions
                    Module module1 = session1.LoadModule("AverageColor1{i}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
                    Module module2 = session2.LoadModule("AverageColor2{i}", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
                    
                    // Access programs
                    var program1 = module1.Program;
                    var program2 = module2.Program;
                    
                    // Test module equality (should be different)
                    bool modulesDifferent = module1 != module2;
                    if (!modulesDifferent)
                    {
                        Console.WriteLine($"Warning: Modules should be different between sessions");
                    }
                }
                
                if (i % 5 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var memoryDuring = GC.GetTotalMemory(false);
                    Console.WriteLine($"Iteration {i}: Memory usage: {memoryDuring / 1024.0 / 1024.0:F2} MB");
                }
            }
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = (memoryAfter - memoryBefore) / 1024.0 / 1024.0;
            
            Console.WriteLine($"Memory before: {memoryBefore / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory after: {memoryAfter / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory difference: {memoryDiff:F2} MB");
            
            // Accept up to 4MB of memory growth for multiple sessions
            bool passed = memoryDiff < 4.0;
            Console.WriteLine($"✓ MultipleSessionsTest: {(passed ? "PASS" : "FAIL")}");
            
            return passed;
        }

        /// <summary>
        /// Stress test to detect potential memory leaks under high load
        /// </summary>
        public static bool MemLeak_StressTest()
        {
            Console.WriteLine("Running: Stress Test - 200 iterations with rapid creation/disposal");
            
            var memoryBefore = GC.GetTotalMemory(true);
            
            // Run a more intensive test
            for (int batch = 0; batch < 20; batch++)
            {
                List<Session> sessions = new List<Session>();
                
                try
                {
                    // Create multiple sessions rapidly
                    for (int i = 0; i < 10; i++)
                    {
                        Session.Builder builder = new Session.Builder()
                            .AddTarget(Targets.Hlsl.cs_5_0)
                            .AddSearchPath($@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\");
                        
                        Session session = builder.Create();
                        sessions.Add(session);
                        
                        // Load module
                        Module module = session.LoadModule("AverageColor", $@"{AppDomain.CurrentDomain.BaseDirectory}Tests\Shaders\AverageColor.slang");
                        var program = module.Program; // Force program creation
                    }
                }
                finally
                {
                    // Dispose all sessions
                    foreach (var session in sessions)
                    {
                        session.Dispose();
                    }
                }
                
                // Force garbage collection between batches
                if (batch % 5 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var memoryDuring = GC.GetTotalMemory(false);
                    Console.WriteLine($"Batch {batch}: Memory usage: {memoryDuring / 1024.0 / 1024.0:F2} MB");
                }
            }
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = (memoryAfter - memoryBefore) / 1024.0 / 1024.0;
            
            Console.WriteLine($"Memory before: {memoryBefore / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory after: {memoryAfter / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"Memory difference: {memoryDiff:F2} MB");
            
            // Accept up to 6MB of memory growth for stress test
            bool passed = memoryDiff < 6.0;
            Console.WriteLine($"✓ StressTest: {(passed ? "PASS" : "FAIL")}");
            
            return passed;
        }
    }
}
