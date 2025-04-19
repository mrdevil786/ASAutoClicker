@echo off

set /p VERSION="Enter release version (e.g. 1.0.0): "

echo Checking if version v%VERSION% exists...

REM Check for local tag more robustly
git show-ref --tags --quiet --verify -- "refs/tags/v%VERSION%"
if %errorlevel% equ 0 (
    echo Local tag v%VERSION% exists. Removing...
    git tag -d v%VERSION%
) else (
    echo Local tag v%VERSION% does not exist. Skipping removal.
)

REM Check for remote tag
git ls-remote --tags origin v%VERSION% | findstr "v%VERSION%" > nul
if %errorlevel% equ 0 (
    echo Remote tag v%VERSION% exists. Removing...
    git push origin :refs/tags/v%VERSION%
) else (
    echo Remote tag v%VERSION% does not exist. Skipping removal.
)

REM Get current branch name
for /f "tokens=*" %%a in ('git rev-parse --abbrev-ref HEAD') do set BRANCH=%%a
echo Current branch is: %BRANCH%

echo Adding modified files...
git add MainForm.cs Program.cs Properties/AssemblyInfo.cs build_release.bat

REM Check if there are changes to commit
git diff --cached --quiet
if %errorlevel% neq 0 (
    echo Committing changes...
    git commit -m "Prepare release v%VERSION%"
) else (
    echo No changes to commit, skipping commit step.
)

echo Creating new v%VERSION% tag...
git tag -a v%VERSION% -m "Release version %VERSION% with improved security measures"

echo Pushing changes to remote...
git push origin %BRANCH%
git push origin v%VERSION%

echo Done!
pause 