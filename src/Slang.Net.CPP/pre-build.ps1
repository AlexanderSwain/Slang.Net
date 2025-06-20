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

Write-Host "===== Building Slang.Net.CPP Managed Library =====" -ForegroundColor DarkGray

# Directories
$nativeOutputDir = "$PSScriptRoot\..\Native\bin\$Configuration\$Platform"
$slangNetCppOutputDir = "$PSScriptRoot\bin\$Configuration\net9.0\$Platform"
if (-not (Test-Path -Path $slangNetCppOutputDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetCppOutputDir -Force | Out-Null
    Write-Host "Created directory: $slangNetCppOutputDir" -ForegroundColor Yellow
}

# STEP 1: Copy native files to Slang.Net.CPP output directory
Write-Host "Build Slang.Net.CPP(STEP 1): Copy native files..." -ForegroundColor Green

# Check alternative locations
$nativeOutputFiles = @(
    "$nativeOutputDir\gfx.dll",
    "$nativeOutputDir\slang.dll",
    "$nativeOutputDir\slang-glslang.dll",
    "$nativeOutputDir\slang-glsl-module.dll",
    "$nativeOutputDir\slang-llvm.dll",
    "$nativeOutputDir\SlangNative.dll",
    "$nativeOutputDir\SlangNative.lib",
    "$nativeOutputDir\slang-rt.dll"
)

foreach ($file in $nativeOutputFiles) {
    if (Test-Path $file) {
        Copy-Item $file $slangNetCppOutputDir -Force
        Write-Host "Copied: $file to $slangNetCppOutputDir" -ForegroundColor Black
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net.CPP build failed due to missing Native file!" -ForegroundColor Red
        exit 1
    }
}

