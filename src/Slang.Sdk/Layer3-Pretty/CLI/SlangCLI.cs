using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Slang.CLI
{
    public static class SlangCLI
    {
        public static string WorkingDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public static ResultsCLI slangc(
            string? target = null,
            string? profile = null,
            string? entry = null,
            string? stage = null,
            string? outputPath = null,
            string? moduleName = null,
            string[]? includePaths = null,
            Dictionary<string, string>? defines = null,
            string? std = null,
            bool warningsAsErrors = false,
            string[]? disableWarnings = null,
            string? optimizationLevel = null,
            bool emitIR = false,
            bool debugInfo = false,
            string? reflectionJsonPath = null,
            string[]? inputFiles = null)
        {
            CompilerOptionsCLI args = new CompilerOptionsCLI()
            {
                Target = target,
                Profile = profile,
                Entry = entry,
                Stage = stage,
                OutputPath = outputPath,
                ModuleName = moduleName,
                IncludePaths = includePaths is null ? new() : includePaths.ToList(),
                Defines = defines,
                Std = std,
                WarningsAsErrors = warningsAsErrors,
                DisableWarnings = disableWarnings is null ? new() : disableWarnings.ToList(),
                OptimizationLevel = optimizationLevel,
                EmitIR = emitIR,
                DebugInfo = debugInfo,
                ReflectionJsonPath = reflectionJsonPath,
                InputFiles = inputFiles is null ? new() : inputFiles.ToList(),
            };

            return slangc(args);
        }

        public static ResultsCLI slangc(CompilerOptionsCLI args)
        {
            return InvokeSlangc(args.ToString());
        }

        public static ResultsCLI slangc(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                throw new ArgumentException("Arguments cannot be null or empty.", nameof(args));
            return InvokeSlangc(args);
        }

        private static ResultsCLI InvokeSlangc(string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Runtime.Directory, "slangc.exe"),
                Arguments = args,
                WorkingDirectory = WorkingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            var result = new ResultsCLI();

            var stdoutBuilder = new StringBuilder();
            var stderrBuilder = new StringBuilder();

            process.OutputDataReceived += (_, e) => { if (e.Data != null) stdoutBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) stderrBuilder.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            result.ExitCode = process.ExitCode;
            result.StdOut = stdoutBuilder.ToString();
            result.StdErr = stderrBuilder.ToString();

            return result;
        }

        // TODO: Implement Async feature in later version
    }
}
