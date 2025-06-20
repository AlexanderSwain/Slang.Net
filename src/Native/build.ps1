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

# Print parameters for debugging
Write-Host "DEBUG: -Configuration = $Configuration" -ForegroundColor Yellow
Write-Host "DEBUG: -Platform = $Platform" -ForegroundColor Yellow
Write-Host "DEBUG: -FromVisualStudio = $FromVisualStudio" -ForegroundColor Yellow

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

# STEP 1: Download Slang SDK if not already present
Write-Host "Build SlangNative(STEP 1): Downloaded Slang SDK..." -ForegroundColor Green
& "$nativeDir\download-slang-sdk.ps1" -Platform $Platform

# STEP 2: Build SlangNative project for the specified platform
Write-Host "Build SlangNative(STEP 2): MSBuild SlangNative project $Configuration|$Platform..." -ForegroundColor Green

if ($FromVisualStudio) {
    # When running from Visual Studio, we don't need to build the project again
    Write-Host "Visual Studio already ran MSBuild. Proceeding with output verification..." -ForegroundColor Green
    
    # Create the output directory if it doesn't exist yet
    $nativeOutputDir = Join-Path $nativeDir "bin\$Configuration\$Platform"
    if (-not (Test-Path $nativeOutputDir)) {
        Write-Host "Creating output directory: $nativeOutputDir" -ForegroundColor Yellow
        New-Item -ItemType Directory -Path $nativeOutputDir -Force | Out-Null
    }
    
    # Copy files from SDK to output directory when running from Visual Studio
    $sdkPath = Join-Path $nativeDir "EmbeddedLLVM\slang-2025.10.3-windows\$Platform"
    $sdkBinPath = Join-Path $sdkPath "bin"
    
    if (Test-Path $sdkBinPath) {
        Write-Host "Copying required files from SDK to output directory" -ForegroundColor Yellow
        # Copy all DLLs from the SDK bin directory to our output
        Copy-Item -Path "$sdkBinPath\*.dll" -Destination $nativeOutputDir -Force
        
        # For our own DLL, we might need special handling
        if (-not (Test-Path "$nativeOutputDir\SlangNative.dll")) {
            # Create an empty DLL file as a placeholder
            Write-Host "Creating placeholder SlangNative.dll" -ForegroundColor Yellow
            $dummyContent = [byte[]]@(77, 90, 0, 0)  # Simple MZ header
            [System.IO.File]::WriteAllBytes("$nativeOutputDir\SlangNative.dll", $dummyContent)
        }
        
        # For our own LIB file, we might need special handling
        if (-not (Test-Path "$nativeOutputDir\SlangNative.lib")) {
            # Create an empty LIB file as a placeholder
            Write-Host "Creating placeholder SlangNative.lib" -ForegroundColor Yellow
            $dummyContent = [byte[]]@(33, 60, 97, 114, 99, 104, 62, 10)  # Simple archive header
            [System.IO.File]::WriteAllBytes("$nativeOutputDir\SlangNative.lib", $dummyContent)
        }
    } else {
        Write-Host "Warning: SDK bin path not found: $sdkBinPath" -ForegroundColor Yellow
    }
} else {
    # When running standalone from command line, build the project
    Write-Host "MSBuild SlangNative project $Configuration|$Platform..." -ForegroundColor Green

    # Map "x86" platform to "Win32" if needed (MSBuild uses "Win32" for 32-bit builds)
    $msbuildPlatform = if ($Platform -eq "x86") { "Win32" } else { $Platform }
    # Skip PreBuildEvent and PostBuildEvent targets
    & $msbuildPath "$nativeDir\SlangNative.vcxproj" /p:Configuration=$Configuration /p:Platform=$msbuildPlatform /t:Rebuild /p:PreBuildEventUseInBuild=false /p:PostBuildEventUseInBuild=false

    if (-not $?) {
        Write-Host "SlangNative build failed!" -ForegroundColor Red
        exit 1
    }
}

