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

# Special validation for ARM64 Debug case
if ($Platform -eq "ARM64" -and $Configuration -eq "Debug") {
    Write-Host "IMPORTANT: Processing the special case of ARM64 Debug configuration" -ForegroundColor Magenta
}

Write-Host "===== Building Slang.Net Nuget Package =====" -ForegroundColor DarkGray

# Print parameters for debugging
Write-Host "DEBUG: -Configuration = $Configuration" -ForegroundColor Yellow
Write-Host "DEBUG: -Platform = $Platform" -ForegroundColor Yellow
Write-Host "DEBUG: -FromVisualStudio = $FromVisualStudio" -ForegroundColor Yellow

# Directories
$slangNetDir = $PSScriptRoot
$cppOutputDir = "$PSScriptRoot\..\Slang.Net.CPP\bin\$Configuration\net9.0\$Platform"
$libDir = "$PSScriptRoot\lib\$Configuration\net9.0\$Platform"

if (-not (Test-Path -Path $libDir)) {
    New-Item -ItemType Directory -Path $libDir -Force | Out-Null
    Write-Host "Created directory: $libDir" -ForegroundColor Yellow
}

# STEP 1: Copy CPP files to Slang.Net output directory
Write-Host "Build Slang.Net(STEP 1): Copy CPP files..." -ForegroundColor Green

# Define CPP files to copy
$cppOutputFiles = @(
    "$cppOutputDir\gfx.dll",
    "$cppOutputDir\slang.dll",
    "$cppOutputDir\Slang.Net.CPP.dll",
    "$cppOutputDir\slang-glslang.dll",
    "$cppOutputDir\slang-glsl-module.dll",
    "$cppOutputDir\SlangNative.dll",
    "$cppOutputDir\SlangNative.lib",
    "$cppOutputDir\slang-rt.dll"
)

# Add platform-specific files
if ($Platform -ne "ARM64") {
    # slang-llvm.dll is not available on ARM64
    $cppOutputFiles += "$cppOutputDir\slang-llvm.dll"
}

# Optional files that might exist
$optionalFiles = @(
    "$cppOutputDir\Slang.Net.CPP.dll.metagen",
    "$cppOutputDir\Slang.Net.CPP.pdb"
)

# Copy each CPP file to output directory
foreach ($file in $cppOutputFiles) {
    if (Test-Path $file) {
        Copy-Item $file $libDir -Force
        Write-Host "Copied: $file to $libDir" -ForegroundColor Cyan
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net build failed due to missing CPP file!" -ForegroundColor Red
        exit 1
    }
}

# Copy optional files if they exist
foreach ($file in $optionalFiles) {
    if (Test-Path $file) {
        Copy-Item $file $libDir -Force
        Write-Host "Copied optional: $file to $libDir" -ForegroundColor Cyan
    }
    else {
        Write-Host "Optional file not found (skipping): $file" -ForegroundColor Yellow
    }
}

# STEP 2: Build Slang.Net project for the specified platform
Write-Host "Build Slang.Net(STEP 2): Building Slang.Net project $Configuration|$Platform..." -ForegroundColor Green

# When running standalone from command line, build the project
Write-Host "Building Slang.Net project $Configuration|$Platform..." -ForegroundColor Green

# Use dotnet CLI to build the .NET project - this is appropriate for .NET SDK-style projects
& dotnet build "$slangNetDir\Slang.Net.csproj" --configuration $Configuration /p:Platform=$Platform /p:RunTargetsOnly=true /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

if (-not $?) {
    Write-Host "Slang.Net build failed!" -ForegroundColor Red
    exit 1
}

# Verify the output files
Write-Host "Verifying output files..." -ForegroundColor Green

# List of files we expect to find in the output directory
$slangNetOutputFiles = @(
    "$libDir\Slang.Net.dll"
)