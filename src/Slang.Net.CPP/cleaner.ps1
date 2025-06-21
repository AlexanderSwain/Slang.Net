param(
    [Parameter(Mandatory=$false)]
    [string]$Configuration = "All",
    
    [Parameter(Mandatory=$false)]
    [string]$Platform = "All",

    [switch]$FromVisualStudio
)

Write-Host "===== Cleaning Slang.Net.CPP Library Output =====" -ForegroundColor DarkGray

# Directories
$projectDir = $PSScriptRoot
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

# Files to clean from output directories
$outputFiles = @(
    "Slang.Net.CPP.dll",
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
        
        # Output directory
        $outputDir = Join-Path $projectDir "bin\$config\net9.0\$plat"
        $intermediateDir = Join-Path $projectDir "$(ProjectName)\$plat\$config"
        
        # Clean output directory
        Write-Host "Cleaning directory: $outputDir" -ForegroundColor Green
        if (Test-Path $outputDir) {
            Write-Host "  Directory exists, removing contents..." -ForegroundColor DarkGray
            try {
                foreach ($file in $outputFiles) {
                    $fullPath = Join-Path $outputDir $file
                    if (Test-Path $fullPath) {
                        Write-Host "  Removing: $fullPath" -ForegroundColor DarkGray
                        Remove-Item -Path $fullPath -Force -ErrorAction Stop
                    }
                }
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
    }
}

Write-Host "Clean complete." -ForegroundColor Green
exit 0
