@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

set ENVIRONMENT=Development
if not "%~1"=="" set ENVIRONMENT=%~1

echo Running API (ASPNETCORE_ENVIRONMENT=%ENVIRONMENT%)...
set ASPNETCORE_ENVIRONMENT=%ENVIRONMENT%
dotnet run --project "src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj"

popd >nul
exit /b %ERRORLEVEL%
