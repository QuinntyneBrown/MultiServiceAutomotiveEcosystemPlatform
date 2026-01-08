[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$webAppRoot = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.WebApp'

Set-Location $webAppRoot

Write-Host "Running frontend E2E tests (Playwright)" -ForegroundColor Cyan
& npm run test:e2e
