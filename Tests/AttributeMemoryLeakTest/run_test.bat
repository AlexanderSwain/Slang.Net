@echo off
echo Building and running Native::Attribute Memory Leak Test
echo.

echo Building SlangNative project...
msbuild "..\..\src\Native\SlangNative.vcxproj" /p:Configuration=Debug /p:Platform=x64

if %ERRORLEVEL% neq 0 (
    echo Error: Failed to build SlangNative project
    pause
    exit /b 1
)

echo Building test project...
msbuild "AttributeMemoryLeakTest.vcxproj" /p:Configuration=Debug /p:Platform=x64

if %ERRORLEVEL% neq 0 (
    echo Error: Failed to build test project
    pause
    exit /b 1
)

echo.
echo Running memory leak test...
echo.

"..\..\bin\Debug\x64\AttributeMemoryLeakTest.exe"

echo.
echo Test completed. Check the output above for memory leak results.
pause