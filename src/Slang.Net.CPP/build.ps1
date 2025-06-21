param(
    [Parameter(Mandatory=$true)]
    [string]$Configuration,
    
    [Parameter(Mandatory=$true)]
    [string]$Platform,

    [switch]$FromVisualStudio
)

# Validate platform parameter
$validPlatforms = @("x64", "ARM64", "All")
if ($validPlatforms -notcontains $Platform) {
    Write-Host "Error: Invalid platform '$Platform'. Valid platforms are: $($validPlatforms -join ', ')" -ForegroundColor Red
    exit 1
}

# Special validation for ARM64 Debug case
if ($Platform -eq "ARM64" -and $Configuration -eq "Debug") {
    Write-Host "IMPORTANT: Processing the special case of ARM64 Debug configuration" -ForegroundColor Magenta
}

Write-Host "===== Building Slang.Net.CPP Managed Library =====" -ForegroundColor DarkGray

# Directories
$slangNetCppDir = $PSScriptRoot
$nativeDir = "$PSScriptRoot\..\Native"
$nativeOutputDir = "$nativeDir\bin\$Configuration\$Platform"
$slangNetCppOutputDir = "$slangNetCppDir\bin\$Configuration\net9.0\$Platform"

# Print parameters for debugging
Write-Host "DEBUG: -Configuration = $Configuration" -ForegroundColor Yellow
Write-Host "DEBUG: -Platform = $Platform" -ForegroundColor Yellow
Write-Host "DEBUG: -FromVisualStudio = $FromVisualStudio" -ForegroundColor Yellow

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

# Modified MSBuild detection algorithm
$msbuildPath = $null

# First, if called from Visual Studio, don't try to find MSBuild - we'll skip calling it later
if ($FromVisualStudio) {
    # Visual Studio is already running MSBuild, so we don't need to find it
    Write-Host "Running from Visual Studio - will skip MSBuild step" -ForegroundColor Green
}
else {
    # Try to find MSBuild in standard locations
    foreach ($path in $msbuildPaths) {
        if (Test-Path $path) {
            $msbuildPath = $path
            Write-Host "Found MSBuild at: $msbuildPath" -ForegroundColor Green
            break
        }
    }
    
    # If not found in standard locations, try to find it using vswhere
    if (-not $msbuildPath) {
        Write-Host "Trying to locate MSBuild using vswhere..." -ForegroundColor Yellow
        
        # vswhere is installed with Visual Studio 2017 and later
        $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
        
        if (Test-Path $vswhere) {
            $vsPath = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
            if ($vsPath) {
                $potentialMsBuildPath = Join-Path $vsPath "MSBuild\Current\Bin\MSBuild.exe"
                if (Test-Path $potentialMsBuildPath) {
                    $msbuildPath = $potentialMsBuildPath
                    Write-Host "Found MSBuild using vswhere at: $msbuildPath" -ForegroundColor Green
                }
            }
        }
    }
    
    # Exit if MSBuild is not found and we need to use it
    if (-not $msbuildPath) {
        Write-Host "Could not find MSBuild.exe in any of the expected locations." -ForegroundColor Red
        Write-Host "Make sure Visual Studio is installed with C++ development tools." -ForegroundColor Red
        exit 1
    }
}

# Setup output directories based on platform
if ($Platform -eq "All") {
    # For "All" platform, we'll use a simplified output structure
    $slangNetCppOutputDir = "$slangNetCppDir\bin\$Configuration\net9.0"
    $nativePlatform = "x64" # Default to x64 for native dependencies when using "All" platform
    $nativeOutputDir = "$nativeDir\bin\$Configuration\$nativePlatform"
} else {
    $slangNetCppOutputDir = "$slangNetCppDir\bin\$Configuration\net9.0\$Platform"
    $nativeOutputDir = "$nativeDir\bin\$Configuration\$Platform"
}

