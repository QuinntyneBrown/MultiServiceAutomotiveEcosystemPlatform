@echo off
setlocal

set REPO_ROOT=%~dp0..
pushd "%REPO_ROOT%\src\MultiServiceAutomotiveEcosystemPlatform.WebApp" >nul

echo Running frontend E2E tests (Playwright)...
npm run test:e2e

popd >nul
exit /b %ERRORLEVEL%
