@echo off
SETLOCAL EnableDelayedExpansion

echo ===== AutoClicker Release Builder =====

set /p VERSION="Enter release version (e.g. 1.0.0): "
echo You entered: %VERSION%

REM Skip validation for now - just build with the provided version
echo.
echo Building release version %VERSION%...
echo.

REM Create backup of AssemblyInfo.cs before modifying
copy /Y Properties\AssemblyInfo.cs Properties\AssemblyInfo.cs.bak >nul

REM Update AssemblyInfo.cs with new version
echo Updating version in AssemblyInfo.cs to %VERSION%...
powershell -Command "(Get-Content Properties\AssemblyInfo.cs) -replace 'AssemblyVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyVersion(\"%VERSION%.0\")' -replace 'AssemblyFileVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyFileVersion(\"%VERSION%.0\")' | Set-Content Properties\AssemblyInfo.cs"

REM Build the release version
echo Building release...
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe AutoClicker.sln /p:Configuration=Release /t:Clean,Build

if %errorlevel% neq 0 (
    echo.
    echo Build failed! Restoring original AssemblyInfo.cs...
    copy /Y Properties\AssemblyInfo.cs.bak Properties\AssemblyInfo.cs >nul
    del Properties\AssemblyInfo.cs.bak >nul
    echo.
    pause
    exit /b 1
)

REM Clean up backup
del Properties\AssemblyInfo.cs.bak >nul

echo.
echo ===== Release build completed successfully =====
echo Version: v%VERSION%
echo Built release is available in bin\Release\AutoClicker.exe
echo.

pause
ENDLOCAL 