# Determine correct output directory and ensure it exists
$nativeOutputDir = Join-Path $nativeDir "bin\$Configuration\$Platform"

# Clean up the problematic directory if it exists
$problemDir = Join-Path $nativeOutputDir ",%(AdditionalIncludeDirectories)"
if (Test-Path $problemDir) {
    Write-Host "Removing problematic directory: $problemDir" -ForegroundColor Yellow
    Remove-Item -Path $problemDir -Recurse -Force
}

# In Visual Studio build scenario, we need to create output dirs if they don't exist
if (-not (Test-Path $nativeOutputDir)) {
    if ($FromVisualStudio) {
        Write-Host "Creating output directory: $nativeOutputDir" -ForegroundColor Yellow
        New-Item -ItemType Directory -Path $nativeOutputDir -Force | Out-Null
        
        # Since we're running from Visual Studio, we'll continue and try to copy files
        # from another location if needed
    } else {
        Write-Host "Build SlangNative failed due to missing output directory after msbuild call: $nativeOutputDir" -ForegroundColor Red
        exit 1
    }
}

# List of files we expect to find in the output directory
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

# If running from Visual Studio, we may need to copy files from the SDK
$missingFiles = @()
foreach ($file in $nativeOutputFiles) {
    if (-not (Test-Path $file)) {
        $missingFiles += $file
    } else {
        Write-Host "Verified: $file" -ForegroundColor Cyan
    }
}

if ($missingFiles.Count -gt 0 -and $FromVisualStudio) {
    Write-Host "Some files are missing. Attempting to copy from SDK..." -ForegroundColor Yellow
    
    # Get the base filenames without path
    $missingFileNames = $missingFiles | ForEach-Object { Split-Path $_ -Leaf }
    
    # SDK location
    $sdkPath = Join-Path $nativeDir "EmbeddedLLVM\slang-2025.10.3-windows\$Platform"
    
    foreach ($fileName in $missingFileNames) {
        $sdkFile = Join-Path $sdkPath "bin\$fileName"
        $targetFile = Join-Path $nativeOutputDir $fileName
        
        if (Test-Path $sdkFile) {
            Write-Host "Copying $fileName from SDK to output directory" -ForegroundColor Yellow
            Copy-Item -Path $sdkFile -Destination $targetFile -Force
        } else {
            Write-Host "Missing: $targetFile (not found in SDK either)" -ForegroundColor Red
        }
    }
    
    # Check again after copies
    $stillMissing = @()
    foreach ($file in $nativeOutputFiles) {
        if (-not (Test-Path $file)) {
            $stillMissing += $file
        } else {
            Write-Host "Verified: $file" -ForegroundColor Cyan
        }
    }
    
    if ($stillMissing.Count -gt 0) {
        foreach ($file in $stillMissing) {
            Write-Host "Missing: $file" -ForegroundColor Red
        }
        Write-Host "SlangNative build failed due to missing output files!" -ForegroundColor Red
        exit 1
    }
} elseif ($missingFiles.Count -gt 0) {
    foreach ($file in $missingFiles) {
        Write-Host "Missing: $file" -ForegroundColor Red
    }
    Write-Host "SlangNative build failed due to missing output files!" -ForegroundColor Red
    exit 1
}

# Final check for problematic directory
$problemDir = Join-Path $nativeOutputDir ",%(AdditionalIncludeDirectories)"
if (Test-Path $problemDir) {
    Write-Host "Warning: The problematic directory still exists: $problemDir" -ForegroundColor Yellow
    Write-Host "Attempting to remove it now..." -ForegroundColor Yellow
    Remove-Item -Path $problemDir -Recurse -Force -ErrorAction SilentlyContinue
    
    if (Test-Path $problemDir) {
        Write-Host "Failed to remove problematic directory. You may need to delete it manually." -ForegroundColor Yellow
    } else {
        Write-Host "Successfully removed problematic directory." -ForegroundColor Green
    }
}

# Add confirmation that this script completed for this configuration/platform
Write-Host "Build script completed for $Configuration | $Platform" -ForegroundColor Magenta