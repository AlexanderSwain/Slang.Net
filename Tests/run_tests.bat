@echo off
setlocal enabledelayedexpansion

echo ===== Slang.Net Test Suite =====
echo.

:: Set default values
set CONFIGURATION=Debug
set PLATFORM=x64

:: Parse command line arguments
:parse_args
if "%~1"=="" goto :start_build
if /i "%~1"=="--config" (
    set CONFIGURATION=%~2
    shift
    shift
    goto :parse_args
)
if /i "%~1"=="--platform" (
    set PLATFORM=%~2
    shift
    shift
    goto :parse_args
)
if /i "%~1"=="--help" (
    goto :show_help
)
if /i "%~1"=="-h" (
    goto :show_help
)
shift
goto :parse_args

:show_help
echo Usage: run_tests.bat [--config Debug^|Release] [--platform x64^|ARM64] [--help]
echo.
echo Options:
echo   --config     Build configuration (Debug or Release, default: Debug)
echo   --platform   Target platform (x64 or ARM64, default: x64)
echo   --help, -h   Show this help message
echo.
echo Examples:
echo   run_tests.bat                           # Debug x64
echo   run_tests.bat --config Release          # Release x64  
echo   run_tests.bat --platform ARM64          # Debug ARM64
echo   run_tests.bat --config Release --platform ARM64  # Release ARM64
echo.
goto :end

:start_build
echo Configuration: %CONFIGURATION%
echo Platform: %PLATFORM%
echo.

:: Check if we're in the correct directory
if not exist "Slang.Net.sln" (
    echo Error: Slang.Net.sln not found. Please run this script from the solution root directory.
    exit /b 1
)

:: Step 1: Build SlangNative
echo [1/3] Building SlangNative (%CONFIGURATION%^|%PLATFORM%)...
msbuild src\Native\SlangNative.vcxproj /p:Configuration=%CONFIGURATION% /p:Platform=%PLATFORM% /nologo /verbosity:minimal
if !errorlevel! neq 0 (
    echo ERROR: SlangNative build failed!
    exit /b 1
)
echo ? SlangNative build successful
echo.

:: Step 2: Build AttributeMemoryLeakTest
echo [2/3] Building AttributeMemoryLeakTest (%CONFIGURATION%^|%PLATFORM%)...
msbuild Tests\AttributeMemoryLeakTest\AttributeMemoryLeakTest.vcxproj /p:Configuration=%CONFIGURATION% /p:Platform=%PLATFORM% /nologo /verbosity:minimal
if !errorlevel! neq 0 (
    echo ERROR: AttributeMemoryLeakTest build failed!
    exit /b 1
)
echo ? AttributeMemoryLeakTest build successful
echo.

:: Step 3: Run tests
echo [3/3] Running tests...
echo =====================================

set TEST_EXE=bin\%CONFIGURATION%\%PLATFORM%\AttributeMemoryLeakTest.exe

if not exist "%TEST_EXE%" (
    echo ERROR: Test executable not found at %TEST_EXE%
    echo Please check the build output for errors.
    exit /b 1
)

:: Run the test and capture the exit code
"%TEST_EXE%"
set TEST_RESULT=!errorlevel!

echo.
echo =====================================
if !TEST_RESULT! equ 0 (
    echo ? All tests passed successfully!
) else (
    echo ? Tests failed with exit code: !TEST_RESULT!
)

echo.
echo Test Summary:
echo   Configuration: %CONFIGURATION%
echo   Platform: %PLATFORM%
echo   Test executable: %TEST_EXE%
echo   Exit code: !TEST_RESULT!

:end
echo.
pause
exit /b !TEST_RESULT!