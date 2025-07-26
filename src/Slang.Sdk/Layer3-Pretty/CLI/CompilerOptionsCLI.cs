using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slang.Sdk
{
    public partial class SlangC_Options
    {
        public string? Target { get; set; }
        public string? Profile { get; set; }
        public string? Entry { get; set; }
        public string? Stage { get; set; }
        public string? OutputPath { get; set; }
        public string? ModuleName { get; set; }
        public List<string> IncludePaths { get; set; } = new();
        public Dictionary<string, string>? Defines { get; set; }
        public string? Std { get; set; }
        public bool WarningsAsErrors { get; set; }
        public List<string> DisableWarnings { get; set; } = new();
        public string? OptimizationLevel { get; set; }
        public bool EmitIR { get; set; }
        public bool DebugInfo { get; set; }
        public string? ReflectionJsonPath { get; set; }
        public List<string> InputFiles { get; set; } = new();

        public override string ToString()
        {
            var builder = new StringBuilder();

            void Append(string flag, string? value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    builder.Append($" {flag} {Quote(value)}");
            }

            string Quote(string str) => $"\"{str}\"";

            Append("-target", Target);
            Append("-profile", Profile);
            Append("-entry", Entry);
            Append("-stage", Stage);
            Append("-o", OutputPath);
            Append("-module-name", ModuleName);
            Append("-std", Std);
            
            // Handle optimization level specially - it should be -O3, -O2, etc. (no space)
            if (!string.IsNullOrWhiteSpace(OptimizationLevel))
                builder.Append($" -O{OptimizationLevel}");
                
            Append("-reflection-json", ReflectionJsonPath);

            if (WarningsAsErrors)
                builder.Append(" -warnings-as-errors");

            if (EmitIR)
                builder.Append(" -emit-ir");

            if (DebugInfo)
                builder.Append(" -g");

            if (IncludePaths != null)
            {
                foreach (var path in IncludePaths)
                    builder.Append($" -I {Quote(path)}");
            }

            if (DisableWarnings != null)
            {
                foreach (var id in DisableWarnings)
                    builder.Append($" -warnings-disable {Quote(id)}");
            }

            if (Defines != null)
            {
                foreach (var kvp in Defines)
                    builder.Append($" -D {Quote($"{kvp.Key}={kvp.Value}")}");
            }

            if (InputFiles != null && InputFiles.Count > 0)
            {
                builder.Append(" --");
                foreach (var file in InputFiles)
                    builder.Append($" {Quote(file)}");
            }

            return builder.ToString();

        }
    }
}
