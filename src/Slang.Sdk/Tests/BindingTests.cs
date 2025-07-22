using Slang.Sdk.Binding;
using Slang.Sdk.Interop;

namespace Slang.Sdk.Tests
{
    internal static class BindingTests
    {
        internal static void RunAllTests()
        {
            Console.WriteLine("=== Testing Session Bindings ===");
            TestSessionBindings();
        }

        public static void TestSessionBindings()
        {
            try
            {
                Session session = new Session(
                [new CompilerOption(CompilerOption.Name.WarningsAsErrors, new CompilerOption.Value(CompilerOption.Value.Kind.String, 0, 0, "All", null))],
                [new PreprocessorMacro("LIGHTING_SCALER", "1.0")],
                [new Target(Target.CompileTarget.Hlsl, "cs_5_0")],
                ["C:\\"]);

                Console.WriteLine($"[Pass]: Successfully Created Session: {session.Handle}");
            }
            catch (SlangException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}