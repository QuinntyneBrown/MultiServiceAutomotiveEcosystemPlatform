@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%\src\MultiServiceAutomotiveEcosystemPlatform.WebApp" >nul

echo Installing frontend dependencies...
npm install

popd >nul
exit /b %ERRORLEVEL%
