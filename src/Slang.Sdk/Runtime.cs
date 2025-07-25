using System.Runtime.InteropServices;

namespace Slang
{
    internal static class Runtime
    {
        public static string Directory
        {
            get
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string runtimeFolder = Path.Combine(appDirectory, "runtimes", GetRuntimeFolderName(), "native");
                return runtimeFolder;
            }
        }

        private static string GetRuntimeFolderName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.Arm64 => "win-arm64",
                    Architecture.X86 => "win-x86",
                    _ => "win-x64" // Default fallback
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "linux-x64",
                    Architecture.Arm64 => "linux-arm64",
                    _ => "linux-x64"
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64 => "osx-x64",
                    Architecture.Arm64 => "osx-arm64",
                    _ => "osx-x64"
                };
            }

            return "win-x64"; // Default fallback
        }
    }
}
