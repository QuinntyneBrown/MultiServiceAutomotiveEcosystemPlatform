@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

set NORESTORE=
if /I "%~1"=="--no-restore" set NORESTORE=--no-restore

echo Running backend tests...
dotnet test "MultiServiceAutomotiveEcosystemPlatform.sln" %NORESTORE%
if errorlevel 1 goto :error

popd >nul
exit /b 0

:error
popd >nul
exit /b 1
