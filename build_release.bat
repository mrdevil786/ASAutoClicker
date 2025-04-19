@echo off
echo Building AutoClicker in Release mode...

REM Try to find MSBuild in standard locations
set MSBUILD_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe
if not exist "%MSBUILD_PATH%" (
    set MSBUILD_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe
)
if not exist "%MSBUILD_PATH%" (
    set MSBUILD_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe
)

if not exist "%MSBUILD_PATH%" (
    echo MSBuild not found. Please build the application using Visual Studio.
    echo Open AutoClicker.sln in Visual Studio and build with Release configuration.
    goto end
)

echo Using MSBuild at: %MSBUILD_PATH%
"%MSBUILD_PATH%" AutoClicker.sln /p:Configuration=Release /p:Platform="Any CPU"

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
) else (
    echo Build successful!
    echo Release files are in the bin\Release directory
    
    REM Create a release ZIP file
    echo Creating release package...
    mkdir Release 2>nul
    copy bin\Release\AutoClicker.exe Release\ /y
    
    REM Remove any existing zip file
    if exist AutoClicker.zip del AutoClicker.zip
    
    REM Try to zip using PowerShell
    powershell -Command "Compress-Archive -Path Release\* -DestinationPath AutoClicker.zip -Force"
    
    if exist AutoClicker.zip (
        echo Release package created: AutoClicker.zip
    ) else (
        echo Failed to create release package. Please manually zip the bin\Release\AutoClicker.exe file.
    )
)

:end
pause 