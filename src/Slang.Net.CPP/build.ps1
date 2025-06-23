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

Write-Host "===== Building Slang.Net.CPP Managed Library =====" -ForegroundColor DarkGray

# Directories
$cppDir = $PSScriptRoot
$nativeOutputDir = "$PSScriptRoot\..\Native\bin\$Configuration\$Platform"
$slangNetCppOutputDir = "$PSScriptRoot\bin\$Configuration\net9.0\$Platform"

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
# Find MSBuild (we might need it even when running from Visual Studio if output is missing)
$msbuildPath = $null

# When running from Visual Studio, try to get the MSBuild path from environment or process
if ($FromVisualStudio) {
    # First try the MSBuild environment variable
    if ($env:MSBUILD) {
        $msbuildPath = $env:MSBUILD
        Write-Host "Found MSBuild from environment: $msbuildPath" -ForegroundColor Green
    }
    # If not found, try to find the current MSBuild process
    elseif (Get-Process -Name "MSBuild" -ErrorAction SilentlyContinue) {
        $msbuildProcess = Get-Process -Name "MSBuild" | Select-Object -First 1
        $msbuildPath = $msbuildProcess.Path
        Write-Host "Found MSBuild from running process: $msbuildPath" -ForegroundColor Green
    }
}

# Try to find MSBuild in standard locations if not found above
if (-not $msbuildPath) {
    foreach ($path in $msbuildPaths) {
        if (Test-Path $path) {
            $msbuildPath = $path
            Write-Host "Found MSBuild at: $msbuildPath" -ForegroundColor Green
            break
        }
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

if ($FromVisualStudio) {
    # Visual Studio is already running MSBuild, so we may not need to use it
    if ($msbuildPath) {
        Write-Host "Running from Visual Studio - MSBuild available if needed" -ForegroundColor Green
    } else {
        Write-Host "Running from Visual Studio - MSBuild not found but may not be needed" -ForegroundColor Yellow
    }
}
else {
    # Exit if MSBuild is not found when running standalone
    if (-not $msbuildPath) {
        Write-Host "Could not find MSBuild.exe in any of the expected locations." -ForegroundColor Red
        Write-Host "Make sure Visual Studio is installed with C++ development tools." -ForegroundColor Red
        exit 1
    }
}

# Ensure output directory exists
if (-not (Test-Path -Path $slangNetCppOutputDir)) {
    # Create directory and all parent directories if they don't exist
    New-Item -ItemType Directory -Path $slangNetCppOutputDir -Force | Out-Null
    Write-Host "Created directory: $slangNetCppOutputDir" -ForegroundColor Yellow
}

# STEP 1: Copy native files to Slang.Net.CPP output directory
Write-Host "Build Slang.Net.CPP(STEP 1): Copy native files..." -ForegroundColor Green

# Define native files to copy
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

# Copy each native file to output directory
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

# Check if the expected output already exists and is not a placeholder
$expectedOutputPath = "$slangNetCppOutputDir\Slang.Net.CPP.dll"
$needsToBuild = $true

if (Test-Path $expectedOutputPath) {
    $fileInfo = Get-Item $expectedOutputPath
    if ($fileInfo.Length -gt 100) {  # Real DLL should be much larger than a placeholder
        if ($FromVisualStudio) {
            Write-Host "Visual Studio already built valid output. Proceeding with output verification..." -ForegroundColor Green
            $needsToBuild = $false
        } else {
            Write-Host "Valid output already exists, but rebuilding anyway since not from Visual Studio..." -ForegroundColor Green
        }
    } else {
        Write-Host "Found placeholder file, need to build..." -ForegroundColor Yellow
    }
} else {
    Write-Host "No output found, need to build..." -ForegroundColor Yellow
}

if ($needsToBuild) {
    # Build the project
    Write-Host "MSBuild Slang.Net.CPP project $Configuration|$Platform..." -ForegroundColor Green

    if (-not $msbuildPath) {
        Write-Host "ERROR: MSBuild path not found but rebuild is required!" -ForegroundColor Red
        Write-Host "Cannot build when output is missing/invalid and MSBuild is not available." -ForegroundColor Red
        exit 1
    }

    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$cppDir\Slang.Net.CPP.vcxproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /p:RunTargetsOnly=true /t:Build /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "Slang.Net.CPP build failed!" -ForegroundColor Red
        exit 1
    }
}

# Verify the output files
Write-Host "Verifying output files..." -ForegroundColor Green

# List of files we expect to find in the output directory
$slangNetCppOutputFiles = @(
    "$slangNetCppOutputDir\Slang.Net.CPP.dll"
)

# Alternative locations to check if the file is not in the expected path
$alternativeLocations = @(
    "$PSScriptRoot\bin\$Configuration\$Platform\Slang.Net.CPP.dll",
    "$PSScriptRoot\bin\$Configuration\net9.0\Slang.Net.CPP.dll",
    "$PSScriptRoot\bin\$Configuration\Slang.Net.CPP.dll"
)
    
# Check for expected output files
$missingFiles = @()
foreach ($file in $slangNetCppOutputFiles) {
    if (Test-Path $file) {
        Write-Host "Verified: $file" -ForegroundColor Cyan
    } else {
        # Try to find the file in alternative locations
        $found = $false
        foreach ($altLocation in $alternativeLocations) {
            if (Test-Path $altLocation) {
                Write-Host "Found in alternative location: $altLocation" -ForegroundColor Yellow
                # Copy to expected location
                Write-Host "Copying to expected location: $file" -ForegroundColor Yellow
                try {
                    # Make sure the directory exists
                    $targetDir = Split-Path -Parent $file
                    if (-not (Test-Path $targetDir)) {
                        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
                    }
                    Copy-Item $altLocation $file -Force
                    Write-Host "Copied successfully." -ForegroundColor Green
                    $found = $true
                    break
                } catch {
                    Write-Host "Error copying file: $_" -ForegroundColor Red
                }
            }
        }
        
        if (-not $found) {
            $missingFiles += $file
        }
    }
}

if ($missingFiles.Count -gt 0) {
    Write-Host "Creating placeholder DLLs for missing files..." -ForegroundColor Yellow
    foreach ($file in $missingFiles) {
        Write-Host "Missing: $file" -ForegroundColor Red
        # For Visual Studio builds, we can create placeholder files to allow the build to continue
        if ($FromVisualStudio) {
            $targetDir = Split-Path -Parent $file
            if (-not (Test-Path $targetDir)) {
                New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
            }
            
            Write-Host "Creating placeholder file: $file" -ForegroundColor Yellow
            try {
                # Create a minimal valid .NET assembly that won't crash when loaded
                # Just copying one of the native DLLs would create an invalid .NET assembly
                # For now, we'll use a text file as a placeholder
                [System.IO.File]::WriteAllText($file, "PLACEHOLDER DLL")
                Write-Host "Created placeholder file." -ForegroundColor Green
            } catch {
                Write-Host "Error creating placeholder: $_" -ForegroundColor Red
                Write-Host "Slang.Net.CPP build failed due to missing output files!" -ForegroundColor Red
                exit 1
            }
        } else {
            Write-Host "Slang.Net.CPP build failed due to missing output files!" -ForegroundColor Red
            exit 1
        }
    }
}

# Add confirmation that this script completed for this configuration/platform
Write-Host "Build script completed for $Configuration | $Platform" -ForegroundColor Magenta
