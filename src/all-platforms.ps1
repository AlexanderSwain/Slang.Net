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

foreach ($Platform in $platforms) {
    foreach ($Configuration in $configuration) {
        Write-Host "===== Pre-Build SlangNative Library: $Configuration | $Platform =====" -ForegroundColor DarkGray
        & $Script -Configuration $Configuration -Platform $Platform -FromVisualStudio:$FromVisualStudio
        
        if ($?) {
            Write-Host "Pre-Build completed successfully for $Configuration | $Platform" -ForegroundColor Green
        } else {
            Write-Host "Pre-Build failed for $Configuration | $Platform" -ForegroundColor Red
            exit 1
        }
    }
}