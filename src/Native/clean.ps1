param(
    [Parameter(Mandatory=$true)]
    [string]$Configuration,
    
    [Parameter(Mandatory=$true)]
    [string]$Platform,

    [switch]$FromVisualStudio
)

Write-Host "===== Cleaning Slang.Net Library Output =====" -ForegroundColor DarkGray

# Directories
$toDelete = "$PSScriptRoot\bin\$Configuration\$Platform"

# Delete everything in bin
if (Test-Path -Path $toDelete) {
    Remove-Item -Path "$toDelete\*" -Force -Recurse -ErrorAction Continue
    Write-Host "Deleted all contents in: $toDelete" -ForegroundColor Green
} else {
    Write-Host "Directory does not exist, skipping: $toDelete" -ForegroundColor Yellow
}