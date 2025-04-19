@echo off

set /p VERSION="Enter release version (e.g. 1.0.0): "

echo Checking if version v%VERSION% exists...
git tag -l v%VERSION% > nul 2>&1
if %errorlevel% equ 0 (
    echo Local tag v%VERSION% exists. Removing...
    git tag -d v%VERSION%

    echo Checking if remote tag v%VERSION% exists...
    git ls-remote --tags origin v%VERSION% > nul 2>&1
    if %errorlevel% equ 0 (
        echo Remote tag v%VERSION% exists. Removing...
        git push origin :refs/tags/v%VERSION%
    ) else (
        echo Remote tag v%VERSION% does not exist. Skipping removal.
    )
) else (
    echo Local tag v%VERSION% does not exist. Skipping removal.
)

REM Get current branch name
for /f "tokens=*" %%a in ('git rev-parse --abbrev-ref HEAD') do set BRANCH=%%a
echo Current branch is: %BRANCH%

echo Adding modified files...
git add MainForm.cs Program.cs Properties/AssemblyInfo.cs build_release.bat

echo Committing changes...
git commit -m "Prepare release v%VERSION%"

echo Creating new v%VERSION% tag...
git tag -a v%VERSION% -m "Release version %VERSION% with improved security measures"

echo Pushing changes to remote...
git push origin %BRANCH%
git push origin v%VERSION%

echo Done!
pause 