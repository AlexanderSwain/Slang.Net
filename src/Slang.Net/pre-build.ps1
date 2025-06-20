param(
    [Parameter(Mandatory=$true)]
    [string]$Configuration,
    
    [Parameter(Mandatory=$true)]
    [string]$Platform,

    [switch]$FromVisualStudio
)

# Validate platform parameter
$validPlatforms = @("x64", "ARM64")
if ($validPlatforms -notcontains $Platform) {
    Write-Host "Error: Invalid platform '$Platform'. Valid platforms are: $($validPlatforms -join ', ')" -ForegroundColor Red
    exit 1
}

Write-Host "===== Pre-build Event: Slang.Net =====" -ForegroundColor Cyan

# Directories
$nativeOutputDir = "$PSScriptRoot\..\Slang.Net.CPP\bin\$Configuration\net9.0\$Platform"
$slangNetOutputDir = "$PSScriptRoot\bin\$Configuration\net9.0\"
$slangNetLibDir = "$PSScriptRoot\lib\$Configuration\$Platform\"

# Create output directory if it doesn't exist
if (-not (Test-Path -Path $slangNetOutputDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetOutputDir -Force | Out-Null
    Write-Host "Created directory: $slangNetOutputDir" -ForegroundColor Yellow
}

# Create output directory if it doesn't exist
if (-not (Test-Path -Path $slangNetLibDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetLibDir -Force | Out-Null
    Write-Host "Created directory: $slangNetLibDir" -ForegroundColor Yellow
}

# STEP 1: Copy native files to Slang.Net output directory
Write-Host "Build Slang.Net.CPP(STEP 1): Copy native files..." -ForegroundColor Green

# Check alternative locations
$nativeOutputFiles = @(
    "$nativeOutputDir\gfx.dll",
    "$nativeOutputDir\slang.dll",
    "$nativeOutputDir\Slang.Net.CPP.dll",
    "$nativeOutputDir\Slang.Net.CPP.dll.metagen",
    "$nativeOutputDir\Slang.Net.CPP.pdb"
    "$nativeOutputDir\slang-glslang.dll",
    "$nativeOutputDir\slang-glsl-module.dll",
    "$nativeOutputDir\slang-llvm.dll",
    "$nativeOutputDir\SlangNative.dll",
    "$nativeOutputDir\SlangNative.lib",
    "$nativeOutputDir\slang-rt.dll"
)

foreach ($file in $nativeOutputFiles) {
    if (Test-Path $file) {
        //Copy-Item $file $slangNetOutputDir -Force
        //Write-Host "Copied: $file to $slangNetOutputDir" -ForegroundColor Black

        Copy-Item $file $slangNetLibDir -Force
        Write-Host "Copied: $file to $slangNetLibDir" -ForegroundColor Black
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net.CPP build failed due to missing Native file!" -ForegroundColor Red
        exit 1
    }
}

