param(
    [Parameter(Mandatory=$true)]
    [string]$Script,

    [Parameter(Mandatory=$true)]
    [string]$Configuration,

    [switch]$FromVisualStudio
)

$platforms = @(
    #"x86", # Uncomment when support for x86 is added
    "x64",
    "ARM64"
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

# Process Debug | ARM64 first to ensure it's handled
Write-Host "===== EXPLICITLY PROCESSING Debug | ARM64 =====" -ForegroundColor Magenta
& $Script -Configuration "Debug" -Platform "ARM64" -FromVisualStudio:$FromVisualStudio

foreach ($Platform in $platforms) {
        # Skip Debug | ARM64 since we already processed it explicitly
        if ($Platform -eq "ARM64" -and $Configuration -eq "Debug") {
            Write-Host "DEBUG: Skipping Debug | ARM64 - already processed explicitly" -ForegroundColor Yellow
            continue
        }
        
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
            $errorCount++        
        }
}

if ($errorCount -gt 0) {
    Write-Host "FAILURE: $errorCount configuration(s) failed to build." -ForegroundColor Red
    exit 1
} else {
    Write-Host "SUCCESS: All configurations completed successfully." -ForegroundColor Green
    exit 0
}