# Create output directory if it doesn't exist
if (-not (Test-Path -Path $slangNetCppOutputDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetCppOutputDir -Force | Out-Null
    Write-Host "Created directory: $slangNetCppOutputDir" -ForegroundColor Yellow
}

# STEP 1: Copy native files to Slang.Net.CPP output directory
Write-Host "Build Slang.Net.CPP(STEP 1): Copy native files..." -ForegroundColor Green

# Define list of native files to copy
$nativeOutputFiles = @(
    "$nativeOutputDir\gfx.dll",
    "$nativeOutputDir\slang.dll",
    "$nativeOutputDir\slang-glslang.dll",
    "$nativeOutputDir\slang-glsl-module.dll",
    "$nativeOutputDir\SlangNative.dll",
    "$nativeOutputDir\SlangNative.lib",
    "$nativeOutputDir\slang-rt.dll"
)

# Add platform-specific files
if ($Platform -ne "ARM64") {
    # slang-llvm.dll is not available on ARM64
    $nativeOutputFiles += "$nativeOutputDir\slang-llvm.dll"
}

# Copy native files to Slang.Net.CPP output directory
foreach ($file in $nativeOutputFiles) {
    if (Test-Path $file) {
        Copy-Item $file $slangNetCppOutputDir -Force
        Write-Host "Copied: $file to $slangNetCppOutputDir" -ForegroundColor Cyan
    }
    else {
        Write-Host "Missing: $file" -ForegroundColor Red
        Write-Host "Slang.Net.CPP build failed due to missing Native file!" -ForegroundColor Red
        exit 1
    }
}

# STEP 2: Build Slang.Net.CPP project for the specified platform
Write-Host "Build Slang.Net.CPP(STEP 2): MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Green

if ($FromVisualStudio) {
    # When running from Visual Studio, we don't need to build the project again
    Write-Host "Visual Studio already ran MSBuild. Proceeding with output verification..." -ForegroundColor Green
}
else {
    # When running standalone from command line, build the project
    Write-Host "MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Green

    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$PSScriptRoot\Slang.Net.CPP.vcxproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /t:Rebuild /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "Slang.Net.CPP build failed!" -ForegroundColor Red
        exit 1
    }
}

# STEP 3: Verify output files
Write-Host "Build Slang.Net.CPP(STEP 3): Verifying output files..." -ForegroundColor Green

# Ensure output directory exists
if (-not (Test-Path $slangNetCppOutputDir)) {
    Write-Host "Build Slang.Net.CPP failed due to missing output directory: $slangNetCppOutputDir" -ForegroundColor Red
    exit 1
}

# List of files we expect to find in the output directory
$cppOutputFiles = @(
    "$slangNetCppOutputDir\Slang.Net.CPP.dll"
)

# Verify output files
$missingFiles = @()
foreach ($file in $cppOutputFiles) {
    if (-not (Test-Path $file)) {
        $missingFiles += $file
    } else {
        Write-Host "Verified: $file" -ForegroundColor Cyan
    }
}

if ($missingFiles.Count -gt 0) {
    # Special handling for All platform - try to find the file in a different location
    if ($Platform -eq "All") {
        Write-Host "Platform is 'All', checking alternative locations for missing files..." -ForegroundColor Yellow
        
        # Check in x64 directory first (most common)
        $alternativeCppOutputFiles = @(
            "$slangNetCppDir\bin\$Configuration\net9.0\x64\Slang.Net.CPP.dll"
        )
        
        foreach ($altFile in $alternativeCppOutputFiles) {
            if (Test-Path $altFile) {
                $targetFile = "$slangNetCppOutputDir\Slang.Net.CPP.dll"
                Write-Host "Found alternative: $altFile, copying to $targetFile" -ForegroundColor Yellow
                Copy-Item -Path $altFile -Destination $targetFile -Force
                Write-Host "Verified (copied): $targetFile" -ForegroundColor Cyan
                $missingFiles = @() # Clear missing files list
                break
            }
        }
    }
    
    # If we still have missing files, report error
    if ($missingFiles.Count -gt 0) {
        foreach ($file in $missingFiles) {
            Write-Host "Missing: $file" -ForegroundColor Red
        }
        Write-Host "Slang.Net.CPP build failed due to missing output files!" -ForegroundColor Red
        exit 1
    }
}

# Add confirmation that this script completed for this configuration/platform
Write-Host "Build script completed for $Configuration | $Platform" -ForegroundColor Magenta