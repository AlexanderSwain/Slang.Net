#!/usr/bin/env pwsh

# Simple test script for Slang SDK download

Write-Host "Testing Slang SDK download..." -ForegroundColor Cyan

# First, clean up any existing downloads
if (Test-Path "src\Native\EmbeddedLLVM") {
    Write-Host "Cleaning up existing downloads..." -ForegroundColor Yellow
    Remove-Item -Path "src\Native\EmbeddedLLVM" -Recurse -Force
}

# Now run the download script
Write-Host "Running download script for x64 platform..." -ForegroundColor Yellow
$scriptPath = "src\Native\download-slang-sdk.ps1"

if (-not (Test-Path $scriptPath)) {
    Write-Host "ERROR: Download script not found at $scriptPath" -ForegroundColor Red
    exit 1
}

try {
    & powershell -ExecutionPolicy Bypass -File $scriptPath -Platform x64
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: Download script failed with exit code $LASTEXITCODE" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Download script executed successfully!" -ForegroundColor Green
    
    # Check if directories were created
    $libDir = "src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\lib"
    $binDir = "src\Native\EmbeddedLLVM\slang-2025.10.3-windows\x64\bin"
    
    if (Test-Path $libDir) {
        Write-Host "SUCCESS: Lib directory created at $libDir" -ForegroundColor Green
    } else {
        Write-Host "ERROR: Lib directory not created at $libDir" -ForegroundColor Red
        exit 1
    }
    
    if (Test-Path $binDir) {
        Write-Host "SUCCESS: Bin directory created at $binDir" -ForegroundColor Green
    } else {
        Write-Host "ERROR: Bin directory not created at $binDir" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "TEST PASSED: Slang SDK download functionality works correctly!" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Exception while testing download script: $_" -ForegroundColor Red
    exit 1
}
