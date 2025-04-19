@echo off
echo Removing local v1.0.0 tag...
git tag -d v1.0.0

echo Removing remote v1.0.0 tag...
git push origin :refs/tags/v1.0.0

echo Adding modified files...
git add MainForm.cs Program.cs Properties/AssemblyInfo.cs

echo Committing changes...
git commit -m "Improve code to reduce antivirus false positives"

echo Creating new v1.0.0 tag...
git tag -a v1.0.0 -m "Release version 1.0.0 with improved security measures"

echo Pushing changes to remote...
git push origin master
git push origin v1.0.0

echo Done!
pause 