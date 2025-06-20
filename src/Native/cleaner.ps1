param(
    [Parameter(Mandatory=$false)]
    [string]$Configuration = "All",
    
    [Parameter(Mandatory=$false)]
    [string]$Platform = "All",

    [switch]$FromVisualStudio
)

Write-Host "===== Cleaning SlangNative Library Output =====" -ForegroundColor DarkGray

# Directories
$nativeDir = $PSScriptRoot
$platforms = @("x64", "ARM64")
$configurations = @("Debug", "Release")

# If a specific platform/configuration was specified, only clean that
if ($Platform -ne "All") {
    $platforms = @($Platform)
}

if ($Configuration -ne "All") {
    $configurations = @($Configuration)
}

Write-Host "Cleaning platforms: $($platforms -join ', ')" -ForegroundColor Cyan
Write-Host "Cleaning configurations: $($configurations -join ', ')" -ForegroundColor Cyan

# Clean directories for each platform and configuration
foreach ($plat in $platforms) {
    foreach ($config in $configurations) {
        $outputDir = Join-Path $nativeDir "bin\$config\$plat"
        $intermediateDir = "$nativeDir\obj\$config\$plat"
        
        # Also clean the target directories in Slang.Net project
        $slangNetOutputDir = Join-Path $nativeDir "..\Slang.Net\bin\$config\net9.0"
        
        Write-Host "Cleaning directory: $outputDir" -ForegroundColor DarkGray
        if (Test-Path $outputDir) {
            Remove-Item -Path "$outputDir\*" -Force -Recurse -ErrorAction SilentlyContinue
        }
        
        Write-Host "Cleaning directory: $intermediateDir" -ForegroundColor DarkGray
        if (Test-Path $intermediateDir) {
            Remove-Item -Path "$intermediateDir\*" -Force -Recurse -ErrorAction SilentlyContinue
        }
        
        # Clean only native DLLs from SlangNet output dir (don't remove managed assemblies)
        $nativeFiles = @(
            "gfx.dll",
            "slang.dll",
            "slang-glslang.dll",
            "slang-glsl-module.dll",
            "slang-llvm.dll",
            "SlangNative.dll",
            "SlangNative.lib",
            "slang-rt.dll"
        )
        
        foreach ($file in $nativeFiles) {
            $fullPath = Join-Path $slangNetOutputDir $file
            if (Test-Path $fullPath) {
                Write-Host "Removing: $fullPath" -ForegroundColor DarkGray
                Remove-Item -Path $fullPath -Force -ErrorAction SilentlyContinue
            }
        }
    }
}

# Clean EmbeddedLLVM folder with downloaded dependencies if it exists
if ($Platform -eq "All" -and $Configuration -eq "All") {
    $llvmDir = Join-Path $nativeDir "EmbeddedLLVM"
    if (Test-Path $llvmDir) {
        Write-Host "Cleaning dependencies: $llvmDir" -ForegroundColor Cyan
        Remove-Item -Path "$llvmDir\*" -Force -Recurse -ErrorAction SilentlyContinue
    }
}

Write-Host "Clean complete." -ForegroundColor Green
exit 0
