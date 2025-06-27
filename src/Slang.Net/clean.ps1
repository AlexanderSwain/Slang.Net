param(
    [Parameter(Mandatory=$true)]
    [string]$Configuration,
    
    [Parameter(Mandatory=$true)]
    [string]$Platform,

    [switch]$FromVisualStudio
)

Write-Host "===== Cleaning Slang.Net.CPP Library Output =====" -ForegroundColor DarkGray

# Directories
$binToDelete = "$PSScriptRoot\bin\$Configuration\net9.0\$Platform"
$libToDelete = "$PSScriptRoot\lib\$Configuration\net9.0\$Platform"

# Delete everything in bin
if (Test-Path -Path $binToDelete) {
    Remove-Item -Path "$binToDelete\*" -Force -Recurse -ErrorAction Stop
    Write-Host "Deleted all contents in: $binToDelete" -ForegroundColor Green
} else {
    Write-Host "Directory does not exist, skipping: $binToDelete" -ForegroundColor Yellow
}

# Delete everything in lib
if (Test-Path -Path $libToDelete) {
    Remove-Item -Path "$libToDelete\*" -Force -Recurse -ErrorAction Stop
    Write-Host "Deleted all contents in: $libToDelete" -ForegroundColor Green
} else {
    Write-Host "Directory does not exist, skipping: $libToDelete" -ForegroundColor Yellow
}