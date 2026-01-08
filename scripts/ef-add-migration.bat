@echo off
setlocal

if "%~1"=="" (
  echo Usage: %~nx0 MigrationName
  exit /b 2
)

set NAME=%~1

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

echo Adding EF migration "%NAME%"...
dotnet ef migrations add "%NAME%" ^
  --project "src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj" ^
  --startup-project "src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj" ^
  --context MultiServiceAutomotiveEcosystemPlatformContext ^
  --output-dir Migrations

popd >nul
exit /b %ERRORLEVEL%
