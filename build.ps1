#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Build script for Slang.Net project
.DESCRIPTION
    Automates the build process for the Slang.Net project using MSBuild.
    Supports both Debug and Release configurations for x64 platform.
.PARAMETER Configuration
    Build configuration: Debug or Release (default: Debug)
.PARAMETER Clean
    Clean the solution before building
.PARAMETER Verbose
    Enable verbose build output
.EXAMPLE
    .\build.ps1
    Build Debug configuration
.EXAMPLE
    .\build.ps1 -Configuration Release
    Build Release configuration
.EXAMPLE
    .\build.ps1 -Clean -Verbose
    Clean and build with verbose output
#>

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    [switch]$Clean,
    [switch]$Verbose
)

# Colors for output
$ErrorColor = "Red"
$SuccessColor = "Green"
$InfoColor = "Yellow"
$HeaderColor = "Cyan"

function Write-Header {
    param([string]$Message)
    Write-Host "=" * 60 -ForegroundColor $HeaderColor
    Write-Host $Message -ForegroundColor $HeaderColor
    Write-Host "=" * 60 -ForegroundColor $HeaderColor
}

function Write-Info {
    param([string]$Message)
    Write-Host $Message -ForegroundColor $InfoColor
}

function Write-Success {
    param([string]$Message)
    Write-Host $Message -ForegroundColor $SuccessColor
}

function Write-Error {
    param([string]$Message)
    Write-Host $Message -ForegroundColor $ErrorColor
}

# Find MSBuild
$msbuildPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)

$msbuild = $null
foreach ($path in $msbuildPaths) {
    if (Test-Path $path) {
        $msbuild = $path
        break
    }
}

if (-not $msbuild) {
    Write-Error "MSBuild not found. Please install Visual Studio 2019 or 2022 with C++ support."
    exit 1
}

Write-Header "Slang.Net Build Script"
Write-Info "Configuration: $Configuration"
Write-Info "Platform: x64"
Write-Info "MSBuild: $msbuild"

# Verify solution file exists
$solutionFile = "Slang.Net.sln"
if (-not (Test-Path $solutionFile)) {
    Write-Error "Solution file '$solutionFile' not found in current directory."
    Write-Error "Please run this script from the Slang.Net project root directory."
    exit 1
}

# Build parameters
$buildParams = @(
    $solutionFile,
    "/p:Configuration=$Configuration",
    "/p:Platform=x64",
    "/m"  # Multi-processor build
)

if ($Verbose) {
    $buildParams += "/v:detailed"
} else {
    $buildParams += "/v:minimal"
}

try {
    if ($Clean) {
        Write-Info "Cleaning solution..."
        $cleanParams = $buildParams + @("/t:Clean")
        & $msbuild @cleanParams
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Clean failed with exit code $LASTEXITCODE"
            exit $LASTEXITCODE
        }
        Write-Success "Clean completed successfully."
    }

    Write-Info "Building solution..."
    & $msbuild @buildParams
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Build completed successfully!"
        
        # Check output files
        $outputDir = "x64\$Configuration"
        if (Test-Path $outputDir) {
            Write-Info "Output files generated in '$outputDir':"
            Get-ChildItem $outputDir -Filter "*.dll" | ForEach-Object {
                Write-Host "  ✓ $($_.Name)" -ForegroundColor $SuccessColor
            }
            Get-ChildItem $outputDir -Filter "*.lib" | ForEach-Object {
                Write-Host "  ✓ $($_.Name)" -ForegroundColor $SuccessColor
            }
        }
        
        Write-Info ""
        Write-Info "Next steps:"
        Write-Info "  1. Run tests: cd $outputDir && dotnet Slang.Net.Test.dll"
        Write-Info "  2. Or open in Visual Studio and set Slang.Net.Test as startup project"
        
    } else {
        Write-Error "Build failed with exit code $LASTEXITCODE"
        exit $LASTEXITCODE
    }
    
} catch {
    Write-Error "Build script failed: $($_.Exception.Message)"
    exit 1
}
