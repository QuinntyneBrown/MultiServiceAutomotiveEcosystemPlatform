@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%" >nul

echo Removing last EF migration...
dotnet ef migrations remove ^
  --project "src\MultiServiceAutomotiveEcosystemPlatform.Infrastructure\MultiServiceAutomotiveEcosystemPlatform.Infrastructure.csproj" ^
  --startup-project "src\MultiServiceAutomotiveEcosystemPlatform.Api\MultiServiceAutomotiveEcosystemPlatform.Api.csproj" ^
  --context MultiServiceAutomotiveEcosystemPlatformContext

popd >nul
exit /b %ERRORLEVEL%
