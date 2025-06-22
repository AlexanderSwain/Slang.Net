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
$slangNetLibDir = "$PSScriptRoot\bin\$Configuration\net9.0\"
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
        Copy-Item $file $slangNetLibDir -Force
        Write-Host "Copied: $file to $slangNetLibDir" -ForegroundColor Black
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net.CPP build failed due to missing Native file!" -ForegroundColor Red
        exit 1
    }
}

# STEP 2: Build Slang.Net project for the specified platform
Write-Host "Build Slang.Net(STEP 2): Building Slang.Net project $Configuration|$Platform..." -ForegroundColor Gray

if (-not $FromVisualStudio) {
    # STEP 2: Build Slang.Net project for the specified platform
    Write-Host "Building Slang.Net project $Configuration|$Platform..." -ForegroundColor Green
    
    # Use dotnet CLI to build the .NET project - this is appropriate for .NET SDK-style projects
    & dotnet build "$PSScriptRoot\Slang.Net.csproj" --configuration $Configuration /p:Platform=$Platform /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "Slang.Net build failed!" -ForegroundColor Red
        exit 1
    }
}
else {
    Write-Host "Visual Studio already built the project. Skipping..." -ForegroundColor Green
}

# Ensure output directory exists and contains needed files
#if (-not (Test-Path $slangNetOutputDir)) {
#    Write-Host "Build failed due to missing output directory after msbuild call: $slangNetOutputDir" -ForegroundColor Red
#    exit 1
#}

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