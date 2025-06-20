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

Write-Host "===== Building Slang.Net Nuget Package =====" -ForegroundColor DarkGray

# Directories
$nativeOutputDir = "$PSScriptRoot\..\Slang.Net.CPP\bin\$Configuration\net9.0\$Platform"
$slangNetOutputDir = "$PSScriptRoot\bin\$Configuration\net9.0\$Platform"
if (-not (Test-Path -Path $slangNetOutputDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetOutputDir -Force | Out-Null
    Write-Host "Created directory: $slangNetOutputDir" -ForegroundColor Yellow
}

# STEP 1: Copy native files to Slang.Net output directory
Write-Host "Build Slang.Net.CPP(STEP 1): Copy native files..." -ForegroundColor Green

# Check alternative locations
$nativeOutputFiles = @(
    "$nativeOutputDir\gfx.dll",
    "$nativeOutputDir\slang.dll",
    "$nativeOutputDir\Slang.Net.CPP.dll",
    "$nativeOutputDir\Slang.Net.CPP.dll.metagen",
    "$nativeOutputDir\Slang.Net.CPP.pdb",
    "$nativeOutputDir\slang-glslang.dll",
    "$nativeOutputDir\slang-glsl-module.dll",
    "$nativeOutputDir\slang-llvm.dll",
    "$nativeOutputDir\SlangNative.dll",
    "$nativeOutputDir\SlangNative.lib",
    "$nativeOutputDir\slang-rt.dll"
)

foreach ($file in $nativeOutputFiles) {
    if (Test-Path $file) {
        Copy-Item $file $slangNetOutputDir -Force
        Write-Host "Copied: $file to $slangNetOutputDir" -ForegroundColor Black
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net.CPP build failed due to missing Native file!" -ForegroundColor Red
        exit 1
    }
}

# STEP 2: Build Slang.Net project for the specified platform
Write-Host "Build Slang.Net(STEP 2): MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Gray

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

# STEP 2: Build Slang.Net project for the specified platform
Write-Host "Build Slang.Net.CPP(STEP 2): MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Gray

if (-not $FromVisualStudio) {
    # STEP 2: Build SlangNative project for the specified platform
    Write-Host "MSBuild Slang.Net project $Configuration|$Platform..." -ForegroundColor Green    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$PSScriptRoot\Slang.Net.csproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /t:Rebuild /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "Slang.Net build failed!" -ForegroundColor Red
        exit 1
    }
}
else {
    Write-Host "Visual Studio already ran MSBUILD.exe. Skipping..." -ForegroundColor Green
}

# Ensure output directory exists and contains needed files
if (-not (Test-Path $slangNetOutputDir)) {
    Write-Host "Build failed due to missing output directory after msbuild call: $slangNetOutputDir" -ForegroundColor Red
    exit 1
}

# Check for essential files
#$outputFiles = @(
#    "$slangNetOutputDir\Slang.Net.CPP.dll"
#)
    
#foreach ($file in $outputFiles) {
#    if (Test-Path $file) {
#        Write-Host "Verified: $file" -ForegroundColor Cyan
#    }
#    else {
#        Write-Host "Missing: $file" -ForegroundColor Red
#        Write-Host "Build failed due to missing output file!" -ForegroundColor Red
#        exit 1
#    }
#}