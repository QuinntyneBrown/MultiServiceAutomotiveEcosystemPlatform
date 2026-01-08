@echo off
setlocal enabledelayedexpansion

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

set CONFIGURATION=Debug
if not "%~1"=="" set CONFIGURATION=%~1

echo Restoring API...
dotnet restore "src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj" -v minimal
if errorlevel 1 goto :error

echo Building API (%CONFIGURATION%)...
dotnet build "src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj" -c %CONFIGURATION% -v minimal
if errorlevel 1 goto :error

popd >nul
exit /b 0

:error
popd >nul
exit /b 1
