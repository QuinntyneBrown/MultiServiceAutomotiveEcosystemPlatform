@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%\src\MultiServiceAutomotiveEcosystemPlatform.WebApp" >nul

echo Starting frontend dev server...
npm start

popd >nul
exit /b %ERRORLEVEL%
