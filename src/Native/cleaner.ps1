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

# Print script location and parameters for debugging
Write-Host "Script running from: $PSScriptRoot" -ForegroundColor Magenta
Write-Host "Configuration: $Configuration" -ForegroundColor Magenta
Write-Host "Platform: $Platform" -ForegroundColor Magenta

# If a specific platform/configuration was specified, only clean that
if ($Platform -ne "All") {
    $platforms = @($Platform)
    Write-Host "Filtering to platform: $Platform" -ForegroundColor Magenta
}

if ($Configuration -ne "All") {
    $configurations = @($Configuration)
    Write-Host "Filtering to configuration: $Configuration" -ForegroundColor Magenta
}

Write-Host "Cleaning platforms: $($platforms -join ", ")" -ForegroundColor Cyan
Write-Host "Cleaning configurations: $($configurations -join ", ")" -ForegroundColor Cyan

# Native files to clean from output directories
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

# Clean directories for each platform and configuration
foreach ($plat in $platforms) {
    foreach ($config in $configurations) {
        Write-Host "Processing: $config | $plat" -ForegroundColor Cyan
        
        # Output and intermediate directories
        $outputDir = Join-Path $nativeDir "bin\$config\$plat"
        $intermediateDir = Join-Path $nativeDir "obj\$config\$plat"
        $slangNetOutputDir = Join-Path $nativeDir "..\Slang.Net\bin\$config\net9.0"
        
        # Clean output directory
        Write-Host "Cleaning directory: $outputDir" -ForegroundColor Green
        if (Test-Path $outputDir) {
            Write-Host "  Directory exists, removing contents..." -ForegroundColor DarkGray
            try {
                Remove-Item -Path "$outputDir\*" -Force -Recurse -ErrorAction Stop
                Write-Host "  Successfully cleaned output directory." -ForegroundColor Green
            } catch {
                $errorMsg = $_.Exception.Message
                Write-Host "  ERROR cleaning output directory: $errorMsg" -ForegroundColor Red
            }
        } else {
            Write-Host "  Directory does not exist: $outputDir" -ForegroundColor Yellow
        }
        
        # Clean intermediate directory
        Write-Host "Cleaning directory: $intermediateDir" -ForegroundColor Green
        if (Test-Path $intermediateDir) {
            Write-Host "  Directory exists, removing contents..." -ForegroundColor DarkGray
            try {
                Remove-Item -Path "$intermediateDir\*" -Force -Recurse -ErrorAction Stop
                Write-Host "  Successfully cleaned intermediate directory." -ForegroundColor Green
            } catch {
                $errorMsg = $_.Exception.Message
                Write-Host "  ERROR cleaning intermediate directory: $errorMsg" -ForegroundColor Red
            }
        } else {
            Write-Host "  Directory does not exist: $intermediateDir" -ForegroundColor Yellow
        }
        
        # Clean SlangNet output directory
        Write-Host "Cleaning SlangNet output directory: $slangNetOutputDir" -ForegroundColor Green
        if (Test-Path $slangNetOutputDir) {
            Write-Host "  Directory exists, removing native files..." -ForegroundColor DarkGray
            foreach ($file in $nativeFiles) {
                $fullPath = Join-Path $slangNetOutputDir $file
                if (Test-Path $fullPath) {
                    Write-Host "  Removing: $fullPath" -ForegroundColor DarkGray
                    try {
                        Remove-Item -Path $fullPath -Force -ErrorAction Stop
                        Write-Host "  Successfully removed $file." -ForegroundColor Green
                    } catch {
                        $errorMsg = $_.Exception.Message
                        Write-Host "  ERROR removing file: $errorMsg" -ForegroundColor Red
                    }
                } else {
                    Write-Host "  File does not exist, skipping: $file" -ForegroundColor Yellow
                }
            }
        } else {
            Write-Host "  SlangNet output directory does not exist: $slangNetOutputDir" -ForegroundColor Yellow
        }
    }
}

# Clean EmbeddedLLVM folder with downloaded dependencies if it exists
if ($Platform -eq "All" -and $Configuration -eq "All") {
    $llvmDir = Join-Path $nativeDir "EmbeddedLLVM"
    if (Test-Path $llvmDir) {
        Write-Host "Cleaning dependencies: $llvmDir" -ForegroundColor Cyan
        try {
            Remove-Item -Path "$llvmDir\*" -Force -Recurse -ErrorAction Stop
            Write-Host "Successfully cleaned LLVM dependencies." -ForegroundColor Green
        } catch {
            $errorMsg = $_.Exception.Message
            Write-Host "ERROR cleaning LLVM dependencies: $errorMsg" -ForegroundColor Red
        }
    } else {
        Write-Host "LLVM directory does not exist, skipping: $llvmDir" -ForegroundColor Yellow
    }
}

Write-Host "Clean complete." -ForegroundColor Green
exit 0
