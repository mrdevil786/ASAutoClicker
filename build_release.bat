@echo off
SETLOCAL EnableDelayedExpansion

echo ===== AutoClicker Release Builder =====

set /p VERSION="Enter release version (e.g. 1.0.0): "

REM Validate version format
echo %VERSION% | findstr /r "^[0-9][0-9]*\.[0-9][0-9]*\.[0-9][0-9]*$" > nul
if %errorlevel% neq 0 (
    echo Invalid version format. Please use format like 1.0.0
    pause
    exit /b 1
)

echo Checking if version v%VERSION% exists...

REM Check for local tag more robustly
git show-ref --tags --quiet --verify -- "refs/tags/v%VERSION%"
if %errorlevel% equ 0 (
    echo Local tag v%VERSION% exists.
    set /p CONTINUE="Do you want to remove the existing tag and continue? (Y/N): "
    if /i "!CONTINUE!" neq "Y" (
        echo Build process canceled.
        pause
        exit /b 0
    )
    echo Removing local tag v%VERSION%...
    git tag -d v%VERSION%
) else (
    echo Local tag v%VERSION% does not exist. Proceeding...
)

REM Check for remote tag
git ls-remote --tags origin v%VERSION% | findstr "v%VERSION%" > nul
if %errorlevel% equ 0 (
    echo Remote tag v%VERSION% exists.
    set /p CONTINUE="Do you want to remove the remote tag and continue? (Y/N): "
    if /i "!CONTINUE!" neq "Y" (
        echo Build process canceled.
        pause
        exit /b 0
    )
    echo Removing remote tag v%VERSION%...
    git push origin :refs/tags/v%VERSION%
) else (
    echo Remote tag v%VERSION% does not exist. Proceeding...
)

REM Get current branch name
for /f "tokens=*" %%a in ('git rev-parse --abbrev-ref HEAD') do set BRANCH=%%a
echo Current branch is: %BRANCH%

REM Update AssemblyInfo.cs with new version
echo Updating version in AssemblyInfo.cs to %VERSION%...
powershell -Command "(Get-Content Properties\AssemblyInfo.cs) -replace 'AssemblyVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyVersion(\"%VERSION%.0\")' -replace 'AssemblyFileVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyFileVersion(\"%VERSION%.0\")' | Set-Content Properties\AssemblyInfo.cs"

echo Adding modified files...
git add MainForm.cs Program.cs MainForm.Designer.cs Properties/AssemblyInfo.cs build_release.bat README.md

REM Check if there are changes to commit
git diff --cached --quiet
if %errorlevel% neq 0 (
    echo Committing changes...
    git commit -m "Prepare release v%VERSION% with code optimizations and improvements"
) else (
    echo No changes to commit, skipping commit step.
)

REM Build the release version
echo Building release...
msbuild AutoClicker.sln /p:Configuration=Release /t:Clean,Build

if %errorlevel% neq 0 (
    echo Build failed! Aborting release process.
    pause
    exit /b 1
)

echo Creating new v%VERSION% tag...
git tag -a v%VERSION% -m "Release version %VERSION% with optimizations and improvements"

echo Pushing changes to remote...
git push origin %BRANCH%
git push origin v%VERSION%

echo ===== Release build process completed successfully =====
echo Version: v%VERSION%
echo Built release is available in bin\Release\AutoClicker.exe
echo Release changes have been committed and tagged.

pause
ENDLOCAL 