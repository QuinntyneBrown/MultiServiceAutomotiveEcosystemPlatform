@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%\src\MultiServiceAutomotiveEcosystemPlatform.WebApp" >nul

echo Building frontend...
npm run build

popd >nul
exit /b %ERRORLEVEL%
