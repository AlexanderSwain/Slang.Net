param(
    [Parameter(Mandatory=$true)]
    [string]$Script,

    [switch]$FromVisualStudio
)

$platforms = @(
    #"x86", # Uncomment when support for x86 is added
    "x64",
    "ARM64"
)

$configuration = @(
    "Debug",
    "Release"
)

$scriptName = Split-Path -Leaf $Script
$scriptType = if ($scriptName -eq "pre-build.ps1") { "Pre-Build" } else { "Post-Build" }
$errorCount = 0

Write-Host "Starting multi-platform $scriptType process for: $Script" -ForegroundColor Cyan
Write-Host "Platforms: $($platforms -join ', ')" -ForegroundColor Cyan 
Write-Host "Configurations: $($configuration -join ', ')" -ForegroundColor Cyan

# Verify the script exists
if (-not (Test-Path $Script)) {
    Write-Host "ERROR: Script '$Script' not found!" -ForegroundColor Red
    exit 1
}

foreach ($Platform in $platforms) {
    foreach ($Configuration in $configuration) {
        # Add more debugging information
        Write-Host "DEBUG: About to process $Configuration | $Platform" -ForegroundColor Yellow
        
        Write-Host "===== $scriptType SlangNative Library: $Configuration | $Platform =====" -ForegroundColor DarkGray
        
        try {
            & $Script -Configuration $Configuration -Platform $Platform -FromVisualStudio:$FromVisualStudio
            
            if ($?) {
                Write-Host "$scriptType completed successfully for $Configuration | $Platform" -ForegroundColor Green
            } else {
                Write-Host "$scriptType failed for $Configuration | $Platform" -ForegroundColor Red
                $errorCount++
            }
        } catch {
            Write-Host "Exception during $scriptType for $Configuration | $Platform" -ForegroundColor Red
            Write-Host $_.Exception.Message -ForegroundColor Red
            $errorCount++        }
    }
}

if ($errorCount -gt 0) {
    Write-Host "FAILURE: $errorCount configuration(s) failed to build." -ForegroundColor Red
    exit 1
} else {
    Write-Host "SUCCESS: All configurations completed successfully." -ForegroundColor Green
    exit 0
}