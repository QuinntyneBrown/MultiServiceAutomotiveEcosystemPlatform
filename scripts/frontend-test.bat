@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%\src\MultiServiceAutomotiveEcosystemPlatform.WebApp" >nul

echo Running frontend unit tests...
npm test -- --watch=false

popd >nul
exit /b %ERRORLEVEL%
