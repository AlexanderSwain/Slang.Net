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

Write-Host "===== Building SlangNative Library =====" -ForegroundColor DarkGray

# Directories
$nativeDir = $PSScriptRoot

# STEP 1: Download Slang SDK if not already present
Write-Host "Build SlangNative(STEP 1): Downloaded Slang SDK..." -ForegroundColor Green
& "$nativeDir\download-slang-sdk.ps1" -Platform $Platform

