[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$webAppRoot = Join-Path $repoRoot 'src\MultiServiceAutomotiveEcosystemPlatform.WebApp'

Set-Location $webAppRoot

Write-Host "Building frontend" -ForegroundColor Cyan
& npm run build
