param(
    [string]$Configuration = "Debug",
    [string]$Platform = "x64"
)

$ErrorActionPreference = "Stop"

# Define paths
$solutionDir = $PSScriptRoot
$slangNetDir = Join-Path $solutionDir "src\Slang.Net"
$nativeDir = Join-Path $solutionDir "src\Native"
$slangNetBinDir = Join-Path $slangNetDir "bin\Debug\net9.0" # Corrected path based on output
$nugetPackagePath = Join-Path $slangNetBinDir "slang.net.0.0.4.nupkg"

Write-Host "===== Building Slang.Net Complete Solution =====" -ForegroundColor Cyan

# STEP 1: Download Slang SDK if not already present
Write-Host "STEP 1: Ensuring Slang SDK is downloaded..." -ForegroundColor Green
& "$nativeDir\download-slang-sdk.ps1" -Platform $Platform

# STEP 2: Build SlangNative project for the specified platform
Write-Host "STEP 2: Building SlangNative project ($Platform)..." -ForegroundColor Green
$msbuildPath = "${env:ProgramFiles}\Microsoft Visual Studio\2022\Preview\MSBuild\Current\Bin\MSBuild.exe"
& $msbuildPath "$nativeDir\SlangNative.vcxproj" /p:Configuration=$Configuration /p:Platform=$Platform /t:Rebuild

if (-not $?) {
    Write-Host "SlangNative build failed!" -ForegroundColor Red
    exit 1
}

# Ensure SlangNative.lib exists
$slangNativeLib = Join-Path $slangNetBinDir "SlangNative.lib"
if (-not (Test-Path $slangNativeLib)) {
    Write-Host "SlangNative.lib not found at expected location: $slangNativeLib" -ForegroundColor Red
    
    # Check alternative locations
    $altLocations = @(
        "$solutionDir\Slang.Net\bin\Debug\net9.0\SlangNative.lib",
        "$solutionDir\src\Slang.Net\bin\Debug\net9.0\SlangNative.lib",
        "$solutionDir\src\Native\Slang.Net\bin\Debug\net9.0\SlangNative.lib"
    )
    
    foreach ($altLocation in $altLocations) {
        if (Test-Path $altLocation) {
            Write-Host "Found SlangNative.lib at alternative location: $altLocation" -ForegroundColor Yellow
            $slangNativeLib = $altLocation
            break
        }
    }
    
    if (-not (Test-Path $slangNativeLib)) {
        Write-Host "Could not find SlangNative.lib at any location. Build may fail." -ForegroundColor Red
    }
} else {
    Write-Host "Found SlangNative.lib at: $slangNativeLib" -ForegroundColor Green
}

# Create the expected directory structure for the Slang.Net.CPP project
$slangNetCppSlangNetPath = Join-Path $solutionDir "src\Slang.Net.CPP\src\Slang.Net\bin\$Configuration\net9.0"
if (-not (Test-Path $slangNetCppSlangNetPath)) {
    Write-Host "Creating directory structure for Slang.Net.CPP: $slangNetCppSlangNetPath" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $slangNetCppSlangNetPath -Force | Out-Null
}

# Copy SlangNative.lib to where Slang.Net.CPP expects it
Write-Host "Copying SlangNative.lib to the path expected by Slang.Net.CPP..." -ForegroundColor Green
Copy-Item -Path $slangNativeLib -Destination $slangNetCppSlangNetPath -Force

# STEP 3: Build Slang.Net.CPP project
Write-Host "STEP 3: Building Slang.Net.CPP project ($Platform)..." -ForegroundColor Green
& $msbuildPath "$solutionDir\src\Slang.Net.CPP\Slang.Net.CPP.vcxproj" /p:Configuration=$Configuration /p:Platform=$Platform /t:Rebuild

if (-not $?) {
    Write-Host "Slang.Net.CPP build failed!" -ForegroundColor Red
    exit 1
}

# STEP 4: Build Slang.Net project to generate the NuGet package
Write-Host "STEP 4: Building Slang.Net project to generate NuGet package..." -ForegroundColor Green
& $msbuildPath "$slangNetDir\Slang.Net.csproj" /p:Configuration=$Configuration /p:Platform=$Platform /t:Rebuild

if (-not $?) {
    Write-Host "Slang.Net build failed!" -ForegroundColor Red
    exit 1
}

# STEP 5: Add the generated package to the local NuGet cache
Write-Host "STEP 5: Adding NuGet package to local cache..." -ForegroundColor Green

