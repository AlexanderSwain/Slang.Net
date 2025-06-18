#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Verification script for Slang.Net build
.DESCRIPTION
    Checks that the Slang.Net project built successfully and all required files are present.
.PARAMETER Configuration
    Build configuration to verify: Debug or Release (default: Debug)
.EXAMPLE
    .\verify-build.ps1
    Verify Debug build
.EXAMPLE
    .\verify-build.ps1 -Configuration Release
    Verify Release build
#>

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

# Colors for output
$ErrorColor = "Red"
$SuccessColor = "Green"
$InfoColor = "Yellow"
$HeaderColor = "Cyan"

function Write-Header {
    param([string]$Message)
    Write-Host "=" * 50 -ForegroundColor $HeaderColor
    Write-Host $Message -ForegroundColor $HeaderColor
    Write-Host "=" * 50 -ForegroundColor $HeaderColor
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

function Test-FileExists {
    param([string]$FilePath, [string]$Description)
    
    if (Test-Path $FilePath) {
        $size = (Get-Item $FilePath).Length
        Write-Success "  [OK] $Description ($([math]::Round($size/1KB, 2)) KB)"
        return $true
    } else {
        Write-Error "  [MISSING] $Description - MISSING"
        return $false
    }
}

Write-Header "Slang.Net Build Verification"
Write-Info "Configuration: $Configuration"
Write-Info "Platform: x64"

$outputDir = "src\Slang.Net\bin\$Configuration\net9.0"
$allFilesPresent = $true

Write-Info ""
Write-Info "Checking output directory: $outputDir"

if (-not (Test-Path $outputDir)) {
    Write-Error "Output directory '$outputDir' not found!"
    Write-Error "Please build the project first using one of these methods:"
    Write-Error "  - .\build.ps1 -Configuration $Configuration"
    Write-Error "  - Open Slang.Net.sln in Visual Studio and build"
    exit 1
}

Write-Info ""
Write-Info "Verifying required output files..."

# Check SlangNative files
$allFilesPresent = (Test-FileExists "$outputDir\SlangNative.dll" "SlangNative.dll (Native C++ library)") -and $allFilesPresent
$allFilesPresent = (Test-FileExists "$outputDir\SlangNative.lib" "SlangNative.lib (Import library)") -and $allFilesPresent

# Check Slang.Net managed files
$allFilesPresent = (Test-FileExists "$outputDir\Slang.Net.dll" "Slang.Net.dll (Managed C++/CLI wrapper)") -and $allFilesPresent

# Check for .NET library (commented out since this is a library project, not executable)
# $allFilesPresent = (Test-FileExists "$outputDir\Slang.Net.exe" "Slang.Net.exe (Test executable)") -and $allFilesPresent

# Check debug symbols for Debug builds
if ($Configuration -eq "Debug") {
    Write-Info ""
    Write-Info "Checking debug symbols..."
    Test-FileExists "$outputDir\SlangNative.pdb" "SlangNative.pdb (Debug symbols)" | Out-Null
    Test-FileExists "$outputDir\Slang.Net.pdb" "Slang.Net.pdb (Debug symbols)" | Out-Null
}

Write-Info ""
Write-Info "Checking Slang dependencies..."

Write-Info ""
Write-Info "Checking Slang dependencies..."

# Check new embedded LLVM directories
$libDir = "src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\lib"
$binDir = "src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\bin"

if (Test-Path $libDir) {
    Write-Success "  ✓ Slang lib directory found"
    $requiredLibs = @("slang.lib", "gfx.lib", "slang-rt.lib")
    foreach ($lib in $requiredLibs) {
        Test-FileExists "$libDir\$lib" $lib | Out-Null
    }
} else {
    Write-Error "  ✗ Slang lib directory not found: $libDir"
    $allFilesPresent = $false
}

if (Test-Path $binDir) {
    Write-Success "  ✓ Slang bin directory found"
    $requiredDlls = @("slang.dll", "gfx.dll", "slang-rt.dll")
    foreach ($dll in $requiredDlls) {
        Test-FileExists "$binDir\$dll" $dll | Out-Null
    }
} else {    Write-Error "  [MISSING] Slang bin directory not found: $binDir"
    $allFilesPresent = $false
}

Write-Info ""
if ($allFilesPresent) {
    Write-Header "[OK] BUILD VERIFICATION PASSED"
    Write-Success "All required files are present and the build appears successful!"
    Write-Info ""    Write-Info "You can now:"
    Write-Info "  1. Use the libraries in your own projects"
    Write-Info "  2. Debug using Visual Studio"
    Write-Info "  3. Install the NuGet package for easier distribution"
} else {
    Write-Header "[FAILED] BUILD VERIFICATION FAILED"
    Write-Error "Some required files are missing. Please rebuild the project."
    Write-Info ""
    Write-Info "To rebuild:"
    Write-Info "  .\build.ps1 -Configuration $Configuration -Clean"
    exit 1
}

Write-Info ""
Write-Info "Build verification completed."
