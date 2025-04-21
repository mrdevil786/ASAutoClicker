@echo off
SETLOCAL EnableDelayedExpansion

echo ===== AutoClicker Release Builder =====

set /p VERSION="Enter release version (e.g. 1.0.0): "
echo You entered: %VERSION%

echo.
echo Preparing release version %VERSION%...
echo.

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

REM Create backup of AssemblyInfo.cs before modifying
copy /Y Properties\AssemblyInfo.cs Properties\AssemblyInfo.cs.bak >nul

REM Update AssemblyInfo.cs with new version
echo Updating version in AssemblyInfo.cs to %VERSION%...
powershell -Command "(Get-Content Properties\AssemblyInfo.cs) -replace 'AssemblyVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyVersion(\"%VERSION%.0\")' -replace 'AssemblyFileVersion\(\"[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\"\)', 'AssemblyFileVersion(\"%VERSION%.0\")' | Set-Content Properties\AssemblyInfo.cs"

REM Verify that the replacement was successful
powershell -Command "if (-not (Select-String -Path 'Properties\AssemblyInfo.cs' -Pattern 'AssemblyVersion\(\"%VERSION%.0\"\)')) { Write-Host 'Error: Failed to update version in AssemblyInfo.cs'; exit 1 }"
if %errorlevel% neq 0 (
    echo Restoring backup of AssemblyInfo.cs...
    copy /Y Properties\AssemblyInfo.cs.bak Properties\AssemblyInfo.cs >nul
    del Properties\AssemblyInfo.cs.bak >nul
    echo Process failed. Please check the format of the version number.
    pause
    exit /b 1
)

REM Clean up backup if everything is fine
del Properties\AssemblyInfo.cs.bak >nul

echo Adding modified files...
git add MainForm.cs Program.cs MainForm.Designer.cs Properties/AssemblyInfo.cs README.md

REM Check if there are changes to commit
git diff --cached --quiet
if %errorlevel% neq 0 (
    echo Committing changes...
    git commit -m "Prepare release v%VERSION% with code optimizations and improvements"
) else (
    echo No changes to commit, skipping commit step.
)

echo Creating new v%VERSION% tag...
git tag -a v%VERSION% -m "Release version %VERSION% with optimizations and improvements"

echo Pushing changes to remote...
git push origin %BRANCH%
git push origin v%VERSION%

echo ===== Release process completed successfully =====
echo Version: v%VERSION%
echo Tag has been pushed to GitHub
echo GitHub Actions should now start building the release
echo.
echo Check the Actions tab in your GitHub repository to monitor the build progress.

pause
ENDLOCAL 