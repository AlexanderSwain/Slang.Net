using System.Runtime.InteropServices;

namespace Slang
{
    public static class Runtime
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

        public static string AsString()
        {
            return GetRuntime();
        }

        private static string GetRuntimeFolderName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Use effective architecture detection which handles ARM64 fallback
                var architecture = RuntimeInformation.OSArchitecture;

                return architecture switch
                {
                    Architecture.X64 => "win-x64",
                    Architecture.Arm64 => "win-arm64",
                    Architecture.X86 => "win-x86",
                    _ => "win-x64" // Default fallback
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return RuntimeInformation.OSArchitecture switch
                {
                    Architecture.X64 => "linux-x64",
                    Architecture.Arm64 => "linux-arm64",
                    _ => "linux-x64"
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return RuntimeInformation.OSArchitecture switch
                {
                    Architecture.X64 => "osx-x64",
                    Architecture.Arm64 => "osx-arm64",
                    _ => "osx-x64"
                };
            }

            return "win-x64"; // Default fallback
        }

        private static string GetRuntime()
        {
            var architecture = RuntimeInformation.OSArchitecture;

            return architecture switch
            {
                Architecture.X64 => "x64",
                Architecture.Arm64 => "ARM64",
                Architecture.X86 => "x86",
                _ => "x64" // Default fallback
            };
        }

        // Delete this
        //private static Architecture GetEffectiveArchitecture()
        //{
        //    var reportedArchitecture = RuntimeInformation.ProcessArchitecture;

        //    // On Windows ARM64 systems, .NET may report ARM64 even when running x64 emulation
        //    // We need to check if the appropriate runtime files exist
        //    if (reportedArchitecture == Architecture.Arm64 && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //    {
        //        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //        // Check if ARM64 runtime folder exists and contains required files
        //        string arm64RuntimePath = Path.Combine(appDirectory, "runtimes", "win-arm64", "native");
        //        string x64RuntimePath = Path.Combine(appDirectory, "runtimes", "win-x64", "native");

        //        bool arm64RuntimeExists = System.IO.Directory.Exists(arm64RuntimePath) && 
        //                                 File.Exists(Path.Combine(arm64RuntimePath, "slangc.exe"));
        //        bool x64RuntimeExists = System.IO.Directory.Exists(x64RuntimePath) && 
        //                               File.Exists(Path.Combine(x64RuntimePath, "slangc.exe"));

        //        // If ARM64 runtime doesn't exist but x64 does, prefer x64 (emulation scenario)
        //        if (!arm64RuntimeExists && x64RuntimeExists)
        //        {
        //            return Architecture.X64;
        //        }

        //        // Check if this is an emulated x64 process on ARM64 by examining environment
        //        // WOW64 processes will have PROCESSOR_ARCHITEW6432 set to ARM64
        //        string? processorArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
        //        if (processorArchitecture == "ARM64")
        //        {
        //            // This is likely an x64 process running under emulation
        //            // Prefer x64 runtime if available
        //            if (x64RuntimeExists)
        //            {
        //                return Architecture.X64;
        //            }
        //        }
        //    }

        //    return reportedArchitecture;
        //}
    }
}
