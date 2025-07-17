param(
    [Parameter()]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    
    [Parameter()]
    [ValidateSet("x64", "ARM64")]
    [string]$Platform = "x64",
    
    [switch]$Help,
    
    [switch]$SkipBuild,
    
    [switch]$Verbose
)

function Show-Help {
    Write-Host @"
Slang.Net Test Runner

Usage: .\run_tests.ps1 [-Configuration <Debug|Release>] [-Platform <x64|ARM64>] [options]

Parameters:
  -Configuration    Build configuration (Debug or Release, default: Debug)
  -Platform         Target platform (x64 or ARM64, default: x64)
  -SkipBuild        Skip building and only run existing test executable
  -Verbose          Show detailed build output
  -Help             Show this help message

Examples:
  .\run_tests.ps1                                    # Debug x64
  .\run_tests.ps1 -Configuration Release             # Release x64
  .\run_tests.ps1 -Platform ARM64                    # Debug ARM64
  .\run_tests.ps1 -Configuration Release -Platform ARM64 # Release ARM64
  .\run_tests.ps1 -SkipBuild                         # Skip build, just run tests
  .\run_tests.ps1 -Verbose                           # Show detailed output

"@
}

function Write-Step {
    param([string]$Message, [string]$Color = "Green")
    Write-Host "[$($MyInvocation.ScriptLineNumber)] $Message" -ForegroundColor $Color
}

function Write-Success {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Red
}

function Write-Warning {
    param([string]$Message)
    Write-Host "?? $Message" -ForegroundColor Yellow
}

if ($Help) {
    Show-Help
    exit 0
}

Write-Host "===== Slang.Net Test Suite =====" -ForegroundColor DarkCyan
Write-Host ""
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Platform: $Platform" -ForegroundColor Yellow
Write-Host "Skip Build: $SkipBuild" -ForegroundColor Yellow
Write-Host ""

# Check if we're in the correct directory
if (-not (Test-Path "Slang.Net.sln")) {
    Write-Error "Slang.Net.sln not found. Please run this script from the solution root directory."
    exit 1
}

# Configure verbosity
$BuildVerbosity = if ($Verbose) { "normal" } else { "minimal" }

if (-not $SkipBuild) {
    # Step 1: Build SlangNative
    Write-Step "Building SlangNative ($Configuration|$Platform)..." "Blue"
    
    $slangNativePath = "src\Native\SlangNative.vcxproj"
    $buildArgs = @(
        $slangNativePath
        "/p:Configuration=$Configuration"
        "/p:Platform=$Platform"
        "/nologo"
        "/verbosity:$BuildVerbosity"
    )
    
    & msbuild @buildArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "SlangNative build failed!"
        exit 1
    }
    Write-Success "SlangNative build successful"
    Write-Host ""
    
    # Step 2: Build AttributeMemoryLeakTest
    Write-Step "Building AttributeMemoryLeakTest ($Configuration|$Platform)..." "Blue"
    
    $testProjectPath = "Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj"
    $buildArgs = @(
        $testProjectPath
        "/p:Configuration=$Configuration"
        "/p:Platform=$Platform"
        "/nologo"
        "/verbosity:$BuildVerbosity"
    )
    
    & msbuild @buildArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "AttributeMemoryLeakTest build failed!"
        exit 1
    }
    Write-Success "AttributeMemoryLeakTest build successful"
    Write-Host ""
}

# Step 3: Run tests
Write-Step "Running tests..." "Blue"
Write-Host "=====================================" -ForegroundColor DarkGray

$testExe = "bin\$Configuration\$Platform\AttributeMemoryLeakTest.exe"

if (-not (Test-Path $testExe)) {
    Write-Error "Test executable not found at $testExe"
    Write-Host "Please check the build output for errors." -ForegroundColor Yellow
    exit 1
}

# Get file info for verification
$exeInfo = Get-Item $testExe
Write-Host "Test executable: $testExe" -ForegroundColor Gray
Write-Host "File size: $([math]::Round($exeInfo.Length / 1MB, 2)) MB" -ForegroundColor Gray
Write-Host "Last modified: $($exeInfo.LastWriteTime)" -ForegroundColor Gray
Write-Host ""

# Run the test and capture the result
$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

try {
    & $testExe
    $testResult = $LASTEXITCODE
}
catch {
    Write-Error "Failed to run test executable: $_"
    exit 1
}

$stopwatch.Stop()

Write-Host ""
Write-Host "=====================================" -ForegroundColor DarkGray

# Report results
if ($testResult -eq 0) {
    Write-Success "All tests passed successfully!"
} else {
    Write-Error "Tests failed with exit code: $testResult"
}

Write-Host ""
Write-Host "Test Summary:" -ForegroundColor Cyan
Write-Host "  Configuration: $Configuration" -ForegroundColor White
Write-Host "  Platform: $Platform" -ForegroundColor White
Write-Host "  Test executable: $testExe" -ForegroundColor White
Write-Host "  Execution time: $($stopwatch.Elapsed.TotalSeconds.ToString('F2')) seconds" -ForegroundColor White
Write-Host "  Exit code: $testResult" -ForegroundColor White

# Additional file verification
Write-Host ""
Write-Host "Runtime Dependencies:" -ForegroundColor Cyan
$binDir = "bin\$Configuration\$Platform"
$requiredDlls = @("SlangNative.dll", "slang.dll", "gfx.dll", "slang-rt.dll")

foreach ($dll in $requiredDlls) {
    $dllPath = Join-Path $binDir $dll
    if (Test-Path $dllPath) {
        $dllInfo = Get-Item $dllPath
        Write-Host "  ? $dll ($([math]::Round($dllInfo.Length / 1KB, 1)) KB)" -ForegroundColor Green
    } else {
        Write-Host "  ? $dll (missing)" -ForegroundColor Red
    }
}

# Check for test data files
$testDataFiles = @("ComprehensiveSlangTest.slang")
Write-Host ""
Write-Host "Test Data Files:" -ForegroundColor Cyan

foreach ($file in $testDataFiles) {
    $filePath = Join-Path $binDir $file
    if (Test-Path $filePath) {
        $fileInfo = Get-Item $filePath
        Write-Host "  ? $file ($([math]::Round($fileInfo.Length / 1KB, 1)) KB)" -ForegroundColor Green
    } else {
        Write-Host "  ? $file (missing)" -ForegroundColor Red
    }
}

Write-Host ""

if ($testResult -eq 0) {
    Write-Host "?? Test run completed successfully!" -ForegroundColor Green
} else {
    Write-Host "?? Test run failed. Check the output above for details." -ForegroundColor Red
}

exit $testResult