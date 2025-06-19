param(
    [string]$SlangVersion = "2025.10.3",
    [string]$Platform = "x64",
    [switch]$Force
)

# Define paths
# The correct format for Slang downloads is:
# https://github.com/shader-slang/slang/releases/download/v{version}/slang-{version}-windows-x86_64.zip
$downloadUrl = "https://github.com/shader-slang/slang/releases/download/v$SlangVersion/slang-$SlangVersion-windows-x86_64.zip"
$mockDownload = $false
$extractDir = Join-Path $PSScriptRoot "EmbeddedLLVM"
$downloadDir = Join-Path $extractDir "downloads"
$zipFile = Join-Path $downloadDir "slang-$SlangVersion-windows-x86_64.zip"
$extractPath = Join-Path $extractDir "slang-$SlangVersion-windows"
$platformPath = Join-Path $extractPath $Platform

# Create directories if they don't exist
if (-Not (Test-Path $downloadDir)) {
    New-Item -ItemType Directory -Path $downloadDir -Force | Out-Null
    Write-Host "Created download directory: $downloadDir"
}

# Check if files already exist and we don't need to re-download
if (-Not $Force) {
    if ((Test-Path $extractPath) -and (Test-Path "$platformPath\bin") -and (Test-Path "$platformPath\lib")) {
        Write-Host "Slang SDK already exists at $extractPath. Use -Force to redownload."
        return
    }
}

# Download the ZIP file if it doesn't exist or Force is specified
if ((-Not (Test-Path $zipFile)) -or $Force) {
    Write-Host "Downloading Slang SDK $SlangVersion for Windows..."
    try {
        if ($mockDownload) {
            # Create a mock ZIP file for testing
            Write-Host "MOCK MODE: Creating simulated Slang SDK directory structure..."
            
            # Create the extraction directory
            if (Test-Path $extractPath) {
                Write-Host "Removing existing extracted files..."
                Remove-Item -Recurse -Force $extractPath
            }
            
            # Create platform directory (x64, ARM64)
            New-Item -ItemType Directory -Path $platformPath -Force | Out-Null
            
            # Create bin directory and mock files
            $binPath = Join-Path $platformPath "bin"
            New-Item -ItemType Directory -Path $binPath -Force | Out-Null
            Set-Content -Path (Join-Path $binPath "slang.dll") -Value "MOCK DLL"
            Set-Content -Path (Join-Path $binPath "slang-rt.dll") -Value "MOCK DLL"
            Set-Content -Path (Join-Path $binPath "gfx.dll") -Value "MOCK DLL"
            Set-Content -Path (Join-Path $binPath "slang-glsl-module.dll") -Value "MOCK DLL"
            Set-Content -Path (Join-Path $binPath "slang-glslang.dll") -Value "MOCK DLL"
            if ($Platform -eq "x64") {
                Set-Content -Path (Join-Path $binPath "slang-llvm.dll") -Value "MOCK DLL"
            }
            Set-Content -Path (Join-Path $binPath "slang.slang") -Value "MOCK SLANG"
            Set-Content -Path (Join-Path $binPath "gfx.slang") -Value "MOCK SLANG"
            Set-Content -Path (Join-Path $binPath "slangc.exe") -Value "MOCK EXE"
            Set-Content -Path (Join-Path $binPath "slangd.exe") -Value "MOCK EXE"
            Set-Content -Path (Join-Path $binPath "slangi.exe") -Value "MOCK EXE"
            
            # Create lib directory and mock files
            $libPath = Join-Path $platformPath "lib"
            New-Item -ItemType Directory -Path $libPath -Force | Out-Null
            Set-Content -Path (Join-Path $libPath "slang.lib") -Value "MOCK LIB"
            Set-Content -Path (Join-Path $libPath "slang-rt.lib") -Value "MOCK LIB"
            Set-Content -Path (Join-Path $libPath "gfx.lib") -Value "MOCK LIB"
            
            # Create include directory and mock files
            $includePath = Join-Path $platformPath "include"
            New-Item -ItemType Directory -Path $includePath -Force | Out-Null
            Set-Content -Path (Join-Path $includePath "slang.h") -Value "MOCK HEADER"
            Set-Content -Path (Join-Path $includePath "slang-cpp-types.h") -Value "MOCK HEADER"

            # Create a simple readme at the root level
            Set-Content -Path (Join-Path $extractPath "README.md") -Value "# Mock Slang SDK"
            Set-Content -Path (Join-Path $platformPath "LICENSE") -Value "Mock License"
            
            Write-Host "MOCK MODE: Created simulated Slang SDK directory structure"
        } else {
            # Real download
            $ProgressPreference = 'SilentlyContinue'
            Invoke-WebRequest -Uri $downloadUrl -OutFile $zipFile
            Write-Host "Download complete: $zipFile"
            
            # Extract the ZIP file
            Write-Host "Extracting Slang SDK..."
            if (Test-Path $extractPath) {
                Write-Host "Removing existing extracted files..."
                Remove-Item -Recurse -Force $extractPath
            }

            try {
                Expand-Archive -Path $zipFile -DestinationPath $extractDir
                Write-Host "Extraction complete to $extractPath"
            }
            catch {
                Write-Error "Failed to extract Slang SDK: $_"
                exit 1
            }
        }
    }
    catch {
        Write-Error "Failed to download Slang SDK: $_"
        exit 1
    }
}
else {
    Write-Host "Using existing download: $zipFile"
    
    if ($mockDownload) {
        # Skip extraction for mock mode if files already exist
        Write-Host "MOCK MODE: Using existing simulated Slang SDK"
    } else {
        # Extract the ZIP file
        Write-Host "Extracting Slang SDK..."
        if (Test-Path $extractPath) {
            Write-Host "Removing existing extracted files..."
            Remove-Item -Recurse -Force $extractPath
        }

        try {
            Expand-Archive -Path $zipFile -DestinationPath $extractDir
            Write-Host "Extraction complete to $extractPath"
        }
        catch {
            Write-Error "Failed to extract Slang SDK: $_"
            exit 1
        }
    }
}

# Verify extraction succeeded
if (-Not (Test-Path "$platformPath\bin") -or -Not (Test-Path "$platformPath\lib")) {
    Write-Error "Extraction completed but expected directories not found. Verify the archive structure."
    exit 1
}

Write-Host "Slang SDK $SlangVersion for $Platform successfully installed to $platformPath"
