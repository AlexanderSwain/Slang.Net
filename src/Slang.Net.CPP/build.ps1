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
Write-Host "Build Slang.Net.CPP(STEP 1): Downloaded Slang SDK..." -ForegroundColor Green

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

# STEP 2: Build Slang.Net.CPP project for the specified platform
Write-Host "Build Slang.Net.CPP(STEP 2): MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Gray

# MSBuild paths
$msbuildPaths = @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
)
$msbuildPath = $null
foreach ($path in $msbuildPaths) {
    if (Test-Path $path) {
        $msbuildPath = $path
        break
    }
}

if (-not $msbuildPath) {
    Write-Host "Could not find MSBuild.exe in any of the expected locations." -ForegroundColor Red
    Write-Host "Make sure Visual Studio is installed with C++ development tools." -ForegroundColor Red
    exit 1
}

if (-not $FromVisualStudio) {
    # STEP 2: Build SlangNative project for the specified platform
    Write-Host "MSBuild SlangNative project $Configuration|$Platform..." -ForegroundColor Green

    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$PSScriptRoot\Slang.Net.CPP.vcxproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /t:Rebuild /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "SlangNative build failed!" -ForegroundColor Red
        exit 1
    }
}
else {
    Write-Host "Visual Studio already ran MSBUILD.exe. Skipping..." -ForegroundColor Green
}

# STEP 2: Build Slang.Net.CPP project for the specified platform
Write-Host "Build Slang.Net.CPP(STEP 2): MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Gray

if (-not $FromVisualStudio) {
    # STEP 2: Build SlangNative project for the specified platform
    Write-Host "MSBuild SlangNative project $Configuration|$Platform..." -ForegroundColor Green

    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$PSScriptRoot\Slang.Net.CPP.vcxproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /t:Rebuild /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "SlangNative build failed!" -ForegroundColor Red
        exit 1
    }
}
else {
    Write-Host "Visual Studio already ran MSBUILD.exe. Skipping..." -ForegroundColor Green
}

# Ensure Slang.Net.CPP.lib exists
$slangNetCppOutputDir = "$PSScriptRoot\bin\$Configuration\net9.0\$Platform"
if (-not (Test-Path $slangNetCppOutputDir)) {
    Write-Host "Build SlangNative failed due to missing output directory after msbuild call: $nativeOutputDir" -ForegroundColor Red
    exit 1
}

# Check alternative locations
$slangNetCppOutputFiles = @(
    "$slangNetCppOutputDir\Slang.Net.CPP.dll",
    "$slangNetCppOutputDir\Slang.Net.CPP.lib"
)
    
foreach ($file in $slangNetCppOutputFiles) {
    if (Test-Path $file) {
        Write-Host "Verified: $file" -ForegroundColor Cyan
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "SlangNative build failed due to missing output file!" -ForegroundColor Red
        exit 1
    }
}