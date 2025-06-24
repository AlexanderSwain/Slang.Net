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

Write-Host "===== SlangNative Library Post Build =====" -ForegroundColor DarkGray

# Directories
$nativeDir = $PSScriptRoot

# Ensure SlangNative.lib exists
$nativeOutputDir = Join-Path $nativeDir "bin\$Configuration\$Platform"
if (-not (Test-Path $nativeOutputDir)) {
    Write-Host "Build SlangNative failed due to missing output directory after msbuild call: $nativeOutputDir" -ForegroundColor Red
    exit 1
}

# Check alternative locations
$nativeOutputFiles = @(
    "$nativeOutputDir\gfx.dll",
    "$nativeOutputDir\slang.dll",
    "$nativeOutputDir\slang-glslang.dll",
    "$nativeOutputDir\slang-glsl-module.dll",
    "$nativeOutputDir\SlangNative.dll",
    "$nativeOutputDir\SlangNative.lib",
    "$nativeOutputDir\slang-rt.dll"
)

if ($Platform -eq "x64") {
    $nativeOutputFiles += "$nativeOutputDir\slang-llvm.dll"
}
    
foreach ($file in $nativeOutputFiles) {
    if (Test-Path $file) {
        Write-Host "Verified: $file" -ForegroundColor Cyan
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "SlangNative build failed due to missing output file!" -ForegroundColor Red
        exit 1
    }
}