# Search for the package in all possible locations
$alternateLocations = @(
    "$slangNetBinDir", # Original path
    "$solutionDir\src\Slang.Net\bin\Debug\net9.0",
    "$solutionDir\src\Slang.Net\bin\$Configuration\net9.0",
    "$solutionDir\src\Slang.Net\bin\$Platform\$Configuration",
    "$solutionDir\src\Slang.Net\bin\$Platform\Debug"
)

$foundPackage = $false
foreach ($location in $alternateLocations) {
    $altPath = Join-Path $location "slang.net.0.0.4.nupkg"
    if (Test-Path $altPath) {
        Write-Host "Found NuGet package at: $altPath" -ForegroundColor Green
        
        # Add package to local cache
        & dotnet nuget add package Slang.Net -s $location -v 0.0.4 --no-cache
        
        if (-not $?) {
            Write-Host "Warning: Failed to add package from $location to local cache." -ForegroundColor Yellow
        } else {
            Write-Host "Successfully added package to local NuGet cache from $location" -ForegroundColor Green
            $foundPackage = $true
            
            # Update the path to use in nuget.config
            $slangNetBinDir = $location
            break
        }
    }
}

if (-not $foundPackage) {
    Write-Host "Error: Could not find NuGet package in any expected locations. Build will likely fail." -ForegroundColor Red
    
    # List all potential package locations to help diagnose
    Get-ChildItem -Path "$solutionDir\src\Slang.Net\bin" -Recurse -Filter "*.nupkg" | ForEach-Object {
        Write-Host "Found package: $($_.FullName)" -ForegroundColor Yellow
    }
}

# STEP 6: Create or update nuget.config
Write-Host "STEP 6: Creating/updating nuget.config..." -ForegroundColor Green
$nugetConfigPath = Join-Path $solutionDir "nuget.config"
$nugetConfigContent = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="local" value="$slangNetBinDir" />
  </packageSources>
</configuration>
"@

Set-Content -Path $nugetConfigPath -Value $nugetConfigContent

# STEP 7: Update sample projects to use NuGet package reference
Write-Host "STEP 7: Updating sample projects to use NuGet package references..." -ForegroundColor Green
$sampleProjects = 
@(
    "Samples\Slang.Net.Samples.SimpleCompileTest\Slang.Net.Samples.SimpleCompileTest.csproj",
    "Samples\Slang.Net.Samples.CrossPlatform\Slang.Net.Samples.CrossPlatform.csproj",
    "Samples\Slang.Net.Samples.DirectXSilkNet\Slang.Net.Samples.DirectXSilkNet.csproj",
    "Samples\Slang.Net.Samples.OpenGLSilkNet\Slang.Net.Samples.OpenGLSilkNet.csproj",
    "Samples\Slang.Net.Samples.VulkanSilkNet\Slang.Net.Samples.VulkanSilkNet.csproj"
)

foreach ($project in $sampleProjects) {
    $projectPath = Join-Path $solutionDir $project
    $content = Get-Content -Path $projectPath -Raw
    
    # Replace project reference with package reference
    if ($content -match '<ProjectReference Include="..\\..\\src\\Slang.Net\\Slang.Net.csproj" />') {
        $newContent = $content -replace '<ProjectReference Include="..\\..\\src\\Slang.Net\\Slang.Net.csproj" />', '<PackageReference Include="Slang.Net" Version="0.0.4" />'
        Set-Content -Path $projectPath -Value $newContent
        Write-Host "Updated $project to use NuGet package reference" -ForegroundColor Yellow
    } elseif (-not ($content -match '<PackageReference Include="Slang\.Net" Version="0\.0\.4" />')) {
        Write-Host "Warning: $project doesn't have expected project reference pattern" -ForegroundColor Yellow
    } else {
        Write-Host "$project already using NuGet package reference" -ForegroundColor Green
    }
}

# STEP 8: Restore and build sample projects
Write-Host "STEP 8: Restoring and building sample projects..." -ForegroundColor Green
& dotnet restore "$solutionDir\Slang.Net.sln"

if (-not $?) {
    Write-Host "Package restore had warnings but continuing..." -ForegroundColor Yellow
}

# Build the SimpleCompileTest project specifically
& $msbuildPath "$solutionDir\Samples\Slang.Net.Samples.SimpleCompileTest\Slang.Net.Samples.SimpleCompileTest.csproj" /p:Configuration=$Configuration /p:Platform=$Platform

Write-Host "===== Build Complete =====" -ForegroundColor Cyan
Write-Host "NuGet package location: $nugetPackagePath" -ForegroundColor Green

# Return package path so it can be used by other scripts
return $nugetPackagePath
