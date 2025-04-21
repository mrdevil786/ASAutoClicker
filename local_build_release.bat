@echo off
echo ===== AutoClicker Build Script =====

set MSBUILD_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

echo Building the project in Release mode...
"%MSBUILD_PATH%" AutoClicker.sln /p:Configuration=Release /t:Clean,Build

if %errorlevel% neq 0 (
    echo Build failed with error code %errorlevel%.
    pause
    exit /b %errorlevel%
)

echo.
echo ===== Build Completed Successfully =====
echo The built release is available in bin\Release\AutoClicker.exe
echo.

pause 