@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

set CONFIGURATION=Debug
if not "%~1"=="" set CONFIGURATION=%~1

echo Restoring solution...
dotnet restore "MultiServiceAutomotiveEcosystemPlatform.sln" -v minimal
if errorlevel 1 goto :error

echo Building solution (%CONFIGURATION%)...
dotnet build "MultiServiceAutomotiveEcosystemPlatform.sln" -c %CONFIGURATION% -v minimal
if errorlevel 1 goto :error

popd >nul
exit /b 0

:error
popd >nul
exit /b 1
